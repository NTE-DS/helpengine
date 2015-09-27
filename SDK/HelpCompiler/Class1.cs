using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;


namespace SandcastleBuilder.HtmlExtract
{
    /// <summary>
    /// This is the console mode application used to extract title and keyword information from HTML files for
    /// use in creating the CHM table of contents and keyword index files.
    /// </summary>
    public class SandcastleHtmlExtract 
    {
        private struct TitleInfo
        {
            /// <summary>The topic title</summary>
            public string TopicTitle { get; set; }
            /// <summary>The TOC title</summary>
            public string TocTitle { get; set; }
            /// <summary>The file in which it occurs</summary>
            public string File { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="topicTitle">The topic title</param>
            /// <param name="tocTitle">The TOC title</param>
            /// <param name="filename">The filename</param>
            public TitleInfo(string topicTitle, string tocTitle, string filename) : this()
            {
                this.TopicTitle = topicTitle;
                this.TocTitle = String.IsNullOrWhiteSpace(tocTitle) ? topicTitle : tocTitle;
                this.File = filename;
            }
        }

        private struct KeywordInfo
        {
            /// <summary>The main entry</summary>
            public string MainEntry { get; set; }
            /// <summary>An optional sub-entry</summary>
            public string SubEntry { get; set; }
            /// <summary>The file in which it occurs</summary>
            public string File { get; set; }
        }
       
        // Options
        private static string outputFolder, websiteFolder, tocFile;
        private static int maxDegreeOfParallelism = Environment.ProcessorCount * 20;

        // Extracted keyword and title information
        private static List<KeywordInfo> keywords;
        private static ConcurrentBag<KeywordInfo> keywordBag;
        private static ConcurrentDictionary<string, TitleInfo> titles;

        // Regular expressions used for title and keyword extraction and element removal
        private static Regex reTitle = new Regex(@"<title>(.*)</title>", RegexOptions.IgnoreCase);
        private static Regex reTocTitle = new Regex("<mshelp:toctitle\\s+title=\"([^\"]+)\"[^>]+>",
            RegexOptions.IgnoreCase);
        private static Regex reKKeyword = new Regex("<mshelp:keyword\\s+index=\"k\"\\s+term=\"([^\"]+)\"[^>]+>",
            RegexOptions.IgnoreCase);
        private static Regex reSubEntry = new Regex(@",([^\)\>]+|([^\<\>]*" +
            @"\<[^\<\>]*\>[^\<\>]*)?|([^\(\)]*\([^\(\)]*\)[^\(\)]*)?)$");
        private static Regex reXmlIsland = new Regex("<xml>.*?</xml>", RegexOptions.Singleline);
        private static Regex reHxLinkCss = new Regex("<link[^>]*?href=\"ms-help://Hx/HxRuntime/HxLink\\.css\".*?/?>",
            RegexOptions.IgnoreCase);
        private static Regex reHtmlElement = new Regex("\\<html.*?\\>");
        private static Regex reUnusedNamespaces = new Regex("\\s*?xmlns:(MSHelp|mshelp|ddue|xlink|msxsl|xhtml)=\".*?\"");

        public static void Function(string folder, string outputFolder)
        {
            websiteFolder = folder;
            SandcastleHtmlExtract.outputFolder = outputFolder;
            ParseFiles(websiteFolder);
        }

        private static void ParseFiles(string fileFolder)
        {
            KeywordInfo kw;
            string mainEntry = String.Empty;
            int htmlFiles = 0;

            keywords.Clear();
            keywordBag = new ConcurrentBag<KeywordInfo>();
            titles.Clear();

            if (fileFolder.Length != 0 && fileFolder[fileFolder.Length - 1] == '\\')
                fileFolder = fileFolder.Substring(0, fileFolder.Length - 1);

            // Process all *.htm and *.html files in the given folder and all of its subfolders.
            Parallel.ForEach(Directory.EnumerateFiles(fileFolder, "*.*", SearchOption.AllDirectories),
              new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, file =>
              {
                  string ext = Path.GetExtension(file).ToLowerInvariant();

                  if (ext == ".htm" || ext == ".html")
                  {
                      ProcessFile(fileFolder, file);
                      Interlocked.Add(ref htmlFiles, 1);
                  }
              });

            Console.WriteLine("Processed {0} HTML files\r\nSorting keywords and generating See Also indices", htmlFiles);

            // Sort the keywords
            keywords.AddRange(keywordBag);
            keywords.Sort((x, y) =>
            {
                string subX, subY;

                if (x.MainEntry != y.MainEntry)
                    return String.Compare(x.MainEntry, y.MainEntry, StringComparison.OrdinalIgnoreCase);

                subX = x.SubEntry;
                subY = y.SubEntry;

                if (subX == null)
                    subX = String.Empty;

                if (subY == null)
                    subY = String.Empty;

                if (subX != subY)
                    return String.Compare(subX, subY, StringComparison.OrdinalIgnoreCase);

                subX = titles[Path.GetFileNameWithoutExtension(x.File)].TopicTitle;
                subY = titles[Path.GetFileNameWithoutExtension(y.File)].TopicTitle;

                return String.Compare(subX, subY, StringComparison.OrdinalIgnoreCase);
            });

            // Insert the See Also indices for each sub-entry
            for (int idx = 0; idx < keywords.Count; idx++)
                if (!String.IsNullOrEmpty(keywords[idx].SubEntry))
                {
                    if (idx > 0)
                        mainEntry = keywords[idx - 1].MainEntry;

                    if (mainEntry != keywords[idx].MainEntry)
                    {
                        kw = new KeywordInfo();
                        kw.MainEntry = keywords[idx].MainEntry;
                        keywords.Insert(idx, kw);
                    }
                }
        }

        /// <summary>
        /// Parse each file looking for the title and index keywords and remove the unnecessary Help 2 constructs
        /// that cause issues in Internet Explorer 10.
        /// </summary>
        /// <param name="basePath">The base folder path</param>
        /// <param name="sourceFile">The file to parse</param>
        /// <param name="localizedOutputFolder">The folder in which to store localized output or null for no
        /// localized output.</param>
        private static void ProcessFile(string basePath, string sourceFile)
        {
            Encoding currentEncoding = Encoding.Default;
            MatchCollection matches;
            Match match;
            KeywordInfo keyword;
            string content, topicTitle, tocTitle, term, folder, key, destFile;
            byte[] currentBytes, convertedBytes;

            // Read the file in using the proper encoding
            using (StreamReader sr = new StreamReader(sourceFile, currentEncoding, true))
            {
                content = sr.ReadToEnd();
                currentEncoding = sr.CurrentEncoding;
            }

            topicTitle = tocTitle = String.Empty;

            // Extract the topic title
            match = reTitle.Match(content);

            if (match.Success)
                topicTitle = match.Groups[1].Value;

            // If a TOC title entry is present, get that too
            match = reTocTitle.Match(content);

            if (match.Success)
                tocTitle = match.Groups[1].Value;

            key = Path.GetFileNameWithoutExtension(sourceFile);

            if (!titles.TryAdd(key, new TitleInfo(WebUtility.HtmlDecode(topicTitle),
              WebUtility.HtmlDecode(tocTitle), sourceFile)))
                Console.WriteLine("SHFB: Warning SHE0004: The key '{0}' used for '{1}' is already in use by '{2}'.  " +
                    "'{1}' will be ignored.", key, sourceFile, titles[key].File);

            // Extract K index keywords
            matches = reKKeyword.Matches(content);

            foreach (Match m in matches)
            {
                keyword = new KeywordInfo();
                term = m.Groups[1].Value;

                if (!String.IsNullOrEmpty(term))
                {
                    term = WebUtility.HtmlDecode(term.Replace("%3C", "<").Replace("%3E", ">").Replace("%2C", ","));

                    // See if there is a sub-entry
                    match = reSubEntry.Match(term);

                    if (match.Success)
                    {
                        keyword.MainEntry = term.Substring(0, match.Index);
                        keyword.SubEntry = term.Substring(match.Index + 1).TrimStart(new char[] { ' ' });
                    }
                    else
                        keyword.MainEntry = term;

                    keyword.File = sourceFile;
                    keywordBag.Add(keyword);
                }
            }

            // Remove the XML data island, the MS Help CSS link, and the unused namespaces
            content = reXmlIsland.Replace(content, String.Empty);
            content = reHxLinkCss.Replace(content, String.Empty);

            Match htmlMatch = reHtmlElement.Match(content);
            string htmlElement = reUnusedNamespaces.Replace(htmlMatch.Value, String.Empty);

            content = reHtmlElement.Replace(content, htmlElement, 1);

            // Save the file to its original location without the Help 2 elements using the original encoding
            using (StreamWriter writer = new StreamWriter(sourceFile, false, currentEncoding))
            {
                writer.Write(content);
            }
        }

        private static void WriteContentLine(TextWriter writer,
          int indentCount, string value)
        {
            writer.WriteLine();

            for (int idx = 0; idx < indentCount; idx++)
                writer.Write("  ");

            writer.Write(value);
        }

        /// <summary>
        /// Write out the website table of contents
        /// </summary>
        private static void WriteWebsiteTableOfContents()
        {
            XmlReaderSettings settings;
            TitleInfo titleInfo;
            string key, title, htmlFile;
            int indentCount, baseFolderLength = websiteFolder.Length + 1;

            Console.WriteLine(@"Saving website table of contents to {0}\WebTOC.xml", outputFolder);

            settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            using (var reader = XmlReader.Create(tocFile, settings))
            {
                // Write the table of contents with UTF-8 encoding
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputFolder, "WebTOC.xml"), false,
                  Encoding.UTF8))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.WriteLine("<HelpTOC>");

                    while (reader.Read())
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "topic")
                                {
                                    key = reader.GetAttribute("file");

                                    if (!String.IsNullOrEmpty(key) && titles.ContainsKey(key))
                                    {
                                        titleInfo = titles[key];
                                        title = titleInfo.TocTitle;
                                        htmlFile = titleInfo.File.Substring(baseFolderLength).Replace('\\', '/');
                                    }
                                    else
                                    {
                                        // Container only topic or unknown element, just use the title or ID attribute
                                        htmlFile = null;
                                        title = reader.GetAttribute("title");

                                        if (String.IsNullOrEmpty(title))
                                            title = reader.GetAttribute("id");

                                        if (String.IsNullOrEmpty(title))
                                            title = key;
                                    }

                                    indentCount = reader.Depth;
                                    title = WebUtility.HtmlEncode(title);
                                    htmlFile = WebUtility.HtmlEncode(htmlFile);

                                    if (reader.IsEmptyElement)
                                        WriteContentLine(writer, indentCount, String.Format(CultureInfo.InvariantCulture,
                                            "<HelpTOCNode Title=\"{0}\" Url=\"{1}\" />", title, htmlFile));
                                    else
                                        if (htmlFile != null)
                                    {
                                        WriteContentLine(writer, indentCount, String.Format(CultureInfo.InvariantCulture,
                                            "<HelpTOCNode Id=\"{0}\" Title=\"{1}\" Url=\"{2}\">", Guid.NewGuid(),
                                            title, htmlFile));
                                    }
                                    else
                                        WriteContentLine(writer, indentCount, String.Format(CultureInfo.InvariantCulture,
                                            "<HelpTOCNode Id=\"{0}\" Title=\"{1}\">", Guid.NewGuid(), title));
                                }
                                break;

                            case XmlNodeType.EndElement:
                                if (reader.Name == "topic")
                                    WriteContentLine(writer, reader.Depth, "</HelpTOCNode>");
                                break;

                            default:
                                break;
                        }

                    writer.WriteLine();
                    writer.WriteLine("</HelpTOC>");
                }
            }
        }

        private static void WriteWebsiteKeywordIndex()
        {
            KeywordInfo kw;
            string title;
            int baseFolderLength = websiteFolder.Length + 1;

            Console.WriteLine(@"Saving website keyword index to {0}\WebKI.xml", outputFolder);

            // Write the keyword index with UTF-8 encoding
            using (StreamWriter writer = new StreamWriter(Path.Combine(outputFolder, "WebKI.xml"), false,
              Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.Write("<HelpKI>");

                foreach (var group in keywords.Where(k => !String.IsNullOrEmpty(k.MainEntry)).GroupBy(k => k.MainEntry))
                    if (group.Count() == 1)
                    {
                        kw = group.First();

                        if (!String.IsNullOrEmpty(kw.File))
                            WriteContentLine(writer, 1, String.Format(CultureInfo.InvariantCulture,
                                "<HelpKINode Title=\"{0}\" Url=\"{1}\" />", WebUtility.HtmlEncode(kw.MainEntry),
                                WebUtility.HtmlEncode(kw.File.Substring(baseFolderLength)).Replace('\\', '/')));
                    }
                    else
                    {
                        WriteContentLine(writer, 1, String.Format(CultureInfo.InvariantCulture,
                             "<HelpKINode Title=\"{0}\">", WebUtility.HtmlEncode(group.Key)));

                        foreach (var k in group)
                            if (!String.IsNullOrEmpty(k.File))
                                if (String.IsNullOrEmpty(k.SubEntry))
                                {
                                    // Use the target page's title as the entry's title as it will be fully
                                    // qualified if necessary.
                                    title = titles[Path.GetFileNameWithoutExtension(k.File)].TopicTitle;

                                    WriteContentLine(writer, 2, String.Format(CultureInfo.InvariantCulture,
                                        "<HelpKINode Title=\"{0}\" Url=\"{1}\" />", WebUtility.HtmlEncode(title),
                                        WebUtility.HtmlEncode(k.File.Substring(baseFolderLength)).Replace('\\', '/')));
                                }
                                else
                                    WriteContentLine(writer, 2, String.Format(CultureInfo.InvariantCulture,
                                        "<HelpKINode Title=\"{0}\" Url=\"{1}\" />", WebUtility.HtmlEncode(k.SubEntry),
                                        WebUtility.HtmlEncode(k.File.Substring(baseFolderLength)).Replace('\\', '/')));

                        WriteContentLine(writer, 1, "</HelpKINode>");
                    }

                writer.WriteLine();
                writer.WriteLine("</HelpKI>");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DocExplorer.Resources.HelpAPI;

namespace HelpServer {
    internal class NullToc : ITOCNode, IIdentify {
        public readonly List<NullToc> m_SubNodes = new List<NullToc>();

        public string Title { get; private set; }

        public int SubCount {
            get { return m_SubNodes.Count; }
        }

        public int ImageId { get; set; }

        public void AddSubNode(ITOCNode subNode) {
            m_SubNodes.Add((NullToc) subNode);
        }

        public bool ContainSubNode(string subNodeId) {
            return m_SubNodes.Any(v => v.Id == subNodeId);
        }

        public ITOCNode GetSubTOCNode(string subNodeId) {
            return m_SubNodes.FirstOrDefault(v => v.Id == subNodeId);
        }

        public void InitializeTOCNode(string title, string url, string id, string namespaceName, string helpFileNamespaceName) {
            this.Title = title;
            this.Url = url;
            this.Id = id;
            this.Namespace = namespaceName;
            this.HelpFileNamespace = helpFileNamespaceName;
        }

        public string Url { get; private set; }

        public string Id { get; private set; }

        public string HelpFileNamespace { get; private set; }

        public string Namespace { get; private set; }
    }

    internal class NullUi : IHelpUi {
        public Type TocTypeGenerator {
            get { return typeof (NullToc); }
        }

        public void SetActiveCollection() {}

        public void RefreshFilters() {}
    }
    
    internal class HelpHttp {
        public HelpHttp()
        {
            var args = new Arguments(Environment.GetCommandLineArgs());

            var help = new DocExplorer.Resources.HelpAPI.Help {HelpUi = new NullUi()};
            help.LoadNamespaces(args["LoadLocalCollection"] != "true" ? DocExplorer.Resources.HelpAPI.Help.GetRegisteredCollection(String.IsNullOrEmpty(args["Collection"]) ? "DefaultCollection" : args["Collection"]) : args["Collection"]);
            help.ActiveNamespace = help.GetNamespace(String.IsNullOrEmpty(args["Namespace"]) ? "NasuTek.Default.CC" : args["Namespace"]);
        }

        public Tuple<byte[], string> handleGETRequest(HttpListenerRequest p)
        {
            if (p.RawUrl == "/" || p.RawUrl == "/index.aspx") {
                var assembly = Assembly.GetExecutingAssembly();

                using (var stream = assembly.GetManifestResourceStream("HelpServer.InterfaceResources.UserInterface.html"))
                    if (stream != null) {
                        using (var reader = new StreamReader(stream)) {
                            var result = reader.ReadToEnd();
                            return Tuple.Create(Encoding.UTF8.GetBytes(result.Replace("###---TOC-GOES-HERE---###", GenTOC()).Replace("###---DEFAULT-TOPIC-URL-HERE---###", "about:blank")), "");
                        }
                    }
                return Tuple.Create(new byte[] {}, "");
            }

            var command = p.RawUrl.Split('/')[1];
            var uri = p.RawUrl.Replace(command, "").Substring(1);

            switch (command) {
                case "help-data":
                    return Tuple.Create(GenSelf("nte-help:/" + uri), "text/html");
                case "interface-resources": {
                    try {
                        Assembly _assembly = Assembly.GetExecutingAssembly();
                        Stream _imageStream = _assembly.GetManifestResourceStream("HelpServer.InterfaceResources" + uri.Replace('/', '.'));

                        var data = new byte[_imageStream.Length];
                        _imageStream.Read(data, 0, (int) _imageStream.Length);

                        return Tuple.Create(data, "");
                    } catch {
                        return Tuple.Create(Encoding.UTF8.GetBytes("Error accessing resources!"), "");
                    }
                }
            }

            return Tuple.Create(new byte[] {}, "");
        }

        public string GenTOC() {
            var nodeRoot = (NullToc) Help.Instance.GetTableOfContentsTree("(no filter)");

            var retVal = "";
            retVal = nodeRoot.m_SubNodes.Aggregate(retVal, (current, subNode) => current + GenTOC(subNode));

            return retVal;
        }

        public string GenTOC(NullToc node) {
            var retVal = "";
            if (node.SubCount > 0) {
                retVal += "<div class=\"TreeNode\">";
            } else {
                retVal += "<div class=\"TreeItem\">";
            }
            retVal += "<img class=\"TreeNodeImg\" onclick=\"javascript: Toggle(this);\" src=\"" + (node.SubCount > 0 ? "interface-resources/Collapsed.gif" : "interface-resources/Item.gif") +
                "\"/><a class=\"UnselectedNode\" onclick=\"javascript: return Expand(this);\" href=\"help-data/" + node.Namespace + "/" + node.HelpFileNamespace + "/" + node.Url + "\" target=\"TopicContent\">" +
                node.Title + "</a>";

            if (node.SubCount > 0) {
                retVal += "<div class=\"Hidden\">";
                retVal = node.m_SubNodes.Aggregate(retVal, (current, subNode) => current + GenTOC(subNode));
                retVal += "</div>";
            }
            retVal += "</div>";

            return retVal;
        }

        public byte[] GenSelf(string uriStr) {
            var uri = new Uri(uriStr);

            try {
                if (uri.Host == "nte-help5-common") {
                    return Encoding.UTF8.GetBytes("COMMON");
                }
                var absolutePath = uri.AbsolutePath.Substring(1).Split('/');
                var finalAbsolutePath = uri.AbsolutePath.Substring(absolutePath[0].Length + 2);
                byte[] array = Help.Instance.GetHelpFile(Help.Instance.GetProperNamespaceName(uri.Host), absolutePath[0]).ReadFile(finalAbsolutePath);

                return array;
            } catch (System.Exception ex) {
                return Encoding.UTF8.GetBytes(ex.ToString());
            }
        }
    }
}

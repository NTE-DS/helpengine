/***************************************************************************************************
 * NasuTek Developer Studio
 * Copyright (C) 2005-2013 NasuTek Enterprises
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***************************************************************************************************/

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
namespace DocExplorer.Resources.HelpAPI
{
	public class HelpNamespace
	{
		private readonly System.Collections.Generic.List<Filter> _filters = new System.Collections.Generic.List<Filter>();
        private readonly System.Collections.Generic.List<string> _pluginObject = new System.Collections.Generic.List<string>();
		private readonly System.Collections.Generic.List<HelpFile> _titles = new System.Collections.Generic.List<HelpFile>();

        public string HelpDisplayLogoPath { get; private set; }
        public string HelpDisplayCollectionInfoPath { get; private set; }
        public bool NamespaceTopicsLoaded { get; private set; }
        public bool CombinedCollection { get; private set; }
        public string ContentStorePath { get; private set; }
        public string NamespaceID { get; private set; }
        public string Title { get; private set; }
        public string RegisteredUser { get; private set; }
        public string RegisteredCompany { get; private set; }
        public string RegisteredSerial { get; private set; }
        private XDocument namespaceDocument;

		public string[] NamespacePlugins
		{
			get
			{
				return this._pluginObject.ToArray();
			}
		}
        public HelpFile[] Titles
		{
			get
			{
				return this._titles.ToArray();
			}
		}
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> FilterElements {
            get {
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> dictionary = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> current in (
                    from helpFile in this.Titles
                    from xDocument in helpFile.Tocs
                    select xDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC").Attribute("FilterDef") into filterDefAttrib
                    where filterDefAttrib != null
                    select (
                        from filter in filterDefAttrib.Value.Split(new char[]
						{
							';'
						})
                        select filter.Split(new char[]
						{
							'='
						}) into filterSplit
                        where filterSplit.Length == 2
                        select filterSplit).ToDictionary((string[] filterSplit) => filterSplit[0], (string[] filterSplit) => filterSplit[1])).SelectMany((System.Collections.Generic.Dictionary<string, string> filters) => filters)) {
                    if (dictionary.ContainsKey(current.Key)) {
                        dictionary[current.Key].Add(current.Value);
                    } else {
                        dictionary.Add(current.Key, new System.Collections.Generic.List<string>
						{
							current.Value
						});
                    }
                }
                return dictionary;
            }
        }
        public Filter[] Filters {
            get {
                return this._filters.ToArray();
            }
        }
		
		internal HelpNamespace(System.Collections.Generic.List<Filter> filters, System.Collections.Generic.List<string> plugins, System.Collections.Generic.List<HelpFile> loadedHelp, string collectionInfoPath, string namespaceName, string title)
		{
			this._filters = filters;
			this._pluginObject = plugins;
			this._titles = loadedHelp;
			this.ContentStorePath = collectionInfoPath;
			this.NamespaceID = namespaceName;
			this.Title = title;
        }

		internal HelpNamespace(string namespaceXmlPath)
		{
            namespaceDocument = XDocument.Load(namespaceXmlPath);

            var namespaceRoot = namespaceDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition");

            ContentStorePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(namespaceXmlPath), "..", "ContentStore"));
            NamespaceID = namespaceRoot.Attribute("id").Value;
            Title = namespaceRoot.Attribute("friendlyName").Value;

            HelpDisplayLogoPath = "nte-help://nte-help5-common/logo.html";
            HelpDisplayCollectionInfoPath = "nte-help://nte-help5-common/collections.html";
            RegisteredUser = "Unregistered User";
            RegisteredCompany = "Unregistered Company";
            RegisteredSerial = "xxxxx-xxxxx-xxxxx-xxxxx-xxxxx";

            if (namespaceRoot.Attribute("logoPath") != null)
                HelpDisplayLogoPath = namespaceRoot.Attribute("logoPath").Value;
            if (namespaceRoot.Attribute("infoPath") != null)
                HelpDisplayCollectionInfoPath = namespaceRoot.Attribute("infoPath").Value;
            if (namespaceRoot.Attribute("userName") != null)
                RegisteredUser = namespaceRoot.Attribute("userName").Value;
            if (namespaceRoot.Attribute("companyName") != null)
                RegisteredCompany = namespaceRoot.Attribute("companyName").Value;
            if (namespaceRoot.Attribute("serialNumber") != null)
                RegisteredSerial = namespaceRoot.Attribute("serialNumber").Value;

            CombinedCollection = Boolean.Parse(namespaceRoot.Attribute("isCombinedCollection").Value);

            var pluginsElement = namespaceRoot.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugins");

            if (pluginsElement != null) {
                foreach (var id in pluginsElement.Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugin")) {
                    _pluginObject.Add(id.Attribute("id").Value);
                }
            }
		}

        public void LoadTopics() {
            if (!NamespaceTopicsLoaded && !CombinedCollection) {
                var namespaceRoot = namespaceDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition");
                var topicsElement = namespaceRoot.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBooks");

                if (topicsElement != null) {
                    foreach (var id in topicsElement.Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBook")) {
                        _titles.Add(new HelpFile(IsOnlineBook(id.Attribute("id").Value) ? GetBookFilePath(id.Attribute("id").Value) : Path.Combine(ContentStorePath, GetBookFilePath(id.Attribute("id").Value)), IsOnlineBook(id.Attribute("id").Value)));
                    }
                }

                NamespaceTopicsLoaded = true;
            }
        }

        private bool IsOnlineBook(string p) {
            var titlesDocument = XDocument.Load(Path.Combine(ContentStorePath, "InstalledBooks.xml"));

            var booksRoot = titlesDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}InstalledBooks");

            var retValue = booksRoot.Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Book").FirstOrDefault(book => book.Attribute("id").Value == p);

            if (retValue != null && retValue.Attribute("onlineBook") != null)
                return Convert.ToBoolean(retValue.Attribute("onlineBook").Value);
            return false;
        }

        private string GetBookFilePath(string p) {
            var titlesDocument = XDocument.Load(Path.Combine(ContentStorePath, "InstalledBooks.xml"));

            var booksRoot = titlesDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}InstalledBooks");

            var retValue = booksRoot.Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Book").FirstOrDefault(book => book.Attribute("id").Value == p);

            return retValue != null ? retValue.Attribute("fileName").Value : null;
        }
	}
}

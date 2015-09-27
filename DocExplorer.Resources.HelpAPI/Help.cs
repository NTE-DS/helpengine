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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NasuTek.DevEnvironment;

namespace DocExplorer.Resources.HelpAPI {
    public class Help {
        public static Help Instance { get; private set; }
        public event System.EventHandler BrowserNavigated;
        public IHelpUi HelpUi { get; set; }
        public SearchEngine SearchEngine { get; private set; }
        private HelpNamespace helpNamespace;
        internal Dictionary<string, HelpNamespace> Namespaces { get; private set; }
        public Settings SettingsInstance { get; set; }

        public HelpNamespace ActiveNamespace {
            get { return helpNamespace; }
            set {
                helpNamespace = value;

                if (!SettingsInstance.RootFavorites.ContainsKey(value.NamespaceID))
                    SettingsInstance.RootFavorites.Add(value.NamespaceID, new FavoriteFolder());

                value.LoadTopics();

                this.HelpUi.SetActiveCollection();
                this.HelpUi.RefreshFilters();
            }
        }

        public Help(bool agent = false) {
            Instance = this;
            Settings.OpenSettings();
            this.Namespaces = new Dictionary<string, HelpNamespace>();
            this.SearchEngine = new SearchEngine();
            this.SearchEngine.SearchProviders.Add(new SearchLocalHelp());
        }

        public static void RegisterNteHelpProtocol() {
            Protocol.RegisterProtocol("nte-help", new HelpScheme());
        }

        public static void UnregisterNteHelpProtocol() {
            Protocol.UnregisterProtocol("nte-help");
        }

        public HelpNamespace GetNamespace(string name) {
            return Namespaces[name];
        }

        public void BrowserNavigatedCall(object sender) {
            if (this.BrowserNavigated != null) {
                this.BrowserNavigated(sender, new System.EventArgs());
            }
        }

        public HelpFile GetHelpFile(string namespaceName, string helpFileId) {
            if (!Namespaces[namespaceName].NamespaceTopicsLoaded)
                Namespaces[namespaceName].LoadTopics();

            HelpFile helpFile = (
                from hf in Namespaces[namespaceName].Titles
                where string.Equals(hf.HelpFileId, helpFileId, StringComparison.CurrentCultureIgnoreCase)
                select hf).FirstOrDefault<HelpFile>();
            if (helpFile == null) {
                string[] namespacePlugins = Namespaces[namespaceName].NamespacePlugins;
                int num = 0;
                if (num < namespacePlugins.Length) {
                    HelpNamespace helpNamespace = Namespaces[namespacePlugins[num]];
                    return (
                        from hf in helpNamespace.Titles
                        where hf.HelpFileId == helpFileId
                        select hf).FirstOrDefault<HelpFile>();
                }
            }
            return helpFile;
        }

        public void LoadNamespaces(string collectionPath) {
            foreach (var ns in GetNamespaces(collectionPath)) {
                Namespaces.Add(ns.NamespaceID, ns);
            }
        }

        public System.Collections.Generic.Dictionary<string, SearchEngine.SearchItem[]> Search(string term) {
            System.Collections.Generic.Dictionary<string, SearchEngine.SearchItem[]> dictionary = new System.Collections.Generic.Dictionary<string, SearchEngine.SearchItem[]>();
            foreach (ISearch current in this.SearchEngine.SearchProviders) {
                System.Collections.Generic.List<SearchEngine.SearchItem> list = new System.Collections.Generic.List<SearchEngine.SearchItem>();
                current.Search(list, term, this);
                dictionary.Add(current.Name, list.ToArray());
            }
            return dictionary;
        }

        public static HelpNamespace[] GetNamespaces(string collectionPath) {
            System.Collections.Generic.List<HelpNamespace> list = new System.Collections.Generic.List<HelpNamespace>();

            foreach (var file in Directory.GetFiles(Path.Combine(collectionPath, "Namespaces"))) {
                list.Add(new HelpNamespace(file));
            }

            return list.ToArray();
        }

        [Obsolete("It is recommended to use the GetTableOfContentsTree method that requests the Namespace Name. This function will be removed in a later version of Help")]
        public ITOCNode GetTableOfContentsTree(string filter) {
            return GetTableOfContentsTree(ActiveNamespace.NamespaceID, filter);
        }

        public static string GetRegisteredCollection(string collectionName) {
    #if DEBUG
                var regPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2-Debug\\RegisteredCollections");
    #else
                var regPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2\\RegisteredCollections");
    #endif

            return (string)regPath.GetValue(collectionName);
        }

        public ITOCNode GetTableOfContentsTree(string namespaceName, string filter) {
            ITOCNode iTOCNode = (ITOCNode)this.HelpUi.TocTypeGenerator.GetConstructor(System.Type.EmptyTypes).Invoke(null);
            iTOCNode.InitializeTOCNode(null, null, null, null, null);
            HelpFile[] titles = Namespaces[namespaceName].Titles;
            foreach (HelpFile helpFile in titles) {
                XDocument[] tocs = helpFile.Tocs;
                foreach (XDocument xDocument in tocs) {
                    this.Fill(iTOCNode, xDocument, xDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC"), namespaceName, helpFile.HelpFileId, filter);
                }
            }
            //TODO: Change this to deal with the new way of reading namespaces 
            string[] namespacePlugins = Namespaces[namespaceName].NamespacePlugins;
            foreach (HelpNamespace helpNamespace in namespacePlugins.Select(t => Namespaces[t])) {
                if (!helpNamespace.NamespaceTopicsLoaded)
                    helpNamespace.LoadTopics();
                HelpFile[] titles2 = helpNamespace.Titles;
                foreach (HelpFile helpFile2 in titles2) {
                    XDocument[] tocs2 = helpFile2.Tocs;
                    foreach (XDocument xDocument2 in tocs2) {
                        this.Fill(iTOCNode, xDocument2, xDocument2.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC"), helpNamespace.NamespaceID, helpFile2.HelpFileId, filter);
                    }
                }
            }
            return iTOCNode;
        }

        public string GetUriFromTOCNode(IIdentify identifyObject) {
            return "nte-help://" + identifyObject.Namespace + "/" + identifyObject.HelpFileNamespace + "/" + identifyObject.Url;
        }

        [Obsolete("It is recommended to use the GetIndex method that requests the Namespace Name. This function will be removed in a later version of Help")]
        public IndexNode GetIndex(string filter) {
            return GetIndex(ActiveNamespace.NamespaceID, filter);
        }

        public IndexNode GetIndex(string namespaceName, string filter) {
            IndexNode indexNode = new IndexNode(null, null, null, null, false);
            HelpFile[] titles = Namespaces[namespaceName].Titles;
            for (int i = 0; i < titles.Length; i++) {
                HelpFile helpFile = titles[i];
                XDocument[] indexes = helpFile.Indexes;
                for (int j = 0; j < indexes.Length; j++) {
                    XDocument xDocument = indexes[j];
                    this.IndexFill(indexNode, xDocument, xDocument.Root, namespaceName, helpFile.HelpFileId, filter, false);
                }
            }

            string[] namespacePlugins = Namespaces[namespaceName].NamespacePlugins;
            for (int k = 0; k < namespacePlugins.Length; k++) {
                HelpNamespace helpNamespace = Namespaces[namespacePlugins[k]];
                if (!helpNamespace.NamespaceTopicsLoaded)
                    helpNamespace.LoadTopics();
                HelpFile[] titles2 = helpNamespace.Titles;
                for (int l = 0; l < titles2.Length; l++) {
                    HelpFile helpFile2 = titles2[l];
                    XDocument[] indexes2 = helpFile2.Indexes;
                    for (int m = 0; m < indexes2.Length; m++) {
                        XDocument xDocument2 = indexes2[m];
                        this.IndexFill(indexNode, xDocument2, xDocument2.Root, helpNamespace.NamespaceID, helpFile2.HelpFileId, filter, false);
                    }
                }
            }
            return indexNode;
        }

        #region Private Functions
        private void LinkFill(ITOCNode root, XDocument xdoc, XElement rootNode, string namespaceName, string helpFileNamespaceName, string filterQuery) {
            foreach (XElement current in
                from x in rootNode.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOCNode")
                where !this.IsLink(x.Attribute("Type"))
                select x) {
                string text = (current.Attribute("Url") != null) ? current.Attribute("Url").Value : null;
                string text2 = (current.Attribute("Title") != null) ? current.Attribute("Title").Value : null;
                string id = (current.Attribute("Id") != null) ? current.Attribute("Id").Value : null;
                string text3 = text.Split(new char[]
				{
					':'
				})[0];
                string filePath = text.Replace(text3 + ":", "");
                ITOCNode iTOCNode;
                if (!root.ContainSubNode(id)) {
                    iTOCNode = (ITOCNode)this.HelpUi.TocTypeGenerator.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    iTOCNode.InitializeTOCNode(text2, null, id, namespaceName, helpFileNamespaceName);
                } else {
                    iTOCNode = root.GetSubTOCNode(id);
                }
                XDocument xDocument = XDocument.Load(new System.IO.MemoryStream(this.GetHelpFile(namespaceName, text3).ReadFile(filePath)));
                this.Fill(iTOCNode, xDocument, xDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC"), namespaceName, helpFileNamespaceName, filterQuery);
                if (!root.ContainSubNode(id) && iTOCNode.SubCount != 0) {
                    root.AddSubNode(iTOCNode);
                }
            }
        }

        private void Fill(ITOCNode root, XDocument xdoc, XElement rootNode, string namespaceName, string helpFileNamespaceName, string filterQuery) {
            if (!this.Filter(this.GetFilterData(namespaceName, filterQuery), (xdoc.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC").Attribute("FilterDef") != null) ? xdoc.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC").Attribute("FilterDef").Value : null)) {
                return;
            }
            foreach (XElement current in
                from x in rootNode.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOCNode")
                where this.IsLink(x.Attribute("Type"))
                select x) {
                    string text = (current.Attribute("Title") != null) ? current.Attribute("Title").Value : null;
                    string url = (current.Attribute("Url") != null) ? current.Attribute("Url").Value : null;
                    string id = (current.Attribute("Id") != null) ? current.Attribute("Id").Value : null;
                    int num = (current.Attribute("TocImageId") != null) ? System.Convert.ToInt32(current.Attribute("TocImageId").Value) : 0;
                    this.LinkFill(root, xdoc, current, namespaceName, helpFileNamespaceName, filterQuery);
                    ITOCNode iTOCNode;
                    if (!root.ContainSubNode(id)) {
                        iTOCNode = (ITOCNode) this.HelpUi.TocTypeGenerator.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                        iTOCNode.InitializeTOCNode(text, url, id, namespaceName, helpFileNamespaceName);
                    } else {
                        iTOCNode = root.GetSubTOCNode(id);
                    }
                    iTOCNode.ImageId = num;
                
                if (current.HasElements) {
                    this.Fill(iTOCNode, xdoc, current, namespaceName, helpFileNamespaceName, filterQuery);
                } else {
                    if (num == 0) {
                        iTOCNode.ImageId = 9;
                    }
                }
                if (!root.ContainSubNode(id)) {
                    root.AddSubNode(iTOCNode);
                }
            }
        }

        private bool IsLink(XAttribute xAttribute) {
            return xAttribute == null || xAttribute.Value == "Plugin" || xAttribute.Value == "Link" || !(xAttribute.Value == "TOC");
        }

        private bool Filter(System.Collections.Generic.Dictionary<string, string> dictionary, string filter) {
            if (dictionary == null) {
                return true;
            }
            if (filter == null) {
                return false;
            }
            System.Collections.Generic.Dictionary<string, string> source = (
                from s in filter.Split(new char[]
				{
					';'
				})
                select s.Split(new char[]
				{
					'='
				})).ToDictionary((string[] match) => match[0], (string[] match) => match[1]);
            return source.Any(delegate(System.Collections.Generic.KeyValuePair<string, string> tocFilterDef) {
                if (dictionary.ContainsKey(tocFilterDef.Key)) {
                    string[] source2 = tocFilterDef.Value.Split(new char[]
					{
						','
					});
                    string[] @object = dictionary[tocFilterDef.Key].Split(new char[]
					{
						','
					});
                    return source2.Any(new Func<string, bool>(@object.Contains<string>));
                }
                return false;
            });
        }

        private System.Collections.Generic.Dictionary<string, string> GetFilterData(string namespaceName, string filterName) {
            if (filterName == "(no filter)") {
                return null;
            }
            Filter filter = (
                from filFile in Namespaces[namespaceName].Filters
                where filFile.FilterName == filterName
                select filFile).FirstOrDefault<Filter>();
            if (filter == null) {
                return null;
            }
            return filter.Params.ToDictionary((FilterParam filterParam) => filterParam.Key, (FilterParam filterParam) => filterParam.Value);
        }

        private void IndexLinkFill(IndexNode root, XDocument xdoc, XElement rootNode, string namespaceName, string helpFileNamespaceName) {
            foreach (XDocument current in
                from linkPath in
                    (
                        from x in rootNode.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}IndexLink")
                        where !this.IsLink(x.Attribute("Type"))
                        select x).Select(delegate(XElement node) {
                            if (node.Attribute("Url") == null) {
                                return null;
                            }
                            return node.Attribute("Url").Value;
                        })
                let namespaceN = linkPath.Split(new char[]
				{
					':'
				})[0]
                let path = linkPath.Replace(namespaceN + ":", "")
                select XDocument.Load(new System.IO.MemoryStream(this.GetHelpFile(namespaceName, namespaceN).ReadFile(path)))) {
                this.IndexFill(root, current, current.Root, namespaceName, helpFileNamespaceName, "(no filter)", false);
            }
        }

        private void IndexFill(IndexNode root, XDocument xdoc, XElement rootNode, string namespaceName, string helpFileNamespaceName, string filterQuery, bool subKeys) {
            if (!this.Filter(this.GetFilterData(namespaceName, filterQuery), (xdoc.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpKI").Attribute("FilterDef") != null) ? xdoc.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpKI").Attribute("FilterDef").Value : null)) {
                return;
            }
            this.IndexLinkFill(root, xdoc, rootNode, namespaceName, helpFileNamespaceName);
            foreach (XElement current in
                from x in rootNode.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpKINode")
                where this.IsLink(x.Attribute("Type"))
                select x) {
                string title = (current.Attribute("Title") != null) ? current.Attribute("Title").Value : null;
                string url = (current.Attribute("Url") != null) ? current.Attribute("Url").Value : null;
                IndexNode indexNode = new IndexNode(title, url, namespaceName, helpFileNamespaceName, subKeys);
                if (current.HasElements) {
                    this.IndexFill(indexNode, xdoc, current, namespaceName, helpFileNamespaceName, filterQuery, true);
                }
                root.SubNodes.Add(indexNode);
            }
        }
        #endregion

        public string GetProperNamespaceName(string p) {
            return Namespaces.First(v => String.Equals(v.Key, p, StringComparison.CurrentCultureIgnoreCase)).Key;
        }
    }
}
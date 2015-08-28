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

using DocExplorer.Resources.HelpAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocExplorer.Resources {
    internal class TOCNode : TreeNode, ITOCNode, IIdentify {
        public string Title {
            get;
            private set;
        }
        public string Url {
            get;
            private set;
        }
        public string UrlWithNamespace {
            get { return "/" + HelpFileNamespace + "/" + Url; }
        }
        public string Id {
            get;
            private set;
        }
        public string HelpFileNamespace {
            get;
            private set;
        }
        public string Namespace {
            get;
            private set;
        }
        public int ImageId {
            get {
                return base.ImageIndex;
            }
            set {
                base.ImageIndex = value;
                base.SelectedImageIndex = value;
            }
        }
        public int SubCount {
            get {
                return base.Nodes.Count;
            }
        }
        public void AddSubNode(ITOCNode subNode) {
            base.Nodes.Add((TOCNode)subNode);
        }
        public bool ContainSubNode(string subNodeId) {
            return base.Nodes.ContainsKey(subNodeId);
        }
        public ITOCNode GetSubTOCNode(string subNodeId) {
            return (ITOCNode)base.Nodes[subNodeId];
        }
        public void InitializeTOCNode(string title, string url, string id, string namespaceName, string helpFileNamespaceName) {
            this.Title = title;
            base.Name = id;
            base.Text = title;
            this.Url = url;
            this.Id = id;
            this.Namespace = namespaceName;
            this.HelpFileNamespace = helpFileNamespaceName;
        }
    }
}

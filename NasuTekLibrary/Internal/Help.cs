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

namespace NasuTekLibrary.Internal {
    public class Help {
        public static Help Instance { get; private set; }

        List<HelpFile> _titles = new List<HelpFile>();

        public List<HelpFile> Titles
        {
            get
            {
                return this._titles;
            }
        }

        public Help() {
            Instance = this;
        }

        public HelpFile GetHelpFile(string helpFileId) {
            HelpFile helpFile = (
                from hf in Titles
                where string.Equals(hf.HelpFileId, helpFileId, StringComparison.CurrentCultureIgnoreCase)
                select hf).FirstOrDefault<HelpFile>();

            return helpFile;
        }

        public TOCNode GetTableOfContentsTree() {
            TOCNode iTOCNode = new TOCNode();
            iTOCNode.InitializeTOCNode(null, null, null, null);
 
            foreach (HelpFile helpFile in Titles) {
                XDocument[] tocs = helpFile.Tocs;
                foreach (XDocument xDocument in tocs) {
                    this.Fill(iTOCNode, xDocument, xDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC"), helpFile.HelpFileId);
                }
            }
            return iTOCNode;
        }

        private void LinkFill(TOCNode root, XDocument xdoc, XElement rootNode, string helpFileNamespace) {
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
                TOCNode iTOCNode;
                if (!root.ContainSubNode(text2)) {
                    iTOCNode = new TOCNode();
                    iTOCNode.InitializeTOCNode(text2, null, id, helpFileNamespace);
                } else {
                    iTOCNode = root.GetSubTOCNode(text2);
                }
                XDocument xDocument = XDocument.Load(new System.IO.MemoryStream(this.GetHelpFile(text3).ReadFile(filePath)));
                this.Fill(iTOCNode, xDocument, xDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOC"), helpFileNamespace);
                if (!root.ContainSubNode(text2) && iTOCNode.SubCount != 0) {
                    root.AddSubNode(iTOCNode);
                }
            }
        }

        private void Fill(TOCNode root, XDocument xdoc, XElement rootNode, string helpFileNamespace) {
            foreach (XElement current in
                from x in rootNode.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOCNode")
                where this.IsLink(x.Attribute("Type"))
                select x) {
                    string text = (current.Attribute("Title") != null) ? current.Attribute("Title").Value : null;
                    string url = (current.Attribute("Url") != null) ? current.Attribute("Url").Value : null;
                    string id = (current.Attribute("Id") != null) ? current.Attribute("Id").Value : null;
                    int num = (current.Attribute("TocImageId") != null) ? System.Convert.ToInt32(current.Attribute("TocImageId").Value) : 0;
                    this.LinkFill(root, xdoc, current, helpFileNamespace);
                    TOCNode iTOCNode;
                    if (!root.ContainSubNode(text)) {
                    iTOCNode = new TOCNode();
                        iTOCNode.InitializeTOCNode(text, url, id, helpFileNamespace);
                    } else {
                        iTOCNode = root.GetSubTOCNode(text);
                    }
                    iTOCNode.ImageId = num;
                
                if (current.HasElements) {
                    this.Fill(iTOCNode, xdoc, current, helpFileNamespace);
                } else {
                    if (num == 0) {
                        iTOCNode.ImageId = 9;
                    }
                }
                if (!root.ContainSubNode(text)) {
                    root.AddSubNode(iTOCNode);
                }
            }
        }

        private bool IsLink(XAttribute xAttribute) {
            return xAttribute == null || xAttribute.Value == "Plugin" || xAttribute.Value == "Link" || !(xAttribute.Value == "TOC");
        }
    }
}

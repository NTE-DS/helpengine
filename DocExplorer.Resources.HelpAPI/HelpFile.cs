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

using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
namespace DocExplorer.Resources.HelpAPI
{
	public class HelpFile
	{
		private readonly XElement _collInfo;
		private readonly string _filePath;
		private readonly System.Collections.Generic.List<HelpObjectFile> _files = new System.Collections.Generic.List<HelpObjectFile>();
		private readonly System.Collections.Generic.List<XDocument> _indexes = new System.Collections.Generic.List<XDocument>();
		private readonly System.Collections.Generic.List<XDocument> _tocs = new System.Collections.Generic.List<XDocument>();
		public HelpObjectFile[] Files
		{
			get
			{
				return this._files.ToArray();
			}
		}
		public XDocument[] Tocs
		{
			get
			{
				return this._tocs.ToArray();
			}
		}
		public XDocument[] Indexes
		{
			get
			{
				return this._indexes.ToArray();
			}
		}
		public string CollectionName
		{
			get
			{
				return this._collInfo.Attribute("Title").Value;
			}
		}
		public string CollectionInfoPath
		{
			get
			{
				if (this._collInfo.Attribute("CollectionInfo") == null)
				{
					return null;
				}
				return this._collInfo.Attribute("CollectionInfo").Value;
			}
		}
		public System.Version Version
		{
			get
			{
				return new System.Version((this._collInfo.Attribute("FileVersion") != null) ? this._collInfo.Attribute("FileVersion").Value : "0.0.0.0");
			}
		}
		public string Copyright
		{
			get
			{
				if (this._collInfo.Attribute("Copyright") == null)
				{
					return null;
				}
				return this._collInfo.Attribute("Copyright").Value;
			}
		}
		public string ShortDescription
		{
			get
			{
				if (this._collInfo.Attribute("ShortDescription") == null)
				{
					return null;
				}
				return this._collInfo.Attribute("ShortDescription").Value;
			}
		}
		public int LangId
		{
			get
			{
				if (this._collInfo.Attribute("LangID") == null)
				{
					return 1033;
				}
				return System.Convert.ToInt32(this._collInfo.Attribute("LangID").Value);
			}
		}
		public string FilePath
		{
			get;
			private set;
		}
		public string HelpFileId
		{
			get
			{
				return this._collInfo.Attribute("Id").Value;
			}
		}
		public HelpFileType HelpFileType
		{
			get;
			set;
		}

	    public HelpFile(string filePath) : this(filePath, false) {}

        static string GetParentUriString(Uri uri)
        {
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length);
        }

	    public HelpFile(string filePath, bool onlineHelp)
		{
            this._filePath = System.IO.Path.GetDirectoryName(filePath);
            this.FilePath = filePath;

	        if (onlineHelp) {
	            _filePath = GetParentUriString(new Uri(filePath));
	            HelpFileType = HelpFileType.WebHelp;
	            this._collInfo = XDocument.Load(filePath).Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpCollection");
	        } else {
	            switch (System.IO.Path.GetExtension(filePath).ToLower()) {
	                case ".nxc":
	                    this.HelpFileType = HelpFileType.CompiledHelp;
	                    this._collInfo = XDocument.Load(new System.IO.MemoryStream(this.ReadFile(this.ReadAttribute("HelpCollection")))).Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpCollection");
	                    break;
	                case ".nxh":
	                    this.HelpFileType = HelpFileType.LooseHelp;
	                    this._collInfo = XDocument.Load(filePath).Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpCollection");
	                    break;
	                default:
                        throw new HelpCollectionNotValidException();
	            }
	        }

            if (this._collInfo == null)
            {
                throw new HelpCollectionNotValidException();
            }

            System.Collections.Generic.IEnumerable<XElement> enumerable = this._collInfo.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}TOCDef");
            System.Collections.Generic.IEnumerable<XElement> enumerable2 = this._collInfo.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}IndexDef");

            foreach (XElement current in enumerable)
            {
                this._tocs.Add(XDocument.Load(new System.IO.MemoryStream(this.ReadFile(current.Attribute("File").Value))));
                this._files.Add(new HelpObjectFile
                {
                    FileName = current.Attribute("File").Value,
                    ObjectType = HelpObjectFile.Type.TOC
                });
            }
            foreach (XElement current2 in enumerable2)
            {
                this._indexes.Add(XDocument.Load(new System.IO.MemoryStream(this.ReadFile(current2.Attribute("File").Value))));
                this._files.Add(new HelpObjectFile
                {
                    FileName = current2.Attribute("File").Value,
                    ObjectType = HelpObjectFile.Type.Index
                });
            }
			
		}

	    public string ReadAttribute(string attribute) {
	        switch (HelpFileType) {
	            case HelpFileType.CompiledHelp: {
	                byte[] array = this.ReadFile(System.IO.Path.Combine("$Attributes", attribute));
	                if (array == null) {
	                    return "";
	                }
	                return System.Text.Encoding.ASCII.GetString(array);
	            }
	            case HelpFileType.WebHelp:
                    //TODO: WebHelp
	                return null;
	            default:
	                throw new HelpFileInstanceNotACompiledHelpException();
	        }
	    }

	    public byte[] ReadFile(string filePath)
		{
			string name = filePath.Replace('\\', '/');
			byte[] result;

	        switch (HelpFileType) {
	            case HelpFileType.CompiledHelp: {
	                ZipFile zipFile = new ZipFile(this.FilePath);
	                ZipEntry entry = zipFile.GetEntry(name);
	                if (entry == null) {
	                    zipFile.Close();
	                    return null;
	                }
	                using (System.IO.Stream inputStream = zipFile.GetInputStream(entry)) {
	                    byte[] array = new byte[(int) entry.Size];
	                    inputStream.Read(array, 0, (int) entry.Size);
	                    zipFile.Close();
	                    result = array;
	                }
	                return result;
	            }
	            case HelpFileType.WebHelp:
	                result = new System.Net.WebClient().DownloadData(System.IO.Path.Combine(this._filePath, filePath).Replace('\\', '/'));
	                return result;
	            default: {
	                try {
	                    result = System.IO.File.ReadAllBytes(System.IO.Path.Combine(this._filePath, filePath));
	                    return result;
	                } catch (System.IO.FileNotFoundException) {
	                    result = null;
	                    return result;
	                }
	            }
	        }
		}
		public bool DecompileHelpFile(string folderPath)
		{
			if (this.HelpFileType != HelpFileType.CompiledHelp)
			{
				throw new HelpFileInstanceNotACompiledHelpException();
			}
			bool result;
			try
			{
				new FastZip().ExtractZip(this.FilePath, folderPath, null);
				result = true;
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		public void PopulateTreeNodeWithHelpFilesystem(TreeNode node)
		{
			if (this.HelpFileType != HelpFileType.CompiledHelp)
			{
				throw new HelpFileInstanceNotACompiledHelpException();
			}
			node.Nodes.Clear();
			ZipFile zipFile = new ZipFile(this.FilePath);
			foreach (ZipEntry zipEntry in zipFile)
			{
				this.AddTreeNode(zipEntry.Name, node);
			}
			zipFile.Close();
		}
		private TreeNode AddTreeNode(string name, TreeNode treeNode)
		{
			if (name.EndsWith("/"))
			{
				name = name.Substring(0, name.Length - 1);
			}
			TreeNode treeNode2 = this.FindNodeForTag(name, treeNode.Nodes);
			if (treeNode2 != null)
			{
				return treeNode2;
			}
			string directoryName = System.IO.Path.GetDirectoryName(name);
			TreeNodeCollection nodes;
			if (string.IsNullOrEmpty(directoryName))
			{
				nodes = treeNode.Nodes;
			}
			else
			{
				TreeNode treeNode3 = this.AddTreeNode(directoryName.Replace("\\", "/"), treeNode);
				nodes = treeNode3.Nodes;
				treeNode3.ImageIndex = this.GetImageIndex(null, true);
				treeNode3.SelectedImageIndex = this.GetImageIndex(null, true);
			}
			treeNode2 = new TreeNode
			{
				Text = System.IO.Path.GetFileName(name),
				Tag = new Tuple<string, byte[]>(name, this.ReadFile(name)),
				ImageIndex = this.GetImageIndex(name, false),
				SelectedImageIndex = this.GetImageIndex(name, false)
			};
			nodes.Add(treeNode2);
			return treeNode2;
		}
		private int GetImageIndex(string name, bool p)
		{
			if (p)
			{
				return 14;
			}
			if (name.Contains("$"))
			{
				return 50;
			}
			string key;
			switch (key = System.IO.Path.GetExtension(name).ToLower())
			{
			case ".nxh":
				return 38;
			case ".nxt":
				return 2;
			case ".nxk":
				return 56;
			case ".xml":
				return 42;
			case ".js":
				return 53;
			case ".htm":
			case ".html":
				return 36;
			case ".jpg":
			case ".jpeg":
			case ".gif":
			case ".png":
				return 53;
			}
			return 20;
		}
		private TreeNode FindNodeForTag(string name, TreeNodeCollection nodes)
		{
			foreach (TreeNode treeNode in nodes)
			{
				if (name == ((Tuple<string, byte[]>)treeNode.Tag).Item1)
				{
					TreeNode result = treeNode;
					return result;
				}
				if (name.StartsWith(((Tuple<string, byte[]>)treeNode.Tag).Item1 + "/"))
				{
					TreeNode result = this.FindNodeForTag(name, treeNode.Nodes);
					return result;
				}
			}
			return null;
		}
	}
}

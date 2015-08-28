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
using System.Xml.Linq;
namespace NasuTekLibrary.Internal
{
    public class HelpFile
    {
        private readonly XElement _collInfo;
        private readonly string _filePath;
        private readonly System.Collections.Generic.List<XDocument> _tocs = new System.Collections.Generic.List<XDocument>();

        public XDocument[] Tocs
        {
            get
            {
                return this._tocs.ToArray();
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

        public HelpFile(string filePath) : this(filePath, false) { }

        static string GetParentUriString(Uri uri)
        {
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length);
        }

        public HelpFile(string filePath, bool onlineHelp)
        {
            this._filePath = System.IO.Path.GetDirectoryName(filePath);
            this.FilePath = filePath;


            this._collInfo = XDocument.Load(new System.IO.MemoryStream(this.ReadFile(this.ReadAttribute("HelpCollection")))).Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpCollection");

            if (this._collInfo == null)
            {
                throw new HelpCollectionNotValidException();
            }

            System.Collections.Generic.IEnumerable<XElement> enumerable = this._collInfo.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}TOCDef");

            foreach (XElement current in enumerable)
            {
                this._tocs.Add(XDocument.Load(new System.IO.MemoryStream(this.ReadFile(current.Attribute("File").Value))));
            }
        }

        public string ReadAttribute(string attribute)
        {
            byte[] array = this.ReadFile(System.IO.Path.Combine("$Attributes", attribute));
            if (array == null)
            {
                return "";
            }
            return System.Text.Encoding.ASCII.GetString(array);
        }

        public byte[] ReadFile(string filePath)
        {
            string name = filePath.Replace('\\', '/');
            byte[] result;

            ZipFile zipFile = new ZipFile(this.FilePath);
            ZipEntry entry = zipFile.GetEntry(name);
            if (entry == null)
            {
                zipFile.Close();
                return null;
            }
            using (System.IO.Stream inputStream = zipFile.GetInputStream(entry))
            {
                byte[] array = new byte[(int)entry.Size];
                inputStream.Read(array, 0, (int)entry.Size);
                zipFile.Close();
                result = array;
            }
            return result;
        }
    }
}
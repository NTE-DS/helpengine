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
using System.Xml.Serialization;
namespace DocExplorer.Resources.HelpAPI
{
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }

	[System.Serializable]
	public class Settings
	{
		public static readonly string Path;

		public SerializableDictionary<string, FavoriteFolder> RootFavorites
		{
			get;
			set;
		}
		static Settings()
		{
			Settings.Path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "NasuTek Document Explorer");
		}
		public Settings()
		{
            this.RootFavorites = new SerializableDictionary<string, FavoriteFolder>();
		}
		public static void OpenSettings()
		{
			if (System.IO.File.Exists(System.IO.Path.Combine(Settings.Path, "Settings.NxSD")))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
				using (System.IO.FileStream fileStream = System.IO.File.Open(System.IO.Path.Combine(Settings.Path, "Settings.NxSD"), System.IO.FileMode.Open))
				{
					Help.Instance.SettingsInstance = (Settings)xmlSerializer.Deserialize(fileStream);
					return;
				}
			}
            Help.Instance.SettingsInstance = new Settings();
		}
		public static void SaveSettings()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
			if (!System.IO.Directory.Exists(Settings.Path))
			{
				System.IO.Directory.CreateDirectory(Settings.Path);
			}
            using (System.IO.FileStream fileStream = System.IO.File.Open(System.IO.Path.Combine(Settings.Path, "Settings.NxSD"), System.IO.FileMode.Create))
			{
                xmlSerializer.Serialize(fileStream, Help.Instance.SettingsInstance);
			}
		}
	}
}

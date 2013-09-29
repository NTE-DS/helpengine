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
using System.Linq;
using System.Xml.Linq;
namespace DocExplorer.Resources.HelpAPI
{
	internal class SearchLocalHelp : ISearch
	{
		public string Name
		{
			get
			{
				return "Local Help";
			}
		}
		public void Search(System.Collections.Generic.List<SearchEngine.SearchItem> items, string term, Help instance)
		{
            foreach (var namespaceObj in instance.Namespaces) {
                foreach (SearchEngine.SearchItem current in
                    from helpFile in namespaceObj.Value.Titles
                    from xDocument in helpFile.Tocs
                    from searchItem in
                        from descendant in xDocument.Descendants("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpTOCNode")
                        where descendant.Attribute("Title").Value.Contains(term)
                        let title = descendant.Attribute("Title").Value
                        let url = descendant.Attribute("Url").Value
                        where !SearchLocalHelp.UrlNamespaceExist(url, namespaceObj.Key, helpFile.HelpFileId, items)
                        select new SearchEngine.SearchItem {
                            Title = title,
                            Url = url,
                            Namespace = namespaceObj.Key,
                            HelpFileNamespace = helpFile.HelpFileId,
                            Source = helpFile.CollectionName
                        }
                    select searchItem) {
                    items.Add(current);
                }
            }
		}
		private static bool UrlNamespaceExist(string url, string namespaceName, string helpFileNamespaceName, System.Collections.Generic.IEnumerable<SearchEngine.SearchItem> items)
		{
			SearchEngine.SearchItem searchItem = (
				from v in items
                where v.Url == url && v.HelpFileNamespace == helpFileNamespaceName && v.Namespace == namespaceName
				select v).FirstOrDefault<SearchEngine.SearchItem>();
			return searchItem != null;
		}
	}
}

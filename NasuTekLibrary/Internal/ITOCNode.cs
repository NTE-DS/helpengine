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

namespace NasuTekLibrary.Internal
{
	public class TOCNode
	{
        List<TOCNode> nodes;

        public TOCNode[] SubNodes { get { return nodes.ToArray(); } }

        public TOCNode()
        {
            nodes = new List<TOCNode>();
        }

		public string Title
		{
			get;
            private set;
		}
        public int SubCount
		{
			get;
            private set;
        }
        public int ImageId
		{
			get;
			set;
		}
        public string Url
        {
            get;
            private set;
        }
        public string Id
        {
            get;
            private set;
        }

        public string HelpFileNamespace
        {
            get;
            private set;
        }

        public void AddSubNode(TOCNode subNode)
        {
            nodes.Add(subNode);
        }
        public bool ContainSubNode(string subNodeTitle)
        {
            return nodes.Any(v => v.Title == subNodeTitle);
        }
        public TOCNode GetSubTOCNode(string subNodeTitle)
        {
            return nodes.FirstOrDefault(v => v.Title == subNodeTitle);
        }
        public void InitializeTOCNode(string title, string url, string id, string helpFileNamespace)
        {
            Title = title;
            Url = url;
            Id = id;
            HelpFileNamespace = helpFileNamespace;
        }
	}
}

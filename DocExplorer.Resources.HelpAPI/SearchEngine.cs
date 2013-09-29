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
namespace DocExplorer.Resources.HelpAPI
{
	public class SearchEngine
	{
		public class SearchItem
		{
			public string Title
			{
				get;
				set;
			}
			public string Description
			{
				get;
				set;
			}
			public string Source
			{
				get;
				set;
			}
			public string Url
			{
				get;
				set;
			}
            public string Namespace {
                get;
                set;
            }
			public string HelpFileNamespace
			{
				get;
				set;
			}
		}
		public System.Collections.Generic.List<ISearch> SearchProviders
		{
			get;
			private set;
		}
		internal SearchEngine()
		{
			this.SearchProviders = new System.Collections.Generic.List<ISearch>();
		}
	}
}

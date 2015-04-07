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
namespace DocExplorer.Resources.HelpAPI
{
	public class Filter
	{
		public string FilterName
		{
			get;
			private set;
		}
		public System.Collections.Generic.List<FilterParam> Params
		{
			get;
			private set;
		}
		public Filter()
		{
			this.Params = new System.Collections.Generic.List<FilterParam>();
		}
		public Filter(string filterName, string filterParams)
		{
			this.FilterName = filterName;
			this.Params = new System.Collections.Generic.List<FilterParam>();
			foreach (FilterParam current in 
				from s in filterParams.Split(new char[]
				{
					';'
				})
				select s.Split(new char[]
				{
					'='
				}) into spl
				select new FilterParam
				{
					Key = spl[0],
					Value = spl[1]
				})
			{
				this.Params.Add(current);
			}
		}
		public string FilterString()
		{
			return this.Params.Aggregate("", (string current, FilterParam param) => string.Concat(new string[]
			{
				current,
				";",
				param.Key,
				"=",
				param.Value
			})).Substring(1);
		}
	}
}

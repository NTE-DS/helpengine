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
	[System.Serializable]
	public class FavoriteFolder
	{
		public System.Collections.Generic.List<FavoriteFolder> SubFolders
		{
			get;
			set;
		}
		public System.Collections.Generic.List<Favorite> Favorites
		{
			get;
			set;
		}
		public string Title
		{
			get;
			set;
		}
		public FavoriteFolder()
		{
			this.SubFolders = new System.Collections.Generic.List<FavoriteFolder>();
			this.Favorites = new System.Collections.Generic.List<Favorite>();
		}
	}
}

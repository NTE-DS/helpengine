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
using System.Drawing;
namespace DocExplorer.Resources.HelpAPI
{
	public class NodeIconServices
	{
		public static Image[] Icons = new Bitmap[]
		{
			NodeIcons.icon_1,
			NodeIcons.icon_1,
			NodeIcons.icon_2,
			NodeIcons.icon_3,
			NodeIcons.icon_4,
			NodeIcons.icon_5,
			NodeIcons.icon_6,
			NodeIcons.icon_7,
			NodeIcons.icon_8,
			NodeIcons.icon_9,
			NodeIcons.icon_10,
			NodeIcons.icon_11,
			NodeIcons.icon_12,
			NodeIcons.icon_13,
			NodeIcons.icon_14,
			NodeIcons.icon_15,
			NodeIcons.icon_16,
			NodeIcons.icon_17,
			NodeIcons.icon_18,
			NodeIcons.icon_19,
			NodeIcons.icon_20,
			NodeIcons.icon_21,
			NodeIcons.icon_22,
			NodeIcons.icon_23,
			NodeIcons.icon_24,
			NodeIcons.icon_25,
			NodeIcons.icon_26,
			NodeIcons.icon_27,
			NodeIcons.icon_28,
			NodeIcons.icon_29,
			NodeIcons.icon_30,
			NodeIcons.icon_31,
			NodeIcons.icon_32,
			NodeIcons.icon_33,
			NodeIcons.icon_34,
			NodeIcons.icon_35,
			NodeIcons.icon_36,
			NodeIcons.icon_37,
			NodeIcons.icon_38
		};
		public static Image ReturnImageFromIndexKey(int index)
		{
			if (NodeIconServices.Icons.Length > index)
			{
				return NodeIconServices.Icons[0];
			}
			return NodeIconServices.Icons[index];
		}
	}
}

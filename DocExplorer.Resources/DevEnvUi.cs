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

using DocExplorer.Resources.HelpAPI;
using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocExplorer.Resources {
    public class DevEnvUi : IHelpUi {
        public System.Type TocTypeGenerator {
            get {
                return typeof(TOCNode);
            }
        }

        public void SetActiveCollection() {
            ((Contents)DevEnvObj.Instance.Extensibility.GetPane("Contents")).Unregister();
            ((Index)DevEnvObj.Instance.Extensibility.GetPane("Index")).Unregister();
            ((Favorites)DevEnvObj.Instance.Extensibility.GetPane("Favorites")).Unregister();

            if (HelpAPI.Help.Instance.ActiveNamespace == null) {
                DevEnvObj.Instance.WorkspaceEnvironment.Text = "NasuTek Document Explorer";
                return;
            }
            DevEnvObj.Instance.WorkspaceEnvironment.Text = "NasuTek Document Explorer" + " - " + HelpAPI.Help.Instance.ActiveNamespace.Title;
        }

        public void RefreshFilters() {
            ((Contents)DevEnvObj.Instance.Extensibility.GetPane("Contents")).RefreshFilters();
            ((Index)DevEnvObj.Instance.Extensibility.GetPane("Index")).RefreshFilters();
            ((Favorites)DevEnvObj.Instance.Extensibility.GetPane("Favorites")).RefreshNodes();
        }
    }

    internal class DevEnvObj
    {
        public static DevEnv Instance
        {
            get
            {
                return ((DevEnv)DevEnvSvc.GetService(DevEnvSvc.DevEnvObject));
            }
        }
    }
}

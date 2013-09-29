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
using NasuTek.DevEnvironment.Resources;
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
            ((Contents)DevEnv.Instance.WorkspaceEnvironment.GetPane("Contents")).Unregister();
            ((Index)DevEnv.Instance.WorkspaceEnvironment.GetPane("Index")).Unregister();
            ((Favorites)DevEnv.Instance.WorkspaceEnvironment.GetPane("Favorites")).Unregister();

            if (HelpAPI.Help.Instance.ActiveNamespace == null) {
                DevEnv.Instance.WorkspaceEnvironment.Text = "NasuTek Document Explorer";
                return;
            }
            DevEnv.Instance.WorkspaceEnvironment.Text = "NasuTek Document Explorer" + " - " + HelpAPI.Help.Instance.ActiveNamespace.Title;
        }

        public void RefreshFilters() {
            ((Contents)DevEnv.Instance.WorkspaceEnvironment.GetPane("Contents")).RefreshFilters();
            ((Index)DevEnv.Instance.WorkspaceEnvironment.GetPane("Index")).RefreshFilters();
            ((Favorites)DevEnv.Instance.WorkspaceEnvironment.GetPane("Favorites")).RefreshNodes();
        }
    }
}

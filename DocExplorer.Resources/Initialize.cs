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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Documents;
using NasuTek.DevEnvironment.Extensibility.Addins;

namespace DocExplorer.Resources
{
    public class Initialize : ICommand
    {
        public object Owner { get; set; }

        public void Run()
        {
            DevEnv.Instance.WorkspaceEnvironment.ShowPane(new WebBrowserDocument());

            var help = new HelpAPI.Help {HelpUi = new DevEnvUi()};
            help.LoadNamespaces(DevEnv.Instance.DevEnvArguments["LoadLocalCollection"] != "true" ? DocExplorer.Resources.HelpAPI.Help.GetRegisteredCollection(String.IsNullOrEmpty(DevEnv.Instance.DevEnvArguments["Collection"]) ? "DefaultCollection" : DevEnv.Instance.DevEnvArguments["Collection"]) : DevEnv.Instance.DevEnvArguments["Collection"]);
            help.ActiveNamespace = help.GetNamespace(String.IsNullOrEmpty(DevEnv.Instance.DevEnvArguments["Namespace"]) ? "NasuTek.Default.CC" : DevEnv.Instance.DevEnvArguments["Namespace"]);

            DevEnv.Instance.WorkspaceEnvironment.GetPane("Contents").Show();
        }

        public event EventHandler OwnerChanged;
    }

    public class SaveSettings : ICommand
    {
        public object Owner { get; set; }

        public void Run() {
            HelpAPI.Settings.SaveSettings();
        }

        public event EventHandler OwnerChanged;
    }

}

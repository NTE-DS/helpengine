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
using NasuTek.DevEnvironment.Extendability;

namespace DocExplorer.Resources
{
    public class Initialize : AbstractCommand
    {
        public override void Run()
        {
            DevEnvObj.Instance.WorkspaceEnvironment.ShowPane(new WebBrowserDocument());

            var help = new HelpAPI.Help {HelpUi = new DevEnvUi()};
            help.LoadNamespaces(DevEnvObj.Instance.DevEnvArguments["LoadLocalCollection"] != "true" ? DocExplorer.Resources.HelpAPI.Help.GetRegisteredCollection(String.IsNullOrEmpty(DevEnvObj.Instance.DevEnvArguments["Collection"]) ? "DefaultCollection" : DevEnvObj.Instance.DevEnvArguments["Collection"]) : DevEnvObj.Instance.DevEnvArguments["Collection"]);
            help.ActiveNamespace = help.GetNamespace(String.IsNullOrEmpty(DevEnvObj.Instance.DevEnvArguments["Namespace"]) ? "NasuTek.Default.CC" : DevEnvObj.Instance.DevEnvArguments["Namespace"]);
        }
    }

    public class SaveSettings : AbstractCommand
    {
        public override void Run() {
            HelpAPI.Settings.SaveSettings();
        }
    }

}

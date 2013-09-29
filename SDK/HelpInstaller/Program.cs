﻿/***************************************************************************************************
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using HelpCompiler;
using Microsoft.Win32;
using HelpInstallerAPI = DocExplorer.Resources.HelpAPI.HelpInstaller;

namespace HelpInstaller {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                MessageBox.Show("This application must be launched by the Collection Manager. It is not ment to be ran as a standalone.", "NasuTek Help 5 Collection Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var arg = new Arguments(args);

            if (arg["CreateRegCollectionKey"] == "true")
            {
                HelpInstallerAPI.CreateRegCollectionKey(arg["CollectionName"], arg["CollectionPath"]);
            }
            else if (arg["CreateNamespaceCollection"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.CreateCollection();
            }
            else if (arg["CreateNamespace"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.CreateNamespace(arg["NamespaceID"], arg["FriendlyName"], arg["CombinedCollection"]);
            }
            else if (arg["DeleteNamespace"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.DeleteNamespace(arg["NamespaceID"]);
            }
            else if (arg["InstallHelp"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.InstallBook(arg["BookID"], arg["BookFilePath"]);
            }
            else if (arg["UninstallHelp"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.UninstallBook(arg["BookID"]);
            }
            else if (arg["LinkBook"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.LinkBook(arg["BookID"], arg["NamespaceID"]);
            }
            else if (arg["UnlinkBook"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.UnlinkBook(arg["BookID"], arg["NamespaceID"]);
            }
            else if (arg["AddPlugin"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.AddPlugin(arg["NamespaceIDToPlugin"], arg["NamespaceID"]);
            }
        }
    }
}

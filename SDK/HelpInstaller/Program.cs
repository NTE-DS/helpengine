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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using HelpCompiler;
using Microsoft.Win32;
using HelpInstallerAPI = DocExplorer.Resources.HelpAPI.HelpInstaller;
using System.Runtime.InteropServices;

namespace HelpInstaller {
    class Program {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        static void Main(string[] args) {
            if(args.Length == 0) {
                MessageBox.Show("This application must be launched by the Collection Manager. It is not ment to be ran as a standalone.", "NasuTek Help 5 Collection Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var arg = new Arguments(args);

            if (arg["?"] == "true")
            {
                AllocConsole();

                Console.WriteLine(@"NasuTek Document Explorer Collection Management Utility
Copyright (C) 2008-2015 NasuTek Enterprises

Collection Registration:
/CreateRegCollectionKey [/CollectionName:<Collection Name>
    /CollectionPath:<Path to Collection>]
        Registers the Collection to the global collection. If CollectionName 
        and CollectionPath are not specified, then it will create the 
        global collection registry key.

/CreateNamespaceCollection /CollectionPath:<Path>
        Creates a Namespace Collection

/Install /InstallPath:<Path to Help>
        Generates the default NasuTek Document Explorer Collection, and runs
        functions needed after installation.

/Uninstall
        Removes the NasuTek Help Registry Entry.

Namespace Management:
/CreateNamespace /CollectionPath:<Path> /NamespaceID:<ID>
    /FriendlyName:<Friendly Name> [/CombinedCollection]
        Creates a new namespace in the namespace collection with the
        following ID and Name. If CombinedCollection is specified
        it will create the namespace as a combined collection.

/DeleteNamespace /CollectionPath:<Path> /NamespaceID:<ID>
        Deletes the Namespace with the following Namespace ID

/ManageGUI /CollectionPath:<Path> /NamespaceID:<ID>
        Opens the Management UI to show installed books and books to
        download.

Help File Management:
/InstallHelp /CollectionPath:<Path> /BookID:<ID> /BookFilePath:<Path>
        Installs the following help file from the path specified in
        BookFilePath with the following ID.

/InstallWebHelp /CollectionPath:<Path> /HelpListUri:<Url>
    /BooksToInstall:<Comma list of books to install>
        Downloads and installs the books from the internet. Automatically
        installs the books to the Help Collection.

/UninstallHelp /CollectionPath:<Path> /BookID:<ID>
        Uninstalls the following help file with the following ID.

/LinkBook /CollectionPath:<Path> /BookID:<ID> /NamespaceID:<ID>
        Links the following Book ID specified in BookID to the
        namespace specified in NamespaceID

/UnlinkBook /CollectionPath:<Path> /BookID:<ID> /NamespaceID:<ID>
        Unlinks the following Book ID specified in BookID to the
        namespace specified in NamespaceID

Namespace Plugin Management:
/AddPlugin /CollectionPath:<Path> /NamespaceIDToPlugin:<ID> 
    /NamespaceID:<ID>
        Plugs in the namespace specified in NamespaceIDToPlugin to
        the namespace specified in NamespaceID

/RemovePlugin /CollectionPath:<Path> /NamespaceIDToPlugin:<ID> 
    /NamespaceID:<ID>
        Removes the plugin specified in NamespaceIDToPlugin in the
        namespace specified in NamespaceID

");

                Console.WriteLine("Press Any Key to Continue ...");
                Console.ReadKey();


                FreeConsole();
            }
            else if (arg["CreateRegCollectionKey"] == "true")
            {
                HelpInstallerAPI.CreateRegCollectionKey(arg["CollectionName"], arg["CollectionPath"]);
            }
            else if (arg["Uninstall"] == "true")
            {
                HelpInstallerAPI.Uninstall();
            }
            else if (arg["Install"] == "true")
            {
                HelpInstallerAPI.Install(Path.GetFullPath(arg["InstallPath"]));
            }
            else if (arg["CreateNamespaceCollection"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.CreateCollection();
            }
            else if (arg["CreateNamespace"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.CreateNamespace(arg["NamespaceID"], arg["FriendlyName"], Convert.ToBoolean(arg["CombinedCollection"]));
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
            else if (arg["RemovePlugin"] == "true")
            {
                var helpInstaller = new HelpInstallerAPI(arg["CollectionPath"]);
                helpInstaller.RemovePlugin(arg["NamespaceIDToPlugin"], arg["NamespaceID"]);
            }
        }
    }
}
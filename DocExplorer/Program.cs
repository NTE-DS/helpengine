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

using DocExplorer.Resources;
using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Extensibility;
using NasuTek.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocExplorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            var arguments = new Arguments(args);

            if (arguments["SimpleUIMode"] == "true") {
                SimpleUI.SimpleUIProgram.LaunchSimpleUI(args);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var settings = new DevEnvSettings
            {
                ProductName = "NasuTek Document Explorer",
                ProductVersionCodebase = new Version(HelpVersion.CodebaseVersion),
                ProductVersionRelease = new Version(HelpVersion.ReleaseVersion),
                ProductBuildStage = HelpVersion.BuildStage,
                ProductBuildLab = HelpVersion.BuildLab,
                ProductCopyrightYear = "2008",
                WindowIcon = Properties.Resources.ProductIcon,
                ProductID = "Help",
            };
            
            if (arguments["LaunchedByDLL"] == "true")
            {
                settings.ShowIDEOnStartup = false;
                settings.ExitThreadOnIDEExit = false;
            }

            DevEnvSvc.InitializeDevEnv(settings, args, false);
        }
    }
}

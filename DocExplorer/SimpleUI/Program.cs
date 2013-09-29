using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Resources;

namespace DocExplorer.SimpleUI {
    static class SimpleUIProgram {
        public static SimpleUIForm MainForm { get; private set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void LaunchSimpleUI(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new SimpleUIForm();

            DocExplorer.Resources.HelpAPI.Help.RegisterNteHelpProtocol();

            var arg = new Arguments(args);

            var helpEngine = new DocExplorer.Resources.HelpAPI.Help {HelpUi = new HelpUi()};
            helpEngine.LoadNamespaces(arg["LoadLocalCollection"] != "true" ? DocExplorer.Resources.HelpAPI.Help.GetRegisteredCollection(String.IsNullOrEmpty(arg["Collection"]) ? "DefaultCollection" : arg["Collection"]) : arg["Collection"]);
            helpEngine.ActiveNamespace = helpEngine.GetNamespace(String.IsNullOrEmpty(arg["Namespace"]) ? "NasuTek.Default.CC" : arg["Namespace"]);

            Application.Run(MainForm);
        }
    }
}

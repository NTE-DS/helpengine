using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NasuTek.Help
{
    public class HelpEngine {
        private AppDomain m_HelpAppDomain;
        private IHelp m_Interface;

        public HelpEngine(string[] args) {
            var lst = args.ToList();
#if DEBUG
            var regPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.0-Debug");
#else
            var regPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.0");
#endif

            var installPath = (string)regPath.GetValue("InstallDir");

            m_HelpAppDomain = AppDomain.CreateDomain("Help5AppDomain");
            lst.Add("/LaunchedByDLL");
            var thred = new Thread(new ThreadStart(() => m_HelpAppDomain.ExecuteAssembly(Path.Combine(installPath, "DocExplorer.exe"), lst.ToArray())));
            thred.SetApartmentState(ApartmentState.STA);
            thred.Start();
        }

        public void ShowHelpWindow()
        {
            foreach (var i in m_HelpAppDomain.GetAssemblies())
                MessageBox.Show(i.FullName);
            m_Interface.ShowWindow();
        }

        public void HideHelpWindow()
        {
            m_Interface.HideWindow();
        }

        public void F1Execute(string f1Query)
        {
            m_Interface.F1Execute(f1Query);
        }

        public void DisposeHelp()
        {
            AppDomain.Unload(m_HelpAppDomain);
        }

        private void NoHelpInstalled() {
            MessageBox.Show("NasuTek Document Explorer is not installed. The NasuTek Document Explorer runtime is required to view help.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

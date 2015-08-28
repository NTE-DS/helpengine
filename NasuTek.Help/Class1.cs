using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.Help
{
    public class HelpEngine {
        private AppDomain m_HelpAppDomain;

        public HelpEngine() {
            var regPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.0");
            var installPath = (string)regPath.GetValue("InstallDir");

            m_HelpAppDomain = AppDomain.CreateDomain("Help5AppDomain");

            m_HelpAppDomain.Load(File.ReadAllBytes(Path.Combine(installPath, "Common8", "Help5", "DocExplorer.exe")));
        }

        private void NoHelpInstalled() {
            MessageBox.Show("NasuTek Document Explorer is not installed. The NasuTek Document Explorer runtime is required to view help.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

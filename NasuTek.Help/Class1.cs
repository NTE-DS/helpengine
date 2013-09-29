using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.Help
{
    public class HelpEngine {
        private AppDomain m_HelpAppDomain;

        public HelpEngine() {
            
        }

        private void NoHelpInstalled() {
            MessageBox.Show("NasuTek Document Explorer is not installed. The NasuTek Document Explorer runtime is required to view help.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

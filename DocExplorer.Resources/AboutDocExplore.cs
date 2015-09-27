using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Extendability;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocExplorer.Resources {
    public partial class AboutDocExplore : Form {
        public AboutDocExplore() {
            InitializeComponent();
            var devEnv = (DevEnv)DevEnvSvc.GetService(DevEnvSvc.DevEnvObject);

            label1.Text = String.Format(label1.Text, devEnv.ProductVersionRelease.ToString(2), devEnv.ProductVersionCodebase + "-" + devEnv.ProductBuildStage + " (" + devEnv.ProductBuildLab + ")", DevEnvVersion.FullVersion + " (" + DevEnvVersion.BuildLab + ")");
            label7.Text = String.Format("{0}\n{1}\nSerial: {2}", HelpAPI.Help.Instance.ActiveNamespace.RegisteredUser, HelpAPI.Help.Instance.ActiveNamespace.RegisteredCompany, HelpAPI.Help.Instance.ActiveNamespace.RegisteredSerial);
            webBrowser1.Navigate(DocExplorer.Resources.HelpAPI.Help.Instance.ActiveNamespace.HelpDisplayCollectionInfoPath);
            webBrowser2.Navigate(DocExplorer.Resources.HelpAPI.Help.Instance.ActiveNamespace.HelpDisplayLogoPath);
        }
    }
}

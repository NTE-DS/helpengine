using NasuTek.DevEnvironment;
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
            label1.Text = String.Format(label1.Text, DevEnv.Instance.ProductVersionRelease.ToString(2), DevEnv.Instance.ProductVersionCodebase + "-" + DevEnv.Instance.ProductBuildStage + " (" + DevEnv.Instance.ProductBuildLab + ")", DevEnvVersion.FullVersion + " (" + DevEnvVersion.BuildLab + ")");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpSimple;

namespace DocExplorer.SimpleUI {
    public partial class SimpleUIForm : Form {
        public SimpleUIForm() {
            InitializeComponent();
        }

        public void RefreshTOC() {
            this.treeView1.Nodes.Clear();
            TOCNode tOCNode = (TOCNode)DocExplorer.Resources.HelpAPI.Help.Instance.GetTableOfContentsTree("(no filter)");
            this.treeView1.Nodes.AddRange(tOCNode.Nodes.Cast<TreeNode>().ToArray<TreeNode>());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            TOCNode tOCNode = e.Node as TOCNode;
            if (tOCNode != null) {
                if (tOCNode.Url != null) {
                    webBrowser1.Navigate(DocExplorer.Resources.HelpAPI.Help.Instance.GetUriFromTOCNode(tOCNode));
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e) {
            if (!splitContainer1.Panel1Collapsed) {
                var subtract = splitContainer1.Panel1.ClientSize.Width + splitContainer1.SplitterWidth;
                splitContainer1.Panel1Collapsed = true;
                ClientSize = new Size(ClientSize.Width - subtract, ClientSize.Height);
            } else {
                splitContainer1.Panel1Collapsed = false;
                var add = splitContainer1.Panel1.ClientSize.Width + splitContainer1.SplitterWidth;
                ClientSize = new Size(ClientSize.Width + add, ClientSize.Height);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}

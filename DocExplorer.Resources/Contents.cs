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

using DocExplorer.Resources.HelpAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Documents;
using NasuTek.DevEnvironment.Extensibility.Workbench;
namespace DocExplorer.Resources
{
	public class Contents : DevEnvPane
	{
		private IContainer components;
		private Panel panel1;
		private ComboBox comboBox1;
		private Label label1;
		private TreeView treeView1;
		private ImageList imageList1;
	    private bool m_SkipSelectionCode;

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 34);
            this.panel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(284, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "F&iltered by:";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 34);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(284, 228);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Contents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "Contents";
            this.TabText = "Contents";
            this.Text = "Contents";
            this.Load += new System.EventHandler(this.Contents_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		public Contents()
		{
			this.InitializeComponent();
			this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList1.Images.AddRange(NodeIconServices.Icons);
		}
		internal void RefreshFilters()
		{
			this.comboBox1.Items.Clear();
			this.comboBox1.Items.Add("(no filter)");
			Filter[] filters = HelpAPI.Help.Instance.ActiveNamespace.Filters;
			for (int i = 0; i < filters.Length; i++)
			{
				Filter filter = filters[i];
				this.comboBox1.Items.Add(filter.FilterName);
			}
			this.comboBox1.Text = "(no filter)";
		}
		internal void ChangeToc(string filter)
		{
			this.treeView1.Nodes.Clear();
            TOCNode tOCNode = (TOCNode)HelpAPI.Help.Instance.GetTableOfContentsTree(filter);
			this.treeView1.Nodes.AddRange(tOCNode.Nodes.Cast<TreeNode>().ToArray<TreeNode>());
		}

	    public void SelectNode(Uri pageUri) {
	        this.SelectNode(pageUri, TopicSelector.Sync);
	    }

	    public void SelectNode(Uri pageUri, TopicSelector topicSelector) {
	        switch (topicSelector) {
	            case TopicSelector.Sync:
	                SyncNode(treeView1.Nodes, pageUri, true);
	                break;
	            case TopicSelector.Next:
	                SelectNode(GetCurrentTopicNode(treeView1.Nodes, pageUri), TopicSelector.Next);
	                break;
	            case TopicSelector.Previous:
	                SelectNode(GetCurrentTopicNode(treeView1.Nodes, pageUri), TopicSelector.Previous);
	                break;
	        }
	    }

	    private void SelectNode(TreeNode node, TopicSelector topicSelector) {
	        // Determine which TreeNode to select. 
	        switch (topicSelector) {
	            case TopicSelector.Previous:
	                node.TreeView.SelectedNode = node.PrevNode;
	                break;
	            case TopicSelector.Next:
	                node.TreeView.SelectedNode = node.NextNode;
	                break;
	        }
	    }

        private TOCNode GetCurrentTopicNode(TreeNodeCollection node, Uri pageUri) {
            if (node.Cast<TOCNode>().Any(secNode => secNode.UrlWithNamespace == pageUri.AbsolutePath)) {
                return (TOCNode) treeView1.SelectedNode;
            }

            return (TOCNode) treeView1.Nodes[0];
        }

	    private void SyncNode(TreeNodeCollection node, Uri pageUri, bool skipSelectionCode) {
	        foreach (TOCNode secNode in node) {
	            if (secNode.UrlWithNamespace == pageUri.AbsolutePath) {
	                m_SkipSelectionCode = skipSelectionCode;
	                treeView1.SelectedNode = secNode;
	                break;
	            }

	            if (secNode.Nodes.Count > 0)
	                SyncNode(secNode.Nodes, pageUri, skipSelectionCode);
	        }
	    }

	    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
            if (m_SkipSelectionCode) {
                m_SkipSelectionCode = false;
                return;
            }

			TOCNode tOCNode = e.Node as TOCNode;
			if (tOCNode != null)
			{
				WebBrowserDocument WebBrowserDocument = base.DockPanel.ActiveDocument as WebBrowserDocument;
				if (WebBrowserDocument != null)
				{
					if (tOCNode.Url != null)
					{
						WebBrowserDocument.Navigate(HelpAPI.Help.Instance.GetUriFromTOCNode(tOCNode));
						return;
					}
				}
				else
				{
					if (tOCNode.Url != null)
					{
						WebBrowserDocument WebBrowserDocument2 = new WebBrowserDocument();
						WebBrowserDocument2.Show(base.DockPanel);
						WebBrowserDocument2.Navigate(HelpAPI.Help.Instance.GetUriFromTOCNode(tOCNode));
					}
				}
			}
		}
		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ChangeToc(this.comboBox1.Text);
		}
		internal void Unregister()
		{
			this.comboBox1.Items.Clear();
			this.comboBox1.Text = "";
			this.treeView1.Nodes.Clear();
		}
		private void Contents_Load(object sender, System.EventArgs e)
		{
		}
	}

    public enum TopicSelector {
        Next,
        Previous,
        Sync,
    }
}

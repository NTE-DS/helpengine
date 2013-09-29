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
using NasuTek.DevEnvironment.Resources;
namespace DocExplorer.Resources
{
	public class Index : DevEnvPane
	{
		private IContainer components;
		private Panel panel1;
		private Label label1;
		private ComboBox comboBox2;
		private Label label2;
		private ListBox listBox1;
		private TextBox textBox1;
		
		
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Index));
			this.panel1 = new Panel();
			this.textBox1 = new TextBox();
			this.label2 = new Label();
			this.comboBox2 = new ComboBox();
			this.label1 = new Label();
			this.listBox1 = new ListBox();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.comboBox2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(284, 67);
			this.panel1.TabIndex = 1;
			this.textBox1.Dock = DockStyle.Top;
			this.textBox1.Location = new System.Drawing.Point(0, 47);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(284, 20);
			this.textBox1.TabIndex = 3;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.label2.AutoSize = true;
			this.label2.Dock = DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "&Look for:";
			this.comboBox2.Dock = DockStyle.Top;
			this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(0, 13);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(284, 21);
			this.comboBox2.TabIndex = 1;
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Dock = DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "F&iltered by:";
			this.listBox1.Dock = DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.HorizontalScrollbar = true;
			this.listBox1.Location = new System.Drawing.Point(0, 67);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(284, 195);
			this.listBox1.TabIndex = 2;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			
			base.ClientSize = new System.Drawing.Size(284, 262);
			base.Controls.Add(this.listBox1);
			base.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.HideOnClose = true;
			
			base.Name = "Index";
			base.TabText = "Index";
			this.Text = "Index";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public Index()
		{
			this.InitializeComponent();
		}
		internal void RefreshFilters()
		{
			this.comboBox2.Items.Clear();
			this.comboBox2.Items.Add("(no filter)");
			Filter[] filters = HelpAPI.Help.Instance.ActiveNamespace.Filters;
			for (int i = 0; i < filters.Length; i++)
			{
				Filter filter = filters[i];
				this.comboBox2.Items.Add(filter.FilterName);
			}
			this.comboBox2.Text = "(no filter)";
		}
		internal void ChangeIndex(string filter)
		{
			this.listBox1.Items.Clear();
			IndexNode index = HelpAPI.Help.Instance.GetIndex(filter);
		    foreach (var indexNode in index.SubNodes) {
		        listBox1.Items.Add(indexNode);
		        foreach (var subNode in indexNode.SubNodes) {
		            listBox1.Items.Add(subNode);
		        }
		    }
		}

	    private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e) {
	        IndexNode indexNode = this.listBox1.SelectedItem as IndexNode;

	        WebBrowserDocument WebBrowserDocument = base.DockPanel.ActiveDocument as WebBrowserDocument;
	        if (WebBrowserDocument != null && indexNode.Url != null) {
	            WebBrowserDocument.Navigate(HelpAPI.Help.Instance.GetUriFromTOCNode(indexNode));
	        }

	    }

	    private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ChangeIndex(this.comboBox2.Text);
		}
		internal void Unregister()
		{
			this.listBox1.Items.Clear();
			this.comboBox2.Items.Clear();
			this.comboBox2.Text = "";
			this.textBox1.Text = "";
		}
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			IndexNode indexNode = (
				from IndexNode node in this.listBox1.Items
				where node.Title.StartsWith(this.textBox1.Text)
				select node).FirstOrDefault<IndexNode>();
			if (indexNode != null)
			{
				this.listBox1.SelectedItem = indexNode;
			}
		}
	}
}

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
using System.Windows.Forms;
using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Documents;

namespace DocExplorer.Resources
{
	internal class ResultItem : UserControl
	{
		private IContainer components;
		private Panel panel1;
		private Label label4;
		private Label label3;
		private Label label2;
		private Label label1;

        public string Namespace {
            get;
            private set;
        }
		public string HelpFileNamespace
		{
			get;
			private set;
		}
		public string Url
		{
			get;
			private set;
		}
		internal SearchPage Page
		{
			get;
			private set;
		}
		public bool IsActive
		{
			get
			{
				return this.panel1.BackColor == System.Drawing.SystemColors.Highlight;
			}
			internal set
			{
				this.panel1.BackColor = (value ? System.Drawing.SystemColors.Highlight : System.Drawing.Color.White);
			}
		}
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
			this.panel1 = new Panel();
			this.label4 = new Label();
			this.label3 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(5, 5);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(672, 75);
			this.panel1.TabIndex = 0;
			this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);
			this.label4.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.label4.AutoEllipsis = true;
			this.label4.Location = new System.Drawing.Point(2, 57);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(667, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Source: {0}";
			this.label3.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.label3.Location = new System.Drawing.Point(18, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(651, 36);
			this.label3.TabIndex = 6;
			this.label3.Text = "label3";
			this.label2.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.label2.Location = new System.Drawing.Point(1, 2);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(668, 18);
			this.label2.TabIndex = 5;
			this.label2.Text = "label2";
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Dock = DockStyle.Bottom;
			this.label1.Location = new System.Drawing.Point(0, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(672, 2);
			this.label1.TabIndex = 4;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			
			this.BackColor = System.Drawing.Color.White;
			base.Controls.Add(this.panel1);
			base.Name = "ResultItem";
			base.Padding = new Padding(5);
			base.Size = new System.Drawing.Size(682, 85);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public ResultItem(SearchEngine.SearchItem searchItem, SearchPage page)
		{
			this.InitializeComponent();

            this.Namespace = searchItem.Namespace;
			this.HelpFileNamespace = searchItem.HelpFileNamespace;
			this.Url = searchItem.Url;
			this.Dock = DockStyle.Top;
			this.panel1.Click += new System.EventHandler(this.ResultItem_Click);
			this.label2.Click += new System.EventHandler(this.ResultItem_Click);
			this.label3.Click += new System.EventHandler(this.ResultItem_Click);
			this.label4.Click += new System.EventHandler(this.ResultItem_Click);
			this.panel1.DoubleClick += new System.EventHandler(this.ResultItem_DoubleClick);
			this.label2.DoubleClick += new System.EventHandler(this.ResultItem_DoubleClick);
			this.label3.DoubleClick += new System.EventHandler(this.ResultItem_DoubleClick);
			this.label4.DoubleClick += new System.EventHandler(this.ResultItem_DoubleClick);
			this.label2.Text = searchItem.Title;
			this.label3.Text = searchItem.Description;
			this.label4.Text = string.Format(this.label4.Text, searchItem.Source);
			this.Page = page;
		}
		private void ResultItem_DoubleClick(object sender, System.EventArgs e)
		{
            WebBrowserDocument browserWindow = new WebBrowserDocument();
            DevEnvObj.Instance.WorkspaceEnvironment.ShowPane(browserWindow);
			browserWindow.Navigate("nte-help://" + this.Namespace + "/" + this.HelpFileNamespace + "/" + this.Url);
		}
		private void ResultItem_Click(object sender, System.EventArgs e)
		{
			if (this.IsActive)
			{
				return;
			}
			if (this.Page.ActiveResult != null)
			{
				this.Page.ActiveResult.IsActive = false;
			}
			this.IsActive = true;
			this.Page.ActiveResult = this;
		}
		private void panel1_Paint(object sender, PaintEventArgs e)
		{
		}
	}
}

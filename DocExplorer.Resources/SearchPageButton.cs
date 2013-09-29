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
namespace DocExplorer.Resources
{
	internal sealed class SearchPageButton : UserControl
	{
		private IContainer components;
        private Label label1;
		private Panel panel1;
		private Label label5;
		private Label label4;
        private PictureBox pictureBox1;
		private Label label2;
		internal SearchControl SearchControl
		{
			get;
			set;
		}
		internal SearchPage Page
		{
			get;
			set;
		}
		public bool IsActive
		{
			get
			{
				return this.pictureBox1.Visible;
			}
			internal set
			{
				this.pictureBox1.Visible = value;
				this.panel1.BackColor = (value ? System.Drawing.Color.White : System.Drawing.SystemColors.Control);
			}
		}
		public string Title
		{
			get;
			set;
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 74);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DocExplorer.Resources.Properties.Resources.DataContainer_MovePreviousHS;
            this.pictureBox1.Location = new System.Drawing.Point(3, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoEllipsis = true;
            this.label5.Location = new System.Drawing.Point(43, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoEllipsis = true;
            this.label4.Location = new System.Drawing.Point(43, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 13);
            this.label4.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoEllipsis = true;
            this.label2.Location = new System.Drawing.Point(43, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 13);
            this.label2.TabIndex = 3;
            // 
            // SearchPageButton
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Name = "SearchPageButton";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.Size = new System.Drawing.Size(208, 84);
            this.Load += new System.EventHandler(this.SearchPageButton_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		internal SearchPageButton(SearchControl control)
		{
			this.InitializeComponent();
			this.Dock = DockStyle.Top;
			this.SearchControl = control;
			this.Page = new SearchPage();
			base.Click += new System.EventHandler(this.SearchPageButton_Click);
			this.label1.Click += new System.EventHandler(this.SearchPageButton_Click);
			this.label2.Click += new System.EventHandler(this.SearchPageButton_Click);
			this.pictureBox1.Click += new System.EventHandler(this.SearchPageButton_Click);
			this.label4.Click += new System.EventHandler(this.SearchPageButton_Click);
			this.label5.Click += new System.EventHandler(this.SearchPageButton_Click);
			this.panel1.Click += new System.EventHandler(this.SearchPageButton_Click);
		}
		private void SearchPageButton_Click(object sender, System.EventArgs e)
		{
			if (this.IsActive)
			{
				return;
			}
			if (this.SearchControl.ActiveButton != null)
			{
				this.SearchControl.ActiveButton.IsActive = false;
			}
			this.IsActive = true;
			this.SearchControl.ChangePage(this);
		}
		private void SearchPageButton_Load(object sender, System.EventArgs e)
		{
		}
		private void panel1_Paint(object sender, PaintEventArgs e)
		{
		}
		internal void SetSearchResultPreview(SearchEngine.SearchItem[] searchItems)
		{
			this.label1.Text = string.Concat(new object[]
			{
				this.Title,
				" (",
				searchItems.Length,
				")"
			});
			this.label2.Text = ((searchItems.Length >= 1) ? searchItems[0].Title : "");
			this.label4.Text = ((searchItems.Length >= 2) ? searchItems[1].Title : "");
			this.label5.Text = ((searchItems.Length >= 3) ? searchItems[2].Title : "");
		}
	}
}

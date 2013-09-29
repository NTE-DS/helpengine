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
using DocExplorer.Resources.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace DocExplorer.Resources
{
	internal class SearchControl : UserControl
	{
		private IContainer components;
		private Panel panel1;
		private Panel panel2;
		private Panel panel3;
		private Label label2;
		private Label label1;
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
		private ToolStripButton toolStripButton2;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripLabel toolStripLabel1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton toolStripButton3;
		private ToolStripButton toolStripButton4;
		private Label label3;
		private readonly System.Collections.Generic.List<SearchPageButton> _pages = new System.Collections.Generic.List<SearchPageButton>();
		internal SearchPageButton ActiveButton
		{
			get;
			set;
		}
		public string SearchTitle
		{
			get
			{
				return this.label3.Text.Replace("Searched for: ", "");
			}
			set
			{
				this.label3.Text = "Searched for: " + value;
			}
		}
		internal SearchPage this[string key]
		{
			get
			{
				SearchPageButton searchPageButton = (
					from p in this._pages
					where p.Title == key
					select p).FirstOrDefault<SearchPageButton>();
				if (searchPageButton != null)
				{
					return searchPageButton.Page;
				}
				throw new System.Collections.Generic.KeyNotFoundException();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(559, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 505);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(559, 505);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.toolStrip1);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(767, 29);
            this.panel3.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(626, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(141, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(36, 15);
            this.toolStripLabel1.Text = "0 of 0";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(767, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Searched for: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(767, 2);
            this.label2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(767, 2);
            this.label1.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = global::DocExplorer.Resources.Properties.Resources.DataContainer_MoveFirstHS;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = global::DocExplorer.Resources.Properties.Resources.DataContainer_MovePreviousHS;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Enabled = false;
            this.toolStripButton3.Image = global::DocExplorer.Resources.Properties.Resources.DataContainer_MoveNextHS;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Enabled = false;
            this.toolStripButton4.Image = global::DocExplorer.Resources.Properties.Resources.DataContainer_MoveLastHS;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // SearchControl
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "SearchControl";
            this.Size = new System.Drawing.Size(767, 534);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

		}
		public SearchControl()
		{
			this.InitializeComponent();
		}
		internal void ChangePage(SearchPageButton button)
		{
			this.ActiveButton = button;
			this.panel2.Controls.Clear();
			this.panel2.Controls.Add(button.Page);
		}
		public void AddPage(string text)
		{
			SearchPageButton searchPageButton = (
				from p in this._pages
				where p.Title == text
				select p).FirstOrDefault<SearchPageButton>();
			if (searchPageButton != null)
			{
				return;
			}
			SearchPageButton searchPageButton2 = new SearchPageButton(this)
			{
				Title = text
			};
			searchPageButton2.SetSearchResultPreview(new SearchEngine.SearchItem[0]);
			this._pages.Add(searchPageButton2);
			this.RefreshPages();
		}
		internal void RefreshPages()
		{
			this.panel1.Controls.Clear();
			System.Collections.Generic.List<SearchPageButton> list = this._pages.ToList<SearchPageButton>();
			list.Reverse();
			this.panel1.Controls.AddRange(list.ToArray());
		}
		internal void ChangePage()
		{
			SearchPageButton searchPageButton = this.panel1.Controls[0] as SearchPageButton;
			searchPageButton.IsActive = true;
			this.ChangePage(searchPageButton);
		}
		internal void SetPageData(int currentPage, int maxPages)
		{
			if (maxPages == 0)
			{
				this.toolStripButton1.Enabled = false;
				this.toolStripButton2.Enabled = false;
				this.toolStripButton3.Enabled = false;
				this.toolStripButton4.Enabled = false;
				this.toolStripLabel1.Text = "0 of 0";
				return;
			}
			this.toolStripLabel1.Text = currentPage + " of " + maxPages;
			if (currentPage == 1)
			{
				this.toolStripButton1.Enabled = false;
				this.toolStripButton2.Enabled = false;
			}
			else
			{
				this.toolStripButton1.Enabled = true;
				this.toolStripButton2.Enabled = true;
			}
			if (currentPage == maxPages)
			{
				this.toolStripButton3.Enabled = false;
				this.toolStripButton4.Enabled = false;
				return;
			}
			this.toolStripButton3.Enabled = true;
			this.toolStripButton4.Enabled = true;
		}
		private void toolStripButton1_Click(object sender, System.EventArgs e)
		{
			this.ActiveButton.Page.First();
		}
		private void toolStripButton2_Click(object sender, System.EventArgs e)
		{
			this.ActiveButton.Page.Back();
		}
		private void toolStripButton3_Click(object sender, System.EventArgs e)
		{
			this.ActiveButton.Page.Forward();
		}
		private void toolStripButton4_Click(object sender, System.EventArgs e)
		{
			this.ActiveButton.Page.Last();
		}
	}
}

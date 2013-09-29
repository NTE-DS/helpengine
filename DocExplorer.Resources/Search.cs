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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Resources;
namespace DocExplorer.Resources
{
	public class Search : DevEnvPane
	{
		
		
		private IContainer components;
		private SearchControl searchControl1;
		private Panel panel1;
		private Button button1;
		private ComboBox comboBox1;
		internal Search()
		{
			this.InitializeComponent();
		}
		private void button1_Click(object sender, System.EventArgs e)
		{
			if (this.comboBox1.Text != "")
			{
				foreach (System.Collections.Generic.KeyValuePair<string, SearchEngine.SearchItem[]> current in HelpAPI.Help.Instance.Search(this.comboBox1.Text))
				{
					this.searchControl1.SearchTitle = this.comboBox1.Text;
					this.searchControl1[current.Key].SetResults(current.Value);
					if (!this.comboBox1.Items.Contains(this.comboBox1.Text))
					{
						this.comboBox1.Items.Add(this.comboBox1.Text);
					}
				}
			}
		}
		private void Search_Load(object sender, System.EventArgs e)
		{
			foreach (ISearch current in HelpAPI.Help.Instance.SearchEngine.SearchProviders)
			{
				this.searchControl1.AddPage(current.Name);
			}
			this.searchControl1.ChangePage();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Search));
			this.searchControl1 = new SearchControl();
			this.panel1 = new Panel();
			this.comboBox1 = new ComboBox();
			this.button1 = new Button();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.searchControl1.Dock = DockStyle.Fill;
			this.searchControl1.Location = new System.Drawing.Point(0, 21);
			this.searchControl1.Name = "searchControl1";
			this.searchControl1.Size = new System.Drawing.Size(745, 519);
			this.searchControl1.TabIndex = 3;
			this.panel1.Controls.Add(this.comboBox1);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(745, 21);
			this.panel1.TabIndex = 4;
			this.comboBox1.Dock = DockStyle.Fill;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(674, 21);
			this.comboBox1.TabIndex = 1;
			this.button1.Dock = DockStyle.Right;
			this.button1.Location = new System.Drawing.Point(674, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(71, 21);
			this.button1.TabIndex = 2;
			this.button1.Text = "Search";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			base.AcceptButton = this.button1;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			
			base.ClientSize = new System.Drawing.Size(745, 540);
			base.Controls.Add(this.searchControl1);
			base.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.HideOnClose = true;
			
			base.Name = "Search";
			base.TabText = "Search";
			this.Text = "Search";
			base.Load += new System.EventHandler(this.Search_Load);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}

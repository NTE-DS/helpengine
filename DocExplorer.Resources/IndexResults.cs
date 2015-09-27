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
using NasuTek.DevEnvironment.Documents;
using NasuTek.DevEnvironment.Extendability.Workbench;
namespace DocExplorer.Resources
{
	public class IndexResults : DevEnvPane
	{
		
		private IContainer components;
		private ListView listView1;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		internal IndexResults(DocExplorer.Resources.HelpAPI.Help instance)
		{
			this.InitializeComponent();
			
		}
		internal void SetResults(IndexNode[] nodes, string root)
		{
			if (nodes == null)
			{
				base.TabText = "Index Results";
				this.Text = "Index Results";
				this.listView1.Items.Clear();
				return;
			}
			base.TabText = string.Concat(new object[]
			{
				"Index Results for ",
				root,
				" - ",
				nodes.Length,
				" topics found"
			});
			this.Text = string.Concat(new object[]
			{
				"Index Results for ",
				root,
				" - ",
				nodes.Length,
				" topics found"
			});
			this.listView1.Items.Clear();
			for (int i = 0; i < nodes.Length; i++)
			{
				IndexNode indexNode = nodes[i];
				this.listView1.Items.Add(new ListViewItem(new string[]
				{
					indexNode.Title,
					this.GetTitle(indexNode.Namespace, indexNode.HelpFileNamespace)
				})
				{
					Tag = new string[]
					{
						indexNode.HelpFileNamespace,
						indexNode.Url
					}
				});
			}
			base.Show();
		}
		private string GetTitle(string namespaceName, string helpFileNamespaceName)
		{
			return HelpAPI.Help.Instance.GetHelpFile(namespaceName, helpFileNamespaceName).CollectionName;
		}
		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.listView1.SelectedItems.Count == 1)
			{
				WebBrowserDocument WebBrowserDocument = base.DockPanel.ActiveDocument as WebBrowserDocument;
				if (WebBrowserDocument != null)
				{
					string[] array = this.listView1.SelectedItems[0].Tag as string[];
					if (array != null)
					{
						WebBrowserDocument.Navigate("nte-help://" + array[0] + "/" + array[1]);
						return;
					}
				}
				else
				{
					string[] array2 = this.listView1.SelectedItems[0].Tag as string[];
					if (array2 != null)
					{
						WebBrowserDocument WebBrowserDocument2 = new WebBrowserDocument();
						
						WebBrowserDocument2.Navigate("nte-help://" + array2[0] + "/" + array2[1]);
					}
				}
			}
		}
		internal void Unregister()
		{
			this.listView1.Items.Clear();
		}
		private void IndexResults_Load(object sender, System.EventArgs e)
		{
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(IndexResults));
			this.listView1 = new ListView();
			this.columnHeader1 = new ColumnHeader();
			this.columnHeader2 = new ColumnHeader();
			base.SuspendLayout();
			this.listView1.Columns.AddRange(new ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2
			});
			this.listView1.Dock = DockStyle.Fill;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(666, 262);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.columnHeader1.Text = "Title";
			this.columnHeader1.Width = 228;
			this.columnHeader2.Text = "Location";
			this.columnHeader2.Width = 240;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			
			base.ClientSize = new System.Drawing.Size(666, 262);
			base.Controls.Add(this.listView1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			
			base.Name = "IndexResults";
			base.TabText = "Index Results";
			this.Text = "Index Results";
			base.Load += new System.EventHandler(this.IndexResults_Load);
			base.ResumeLayout(false);
		}
	}
}

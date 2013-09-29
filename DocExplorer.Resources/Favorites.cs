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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Resources;
namespace DocExplorer.Resources
{
	public class Favorites : DevEnvPane
	{
		private IContainer components;
		private TreeView treeView1;
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
		private ToolStripButton toolStripButton2;
		private ImageList imageList1;
		private ToolStripButton toolStripButton3;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Favorites));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(284, 503);
            this.treeView1.TabIndex = 0;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "page_white_world.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::DocExplorer.Resources.Properties.Resources.NewFolderHS;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Add Folder";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::DocExplorer.Resources.Properties.Resources.AddToFavoritesHS;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Add Favorate";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::DocExplorer.Resources.Properties.Resources.DeleteHS;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Delete";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // Favorites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(284, 528);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "Favorites";
            this.TabText = "Favorites";
            this.Text = "Favorites";
            this.Load += new System.EventHandler(this.Favorates_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		public Favorites()
		{
			this.InitializeComponent();
		}
		private void Fill(TreeNode node, FavoriteFolder folder)
		{
			foreach (Favorite current in folder.Favorites)
			{
				node.Nodes.Add(new TreeNode(current.Title, 1, 1)
				{
					Tag = current
				});
			}
			foreach (FavoriteFolder current2 in folder.SubFolders)
			{
				TreeNode node2 = new TreeNode(current2.Title)
				{
					Tag = current2
				};
				this.Fill(node2, current2);
				node.Nodes.Add(node2);
			}
		}
		internal void Unregister()
		{
			this.treeView1.Nodes.Clear();
		}
		private void Favorates_Load(object sender, System.EventArgs e)
		{
			
		}
		public void RefreshNodes()
		{
			this.treeView1.Nodes.Clear();
			this.treeView1.Nodes.Add(new TreeNode("Favorites")
			{
				Name = "Favorites",
                Tag = HelpAPI.Help.Instance.SettingsInstance.RootFavorites[HelpAPI.Help.Instance.ActiveNamespace.NamespaceID]
			});
            this.Fill(this.treeView1.Nodes["Favorites"], HelpAPI.Help.Instance.SettingsInstance.RootFavorites[HelpAPI.Help.Instance.ActiveNamespace.NamespaceID]);
			this.treeView1.ExpandAll();
		}
		private void toolStripButton1_Click(object sender, System.EventArgs e)
		{
			if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Tag is FavoriteFolder)
			{
				new AddFolder((FavoriteFolder)this.treeView1.SelectedNode.Tag).ShowDialog();
			}
			else
			{
				if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Text == "Favorites")
				{
                    new AddFolder(HelpAPI.Help.Instance.SettingsInstance.RootFavorites[HelpAPI.Help.Instance.ActiveNamespace.NamespaceID]).ShowDialog();
				}
			}
			this.RefreshNodes();
		}
		private void toolStripButton2_Click(object sender, System.EventArgs e)
		{
            AddFavorite();
		}

        public void AddFavoriteToRoot() {
            if (DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument is WebBrowserDocument) {
                WebBrowserDocument WebBrowserDocument2 = (WebBrowserDocument)DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument;
                new AddFavorite(HelpAPI.Help.Instance.SettingsInstance.RootFavorites[HelpAPI.Help.Instance.ActiveNamespace.NamespaceID], WebBrowserDocument2.Url, WebBrowserDocument2.Text).ShowDialog();
            }
            this.RefreshNodes();
        }

        public void AddFavorite() {
            if (DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument is WebBrowserDocument) {
                if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Tag is FavoriteFolder) {
                    WebBrowserDocument WebBrowserDocument = (WebBrowserDocument)DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument;
                    new AddFavorite((FavoriteFolder)this.treeView1.SelectedNode.Tag, WebBrowserDocument.Url, WebBrowserDocument.Text).ShowDialog();
                } else {
                    if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Text == "Favorites") {
                        WebBrowserDocument WebBrowserDocument2 = (WebBrowserDocument)DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument;
                        new AddFavorite(HelpAPI.Help.Instance.SettingsInstance.RootFavorites[HelpAPI.Help.Instance.ActiveNamespace.NamespaceID], WebBrowserDocument2.Url, WebBrowserDocument2.Text).ShowDialog();
                    }
                }
            }
            this.RefreshNodes();
        }

		private void toolStripButton3_Click(object sender, System.EventArgs e)
		{
			if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Tag is FavoriteFolder && this.treeView1.SelectedNode.Text != "Favorites")
			{
				if (this.treeView1.SelectedNode.Parent.Text != "Favorites")
				{
					((FavoriteFolder)this.treeView1.SelectedNode.Parent.Tag).SubFolders.Remove((FavoriteFolder)this.treeView1.SelectedNode.Tag);
				}
				else
				{
					if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Text != "Favorites")
					{
						HelpAPI.Help.Instance.SettingsInstance.RootFavorites[HelpAPI.Help.Instance.ActiveNamespace.NamespaceID].SubFolders.Remove((FavoriteFolder)this.treeView1.SelectedNode.Tag);
					}
				}
			}
			else
			{
				if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Tag is Favorite && this.treeView1.SelectedNode.Text != "Favorites")
				{
					if (this.treeView1.SelectedNode.Parent.Text != "Favorites")
					{
						((FavoriteFolder)this.treeView1.SelectedNode.Parent.Tag).Favorites.Remove((Favorite)this.treeView1.SelectedNode.Tag);
					}
					else
					{
						if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Text != "Favorites")
						{
							HelpAPI.Help.Instance.SettingsInstance.RootFavorites[HelpAPI.Help.Instance.ActiveNamespace.NamespaceID].Favorites.Remove((Favorite)this.treeView1.SelectedNode.Tag);
						}
					}
				}
			}
			this.RefreshNodes();
		}
		private void treeView1_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.treeView1.SelectedNode == null || !(this.treeView1.SelectedNode.Tag is Favorite))
			{
				return;
			}
			WebBrowserDocument WebBrowserDocument = base.DockPanel.ActiveDocument as WebBrowserDocument;
			Favorite favorite = (Favorite)this.treeView1.SelectedNode.Tag;
			if (WebBrowserDocument == null)
			{
				WebBrowserDocument = new WebBrowserDocument();
				WebBrowserDocument.Show(base.DockPanel);
			}
			WebBrowserDocument.Navigate(favorite.Url);
		}
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
		}
		private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
		{
			base.DoDragDrop(e.Item, DragDropEffects.Move);
		}
		private void treeView1_DragDrop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
			{
				return;
			}
			System.Drawing.Point pt = ((TreeView)sender).PointToClient(new System.Drawing.Point(e.X, e.Y));
			TreeNode nodeAt = ((TreeView)sender).GetNodeAt(pt);
			TreeNode treeNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
			if (nodeAt == treeNode || nodeAt == null || !(nodeAt.Tag is FavoriteFolder))
			{
				return;
			}
			if (treeNode.Tag is Favorite)
			{
				((FavoriteFolder)treeNode.Parent.Tag).Favorites.Remove((Favorite)treeNode.Tag);
				((FavoriteFolder)nodeAt.Tag).Favorites.Add((Favorite)treeNode.Tag);
			}
			else
			{
				if (treeNode.Tag is FavoriteFolder)
				{
					((FavoriteFolder)treeNode.Parent.Tag).SubFolders.Remove((FavoriteFolder)treeNode.Tag);
					((FavoriteFolder)nodeAt.Tag).SubFolders.Add((FavoriteFolder)treeNode.Tag);
				}
			}
			nodeAt.Nodes.Add((TreeNode)treeNode.Clone());
			nodeAt.Expand();
			treeNode.Remove();
		}
		private void treeView1_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}
	}
}

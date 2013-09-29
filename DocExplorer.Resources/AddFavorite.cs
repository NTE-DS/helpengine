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
namespace DocExplorer.Resources
{
	internal class AddFavorite : Form
	{
		private readonly FavoriteFolder _favoriteFolder;
		private readonly string _url;
		private IContainer components;
		private Label label1;
		private TextBox textBox1;
		private Button button1;
		private Button button2;
		internal AddFavorite(FavoriteFolder favoriteFolder, string url, string defaultTitle)
		{
			this.InitializeComponent();
			this._favoriteFolder = favoriteFolder;
			this._url = url;
			this.textBox1.Text = defaultTitle;
		}
		private void button2_Click(object sender, System.EventArgs e)
		{
			base.Close();
		}
		private void button1_Click(object sender, System.EventArgs e)
		{
			if (this.textBox1.Text != null)
			{
				if ((
					from v in this._favoriteFolder.Favorites
					where v.Title == this.textBox1.Text
					select v).Count<Favorite>() == 0)
				{
					this._favoriteFolder.Favorites.Add(new Favorite
					{
						Title = this.textBox1.Text,
						Url = this._url
					});
				}
			}
			base.Close();
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
			this.label1 = new Label();
			this.textBox1 = new TextBox();
			this.button1 = new Button();
			this.button2 = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Favorite Name:";
			this.textBox1.Location = new System.Drawing.Point(97, 6);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(401, 20);
			this.textBox1.TabIndex = 1;
			this.button1.Location = new System.Drawing.Point(347, 32);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			this.button2.Location = new System.Drawing.Point(423, 32);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			
			base.ClientSize = new System.Drawing.Size(512, 65);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AddFavorite";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Add Favorite";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}

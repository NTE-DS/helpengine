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
using System.Linq;
using System.Windows.Forms;
namespace DocExplorer.Resources
{
	internal sealed class SearchPage : UserControl
	{
		
		private int _currentPage;
		private int _maxPages;
		
		private SearchEngine.SearchItem[] _searchItems;
		private IContainer components;
		private SearchControl SearchControl
		{
			get
			{
				return (SearchControl)base.Parent.Parent;
			}
		}
		internal ResultItem ActiveResult
		{
			get;
			set;
		}
		public SearchPage()
		{
			this.InitializeComponent();
			
			this.Dock = DockStyle.Fill;
		}
		private void SearchPage_Load(object sender, System.EventArgs e)
		{
		}
		internal void SetResults(SearchEngine.SearchItem[] searchItems)
		{
			this.SearchControl.ActiveButton.SetSearchResultPreview(searchItems);
			this._searchItems = searchItems;
			this._maxPages = this._searchItems.Length / 20;
			this.Page(1);
		}
		private void Page(int page)
		{
			base.Controls.Clear();
			System.Collections.Generic.List<ResultItem> list = (
				from searchItem in this._searchItems.Skip((page - 1) * 20).Take(20)
				select new ResultItem(searchItem, this)).ToList<ResultItem>();
			list.Reverse();
			base.Controls.AddRange(list.ToArray());
			this._currentPage = page;
			this.SearchControl.SetPageData(this._currentPage, this._maxPages);
		}
		public void First()
		{
			this.Page(1);
		}
		public void Last()
		{
			this.Page(this._maxPages);
		}
		public void Back()
		{
			this.Page(this._currentPage - 1);
		}
		public void Forward()
		{
			this.Page(this._currentPage + 1);
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			base.Name = "SearchPage";
			base.Size = new System.Drawing.Size(587, 415);
			base.Load += new System.EventHandler(this.SearchPage_Load);
			base.ResumeLayout(false);
		}
	}
}

﻿/***************************************************************************************************
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
using NasuTek.DevEnvironment.Resources;
using NasuTek.DevEnvironment.Resources.Addins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocExplorer.Resources {
    public class SelectNextTopic : AbstractMenuCommand
    {
        public override void Run()
        {
            var WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null)
            {
                var contentsPane = (Contents)DevEnv.Instance.WorkspaceEnvironment.GetPane("Contents");
                contentsPane.SelectNode(WebBrowserDocument.DocumentUri, TopicSelector.Next);
            }
        }
    }

    public class SelectPreviousTopic : AbstractMenuCommand
    {
        public override void Run()
        {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null)
            {
                var contentsPane = (Contents)DevEnv.Instance.WorkspaceEnvironment.GetPane("Contents");
                contentsPane.SelectNode(WebBrowserDocument.DocumentUri, TopicSelector.Previous);
            }
        }
    }

    public class SelectActiveTopic : AbstractMenuCommand
    {
        public override void Run()
        {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                var contentsPane = (Contents) DevEnv.Instance.WorkspaceEnvironment.GetPane("Contents");
                contentsPane.SelectNode(WebBrowserDocument.DocumentUri);
                contentsPane.Show();
            }
        }
    }

    public class ShowDocHelp : AbstractMenuCommand
    {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {

                WebBrowserDocument.Navigate("nte-help://nasutek.help5.hoh/HelpOnHelp/html/af7524fe-8e2d-7a54-67cb-550f480f76c7.htm");
                return;
            } else {

                WebBrowserDocument WebBrowserDocument2 = new WebBrowserDocument();
                WebBrowserDocument2.Show(DevEnv.Instance.WorkspaceEnvironment.DockPanel);
                WebBrowserDocument2.Navigate("nte-help://nasutek.help5.hoh/HelpOnHelp/html/af7524fe-8e2d-7a54-67cb-550f480f76c7.htm");
            }
        }
    }

    public class ShowContents : AbstractMenuCommand {

        public override void Run() {
            DevEnv.Instance.WorkspaceEnvironment.GetPane("Contents").Show();
        }
    }
    public class NotYetImplimented : AbstractMenuCommand {

        public override void Run() {
            System.Windows.Forms.MessageBox.Show("This feature is not yet implimented.", "NasuTek Document Explorer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
    public class ShowIndex : AbstractMenuCommand {

        public override void Run() {
            DevEnv.Instance.WorkspaceEnvironment.GetPane("Index").Show();
        }
    }
    public class ShowFavorites : AbstractMenuCommand {

        public override void Run() {
            DevEnv.Instance.WorkspaceEnvironment.GetPane("Favorites").Show();
        }
    }
    public class AddFavoriteMenuCommand : AbstractMenuCommand {

        public override void Run() {
            ((Favorites)DevEnv.Instance.WorkspaceEnvironment.GetPane("Favorites")).AddFavoriteToRoot();
        }
    }
    public class GoBackWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Back();
            }
        }
    }
    public class PrintWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Print();
            }
        }
    }
    public class CopyWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Copy();
            }
        }
    }
    public class FontSizeWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.FontSize();
            }
        }
    }
    public class GoForwardWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Forward();
            }
        }
    }
    public class StopWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Stop();
            }
        }
    }
    public class RefreshWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Refresh();
            }
        }
    }
    public class HomeWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnv.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                //WebBrowserDocument.Back();
            }
        }
    }
    public class NewWebWindow : AbstractCommand {
        public override void Run() {
            DevEnv.Instance.WorkspaceEnvironment.ShowPane(new WebBrowserDocument());
        }
    }
    public class SearchOpen : AbstractCommand {
        public override void Run() {
            DevEnv.Instance.WorkspaceEnvironment.ShowPane(new Search());
        }
    }
}

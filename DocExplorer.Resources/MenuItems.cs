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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Documents;
using NasuTek.DevEnvironment.Extendability;

namespace DocExplorer.Resources {
    public class AboutDocExplorer : AbstractCommand {
        public override void Run() {
            new AboutDocExplore().ShowDialog();
        }
    }

    public class SelectNextTopic : AbstractCommand
    {
        public override void Run()
        {
            var WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null)
            {
                var contentsPane = (Contents)DevEnvObj.Instance.Extendability.GetPane("Contents");
                contentsPane.SelectNode(WebBrowserDocument.DocumentUri, TopicSelector.Next);
            }
        }
    }

    public class SelectPreviousTopic : AbstractCommand
    {
        public override void Run()
        {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null)
            {
                var contentsPane = (Contents)DevEnvObj.Instance.Extendability.GetPane("Contents");
                contentsPane.SelectNode(WebBrowserDocument.DocumentUri, TopicSelector.Previous);
            }
        }
    }

    public class SelectActiveTopic : AbstractCommand
    {
        public override void Run()
        {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                var contentsPane = (Contents) DevEnvObj.Instance.Extendability.GetPane("Contents");
                contentsPane.SelectNode(WebBrowserDocument.DocumentUri);
                contentsPane.Show();
            }
        }
    }

    public class ShowDocHelp : AbstractCommand
    {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {

                WebBrowserDocument.Navigate("nte-help://nasutek.help5.hoh/HelpOnHelp/html/af7524fe-8e2d-7a54-67cb-550f480f76c7.htm");
                return;
            } else {

                WebBrowserDocument WebBrowserDocument2 = new WebBrowserDocument();
                WebBrowserDocument2.Show(DevEnvObj.Instance.WorkspaceEnvironment.DockPanel);
                WebBrowserDocument2.Navigate("nte-help://nasutek.help5.hoh/HelpOnHelp/html/af7524fe-8e2d-7a54-67cb-550f480f76c7.htm");
            }
        }
    }

    public class ShowContents : AbstractCommand {

        public override void Run() {
            DevEnvObj.Instance.WorkspaceEnvironment.ShowPane(DevEnvObj.Instance.Extendability.GetPane("Contents"));
        }
    }
    public class NotYetImplimented : AbstractCommand {

        public override void Run() {
            System.Windows.Forms.MessageBox.Show("This feature is not yet implimented.", "NasuTek Document Explorer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
    public class ShowIndex : AbstractCommand {

        public override void Run() {
            DevEnvObj.Instance.WorkspaceEnvironment.ShowPane(DevEnvObj.Instance.Extendability.GetPane("Index"));
        }
    }
    public class ShowFavorites : AbstractCommand {

        public override void Run() {
            DevEnvObj.Instance.WorkspaceEnvironment.ShowPane(DevEnvObj.Instance.Extendability.GetPane("Favorites"));
        }
    }
    public class AddFavoriteMenuCommand : AbstractCommand {

        public override void Run() {
            ((Favorites)DevEnvObj.Instance.Extendability.GetPane("Favorites")).AddFavoriteToRoot();
        }
    }
    public class GoBackWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Back();
            }
        }
    }
    public class PrintWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Print();
            }
        }
    }
    public class CopyWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Copy();
            }
        }
    }
    public class FontSizeWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.FontSize();
            }
        }
    }
    public class GoForwardWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Forward();
            }
        }
    }
    public class StopWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Stop();
            }
        }
    }
    public class RefreshWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                WebBrowserDocument.Refresh();
            }
        }
    }
    public class HomeWeb : AbstractCommand {
        public override void Run() {
            WebBrowserDocument WebBrowserDocument = DevEnvObj.Instance.WorkspaceEnvironment.DockPanel.ActiveDocument as WebBrowserDocument;
            if (WebBrowserDocument != null) {
                //WebBrowserDocument.Back();
            }
        }
    }
    public class NewWebWindow : AbstractCommand {
        public override void Run() {
            DevEnvObj.Instance.WorkspaceEnvironment.ShowPane(new WebBrowserDocument());
        }
    }
    public class SearchOpen : AbstractCommand {
        public override void Run() {
            DevEnvObj.Instance.WorkspaceEnvironment.ShowPane(new Search());
        }
    }
}

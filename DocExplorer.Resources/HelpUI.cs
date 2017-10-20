using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.MenuCommands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocExplorer.Resources
{
    class HelpUI : IPackage
    {
        public void Load()
        {
            var uiSvc = (IDevEnvUISvc)DevEnvSvc.GetService(DevEnvSvc.UISvc);
            var pluginSvc = (IDevEnvPackageSvc)DevEnvSvc.GetService(DevEnvSvc.PackageSvc);
            var devEnvObj = (DevEnv)DevEnvSvc.GetService(DevEnvSvc.DevEnvObject);

            uiSvc.RegisterPane(new Contents() { Icon = Icon.FromHandle(Properties.Resources.Contents.GetHicon())});
            uiSvc.RegisterPane(new Index() { Icon = Icon.FromHandle(Properties.Resources.Index.GetHicon())});
            uiSvc.RegisterPane(new Favorites() { Icon = Icon.FromHandle(Properties.Resources.AddToFavoritesHS.GetHicon())});

            var fileMenu = new MenuItem("File", "File", null);
            fileMenu.SubItems.Add(new MenuItem("Print", "Print", new PrintWeb()) { Icon = Properties.Resources.PrintHS });
            fileMenu.SubItems.Add(new MenuItem("Seperator1", null, null));
            fileMenu.SubItems.Add(new MenuItem("Exit", "Exit", new ExitDevEnv()));
            var editMenu = new MenuItem("Edit", "Edit", null);
            editMenu.SubItems.Add(new MenuItem("Undo", "Undo", null) { Icon = Properties.Resources.Edit_UndoHS });
            editMenu.SubItems.Add(new MenuItem("Redo", "Redo", null) { Icon = Properties.Resources.Edit_RedoHS });
            editMenu.SubItems.Add(new MenuItem("Seperator1", null, null));
            editMenu.SubItems.Add(new MenuItem("Cut", "Cut", null) { Icon = Properties.Resources.CutHS });
            editMenu.SubItems.Add(new MenuItem("Copy", "Copy", new CopyWeb()) { Icon = Properties.Resources.CopyHS });
            editMenu.SubItems.Add(new MenuItem("Paste", "Paste", null) { Icon = Properties.Resources.PasteHS });
            editMenu.SubItems.Add(new MenuItem("Delete", "Delete", null) { Icon = Properties.Resources.DeleteHS });
            editMenu.SubItems.Add(new MenuItem("Seperator2", null, null));
            editMenu.SubItems.Add(new MenuItem("SelectAll", "Select All", null));
            editMenu.SubItems.Add(new MenuItem("Seperator3", null, null));
            editMenu.SubItems.Add(new MenuItem("FindInThisDocument", "Find in this Document", null) { Icon = Properties.Resources.FindHS });
            var viewMenu = new MenuItem("View", "View", null);
            var webBrowserMenu = new MenuItem("WebBrowser", "Web Browser", null);
            viewMenu.SubItems.Add(webBrowserMenu);
            var naviMenu = new MenuItem("Navigation", "Navigation", null);
            naviMenu.SubItems.Add(new MenuItem("Contents", "Contents", new ShowContents()) { Icon = Properties.Resources.Contents });
            naviMenu.SubItems.Add(new MenuItem("Index", "Index", new ShowIndex()) { Icon = Properties.Resources.Index });
            naviMenu.SubItems.Add(new MenuItem("Favorites", "Favorites", new ShowFavorites()) { Icon = Properties.Resources.AddToFavoritesHS });
            viewMenu.SubItems.Add(naviMenu);
            var windowMenu = new MenuItem("Window", "Window", null);
            var helpMenu = new MenuItem("Help", "Help", null);
            helpMenu.SubItems.Add(new MenuItem("Contents", "Contents", new ShowContents()) { Icon = Properties.Resources.Contents });
            helpMenu.SubItems.Add(new MenuItem("Index", "Index", new ShowIndex()) { Icon = Properties.Resources.Index });
            helpMenu.SubItems.Add(new MenuItem("Search", "Search", new SearchOpen()) { Icon = Properties.Resources.FindHS });
            helpMenu.SubItems.Add(new MenuItem("Favorites", "Favorites", new ShowFavorites()) { Icon = Properties.Resources.AddToFavoritesHS });
            helpMenu.SubItems.Add(new MenuItem("Seperator1"));
            helpMenu.SubItems.Add(new MenuItem("About", "About NasuTek Document Explorer", new AboutDocExplorer()));
            
            uiSvc.AddRootMenuItem(fileMenu);
            uiSvc.AddRootMenuItem(editMenu);
            uiSvc.AddRootMenuItem(viewMenu);
            uiSvc.AddRootMenuItem(windowMenu);
            uiSvc.AddRootMenuItem(helpMenu);

            //devEnvObj.ActiveWorkbenchSettings = Encoding.UTF8.GetBytes(Properties.Resources.InitialUi);
            pluginSvc.AttachCommand(DevEnvSvc.CmdAfterInitialization, new Initialize());

            var stdTb = new ToolBar("Standard");

            stdTb.Items.Add(new ToolBarItem("Back", "Back", Properties.Resources.NavBack, new GoBackWeb()));
            stdTb.Items.Add(new ToolBarItem("Forward", "Forward", Properties.Resources.NavForward, new GoForwardWeb()));
            stdTb.Items.Add(new ToolBarItem("Stop", "Stop", Properties.Resources.Stop, new StopWeb()));
            stdTb.Items.Add(new ToolBarItem("Refresh", "Refresh", Properties.Resources.RefreshDocViewHS, new RefreshWeb()));
            stdTb.Items.Add(new ToolBarItem("Home", "Home", Properties.Resources.RefreshDocViewHS, new HomeWeb()));
            stdTb.Items.Add(new ToolBarItem("Seperator1"));
            stdTb.Items.Add(new ToolBarItem("Favorites", "Favorites", Properties.Resources.AddToFavoritesHS, new ShowFavorites()));
            stdTb.Items.Add(new ToolBarItem("AddFavorites", "Add to Favorites", Properties.Resources.AddToFavoritesHS, new AddFavoriteMenuCommand()));
            stdTb.Items.Add(new ToolBarItem("Seperator2"));
            stdTb.Items.Add(new ToolBarItem("NewWindow", "New Window", Properties.Resources.NewWindow, new NewWebWindow()));
            stdTb.Items.Add(new ToolBarItem("Seperator3"));
            stdTb.Items.Add(new ToolBarItem("SyncTopic", "Sync Contents", Properties.Resources.Sync, new SelectActiveTopic()));
            stdTb.Items.Add(new ToolBarItem("Seperator4"));
            stdTb.Items.Add(new ToolBarItem("FontSize", "Font Size", Properties.Resources.FontHS, new FontSizeWeb()));

            uiSvc.AddToolbar(stdTb);
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}

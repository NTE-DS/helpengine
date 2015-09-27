using NasuTek.DevEnvironment.Extendability;
using NasuTek.DevEnvironment.MenuCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocExplorer.Resources
{
    class HelpUI : IPlugin
    {
        public void Load()
        {
            var uiSvc = (IDevEnvUISvc)DevEnvSvc.GetService(DevEnvSvc.UISvc);
            var pluginSvc = (IDevEnvPluginSvc)DevEnvSvc.GetService(DevEnvSvc.PluginSvc);

            uiSvc.RegisterPane(new Contents());
            uiSvc.RegisterPane(new Index());
            uiSvc.RegisterPane(new Favorites());

            var fileMenu = new MenuItem("File", "File", null);
            fileMenu.SubItems.Add(new MenuItem("Print", "Print", new PrintWeb()));
            fileMenu.SubItems.Add(new MenuItem("Seperator1", null, null));
            fileMenu.SubItems.Add(new MenuItem("Exit", "Exit", new ExitDevEnv()));
            var editMenu = new MenuItem("Edit", "Edit", null);
            editMenu.SubItems.Add(new MenuItem("Undo", "Undo", null));
            editMenu.SubItems.Add(new MenuItem("Redo", "Redo", null));
            editMenu.SubItems.Add(new MenuItem("Seperator1", null, null));
            editMenu.SubItems.Add(new MenuItem("Cut", "Cut", null));
            editMenu.SubItems.Add(new MenuItem("Copy", "Copy", new CopyWeb()));
            editMenu.SubItems.Add(new MenuItem("Paste", "Paste", null));
            editMenu.SubItems.Add(new MenuItem("Delete", "Delete", null));
            editMenu.SubItems.Add(new MenuItem("Seperator2", null, null));
            editMenu.SubItems.Add(new MenuItem("SelectAll", "Select All", null));
            editMenu.SubItems.Add(new MenuItem("Seperator3", null, null));
            editMenu.SubItems.Add(new MenuItem("FindInThisDocument", "Find in this Document", null));
            var viewMenu = new MenuItem("View", "View", null);
            var webBrowserMenu = new MenuItem("WebBrowser", "Web Browser", null);
            viewMenu.SubItems.Add(webBrowserMenu);
            var naviMenu = new MenuItem("Navigation", "Navigation", null);
            naviMenu.SubItems.Add(new MenuItem("Contents", "Contents", new ShowContents()));
            naviMenu.SubItems.Add(new MenuItem("Index", "Index", new ShowIndex()));
            naviMenu.SubItems.Add(new MenuItem("Favorites", "Favorites", new ShowFavorites()));
            viewMenu.SubItems.Add(naviMenu);
            var windowMenu = new MenuItem("Window", "Window", null);
            var helpMenu = new MenuItem("Help", "Help", null);
            helpMenu.SubItems.Add(new MenuItem("Contents", "Contents", new ShowContents()));
            helpMenu.SubItems.Add(new MenuItem("Index", "Index", new ShowIndex()));
            helpMenu.SubItems.Add(new MenuItem("Search", "Search", new SearchOpen()));
            helpMenu.SubItems.Add(new MenuItem("Favorites", "Favorites", new ShowFavorites()));
            helpMenu.SubItems.Add(new MenuItem("Seperator1"));
            helpMenu.SubItems.Add(new MenuItem("About", "About NasuTek Document Explorer", new AboutDocExplorer()));
            
            uiSvc.AddRootMenuItem(fileMenu);
            uiSvc.AddRootMenuItem(editMenu);
            uiSvc.AddRootMenuItem(viewMenu);
            uiSvc.AddRootMenuItem(windowMenu);
            uiSvc.AddRootMenuItem(helpMenu);

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

using System;
using System.Collections.Generic;
using System.Linq;
using DocExplorer.Resources.HelpAPI;
using HelpSimple;

namespace DocExplorer.SimpleUI {
    class HelpUi : IHelpUi{
        public Type TocTypeGenerator {
            get { return typeof(TOCNode); }
        }

        public void SetActiveCollection() {
            SimpleUIProgram.MainForm.Text = Help.Instance.ActiveNamespace.Title;
        }

        public void RefreshFilters() {
            SimpleUIProgram.MainForm.RefreshTOC();
        }
    }
}

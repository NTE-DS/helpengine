using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.Help
{
    public interface IHelp
    {
        void ShowWindow();
        void HideWindow();
        void F1Execute(string f1Query);
    }
}

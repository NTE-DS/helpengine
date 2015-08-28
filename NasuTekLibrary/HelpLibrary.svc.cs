using NasuTekLibrary.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;

namespace NasuTekLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class HelpLibrary : IHelpLibrary
    {
        string lang;
        string namespaceN;
        Help hlp;

        public byte[] GetHelpFile(string helpFileNamespace, string filename)
        {
            throw new NotImplementedException();
        }

        public string GetHelpTOC()
        {
            return GenTOC();
        }

        public void LoadLibrary(int langCode, string namespaceName)
        {
            if (hlp == null)
                return;

            lang = langCode.ToString();
            namespaceN = namespaceName;

            var namespaceXML = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.xml"));
            var nsid = namespaceXML.Root.Elements("{http://schemas.nasutek.com/2015/Help5/HelpForWebExtensions}Library").FirstOrDefault(v => v.Attribute("Name").Value == namespaceN);
            hlp = new Help();

            foreach (var i in nsid.Element("{http://schemas.nasutek.com/2015/Help5/HelpForWebExtensions}Books").Elements("{http://schemas.nasutek.com/2015/Help5/HelpForWebExtensions}Book"))
            {
                if (i.Attribute("Lang").Value == lang)
                    hlp.Titles.Add(new HelpFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, i.Attribute("File").Value)));
            }
        }

        public string GenTOC()
        {
            var nodeRoot = Help.Instance.GetTableOfContentsTree();

            var retVal = "";
            retVal = nodeRoot.SubNodes.Aggregate(retVal, (current, subNode) => current + GenTOC(subNode));

            return retVal;
        }

        public string GenTOC(TOCNode node)
        {
            var retVal = "";
            if (node.SubCount > 0)
            {
                retVal += "<div class=\"TreeNode\">";
            }
            else
            {
                retVal += "<div class=\"TreeItem\">";
            }
            retVal += "<img class=\"TreeNodeImg\" onclick=\"javascript: Toggle(this);\" src=\"" + (node.SubCount > 0 ? "interface-resources/Collapsed.gif" : "interface-resources/Item.gif") +
                "\"/><a class=\"UnselectedNode\" onclick=\"javascript: return Expand(this);\" href=\"" + lang + "/" + node.HelpFileNamespace + "/" + node.Url + "\" target=\"TopicContent\">" +
                node.Title + "</a>";

            if (node.SubCount > 0)
            {
                retVal += "<div class=\"Hidden\">";
                retVal = node.SubNodes.Aggregate(retVal, (current, subNode) => current + GenTOC(subNode));
                retVal += "</div>";
            }
            retVal += "</div>";

            return retVal;
        }
    }
}

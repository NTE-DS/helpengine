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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NasuTek.DevEnvironment;
using Help = DocExplorer.Resources.HelpAPI.Help;
using System.Drawing.Imaging;

namespace DocExplorer.Resources.HelpAPI
{
    public class HelpScheme : IProtocol
    {
        public int GetStream(string url, out Stream objStream) {
            objStream = new MemoryStream();
            var uri = new Uri(url);

            try {
                if (uri.Host == "nte-help5-common")
                {
                    var lol = uri.AbsolutePath.Substring(1).Split('/');
                    switch (lol[0])
                    {
                        case "logo.png":
                            {
                                var logo = new MemoryStream();
                                Properties.Resources.AboutBox.Save(logo, ImageFormat.Png);
                                objStream.Write(logo.GetBuffer(), 0, (int)logo.Length);
                                return (int)logo.Length;
                            }
                        case "logo.html":
                            {
                                byte[] bytes = System.Text.Encoding.ASCII.GetBytes("<html><head><style>body{margin: 0;}</style></head><body oncontextmenu=\"return false\"><img src=\"nte-help://nte-help5-common/logo.png\" /></body></html>");
                                objStream.Write(bytes, 0, bytes.Length);
                                return bytes.Length;
                            }
                        case "collections.html":
                            {
                                var btl = new StringBuilder();
                                btl.Append("<html><head><style>body{margin:0;font-family:Verdana,Helvetica,sans-serif;font-size:0.8em;}table{border-collapse:collapse;font-family:Verdana,Helvetica,sans-serif;font-size:0.8em;}th{background-color:lightgrey;}table,td,th{border:1px solid black;}</style></head><body>");
                                btl.Append("<p style=\"font-weight: bold\">Registered Collections:</p>");
                                btl.Append("<table style=\"width: 100%\">");
                                btl.Append("<tr>");
                                btl.Append("<th>Collection ID</th>");
                                btl.Append("<th>Collection Title</th>");
                                btl.Append("</tr>");
                                foreach (var i in Help.Instance.Namespaces)
                                {
                                    btl.Append("<tr>");
                                    btl.AppendFormat("<td>{0}</td>", i.Value.NamespaceID);
                                    btl.AppendFormat("<td>{0}</td>", i.Value.Title);
                                    btl.Append("</tr>");
                                }
                                btl.Append("</table>");
                                btl.Append("</body></html>");
                                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(btl.ToString());
                                objStream.Write(bytes, 0, bytes.Length);
                                return bytes.Length;
                            }
                        default:
                            {
                                byte[] bytes = System.Text.Encoding.ASCII.GetBytes("COMMON");
                                objStream.Write(bytes, 0, bytes.Length);
                                return bytes.Length;
                            }
                    }
                }
                var absolutePath = uri.AbsolutePath.Substring(1).Split('/');
                var finalAbsolutePath = uri.AbsolutePath.Substring(absolutePath[0].Length + 2);
                byte[] array = Help.Instance.GetHelpFile(Help.Instance.GetProperNamespaceName(uri.Host), absolutePath[0]).ReadFile(finalAbsolutePath);
                objStream.Write(array, 0, array.Length);
                return array.Length;
            } catch (System.Exception ex) {
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(ex.ToString());
                objStream.Write(bytes, 0, bytes.Length);
                return bytes.Length;
            }
        }
    }
}

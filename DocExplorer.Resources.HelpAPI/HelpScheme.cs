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
                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes("COMMON");
                    objStream.Write(bytes, 0, bytes.Length);
                    return bytes.Length;
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

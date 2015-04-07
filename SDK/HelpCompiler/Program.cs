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

using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HelpCompiler {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("NasuTek Help 5 Compiler");
            Console.WriteLine("Copyright (C) 2008-2015 NasuTek Enterprises");
            Console.WriteLine();
            Arguments arguments = new Arguments(args);
            if (arguments["?"] == "true" || args.Length == 0) {
                Console.WriteLine("/CompileNXC:<NXH File Path>");
                Console.WriteLine("\tCompiles an NXH File to an NXC file.");
                return;
            }
            string text = Path.Combine(Path.GetTempPath(), "CompileDirectory");
            Directory.CreateDirectory(text);

            try {
                XDocument xDocument = XDocument.Load(arguments["CompileNXC"]);
                string directoryName = Path.GetDirectoryName(Path.GetFullPath(arguments["CompileNXC"]));
                XElement xElement = xDocument.Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}HelpCollection").Element("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}CompileDirective");
                foreach (XElement current in xElement.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}Include")) {
                    string text2 = Path.Combine(directoryName, current.Attribute("Path").Value);
                    try {
                        new Computer().FileSystem.CopyDirectory(text2, Path.Combine(text, Path.GetFileName(text2)));
                    } catch (DirectoryNotFoundException) {
                        File.Copy(text2, Path.Combine(text, Path.GetFileName(text2)));
                    }
                }
                string value = xElement.Attribute("FilePath").Value;
                xElement.Remove();
                string fileName = Path.GetFileName(Path.GetFullPath(arguments["CompileNXC"]));
                Directory.CreateDirectory(Path.Combine(text, "$Attributes"));
                foreach (XElement current2 in xElement.Elements("{http://schemas.nasutek.com/2013/Help5/Help42Extensions}Attribute")) {
                    File.WriteAllText(Path.Combine(text, "$Attributes", current2.Attribute("Name").Value), current2.Attribute("Value").Value);
                }
                File.WriteAllText(Path.Combine(text, "$Attributes", "HelpCollection"), fileName);
                xDocument.Save(Path.Combine(text, fileName));
                if (File.Exists(Path.Combine(directoryName, value))) {
                    File.Delete(Path.Combine(directoryName, value));
                }
                new FastZip().CreateZip(Path.Combine(directoryName, value), text, true, null);
                Directory.Delete(text, true);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                Directory.Delete(text, true);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Win32;

namespace DocExplorer.Resources.HelpAPI {
    public class HelpInstaller {
        private string collectionPath;

        public HelpInstaller(string collectionPath) {
            this.collectionPath = collectionPath;
        }

        public static void CreateRegCollectionKey(string collectionName, string collectionPath) {
            if (!String.IsNullOrEmpty(collectionName)) {
#if DEBUG
                var kpl = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2-Debug\\RegisteredCollections", true);
#else
                var kpl = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2\\RegisteredCollections", true);
#endif
                kpl.SetValue(collectionName, collectionPath);
                return;
            }
#if DEBUG
            Registry.LocalMachine.CreateSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2-Debug\\RegisteredCollections");
#else
            Registry.LocalMachine.CreateSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2\\RegisteredCollections");
#endif
        }

        public static void Install(string installDir)
        {
            CreateRegCollectionKey(null, null);
            CreateRegCollectionKey("DefaultCollection", Path.Combine(installDir, "DefaultCollection"));
            var helpInstaller = new HelpInstaller(Path.Combine(installDir, "DefaultCollection"));
            helpInstaller.CreateCollection();
            helpInstaller.CreateNamespace("NasuTek.Default.CC", "NasuTek Default Combined Collection", true, null, null);

#if DEBUG
            var kpl = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2-Debug", true);
#else
            var kpl = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2", true);
#endif

            kpl.SetValue("InstallDir", installDir);
        }

        public static void Uninstall()
        {
#if DEBUG
            var kpl = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2-Debug", true);
#else
            var kpl = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\Help\\5.2", true);
#endif
            var installDir = (string)kpl.GetValue("InstallDir");
#if DEBUG
            Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\NasuTek Enterprises\\Help\\5.2-Debug");
#else
            Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\NasuTek Enterprises\\Help\\5.2");
#endif
            Directory.Delete(Path.Combine(installDir, "DefaultCollection"), true);
        }

        public void CreateCollection() {
            var fullPath = Path.GetFullPath(collectionPath);
            if (Directory.Exists(fullPath))
                return;

            Directory.CreateDirectory(fullPath);
            Directory.CreateDirectory(Path.Combine(fullPath, "Namespaces"));
            Directory.CreateDirectory(Path.Combine(fullPath, "ContentStore"));

            var doc = new XDocument();
            doc.Add(new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}InstalledBooks"));
            doc.Save(Path.Combine(fullPath, "ContentStore", "InstalledBooks.xml"));
        }

        public void CreateNamespace(string namespaceID, string friendlyName, bool combinedCollection, string infoPath, string logoPath) {
            var doc = new XDocument();
            var rootElement =
                new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition");

            rootElement.Add(new XAttribute("id", namespaceID));
            rootElement.Add(new XAttribute("friendlyName", friendlyName));
            rootElement.Add(new XAttribute("isCombinedCollection", combinedCollection ? "True" : "False"));
            if(!String.IsNullOrEmpty(infoPath))
                rootElement.Add(new XAttribute("infoPath", infoPath));
            if (!String.IsNullOrEmpty(logoPath))
                rootElement.Add(new XAttribute("logoPath", logoPath));

            rootElement.Add(new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugins"));
            rootElement.Add(new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBooks"));
            rootElement.Add(new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Filters"));

            doc.Add(rootElement);

            doc.Save(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));
        }

        public void RegisterNamespace()
        {

        }

        public void DeleteNamespace(string namespaceID) {
            File.Delete(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));

            foreach (var file in Directory.GetFiles(Path.Combine(collectionPath, "Namespaces"), "*.NxN")) {
                var doc = XDocument.Load(file);
                var pluginToDelete = doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition")
                    .Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugins")
                    .Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugin").FirstOrDefault(v => v.Attribute("id").Value == namespaceID);
                if (pluginToDelete != null)
                    pluginToDelete.Remove();

                doc.Save(file);
            }
        }

        public void InstallBook(string bookId, string bookFilePath) {
            var doc = XDocument.Load(Path.Combine(collectionPath, "ContentStore", "InstalledBooks.xml"));

            var book = new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Book");
            book.Add(new XAttribute("id", bookId));
            book.Add(new XAttribute("fileName", Path.GetFileName(bookFilePath)));

            File.Copy(Path.GetFullPath(bookFilePath), Path.Combine(collectionPath, "ContentStore", Path.GetFileName(bookFilePath)));

            doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}InstalledBooks").Add(book);
            doc.Save(Path.Combine(collectionPath, "ContentStore", "InstalledBooks.xml"));
        }

        public void UninstallBook(string bookId) {
            var doc = XDocument.Load(Path.Combine(collectionPath, "ContentStore", "InstalledBooks.xml"));

            var bookToUninstall = doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}InstalledBooks").Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Book").FirstOrDefault(v => v.Attribute("id").Value == bookId);

            if (bookToUninstall == null) return;

            File.Delete(Path.Combine(collectionPath, "ContentStore", bookToUninstall.Attribute("fileName").Value));
            bookToUninstall.Remove();
            doc.Save(Path.Combine(collectionPath, "ContentStore", "InstalledBooks.xml"));

            foreach (var file in Directory.GetFiles(Path.Combine(collectionPath, "Namespaces"), "*.NxN")) {
                var namespaceRemove = XDocument.Load(file);
                var pluginToDelete = doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition")
                    .Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBooks")
                    .Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBook").FirstOrDefault(v => v.Attribute("id").Value == bookId);
                if (pluginToDelete != null)
                    pluginToDelete.Remove();

                doc.Save(file);
            }
        }

        public void LinkBook(string bookId, string namespaceID) {
            var doc = XDocument.Load(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));

            var book = new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBook");
            book.Add(new XAttribute("id", bookId));

            doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition").Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBooks").Add(book);
            doc.Save(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));
        }

        public void UnlinkBook(string bookId, string namespaceID) {
            var doc = XDocument.Load(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));

            var linkedBook =
                doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition")
                    .Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBooks")
                    .Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}LinkedBook")
                    .FirstOrDefault(v => v.Attribute("id").Value == bookId);

            if (linkedBook == null) return;

            linkedBook.Remove();

            doc.Save(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));
        }

        public void AddPlugin(string namespaceIDToPlugin, string namespaceID) {
            var doc = XDocument.Load(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));

            var plugin = new XElement("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugin");
            plugin.Add(new XAttribute("id", namespaceIDToPlugin));

            doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition").Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugins").Add(plugin);
            doc.Save(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));
        }

        public void RemovePlugin(string namespaceIDToPlugin, string namespaceID) {
            var doc = XDocument.Load(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));

            var plugin =
                doc.Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}NasuTekNamespaceDefinition")
                    .Element("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugins")
                    .Elements("{http://schemas.nasutek.com/2013/Help5/Help5Extensions}Plugin")
                    .FirstOrDefault(v => v.Attribute("id").Value == namespaceIDToPlugin);

            if (plugin == null) return;
            plugin.Remove();
            doc.Save(Path.Combine(collectionPath, "Namespaces", namespaceID + ".NxN"));
        }
    }
}
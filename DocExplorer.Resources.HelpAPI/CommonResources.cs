using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace DocExplorer.Resources.HelpAPI {
    public class CommonResources {
        public static byte[] ReadFile(string filePath) {
            string name = filePath.Replace('\\', '/');
            byte[] result;

            ZipFile zipFile = new ZipFile(new MemoryStream());
            ZipEntry entry = zipFile.GetEntry(name);
            if (entry == null) {
                zipFile.Close();
                return null;
            }
            using (System.IO.Stream inputStream = zipFile.GetInputStream(entry)) {
                byte[] array = new byte[(int) entry.Size];
                inputStream.Read(array, 0, (int) entry.Size);
                zipFile.Close();
                result = array;
            }
            return result;

        }
    }
}

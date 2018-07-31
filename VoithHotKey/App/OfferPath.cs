using System;
using System.Configuration;
using System.IO;

namespace VoithHotKey.App
{
    class OfferPath : AbstractPath {

        private string PathPrefix = ConfigurationManager.AppSettings["angebotsordner"];

        public OfferPath(string id) : base(id) {}

        public override string CanonicalPath() {
            // 17sh0132
            // 17sst0132
            string folderYear = base.FolderYear(id.Substring(0,2));
            return FindSubDir(folderYear);
        }

        private string FindSubDir(string folderYear) {
            string year = id.Substring(0,2);
            string user = id.Substring(2, id.Length -/* letzten vier */ 4 - /* Jahreszahl vorne */ 2);
            string nr = id.Substring(id.Length - 3);

            string folderName = nr + "_" + user + "_" + year;

            // Ordner suchen zB 132_SH_17_Bieber
            foreach (string directory in Directory.GetDirectories(PathPrefix + folderYear)) {
                DirectoryInfo dir = new DirectoryInfo(directory);
                if (dir.Name.StartsWith(folderName)) {
                    return dir.FullName;
                }
            }

            throw new DirectoryNotFoundException("Verzeichnis nicht gefunden: " + id + " (Jahr: " + folderYear + ")");

        }
    }
}

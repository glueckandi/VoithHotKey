using System;
using System.IO;
using System.Configuration;

namespace VoithHotKey.App
{
    class OrderPath : AbstractPath {

        private string PathPrefix = ConfigurationManager.AppSettings["auftragsordner"];

        public OrderPath(string nr) : base(nr) {}

        public override string CanonicalPath() {

            string folderYear = base.FolderYear(id.Substring(0, 2));

            // Der Ordnername der alten Aufraege (vor 2011) hat mit der dreistelligen Krannummer begonnen.
            // Daher ist die Verabeitung unterschiedlich zu realisieren.
            if (Convert.ToInt16(folderYear) >= 2011) {
                return FindSubDir(folderYear);
            } else {
                return FindOldStructureDir(folderYear);
            }
            
        }

        private string FindSubDir(string folderYear) {
            string folderName = id + "_" + id;

            //Einzelnen Ordner suchen zB 15A0300_15A0300_Bieber
            foreach (string directory in Directory.GetDirectories(PathPrefix + folderYear)) {
                DirectoryInfo dir = new DirectoryInfo(directory);
                if (dir.Name.StartsWith(folderName)) {
                    return dir.FullName;
                }
            }

            //Ordner wurde noch nicht gefunden, ev. verschachtelt zB gesucht 15A0301 -> 15A0300_15A0302
            foreach (string directory in Directory.GetDirectories(PathPrefix + folderYear)) {
                DirectoryInfo dir = new DirectoryInfo(directory);

                try {
                    string[] numbers = dir.Name.Split('_');
                    ushort first = Convert.ToUInt16(numbers[0].Substring(3));
                    ushort second = Convert.ToUInt16(numbers[1].Substring(3));

                    ushort craneNr = Convert.ToUInt16(id.Substring(3));

                    if (craneNr >= first && craneNr <= second) {
                        return dir.FullName;
                    }
                }catch(Exception ex) {
                    base.Log(ex, "Ordner beginnt nicht mit zwei Auftragsnummern: " + dir.FullName);
                }
                
            }

            throw new DirectoryNotFoundException("Verzeichnis nicht gefunden: " + id + " (Jahr: " + folderYear + ")");

        }

        public string FindOldStructureDir(string folderYear) {
            string folderName = id.Substring(4) + "_" + id.Substring(4);

            //Einzelnen Ordner suchen zB 15A0300_15A0300_Bieber
            foreach (string directory in Directory.GetDirectories(PathPrefix + folderYear)) {
                DirectoryInfo dir = new DirectoryInfo(directory);
                if (dir.Name.StartsWith(folderName)) {
                    return dir.FullName;
                }
            }

            //Ordner wurde noch nicht gefunden, ev. verschachtelt zB gesucht 15A0301 -> 15A0300_15A0302
            foreach (string directory in Directory.GetDirectories(PathPrefix + folderYear)) {
                DirectoryInfo dir = new DirectoryInfo(directory);

                try {
                    string[] numbers = dir.Name.Split('_');
                    ushort first = Convert.ToUInt16(numbers[0]);
                    ushort second = Convert.ToUInt16(numbers[1]);

                    ushort craneNr = Convert.ToUInt16(id.Substring(3));

                    if (craneNr >= first && craneNr <= second) {
                        return dir.FullName;
                    }
                } catch (FormatException ex) {
                    base.Log(ex, "Ordner beginnt nicht mit zwei Auftragsnummern: " + dir.FullName);
                }
                
            }

            throw new DirectoryNotFoundException("Verzeichnis nicht gefunden: " + id + " (Jahr: " + folderYear + ")");
        }
    }
}

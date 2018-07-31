using System;

namespace VoithHotKey.App
{
    abstract class AbstractPath {

        protected string id;

        public AbstractPath(string id) {
            this.id = id;
        }

        public abstract string CanonicalPath();

        // Jahreszahl zweistellig uebergeben zB 17 fuer 2017
        protected string FolderYear(string year) {

            UInt16 y = Convert.ToUInt16(year);
            string folderYear;

            if (y < 66 && y >= 0) {
                folderYear = "20" + year;
            } else {
                folderYear = "19" + year;
            }

            return folderYear;
        }

        public void Log(Exception ex, string msg) {
            // TODO
        }

    }
}

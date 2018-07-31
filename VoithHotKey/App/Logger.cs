using System;
using System.IO;

namespace VoithHotKey.App
{
    class Logger {
        
        private string LogDir {
            get {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return appDataPath + "\\VoithHotKey";
            }
        }

        private string Path {
            get {
                return LogDir + "\\VoithHotKey.log";
            }
        }

        public Logger() {

            if (!Directory.Exists(LogDir)) {
                try {
                    Directory.CreateDirectory(LogDir);
                } catch (Exception) { }
            }

            if (!File.Exists(Path)) {
                try {
                    using (File.Create(Path)) { }
                } catch (Exception) { }
            }

        }

        public void Log(Exception ex) {
            using (StreamWriter writer = new StreamWriter(Path, /*append*/ true)) {
                writer.WriteLine(DateTime.Now + " -- " + ex.Message);
                writer.WriteLine(ex.StackTrace);
            }
        }

        public void Log(Exception ex, string msg) {
            using (StreamWriter writer = new StreamWriter(Path, /*append*/ true)) {
                writer.WriteLine(DateTime.Now + " -- " + ex.Message + " -- " + msg);
                writer.WriteLine(ex.StackTrace);
            }
        }

    }
}

using System;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace VoithHotKey.App
{
    class OrderProcessor : AbstractProcessor {

        public OrderProcessor(string text) : base(text) {}

        public override void Process() {
            FindPatterns();

            //TODO für alle items explorer öffnen

        }

        private void FindPatterns() {
            SingleOrder(base.text);
            //OrderRange(base.text);
        }

        private void SingleOrder(string text) {

            // zB 15A0300
            string pattern1 = "[0-9][0-9]A[0-9][0-9][0-9][0-9]";

            // zB  17A150 (wenn die Auftragsnummer nur dreistellig ist, muss danach ein Leerzeichen folgen)
            string pattern2 = "[0-9][0-9]A[0-9][0-9][0-9]\\s";

            // zB 517/15
            string pattern3 = ".[0-9][0-9]/[0-9][0-9]";

            MatchCollection collection1 = Regex.Matches(text, pattern1);
            MatchCollection collection2 = Regex.Matches(text, pattern2);
            MatchCollection collection3 = Regex.Matches(text, pattern3);

            foreach (Match m in collection1) {
                base.items.Add(new TrackingNumber(m.Value));
            }

            // mit fehlender Null auffüllen
            foreach (Match m in collection2) {
                string nr = m.Value.Trim();
                nr = nr.Insert(3, "0");
                base.items.Add(new TrackingNumber(nr));
            }

            // 517/15 wird zu 15A0517
            foreach (Match m in collection3) {
                string[] parts = m.Value.Trim().Split('/');
                if (parts.Length == 2) {
                    // Jahr wenn zweistellig mit 0 auf drei Stellen auffuellen
                    string nr = parts[0].Length == 3 ? parts[0] : "0" + parts[0];
                    base.items.Add(new TrackingNumber(parts[1] + "A0" + nr));
                }
                
            }
        }

        private void OrderRange(string text) {

            string pattern1 = "[0-9][0-9]A[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]";        // zB 15A0300-305
            string pattern2 = "[0-9][0-9]A[0-9][0-9][0-9]-[0-9][0-9][0-9]";             // zB 16A300-304 (fehlende Null)
            string pattern3 = @"[0-9][0-9]A[0-9][0-9][0-9][0-9]\s-\s[0-9][0-9][0-9]";  // zB 15A0300 - 305
            string pattern4 = @"[0-9][0-9]A[0-9][0-9][0-9]\s-\s[0-9][0-9][0-9]";       // zB 15A300 - 304
            string pattern5 = @"[0-9][0-9]A[0-9][0-9][0-9][0-9]\s–\s[0-9][0-9][0-9]";   // zB 15A0300 – 305 (Bindestrich durch Outlook-Autokorrektur)
            string pattern6 = @"[0-9][0-9]A[0-9][0-9][0-9]\s–\s[0-9][0-9][0-9]";        // zB 15A300 – 304 (Bindestrich durch Outlook-Autokorrektur)
            //string pattern7 = "[0-9][0-9]A[0-9][0-9][0-9][0-9]-[0-9][0-9]A[0-9][0-9][0-9][0-9]"; // 17A0305 - 17A0309

            string pattern = $"({pattern1})|({pattern2})|({pattern3})|({pattern4})|({pattern5})|({pattern6})";

            MatchCollection collection1 = Regex.Matches(text, pattern);

            foreach (Match m in collection1) {
                SplitNummer(m.Value);
            }

            // Auf einzelene Auftragsnummern splitten
            void SplitNummer(string nr) {

                nr = nr.Replace('–', '-'); // Bindestrich Outlook-Autokorrektur durch normalen Bindestrich ersetzen

                string prefix = nr.Substring(0, 3); // zB 16A
                string postfix = nr.Substring(3); // zB 0340-342
                string[] numbers = postfix.Split('-');
                ushort from = Convert.ToUInt16(numbers[0].Trim()); // zB 340
                ushort to = Convert.ToUInt16(numbers[1].Trim()); // zB 342

                for (ushort i = from; i <= to; i++) {
                    string value = prefix + "0" + i; // zB 16A0340
                    base.items.Add(new TrackingNumber(value));
                }
            }

        }
     
    }
}

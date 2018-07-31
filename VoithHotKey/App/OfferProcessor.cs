using System;
using System.Text.RegularExpressions;

namespace VoithHotKey.App
{
    class OfferProcessor : AbstractProcessor {
        
        private static string pattern = "[0-9][0-9](la|sh|sst|sb|kc|wp|ph|af|fp|pe|sw|sj)[0-9][0-9][0-9][0-9]"; // 17sh0312

        public OfferProcessor(string text) : base(text) { }

        public override void Process() {

            FindPattern();

            // TODO für jedes item explorer öffnen

        }

        private void FindPattern() {

            MatchCollection collection1 = Regex.Matches(base.text.ToLower(), OfferProcessor.pattern);

            foreach (Match match in collection1) {
                base.items.Add(new TrackingNumber(match.Value));
            }
        }

    }
}

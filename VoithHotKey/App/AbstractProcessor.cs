using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace VoithHotKey.App
{
    abstract class AbstractProcessor {

        protected HashSet<TrackingNumber> items = new HashSet<TrackingNumber>();
        protected string text;
        protected Logger Logger { get; }

        public AbstractProcessor(string text) {
            this.text = text;
            Logger = new Logger();
        }

        public abstract void Process();

    }
}

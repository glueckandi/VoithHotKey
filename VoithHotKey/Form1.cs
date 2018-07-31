using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoithHotKey.App;
using System.IO;
using System.Windows.Automation;


namespace VoithHotKey
{
    public partial class Form1 : Form
    {

        [System.Runtime.InteropServices.DllImport("User32")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [System.Runtime.InteropServices.DllImport("User32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int MOD_SHIFT = 0x4;
        public const int MOD_CONTROL = 0x2;
        public const int MOD_ALT = 0x1;
        public const int WM_HOTKEY = 0x312;
        public const int MOD_WIN = 0x0008;

        public Form1()
        {
            InitializeComponent();
            Form1.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), /*STRG*/ 0x0002, /*F2*/ 0x71);
            this.WindowState = FormWindowState.Minimized;
        }

        protected override void WndProc(ref Message m)
        {

            if (m.Msg == WM_HOTKEY)
            {
                string text = Clipboard.GetText();
                
                OrderPath orderPath = new OrderPath(text);
                OfferPath offerPath = new OfferPath(text);
                string path = null;

                try
                {

                    try
                    {
                        path = orderPath.CanonicalPath();
                    }
                    catch (DirectoryNotFoundException) { }

                    try
                    {
                        path = offerPath.CanonicalPath();
                    }
                    catch (DirectoryNotFoundException) { }

                    if (path != null && path != "")
                    {
                        Process.Start(path);
                    }
                    else
                    {
                        MessageBox.Show("Es konnte kein Ordner ermittelt werden!", text);
                    }
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fehler");
                }
                
                
            }

            base.WndProc(ref m);
        }

        protected void Form1_Load(object sender, EventArgs e)
        {

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            RegisterHotKey(this.Handle, 0, MOD_WIN, (int)Keys.Z);
            RegisterHotKey(this.Handle, 0, MOD_WIN + MOD_CONTROL, (int)Keys.A);
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoImageDeltaApp.Models;
using VideoImageDeltaApp.Forms;

namespace VideoImageDeltaApp
{
    static class Program
    {
        public static List<Video> Videos = new List<Video>();
        public static List<GameProfile> GameProfiles = new List<GameProfile>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}

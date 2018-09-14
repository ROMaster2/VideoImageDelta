using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoImageDeltaApp.Forms;
using VideoImageDeltaApp.Models;

namespace VideoImageDeltaApp
{
    static class Program
    {
        public static List<Video> Videos = new List<Video>();
        public static List<GameProfile> GameProfiles = new List<GameProfile>();

        [STAThread]
        static void Main()
        {
            bool ffExists = RawFFmpeg.FindFFExecutables();

            if (ffExists)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
        }
    }
}

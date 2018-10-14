using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoImageDeltaApp.Forms;
using VideoImageDeltaApp.Models;

namespace VideoImageDeltaApp
{
    static class Program
    {
        public static Processor Processor = new Processor();
        public static List<Video> Videos = new List<Video>();
        public static List<GameProfile> GameProfiles = new List<GameProfile>();
        public static MainWindow MainWindow;
        
        [STAThread]
        static void Main()
        {
            bool ffExists = RawFFmpeg.FindFFExecutables();

            Test.Run();

            if (ffExists)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainWindow = new MainWindow();
                Application.Run(MainWindow);
                //Application.Run(new TestForm());
            }
        }
    }
}

using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using VideoImageDeltaApp;
using VideoImageDeltaApp.Forms;
using VideoImageDeltaApp.Models;

using Screen = VideoImageDeltaApp.Models.Screen;

namespace VideoImageDeltaApp
{
    static class Test
    {
        public static void Run()
        {
            var mi = new MagickImage(@"C:\Users\Administrator\Pictures\VID\splat.png");
            var a = mi.Separate(Channels.Alpha).First().GetPixels().GetValues();
            var c = (255L - a.Average(x => (decimal)x)) / 255L;
            var d = (255d - a.Average(x => (double)x)) / 255d;
        }
    }
}

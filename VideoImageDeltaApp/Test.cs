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
    class Test
    {
        public static void Run()
        {
            var txt = "-01:23:45.67";
            var l = new List<List<char?>>();

            for (int i = 0; i < txt.Length; i++)
            {
                l.Add(new List<char?>() { txt[i], null });
            }

            var test = SuperConcat(0, l).Distinct().ToArray();
            var bools1 = new List<string>();
            for (int i = 0; i < test.Length; i++)
            {
                if (Utilities.ValidateTimeOCR(test[i]))
                {
                    bools1.Add(test[i]);
                }
            }
            var bools2 = new bool[bools1.Count];
            for (int i = 0; i < bools1.Count; i++)
            {
                bools2[i] = TimeSpan.TryParse(bools1[i], out TimeSpan nothing);
            }
        }

        public delegate bool Validator(string str, bool tryhard = false);

        private static IEnumerable<string> SuperConcat(int curChar, List<List<char?>> charss, Validator f = null)
        {
            ConcurrentBag<string> retval = new ConcurrentBag<string>();
            if (curChar == charss.Count)
            {
                retval.Add("");
                return retval;
            }
            foreach (var y in charss[curChar])
            {
                foreach (var x2 in SuperConcat(curChar + 1, charss, f))
                {
                    var str = y + x2;
                    if (f == null || f(str))
                    {
                        retval.Add(str);
                    }
                }
            }
            return retval.AsEnumerable();
        }

    }
}

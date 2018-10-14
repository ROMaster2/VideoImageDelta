using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract;

namespace VideoImageDeltaApp.Models
{
    public static partial class Extensions
    {
        public static ScanResults GetResults(this Tesseract.ResultIterator iter)
        {
            var llsr = new List<CharResult[]>();
            iter.Begin();


            if (iter.TryGetBoundingBox(PageIteratorLevel.Symbol, out Rect symbolBounds))
            {
                var lsr = new List<CharResult>();
                using (var choice = iter.GetChoiceIterator())
                {
                    do
                    {
                        lsr.Add(new CharResult()
                        {
                            Character = choice.GetText().First(),
                            Confidence = choice.GetConfidence()
                        });
                    } while (choice.Next());
                }
                llsr.Add(lsr.ToArray());
            }

            while (!iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Symbol))
            {
                iter.Next(PageIteratorLevel.Symbol);
                if (iter.TryGetBoundingBox(PageIteratorLevel.Symbol, out symbolBounds))
                {
                    var lsr = new List<CharResult>();
                    using (var choice = iter.GetChoiceIterator())
                    {
                        do
                        {
                            lsr.Add(new CharResult()
                            {
                                Character = choice.GetText().First(),
                                Confidence = choice.GetConfidence()
                            });
                        } while (choice.Next());
                    }
                    llsr.Add(lsr.ToArray());
                }
            };

            return new ScanResults() { Results = llsr.ToArray() };
        }

        public struct ScanResults
        {
            public CharResult[][] Results;

            public delegate bool Validator(string str);

            public IEnumerable<string> AllResults(bool includeNull, Validator f = null)
            {
                var charss = new List<List<char?>>();
                for (int a = 0; a < Results.Count(); a++)
                {
                    var chars = new List<char?>();
                    if (includeNull)
                    {
                        chars.Add(null);
                    }
                    for (int b = 0; b < Results[a].Count(); b++)
                    {
                        // Todo: Make parameter instead for portability. (How?)
                        if(Results[a][b].Character == ' ')
                        {
                            chars.Add('.');
                            chars.Add(':');
                        }
                        else
                        {
                            chars.Add(Results[a][b].Character);
                        }
                    }
                    charss.Add(chars);
                }
                return SuperConcat(0, charss, f);
            }

            private static IEnumerable<string> SuperConcat(int curChar, List<List<char?>> charss, Validator f = null)
            {
                ConcurrentBag<string> retval = new ConcurrentBag<string>();
                if (curChar == charss.Count)
                {
                    retval.Add("");
                    return retval;
                }
                Parallel.ForEach<char?>(charss[curChar], (y) =>
                {
                    foreach (var x2 in SuperConcat(curChar + 1, charss, f))
                    {
                        var str = y + x2;
                        if (f == null || f(str))
                        {
                            retval.Add(str);
                        }
                    }
                });
                return retval.AsEnumerable();
            }

            public string BestResult()
            {
                var str = "";
                Results.ToList().ForEach(x => str += x.First().Character);
                return str;
            }

        }

        public struct CharResult
        {
            public char Character;
            public float Confidence;
        }

        public static double TransparencyRate(this ImageMagick.MagickImage mi)
        {
            if (!mi.HasAlpha) return 0;
            var bytes = mi.Separate(ImageMagick.Channels.Alpha).First().GetPixels().GetValues();
            return (255 - bytes.Average(x => (double)x)) / 255;
        }


    }

}
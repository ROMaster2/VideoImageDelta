using System.Collections.Generic;
using System.Linq;
using Tesseract;

namespace VideoImageDeltaApp.Models
{
    public static partial class Extensions
    {
        public static ScanResults GetResults(this Tesseract.ResultIterator iter)
        {
            var llsr = new List<CharResult[]>();
            iter.Begin();


            if (iter.TryGetBoundingBox(PageIteratorLevel.Symbol, out Geometry symbolBounds))
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

            public string BestResult()
            {
                string str = null;
                Results.ToList().ForEach(x => str += x.First().Character);
                return str;
            }

            public string[] AllResults()
            {
                var l = new List<List<char>>();
                for (int a = 0; a < Results.Count(); a++)
                {
                    var l2 = new List<char>();
                    for (int b = 0; b < Results.ElementAt(a).Count(); b++)
                    {
                        l2.Add(Results.ElementAt(a).ElementAt(b).Character);
                    }
                    l.Add(l2);
                }
                return Utilities.Untitled2(l);
            }

        }
        public struct CharResult
        {
            public char Character;
            public float Confidence;
        }
    }

}
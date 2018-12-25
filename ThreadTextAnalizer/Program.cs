using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkkaTextAnalizatorCommon.Utils;

namespace ThreadTextAnalizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var startNew = Stopwatch.StartNew();
            var storage = new TextMessageStorage();
            var textMessages = storage.Get(100);
            startNew.Start();
            Parallel.ForEach(textMessages, tm =>
            {
                AnalizatorEngine.CalculateTotalSpaces(tm.Message, tm.Id);
            });
            startNew.Stop();
            Console.WriteLine($"{startNew.ElapsedMilliseconds}");
            Console.ReadKey();
        }
    }
}

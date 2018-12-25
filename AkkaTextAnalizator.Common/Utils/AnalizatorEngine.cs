using System;
using System.Threading;

namespace AkkaTextAnalizatorCommon.Utils
{
    public static class AnalizatorEngine
    {
        public static int CalculateTotalSpaces(string msg, int id)
        {
            Console.WriteLine($"Message id - {id}");
            Thread.Sleep(1000);
            return default(int);
        }
    }
}
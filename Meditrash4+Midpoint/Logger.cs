using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint
{
    class Logger
    {
        public static void LogE(String s,Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(s);
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(e.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogE(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Log(String s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(s);
        }
        public static void Log(String s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(s);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

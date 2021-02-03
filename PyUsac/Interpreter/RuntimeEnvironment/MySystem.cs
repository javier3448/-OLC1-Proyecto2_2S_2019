using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.RuntimeEnvironment
{
    public static class MySystem
    {
        public static Console Console { get; } = Console.Instance;
        public static Logger Logger { get; } = Logger.Instance;

        public static void SetPrint(PrintMethod printMethod)
        {
            Console.Print = printMethod;
        }

        public static void SetPrintLine(PrintLineMethod printLineMethod)
        {
            Console.PrintLine = printLineMethod;
        }

        public static void SetLog(LogMethod logMethod)
        {
            Logger.Log = logMethod;
        }
    }
}

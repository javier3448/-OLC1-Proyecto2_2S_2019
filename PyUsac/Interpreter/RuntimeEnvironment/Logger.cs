using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.RuntimeEnvironment
{
    public class Logger
    {
        public static Logger Instance { get; } = new Logger();
        private Logger()
        {
            Log = DoLog;
        }

        public LogMethod Log;

        /// <summary>
        /// Default Log method.
        /// </summary>
        /// <param name="obj"></param>
        private void DoLog(MyError error)
        {
            System.Console.Write(error.ToString());
        }
    }
}

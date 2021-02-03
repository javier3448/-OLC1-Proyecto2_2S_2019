using _Compi1_Proyecto2.Graphviz;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.RuntimeEnvironment
{
    public class Console
    {
        internal static Console Instance { get; } = new Console();
        private Console()
        {
            Print = DoPrint;
            PrintLine = DoPrintLine;
            Alert = DoAlert;
            Graph = DoGraph;
        }

        public PrintMethod Print;
        public PrintLineMethod PrintLine;
        public AlertMethod Alert;
        public GraphMethod Graph;

        /// <summary>
        /// Default Print method.
        /// </summary>
        /// <param name="obj"></param>
        private void DoPrint(PyObj obj)
        {
            System.Console.Write(obj.MyToString());
        }
        /// <summary>
        /// Default PrintLine method.
        /// </summary>
        /// <param name="obj"></param>
        private void DoPrintLine(PyObj obj)
        {
            System.Console.WriteLine(obj.MyToString());
        }

        private void DoAlert(PyObj obj)
        {
            MessageBox.Show(obj.MyToString());
        }

        private string DoGraph(MyString name, MyString dotSrc)
        {
            PrintLine(name);
            PrintLine(dotSrc);
            return null;
        }
    }
}

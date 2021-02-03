using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.RuntimeEnvironment
{
    public delegate void PrintMethod(PyObj obj);
    public delegate void PrintLineMethod(PyObj obj);
    
    public delegate void AlertMethod(PyObj obj);

    /// <summary>
    /// si retorna null se toma como si no hubo error al hacer el graph, si no el string retornado es el mensaje del error reportado
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dotSrc"></param>
    public delegate string GraphMethod(MyString name, MyString dotSrc);

    public delegate void LogMethod(MyError error);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers
{
    public class Breaker : Jumper
    {
        public override JumperType GetJumperType()
        {
            return JumperType.Breaker;
        }
    }
}

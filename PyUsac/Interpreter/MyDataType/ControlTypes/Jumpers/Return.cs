using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers
{
    public class Return : Jumper
    {
        public PyObj Obj { get; private set; }

        public Return() { }

        public Return(PyObj obj)
        {
            Obj = obj;
        }

        public override JumperType GetJumperType()
        {
            if (Obj == null)
                return JumperType.VoidReturn;
            return JumperType.Return;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers
{
    public abstract class Jumper : Word
    {
        public bool Popped { get; set; } = false;

        public override bool IsError()
        {
            return false;
        }

        public override bool IsMemoryBlock()
        {
            return false;
        }

        public override bool IsJumper()
        {
            return true;
        }

        public bool WasPopped()
        {
            return Popped;
        }

        public abstract JumperType GetJumperType();
    }

    public enum JumperType
    {
        VoidReturn,
        Return,
        Breaker,
        Continue
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop
{
    public abstract class Definition : Word
    {
        public sealed override bool IsError()
        {
            return false;
        }

        public sealed override bool IsJumper()
        {
            return false;
        }

        public sealed override bool IsMemoryBlock()
        {
            return false;
        }

        public abstract DefinitionType GetDefinitionType();
    }

    public enum DefinitionType
    {
        NA,
        Class,
        Method,
        Function
    }
}

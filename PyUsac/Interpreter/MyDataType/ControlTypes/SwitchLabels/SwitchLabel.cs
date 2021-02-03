using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.SwitchLabels
{
    public class SwitchLabel : Word
    {
        public PyObj Value { get; private set; }

        public SwitchLabel() { }

        public SwitchLabel(PyObj val)
        {
            Value = val;
        }

        public LabelType GetLabelType()
        {
            if (Value == null)
                return LabelType.Default;
            return LabelType.Case;
        }

        public override bool IsError()
        {
            return false;
        }

        public override bool IsJumper()
        {
            return false;
        }

        public override bool IsMemoryBlock()
        {
            return false;
        }
    }
    public enum LabelType
    {
        Case,
        Default
    }
}


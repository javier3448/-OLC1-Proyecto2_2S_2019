using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType
{
    public class MemoryBlock : Word
    {
        private PyObj _value;

        /// <summary>
        /// get to Dereference pointer.
        /// set to make it point to a new value
        /// </summary>
        public PyObj Value 
        { 
            get { return _value; }
            set {
                if (value.IsNull())
                    _value = MyNull.GetInstance();
                if (value.IsPrimitive())
                    _value = MyPrimitiveFactory.CreateCopy((MyPrimitive)value);
                else//Only option left is for value to be customInstance or myArray
                    _value = value;
            } 
        }

        /// <summary>
        /// "Crea una nueva direccione en memoria"
        /// </summary>
        /// <param name="obj"></param>
        public MemoryBlock(PyObj obj)
        {
            Value = obj;
        }

        //Crea una nueva direccion en memoria apuntando a null
        public MemoryBlock()
        {
            Value = MyNull.GetInstance();
        }

        public override bool IsError()
        {
            return false;
        }

        public override bool IsMemoryBlock()
        {
            return true;
        }

        public override bool IsJumper()
        {
            return false;
        }
    }
}

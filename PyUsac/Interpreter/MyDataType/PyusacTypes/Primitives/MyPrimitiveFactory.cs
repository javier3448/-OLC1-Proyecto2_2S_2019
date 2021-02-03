using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives
{
    public static class MyPrimitiveFactory
    {
        public static MyPrimitive Create(object obj)
        {
            if (obj is string)
                return new MyString((string)obj);
            if (obj is bool)
                return new MyBoolean((bool)obj);
            if (obj is int)
                return new MyInt((int)obj);
            if (obj is char)
                return new MyChar((char)obj);
            if (obj is double)
                return new MyDouble((double)obj);
            throw new Exception("Se esperaba string, double, int, bool o char " + obj.GetType().Name);
        }

        public static MyPrimitive CreateCopy(MyPrimitive primitiveObj)
        {
            var typeId = primitiveObj.GetMyType();
            switch (typeId)
            {
                case TypeConstants.BOOLEAN:
                    return new MyBoolean(((MyBoolean)primitiveObj).Bool);
                case TypeConstants.INT:
                    return new MyInt(((MyInt)primitiveObj).Int);
                case TypeConstants.CHAR:
                    return new MyChar(((MyChar)primitiveObj).CharValue);
                case TypeConstants.DOUBLE:
                    return new MyDouble(((MyDouble)primitiveObj).DoubleValue);
                case TypeConstants.STRING:
                    return new MyString(((MyString)primitiveObj).StringValue);
                default:
                    throw new Exception("Tipo de primitivo no valido id: " + typeId);
            }
        }
    }
}

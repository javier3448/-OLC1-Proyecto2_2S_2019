using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes
{
    public class MyNull : PyObj
    {
        private MyNull() 
        {
            Calculator = NullOperation.GetInstance();
        }

        private static MyNull Instance = new MyNull();

        public static MyNull GetInstance()
        {
            return Instance;
        }

        public override Word GetMember(MemberSegment memberSegment)
        {
            return new MyError("Null Pointer exception!");
        }

        public override int GetMyType()
        {
            return TypeConstants.NULL;
        }

        public override bool IsNull()
        {
            return true;
        }
        public override bool IsPrimitive()
        {
            return false;
        }

        public override string MyToString()
        {
            return "MyNull";
        }
    }
}

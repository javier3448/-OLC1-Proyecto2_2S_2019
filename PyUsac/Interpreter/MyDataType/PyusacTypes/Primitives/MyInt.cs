using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives
{
    class MyInt : MyPrimitive
    {
        public readonly int Int;

        public MyInt(int number)
        {
            Int = number;
            Calculator = IntOperation.GetInstance();
        }

        public override Word GetMember(MemberSegment memberSegment)
        {
            switch (memberSegment.GetSegmentType())
            {
                case SegmentType.Identifier:
                    return ErrorFactory.BadAttributeError(this, (IdentifierSegment)memberSegment);
                case SegmentType.Index:
                    return ErrorFactory.BadIndexError(this, (IndexSegment)memberSegment);
                case SegmentType.Procedure:
                    return ErrorFactory.BadProcedureError(this, (ProcedureSegment)memberSegment);
                default:
                    throw new Exception("tipo de member segment no valido: " + memberSegment.GetSegmentType().ToString());
            }
        }

        public override int GetMyType()
        {
            return TypeConstants.INT;
        }

        public override string MyToString()
        {
            return Int.ToString();
        }
    }
}

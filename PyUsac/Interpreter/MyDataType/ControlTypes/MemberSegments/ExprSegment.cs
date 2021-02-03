using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments
{
    public class ExprSegment : CreationSegment
    {
        public PyObj Expr { get; private set; }

        public ExprSegment(PyObj pyObj)
        {
            Expr = pyObj;
        }

        public override SegmentType GetSegmentType()
        {
            return SegmentType.Expr;
        }
    }
}

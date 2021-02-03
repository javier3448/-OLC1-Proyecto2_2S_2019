using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments
{
    public abstract class MemberSegment : Segment
    {
        public sealed override bool IsCreationSegment()
        {
            return false;
        }
    }
}

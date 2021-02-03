using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments
{
    public class IdentifierSegment : MemberSegment
    {
        public string Id { get; private set; }

        public IdentifierSegment(string id)
        {
            Id = id;
        }

        public override SegmentType GetSegmentType()
        {
            return SegmentType.Identifier;
        }
    }
}

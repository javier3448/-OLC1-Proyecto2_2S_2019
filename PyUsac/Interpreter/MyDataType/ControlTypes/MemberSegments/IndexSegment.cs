using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments
{
    public class IndexSegment : MemberSegment
    {
        public PyObj Index { get; private set; }
        
        public IndexSegment(PyObj pyObj)
        {
            Index = pyObj;
        }

        public override SegmentType GetSegmentType()
        {
            return SegmentType.Index;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Cambiarle el nombre a este namespace a Segments
namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments
{
    public abstract class CreationSegment : Segment
    {
        public override bool IsCreationSegment()
        {
            return true;
        }
    }
}

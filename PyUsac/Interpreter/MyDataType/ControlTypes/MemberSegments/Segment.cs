using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments
{
    public abstract class Segment : Word
    {
        public abstract SegmentType GetSegmentType();

        public abstract bool IsCreationSegment();//Chapuz medio bajo para que sirva de algo la clase CreationSegment y MemberSegment tenga algo que hacer

        public override sealed bool IsError()
        {
            return false;
        }

        public override sealed bool IsJumper()
        {
            return false;
        }

        public override sealed bool IsMemoryBlock()
        {
            return false;
        }
    }

    public enum SegmentType
    {
        Identifier,
        Index,
        Procedure,
        ObjectCreation,
        Expr
    }
}

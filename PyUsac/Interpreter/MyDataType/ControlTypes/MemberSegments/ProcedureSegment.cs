using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments
{
    public class ProcedureSegment : MemberSegment
    {
        public string Id { get; private set; }
        public List<MemoryBlock> Arguments { get; private set; }

        public int CountArguments { 
            get
            {
                return Arguments.Count;
            } 
        }

        public ProcedureSegment(string id, ICollection<MemoryBlock> arguments)
        {
            Id = id;
            Arguments = new List<MemoryBlock>(arguments);
        }

        //llena los agumentos con myNull, sirve para llamar y buscar procedures desde c#, hardcoded
        public static ProcedureSegment EmptyProcedureSegment(string id, int argumentCount)
        {
            var arguments = new List<MemoryBlock>(argumentCount);
            for (int i = 0; i < argumentCount; i++)
            {
                arguments.Add(new MemoryBlock(MyNull.GetInstance()));
            }
            return new ProcedureSegment(id, arguments);
        }

        public override SegmentType GetSegmentType()
        {
            return SegmentType.Procedure;
        }
    }
}

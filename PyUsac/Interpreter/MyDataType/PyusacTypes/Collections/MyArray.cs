using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Collections
{
    public class MyArray : PyObj
    {
        public MemoryBlock[] Array { get; private set; }

        public int Length { get { return Array.Length; } }

        public MyArray(int size)
        {
            Array = new MemoryBlock[size];
            for (int i = 0; i < size; i++)
            {
                Array[i] = new MemoryBlock();
            }
            //Chapuz medio bajo. Deberia de tener otro IOperates para exclusivo para MyArray
            Calculator = CustomInstanceOperation.GetInstance();
        }
        public bool IsNDimensionalArray(params int[] dimensions)
        {
            if (Length != dimensions.First())
                return false;
            if (dimensions.Length < 2)
                return true;

            MyArray innerArray;
            dimensions = dimensions.RemoveFirst();
            foreach (var memBlock in Array)
            {
                if (memBlock.Value.GetMyType() != TypeConstants.ARRAY)
                    return false;
                innerArray = (MyArray)memBlock.Value;
                
                if (!innerArray.IsNDimensionalArray(dimensions))
                    return false;    
            }
            return true;
        }

        public override Word GetMember(MemberSegment memberSegment)
        {
            IdentifierSegment identSegment;
            IndexSegment indexSegment;
            ProcedureSegment functionSegment;
            switch (memberSegment.GetSegmentType())
            {
                case SegmentType.Identifier:
                    identSegment = (IdentifierSegment)memberSegment;
                    return ErrorFactory.BadAttributeError(this, (IdentifierSegment)memberSegment);
                case SegmentType.Index:
                    indexSegment = (IndexSegment)memberSegment;
                    var pyObj = indexSegment.Index;
                    var pyObjType = pyObj.GetMyType();
                    if (pyObjType != TypeConstants.INT && pyObjType != TypeConstants.CHAR)
                        return ErrorFactory.BadIndexError(this, indexSegment);
                    int index;
                    if (pyObjType == TypeConstants.INT)
                        index = ((MyInt)pyObj).Int;
                    else
                        index = ((MyChar)pyObj).CharValue;

                    if (index >= Array.Length || index < 0)
                        return ErrorFactory.OutOfBounds(this, index, 0, Array.Length - 1);

                    return Array[index];
                case SegmentType.Procedure:
                    functionSegment = (ProcedureSegment)memberSegment;
                    return ErrorFactory.BadAttributeError(this, (IdentifierSegment)memberSegment);
                default:
                    throw new Exception("tipo de member segment no valido");
            }
        }

        public override int GetMyType()
        {
            return TypeConstants.ARRAY;
        }

        public sealed override bool IsNull()
        {
            return false;
        }

        public sealed override bool IsPrimitive()
        {
            return false;
        }

        public override string MyToString()
        {
            var sb = new StringBuilder("{ ");
            foreach (var memBlock in Array)
            {
                sb.Append(memBlock.Value.MyToString() + ", ");
            }
            if (sb.Length - 2 > 0)
                sb.Remove(sb.Length - 2, 1);
            sb.Append("}");
            return sb.ToString();
        }
    }
}

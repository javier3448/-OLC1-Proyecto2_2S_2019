using _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop
{
    /// <summary>
    /// Wrapper de InstanceVisitor, representa un 'entorno estatico'
    /// </summary>
    public class ClassInstance : PyObj
    {
        private InstanceVisitor Visitor;
        private ClassDefinition ClassDefinition;//Contien las variables estaticas tambien
        internal ClassInstance(InstanceVisitor visitor, ClassDefinition classDefinition)//Solo puede ser usado por class definition
        {
            Visitor = visitor;
            ClassDefinition = classDefinition;
            Calculator = CustomInstanceOperation.GetInstance();
        }

        public override Word GetMember(MemberSegment memberSegment)
        {
            if (Visitor == null)
                throw new Exception("Tiene que inicializar visitor con InitVisitor antes de usar cualquier GetMember");
            Word result;
            switch (memberSegment.GetSegmentType())
            {
                case SegmentType.Identifier:
                    result = Visitor.GetGlobalAttribute((IdentifierSegment)memberSegment);
                    if (ErrorHelper.IsError(result))//se modifica el mensaje
                        return ErrorFactory.BadAttributeError(this, (IdentifierSegment)memberSegment);//Chapuz alto: es el unico caso en el que no se retoran el mismo error de un result o se modifica el mismo. I.e. No se puede decir que todo MyError puede ser reportado
                    return result;
                case SegmentType.Procedure:
                    result = Visitor.InvokeProcedure((ProcedureSegment)memberSegment);
                    if (ErrorHelper.IsError(result))//se modifica el mensaje
                        return ErrorFactory.BadProcedureError(this, (ProcedureSegment)memberSegment);
                    return result;
                case SegmentType.Index:
                    return ErrorFactory.BadIndexError(this, (IndexSegment)memberSegment);
                default:
                    throw new Exception("Member segment no valido para ClassInstance.GetMember" + memberSegment.GetSegmentType());
            }
        }

        public override int GetMyType()
        {
            return ClassDefinition.MyType;
        }

        public override bool IsNull()
        {
            return false;
        }

        public override bool IsPrimitive()
        {
            return false;
        }

        public override string MyToString()
        {
            var sb = new StringBuilder("[ ");
            foreach (var attribute in Visitor.GetAttributeValues())
            {
                sb.Append(attribute.MyToString() + ", ");
            }
            if (sb.Length - 2 > 0)
                sb.Remove(sb.Length - 2, 1);
            sb.Append("]");
            return sb.ToString();
        }
    }
}

using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Collections;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes
{
    public abstract class PyObj : Word
    {
        protected IOperates Calculator;

        public sealed override bool IsError()
        {
            return false;
        }

        public abstract int GetMyType();

        public virtual string MyToString()
        {
            return this.ToString();
        }

        //TODO: ver si no hay una mejor manera de implementar estos atributos definidos por cada clase y poner que todos los primitivos hereden de una clase que tenga este metodo como sealed
        public abstract bool IsNull();

        public abstract bool IsPrimitive();


        //Operaciones
        public virtual Word BinaryOperation(BinaryOperator op, PyObj obj0)
        {
            switch (op)
            {
                case BinaryOperator.Plus:
                    return Calculator.Plus(this, obj0);
                case BinaryOperator.Minus:
                    return Calculator.Minus(this, obj0);
                case BinaryOperator.Mult:
                    return Calculator.Mult(this, obj0);
                case BinaryOperator.Div:
                    return Calculator.Div(this, obj0);
                case BinaryOperator.Pow:
                    return Calculator.Pow(this, obj0);
                case BinaryOperator.GreaterThan:
                    return Calculator.GreaterThan(this, obj0);
                case BinaryOperator.LessThan:
                    return Calculator.LessThan(this, obj0);
                case BinaryOperator.PyEquals:
                    return Calculator.PyEquals(this, obj0);
                case BinaryOperator.PyNotEquals:
                    return Calculator.PyNotEquals(this, obj0);
                case BinaryOperator.GreaterOrEqualTo:
                    return Calculator.GreaterOrEqualTo(this, obj0);
                case BinaryOperator.LessOrEqualTo:
                    return Calculator.LessOrEqualTo(this, obj0);
                case BinaryOperator.And:
                    return Calculator.And(this, obj0);
                case BinaryOperator.Or:
                    return Calculator.Or(this, obj0);
                case BinaryOperator.Xor:
                    return Calculator.Xor(this, obj0);
                default:
                    //TODO: Tirar una mejor exception, no una generica
                    throw new Exception("Operador Binario no valido: " + op.ToString());
            }
        }
        public virtual Word UnaryOperation(UnaryOperator op)
        {
            switch (op)
            {
                case UnaryOperator.Minus:
                    return Calculator.Minus(this);
                case UnaryOperator.Not:
                    return Calculator.Not(this);
                default:
                    throw new Exception("Operador Unario no valido: " + op.ToString());
            }
        }

        //Puede retorna error, memoryblock o pyObj
        public abstract Word GetMember(MemberSegment memberSegment);

        public override sealed bool IsMemoryBlock()
        {
            return false;
        }

        public override sealed bool IsJumper()
        {
            return false;
        }

        public PyObj PreBinaryOperation(BinaryOperator binaryOperator)
        {
            return Calculator.PreBinaryOperation(this, binaryOperator);
        }

        public static class ErrorFactory
        {

            public static MyError BadIndexError(PyObj pyObj, IndexSegment indexSegment)
            {
                var indexValue = indexSegment.Index;
                return new MyError("El indice: " + indexValue.MyToString() + " (" + TypeConstants.GetMyTypeName(indexValue.GetMyType()) + ") no es un indice valido para el tipo: " + TypeConstants.GetMyTypeName(pyObj.GetMyType()) + " valor: " + pyObj.MyToString()); ;
            }

            public static MyError BadProcedureError(PyObj pyObj, ProcedureSegment procedureSegment)
            {
                return new MyError(String.Format("La funcion o metodo: {0} con: {1} argumentos no es una funcion o metodo valida para el tipo: {2}",
                    procedureSegment.Id, 
                    procedureSegment.CountArguments,
                    TypeConstants.GetMyTypeName(pyObj.GetMyType()))
                    );
            }

            public static MyError BadAttributeError(PyObj pyObj, IdentifierSegment identifierSegment)
            {
                return new MyError(String.Format("El atributo: {0} no existe en el tipo: {1}",
                    identifierSegment.Id,
                    TypeConstants.GetMyTypeName(pyObj.GetMyType()))
                    );
            }

            public static MyError OutOfBounds(PyObj pyObj, int index, int lowerBound, int upperBound)
            {
                return new MyError(String.Format("Indice fuera de rango: {0}. rango esperado: [{1}-{2}]",
                        index,
                        lowerBound,
                        upperBound)
                    );
            }
        }
    }
}

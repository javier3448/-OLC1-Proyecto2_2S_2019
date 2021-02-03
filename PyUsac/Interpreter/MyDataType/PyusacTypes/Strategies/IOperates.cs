using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies
{
     public abstract class IOperates
     {
        //Binary exprs
        public abstract Word Plus(PyObj obj0, PyObj obj1);
        public abstract Word Minus(PyObj obj0, PyObj obj1);
        public abstract Word Mult(PyObj obj0, PyObj obj1);
        public abstract Word Div(PyObj obj0, PyObj obj1);
        public abstract Word Pow(PyObj obj0, PyObj obj1);
        public abstract Word GreaterThan(PyObj obj0, PyObj obj1);
        public abstract Word LessThan(PyObj obj0, PyObj obj1);
        public abstract Word PyEquals(PyObj obj0, PyObj obj1);
        public abstract Word PyNotEquals(PyObj obj0, PyObj obj1);
        public abstract Word GreaterOrEqualTo(PyObj obj0, PyObj obj1);
        public abstract Word LessOrEqualTo(PyObj obj0, PyObj obj1);
        public abstract Word And(PyObj obj0, PyObj obj1);
        public abstract Word Or(PyObj obj0, PyObj obj1);
        public abstract Word Xor(PyObj obj0, PyObj obj1);

        //unary expression
        public abstract Word Not(PyObj obj0);
        public abstract Word Minus(PyObj obj0);

        //preoperations
        public virtual PyObj PreBinaryOperation(PyObj obj0, BinaryOperator binaryOperator)
        {
            return null;
        }

        public virtual MyError BinaryError(PyObj obj0, BinaryOperator op, PyObj obj1)
        {
            return new MyError(string.Format("No se puede realizar la operacion binaria: {0} ({1}).  Con tipos: <{2}, {3}> y valores: <{4}, {5}>"
                , op.ToString()
                , op.ToStringSymbol()
                , TypeConstants.GetMyTypeName(obj0.GetMyType())
                , TypeConstants.GetMyTypeName(obj1.GetMyType())
                , obj0.MyToString()
                , obj1.MyToString()
                ));
        }

        public virtual MyError UnaryError(UnaryOperator op, PyObj obj0)
        {
            return new MyError(string.Format("No se puede realizar la operacion unaria: {0} ({1}).  Con tipo: <{2}> y valor: <{3}>"
                , op.ToString()
                , op.ToStringSymbol()
                , TypeConstants.GetMyTypeName(obj0.GetMyType())
                , obj0.MyToString()
                ));
        }
    }
}

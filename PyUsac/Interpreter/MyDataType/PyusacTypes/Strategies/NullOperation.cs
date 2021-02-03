using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies
{
    public class NullOperation : IOperates
    {
        private static readonly NullOperation Instance = new NullOperation();
        public static NullOperation GetInstance()
        {
            return Instance;
        }
        private NullOperation() { }

        #region Operaciones validas
        public override Word PyEquals(PyObj obj0, PyObj obj1)
        {
            return new MyBoolean(obj1.IsNull());
        }
        public override Word PyNotEquals(PyObj obj0, PyObj obj1)
        {
            return new MyBoolean(!obj1.IsNull());
        }
        #endregion

        #region Operaciones no validas
        public override Word Plus(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.Plus, obj1);
        }
        public override Word Minus(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.Minus, obj1);
        }
        public override Word Mult(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.Mult, obj1);
        }
        public override Word Div(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.Div, obj1);
        }
        public override Word Pow(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.Pow, obj1);
        }
        public override Word GreaterThan(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.GreaterThan, obj1);
        }
        public override Word LessThan(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.LessThan, obj1);
        }
        public override Word GreaterOrEqualTo(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.GreaterOrEqualTo, obj1);
        }
        public override Word LessOrEqualTo(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.LessOrEqualTo, obj1);
        }
        public override Word And(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.And, obj1);
        }
        public override Word Or(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.Or, obj1);
        }
        public override Word Xor(PyObj obj0, PyObj obj1)
        {
            return BinaryError(obj0, BinaryOperator.Xor, obj1);
        }

        public override Word Not(PyObj obj0)
        {
            return UnaryError(UnaryOperator.Not, obj0);
        }
        public override Word Minus(PyObj obj0)
        {
            return UnaryError(UnaryOperator.Minus, obj0);
        }
        #endregion
    }
}

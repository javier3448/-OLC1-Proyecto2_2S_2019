using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies
{
    public class BoolOperation : IOperates
    {
        
        private static readonly BoolOperation Instance = new BoolOperation();

        public static BoolOperation GetInstance()
        {
            return Instance;
        }
        private BoolOperation() { }

        #region Operaciones validas

        public override PyObj PreBinaryOperation(PyObj obj0, BinaryOperator binaryOperator)
        {
            var myBool = (MyBoolean)obj0;
            switch (binaryOperator)
            {
                case BinaryOperator.And:
                    if (myBool.Bool == false)
                        return new MyBoolean(false);
                    return null;
                case BinaryOperator.Or:
                    if (myBool.Bool == true)
                        return new MyBoolean(true);
                    return null;
                default:
                    return null;
            }
        }

        public override Word Plus(PyObj obj0, PyObj obj1)
        {
            if (obj1.GetMyType() == TypeConstants.STRING)
            {
                return new MyString(((MyBoolean)obj0).MyToString() + obj1.MyToString());
            }
            return BinaryError(obj0, BinaryOperator.Plus, obj1);
        }
        public override Word PyEquals(PyObj obj0, PyObj obj1)
        {
            if (obj1.IsNull())
            {
                return new MyBoolean(false);
            }
            if (obj1.GetMyType() == TypeConstants.BOOLEAN)
            {
                return new MyBoolean(((MyBoolean)obj0).Bool == ((MyBoolean)obj1).Bool);
            }
            return BinaryError(obj0, BinaryOperator.PyEquals, obj1);
        }
        public override Word PyNotEquals(PyObj obj0, PyObj obj1)
        {
            if (obj1.IsNull())
            {
                return new MyBoolean(true);
            }
            if (obj1.GetMyType() == TypeConstants.BOOLEAN)
            {
                return new MyBoolean(((MyBoolean)obj0).Bool != ((MyBoolean)obj1).Bool);
            }
            return BinaryError(obj0, BinaryOperator.PyNotEquals, obj1);
        }
        public override Word And(PyObj obj0, PyObj obj1)
        {
            if (obj1.GetMyType() == TypeConstants.BOOLEAN)
            {
                return new MyBoolean(((MyBoolean)obj0).Bool && ((MyBoolean)obj1).Bool);
            }
            return BinaryError(obj0, BinaryOperator.And, obj1);
        }
        public override Word Or(PyObj obj0, PyObj obj1)
        {
            if (obj1.GetMyType() != TypeConstants.BOOLEAN)
            {
                return BinaryError(obj0, BinaryOperator.Or, obj1);
            }
            return new MyBoolean(((MyBoolean)obj0).Bool || ((MyBoolean)obj1).Bool);
        }
        public override Word Xor(PyObj obj0, PyObj obj1)
        {
            if (obj1.GetMyType() == TypeConstants.BOOLEAN)
            {
                return new MyBoolean(((MyBoolean)obj0).Bool ^ ((MyBoolean)obj1).Bool);
            }
            return BinaryError(obj0, BinaryOperator.Xor, obj1);
        }

        public override Word Not(PyObj obj0)
        {
            return new MyBoolean(!((MyBoolean)obj0).Bool);
        }
        #endregion

        #region Operaciones no validas
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
        public override Word Minus(PyObj obj0)
        {
            return UnaryError(UnaryOperator.Minus, obj0);
        }
        #endregion
    }
}

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
    public class DoubleOperation : IOperates
    {
        private static readonly DoubleOperation Instance = new DoubleOperation();

        public static DoubleOperation GetInstance()
        {
            return Instance;
        }
        private DoubleOperation() { }

        #region Operaciones validas
        public override Word Plus(PyObj obj0, PyObj obj1)
        {
            if (obj1.GetMyType() == TypeConstants.STRING)
            {
                return new MyString(obj0.MyToString() + obj1.MyToString());
            }
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyDouble(doubleObj0 + intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyDouble(doubleObj0 + intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyDouble(doubleObj0 + doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.Plus, obj1);
        }
        public override Word Minus(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyDouble(doubleObj0 - intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyDouble(doubleObj0 - intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyDouble(doubleObj0 - doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.Minus, obj1);
        }
        public override Word Mult(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyDouble(doubleObj0 * intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyDouble(doubleObj0 * intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyDouble(doubleObj0 * doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.Mult, obj1);
        }
        public override Word Div(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                if (intObj1 == 0)
                    return BinaryError(obj0, BinaryOperator.Div, obj1);
                return new MyDouble(doubleObj0 / intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                if (intObj1 == 0)
                    return BinaryError(obj0, BinaryOperator.Div, obj1);
                return new MyDouble(doubleObj0 / intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                if (doubleObj1 == 0)
                    return BinaryError(obj0, BinaryOperator.Div, obj1);
                return new MyDouble(doubleObj0 / doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.Div, obj1);
        }
        public override Word Pow(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyDouble(Math.Pow(doubleObj0, intObj1));
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyDouble(Math.Pow(doubleObj0, intObj1));
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyDouble(Math.Pow(doubleObj0, doubleObj1));
            }
            return BinaryError(obj0, BinaryOperator.Pow, obj1);
        }
        public override Word GreaterThan(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyBoolean(doubleObj0 > intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyBoolean(doubleObj0 > intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyBoolean(doubleObj0 > doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.GreaterThan, obj1);
        }
        public override Word LessThan(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyBoolean(doubleObj0 < intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyBoolean(doubleObj0 < intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyBoolean(doubleObj0 < doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.LessThan, obj1);
        }
        public override Word PyEquals(PyObj obj0, PyObj obj1)
        {
            if (obj1.IsNull())
            {
                return new MyBoolean(false);
            }
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyBoolean(doubleObj0 == intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyBoolean(doubleObj0 == intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyBoolean(doubleObj0 == doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.PyEquals, obj1);
        }
        public override Word PyNotEquals(PyObj obj0, PyObj obj1)
        {
            if (obj1.IsNull())
            {
                return new MyBoolean(true);
            }
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyBoolean(doubleObj0 != intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyBoolean(doubleObj0 != intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyBoolean(doubleObj0 != doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.PyNotEquals, obj1);
        }
        public override Word GreaterOrEqualTo(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyBoolean(doubleObj0 >= intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyBoolean(doubleObj0 >= intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyBoolean(doubleObj0 >= doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.GreaterOrEqualTo, obj1);
        }
        public override Word LessOrEqualTo(PyObj obj0, PyObj obj1)
        {
            var doubleObj0 = ((MyDouble)obj0).DoubleValue;
            if (obj1.GetMyType() == TypeConstants.CHAR)
            {
                var intObj1 = (int)((MyChar)obj1).CharValue;
                return new MyBoolean(doubleObj0 <= intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.INT)
            {
                var intObj1 = ((MyInt)obj1).Int;
                return new MyBoolean(doubleObj0 <= intObj1);
            }
            if (obj1.GetMyType() == TypeConstants.DOUBLE)
            {
                var doubleObj1 = ((MyDouble)obj1).DoubleValue;
                return new MyBoolean(doubleObj0 <= doubleObj1);
            }
            return BinaryError(obj0, BinaryOperator.LessOrEqualTo, obj1);
        }
        public override Word Minus(PyObj obj0)
        {
            return new MyDouble(-((MyDouble)obj0).DoubleValue);
        }
        #endregion

        #region Operaciones no validas
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
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Ast.Base
{
    public enum NodeType
    {
        AtomicExpr,
        BinaryExpr,
        IncDecExpr,
        UnaryExpr,
        ArrayExpr,
        Class,
        Method,
        Function,
        ProgramNode,
        StmtList,
        Block,
        While,
        DoWhile,
        ForNode,
        IfNode,
        SwitchLabel,
        Switch,
        Break,
        Continue,
        Return,
        Assignment,
        Declaration,
        FunctionAccess,
        IdentifierAccess,
        IndexAccess,
        MemberAccess,
        Alert,
        Graph,
        Log,
        BooleanLiteralNode,
        CharLiteralNode,
        NullLiteralNode,
        IdentifierNode,
        NumberLiteralNode,
        StringLiteralNode,
        ObjectCreationAccess,
        AstTransient,
        ImportNode
    }

    public enum BinaryOperator
    {
        Plus,
        Minus,
        Mult,
        Div,
        Pow,
        GreaterThan,
        LessThan,
        PyEquals,
        PyNotEquals,
        GreaterOrEqualTo,
        LessOrEqualTo,
        And,
        Or,
        Xor
    }

    public enum UnaryOperator
    {
        Minus,
        Not
    }

    public enum IncDecOperator
    {
        PlusPlus,
        MinusMinus
    }

    public static class BinaryOperatorExtensions
    {
        public static string ToStringSymbol(this BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Plus:
                    return "+";
                case BinaryOperator.Minus:
                    return "-";
                case BinaryOperator.Mult:
                    return "*";
                case BinaryOperator.Div:
                    return "/";
                case BinaryOperator.Pow:
                    return "pow";
                case BinaryOperator.GreaterThan:
                    return ">";
                case BinaryOperator.LessThan:
                    return "<";
                case BinaryOperator.PyEquals:
                    return "==";
                case BinaryOperator.PyNotEquals:
                    return "<>";
                case BinaryOperator.GreaterOrEqualTo:
                    return ">=";
                case BinaryOperator.LessOrEqualTo:
                    return "<=";
                case BinaryOperator.And:
                    return "&&";
                case BinaryOperator.Or:
                    return "||";
                case BinaryOperator.Xor:
                    return "^";
                default:
                    throw new Exception("Operador: " + op.ToString() + " no se puede pasar a symbolString");
            }
        }

        /// <summary>
        /// NOT SAFE. TIRA EXCEPTION SI NO EL STRING NO ES UN BINARY OPERATOR VALIDO
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static BinaryOperator SymbolToBinaryOperator(this string s)
        {
            switch (s)
            {
                case "+":
                    return BinaryOperator.Plus;
                case "-":
                    return BinaryOperator.Minus;
                case "*":
                    return BinaryOperator.Mult;
                case "/":
                    return BinaryOperator.Div;
                case "pow":
                    return BinaryOperator.Pow;
                case ">":
                    return BinaryOperator.GreaterThan;
                case "<":
                    return BinaryOperator.LessThan;
                case "==":
                    return BinaryOperator.PyEquals;
                case "<>":
                    return BinaryOperator.PyNotEquals;
                case ">=":
                    return BinaryOperator.GreaterOrEqualTo;
                case "<=":
                    return BinaryOperator.LessOrEqualTo;
                case "&&":
                    return BinaryOperator.And;
                case "||":
                    return BinaryOperator.Or;
                case "^":
                    return BinaryOperator.Xor;
                default:
                    throw new Exception("String: " + s + " no se puede pasar a BinaryOperator");
            }
        }
    }

    public static class UnaryOperatorExtension
    {
        public static string ToStringSymbol(this UnaryOperator op)
        {
            switch (op)
            {
                case UnaryOperator.Minus:
                    return "-";
                case UnaryOperator.Not:
                    return "!";
                default:
                    throw new Exception("Operador: " + op.ToString() + " no se puede pasar a symbolString");
            }
        }

        /// <summary>
        /// NOT SAFE. TIRA EXCEPTION SI NO EL STRING NO ES UN BINARY OPERATOR VALIDO
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static UnaryOperator SymbolToUnaryOperator(this string s)
        {
            switch (s)
            {
                case "-":
                    return UnaryOperator.Minus;
                case "!":
                    return UnaryOperator.Not;
                default:
                    throw new Exception("String: " + s + " no se puede pasar a UnaryOperator");
            }
        }
    }

    public static class IncDecOperatorExtension
    {
        public static string ToStringSymbol(this IncDecOperator op)
        {
            switch (op)
            {
                case IncDecOperator.MinusMinus:
                    return "--";
                case IncDecOperator.PlusPlus:
                    return "++";
                default:
                    throw new Exception("Operador: " + op.ToString() + " no se puede pasar a symbolString");
            }
        }

        /// <summary>
        /// NOT SAFE. TIRA EXCEPTION SI NO EL STRING NO ES UN BINARY OPERATOR VALIDO
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IncDecOperator SymbolToIncDecOperator(this string s)
        {
            switch (s)
            {
                case "--":
                    return IncDecOperator.MinusMinus;
                case "++":
                    return IncDecOperator.PlusPlus;
                default:
                    throw new Exception("String: " + s + " no se puede pasar a UnaryOperator");
            }
        }
    }
}

using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker
{
    /// <summary>
    /// Contiene el stack de funciones de control que se estan ejecutando.
    /// </summary>
    public static class ControlStack
    {
        private static LinkedList<ControlType> Stack = new LinkedList<ControlType>();

        public static void Push(ControlType controlType)
        {
            Stack.AddLast(controlType);
        }

        public static ControlType Pop()
        {
            var type = Stack.Last();
            Stack.RemoveLast();
            return type;
        }


        /// <summary>
        /// Retorna verdadero si contiene algun elemento en el stack al que se le pueda hacer pop
        /// con el jumper y le hace pop a todos esos elementos. De no ser posible retorna false
        /// y no se hace ningun pop
        /// </summary>
        /// <returns></returns>
        public static bool PopUntil(Jumper jumper)
        {
            //Chapuz medio alto. Performance wise deberiamos de ver como usamos el iterador para 
            //revisar que contenga algo que se pueda popear con el jumper y luego con ese iterador
            //ir a la direccion contraria popeando cada uno
            var stackArray = new ControlType[Stack.Count];
            Stack.CopyTo(stackArray, 0);
            for (int i = stackArray.Length - 1; i > -1; i--)
            {
                if (stackArray[i].CanBeJumpedWith(jumper))
                {
                    if (jumper.GetJumperType() == JumperType.Continue)//porque si es continue no hace pop al loop.//Chapus medio bajo
                        i++;
                    PopNTimes(stackArray.Length - i);
                    jumper.Popped = true;
                    return true;
                }
                if (stackArray[i] == ControlType.Method || stackArray[i] == ControlType.Function)//Lo obliga a terminar el ciclo
                    break;
            }
            return false;
        }

        public static string MyToString()
        {
            var sb = new StringBuilder("Bottom->");
            foreach (var controlType in Stack)
            {
                sb.Append(controlType.ToString() + ", ");
            }
            if (sb.Length - 2 > 6)
                sb.Remove(sb.Length - 2, 1);
            sb.Append("<-Top");
            return sb.ToString();

        }

        /// <summary>
        /// Not safe
        /// </summary>
        /// <param name="n"></param>
        private static void PopNTimes(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Pop();
            }
        }

        public static bool CanJump(this Jumper jumper, ControlType controlType)
        {
            switch (jumper.GetJumperType())
            {
                case JumperType.VoidReturn:
                    switch (controlType)
                    {
                        case ControlType.Method:
                            return true;
                        default:
                            return false;
                    }
                case JumperType.Return:
                    switch (controlType)
                    {
                        case ControlType.Function:
                            return true;
                        default:
                            return false;
                    }
                case JumperType.Breaker:
                    switch (controlType)
                    {
                        case ControlType.Switch:
                        case ControlType.Loop:
                            return true;
                        default:
                            return false;
                    }
                case JumperType.Continue:
                    switch (controlType)
                    {
                        case ControlType.Loop:
                            return true;
                        default:
                            return false;
                    }
                default:
                    throw new Exception("Control type no valido");
            }
        }

        public static bool CanBeJumpedWith(this ControlType controlType, Jumper jumper)
        {
            switch (controlType)
            {
                case ControlType.Method:
                    switch (jumper.GetJumperType())
                    {
                        case JumperType.VoidReturn:
                            return true;
                        default:
                            return false;
                    }
                case ControlType.Function:
                    switch (jumper.GetJumperType())
                    {
                        case JumperType.Return:
                            return true;
                        default:
                            return false;
                    }
                case ControlType.Switch:
                    switch (jumper.GetJumperType())
                    {
                        case JumperType.Breaker:
                            return true;
                        default:
                            return false;
                    }
                case ControlType.Loop:
                    switch (jumper.GetJumperType())
                    {
                        case JumperType.Breaker:
                        case JumperType.Continue:
                            return true;
                        default:
                            return false;
                    }
                default:
                    throw new Exception("Control type no valido");
            }
        }
    }

    public enum ControlType
    {
        Method,//Void function
        Function,
        Switch,
        Loop//No distingue entre dowhile y while
    }
}

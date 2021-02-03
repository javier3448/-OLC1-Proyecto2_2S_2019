using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives
{
    //TODO: Agregarle funciones a los primitivos
    public abstract class MyPrimitive : PyObj
    {

        public sealed override bool IsPrimitive()
        {
            return true;
        }

        public sealed override bool IsNull()
        {
            return false;
        }

        
    }
}

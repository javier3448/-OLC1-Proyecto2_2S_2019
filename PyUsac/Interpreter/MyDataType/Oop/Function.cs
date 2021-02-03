using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi1_Proyecto2.PyUsac.Ast.Base;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop
{
    public class Function : Procedure
    {
        public Function(AstNode definition) : base(definition){
            if (definition.GetNodeType() != NodeType.Function)
                throw new Exception("No se puede crear una funcion con un nodo no funcion");//Chapuz
        }

        public override DefinitionType GetDefinitionType()
        {
            return DefinitionType.Function;
        }
    }
}

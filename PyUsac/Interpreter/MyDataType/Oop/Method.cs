using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interfaces;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop
{
    public class Method : Procedure
    {
        public Method(AstNode definition) : base(definition) 
        {
            if (definition.GetNodeType() != NodeType.Method)
                throw new Exception("No se puede crear una funcion con un nodo no funcion");//Chapuz
        }

        public override DefinitionType GetDefinitionType()
        {
            return DefinitionType.Method;
        }
    }
}

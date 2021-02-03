using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi1_Proyecto2.PyUsac.Interfaces;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using Irony.Ast;
using Irony.Parsing;

namespace _Compi1_Proyecto2.PyUsac.Ast.Base
{
    /// <summary>
    /// Nodo para aquellos parseTreeNode que no son transient en el ParseTree pero no tienen nodo en el arbol Ast.
    /// Nota: Pasar los hijos del ParseTreeNode al padre del parseTreeNode. Para simular que es transient exitosamente
    /// </summary>
    public class TransientNode : AstNode
    {
        //TODO: Corregir este chapus:
        //Chapus temporal No deberia de tirar exception solo deberia de no hacer nada los metodos Accept y DotLabel
        public override Word Accept(IAstVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public override string DotLabel()
        {
            throw new NotImplementedException();
        }

        public override NodeType GetNodeType()
        {
            return NodeType.AstTransient;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            
        }
    }
}

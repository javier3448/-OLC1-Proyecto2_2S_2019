using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Ast.Base
{
    public class PyAstBuilder : AstBuilder
    {
        PyPath PyPath;
        public PyAstBuilder(AstContext context, string path) : base(context)
        {
            PyPath = new PyPath(path);
        }

        public override void BuildAst(ParseTreeNode parseNode)
        {
            base.BuildAst(parseNode);
            var astNode = (AstNode)parseNode.AstNode;
            //Chapuz medio bajo para que igonore los nodos transient y los que no tiene ast
            if (astNode == null)
                return;
            if (astNode.GetNodeType() == NodeType.AstTransient)
                return;
            astNode.NodePath = PyPath;
        }
    }
}

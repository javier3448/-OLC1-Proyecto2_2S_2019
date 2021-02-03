using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interfaces;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Ast.Node.Terminal
{
    public class IdentifierNode: AstNode
    {
        public string Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Value = (string)treeNode.Token.Value;
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, Value.ToString());
        }

        public override NodeType GetNodeType()
        {
            return NodeType.IdentifierNode;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

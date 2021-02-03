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
    public class NumberLiteralNode : AstNode
    {
        /// <summary>
        /// Se sabe que es un number (int o double)
        /// </summary>
        public object Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Value = treeNode.Token.Value;
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, "type: " + Value.GetType().Name, Value.ToString());
        }

        public override NodeType GetNodeType()
        {
            return NodeType.NumberLiteralNode;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

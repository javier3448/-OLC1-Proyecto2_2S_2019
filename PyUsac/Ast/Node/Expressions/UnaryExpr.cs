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

namespace _Compi1_Proyecto2.PyUsac.Ast.Node.Expressions
{
    public class UnaryExpr : AstNode
    {
        public UnaryOperator UnaryOperator { get; private set; }
        public AstNode RightExpr { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            UnaryOperator = nodes[0].Token.ValueString.SymbolToUnaryOperator();
            RightExpr = AddChild("right: ", nodes[1]);
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, UnaryOperator.ToString());
        }
        public override NodeType GetNodeType()
        {
            return NodeType.UnaryExpr;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

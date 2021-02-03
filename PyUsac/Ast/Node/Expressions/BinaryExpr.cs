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
    public class BinaryExpr : AstNode
    {
        public AstNode LeftExpr { get; private set; }
        public BinaryOperator BinaryOperator { get; private set; }
        public AstNode RightExpr { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            LeftExpr = AddChild("left: ", nodes[0]);
            BinaryOperator = nodes[1].Token.ValueString.SymbolToBinaryOperator();
            RightExpr = AddChild("right: ", nodes[2]);
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, BinaryOperator.ToString());
        }

        public override NodeType GetNodeType()
        {
            return NodeType.BinaryExpr;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

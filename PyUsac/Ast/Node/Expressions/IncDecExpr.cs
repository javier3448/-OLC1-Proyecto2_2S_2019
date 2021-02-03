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
    public class IncDecExpr : AstNode
    {
        public IncDecOperator IncDecOperator { get; private set; }
        public AstNode Value { get; private set; }//MemberAccess

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Value = AddChild("member access: ", nodes[0]);
            IncDecOperator = nodes[1].Token.ValueString.SymbolToIncDecOperator();
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, IncDecOperator.ToStringSymbol());
        }

        public override NodeType GetNodeType()
        {
            return NodeType.IncDecExpr;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

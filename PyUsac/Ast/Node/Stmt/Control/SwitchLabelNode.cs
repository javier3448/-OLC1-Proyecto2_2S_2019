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

namespace _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Control
{
    public class SwitchLabelNode : AstNode
    {
        public AstNode Expr { get; private set; }//Expr o null si es default!

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            if (nodes.Count > 1)
                Expr = AddChild("Expr: ", nodes[1]);
        }
        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }

        public override NodeType GetNodeType()
        {
            return NodeType.SwitchLabel;
        }

        public bool IsDefault()
        {
            return Expr == null;
        }
    }
}

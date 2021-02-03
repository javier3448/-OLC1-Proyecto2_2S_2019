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
    public class IfNode : AstNode
    {

        public AstNode Condition { get; private set; }//Expr
        public AstNode Block { get; private set; }
        public AstNode Else { get; private set; } //null, Block, if

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Condition = AddChild("Condition: ", nodes[1]);
            Block = AddChild("Block: ", nodes[2]);
            if (nodes.Count > 4)
                Else = AddChild("Else: ", nodes[4]);
        }
        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override string DotLabel()
        {
            if (Else == null)
                return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, "Has else!");
        }

        public override NodeType GetNodeType()
        {
            return NodeType.IfNode;
        }
    }
}

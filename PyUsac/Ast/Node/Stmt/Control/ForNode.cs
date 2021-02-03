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
    public class ForNode : AstNode
    {
        public AstNode Flag { get; private set; }//Assignment | Declaration
        public AstNode Condition { get; private set; }//Expr
        public AstNode Update { get; private set; }//IncDecExpr
        public AstNode Block { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Flag = AddChild("Flag: ", nodes[1]);
            Condition = AddChild("Condition: ", nodes[2]);
            Update = AddChild("Update: ", nodes[3]);
            Block = AddChild("Block: ", nodes[4]);
        }
        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        /// <summary>
        /// Si no es declaracion entonces es asignment
        /// </summary>
        /// <returns></returns>
        public bool HasDeclaration()
        {
            return Flag.GetNodeType() == NodeType.Declaration;
        }

        public override string DotLabel()
        {
            if (Flag.GetNodeType() == NodeType.Assignment)
                return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, "Assignment");
            else
                return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, "Declaration");
            throw new Exception("ParseTree no valido");
        }

        public override NodeType GetNodeType()
        {
            return NodeType.ForNode;
        }
    }
}

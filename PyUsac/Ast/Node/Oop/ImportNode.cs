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

namespace _Compi1_Proyecto2.PyUsac.Ast.Node.Oop
{
    public class ImportNode : AstNode
    {
        public AstNode Path { get; private set; }//String Literal

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Path = AddChild("Path: ", nodes[1]);
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
            return NodeType.ImportNode;
        }
    }
}

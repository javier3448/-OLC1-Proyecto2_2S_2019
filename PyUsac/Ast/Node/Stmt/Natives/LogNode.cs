﻿using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Interfaces;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using Irony.Ast;
using Irony.Parsing;

namespace _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Natives
{
    public class LogNode : AstNode
    {
        public AstNode Expr { get; private set; }//expr

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("Value: ", nodes[1]);
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }

        public override NodeType GetNodeType()
        {
            return NodeType.Log;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

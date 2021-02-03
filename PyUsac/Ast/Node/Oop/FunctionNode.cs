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
    public class FunctionNode : AstNode
    {
        public AstNode Name { get; private set; }//IdentifierNode
        public AstNodeList ParamList { get; private set; } = new AstNodeList();
        public AstNodeList Stmts { get; private set; } = new AstNodeList();

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Name = AddChild("Name: ", nodes[0]);
            var paramsNodes = nodes[1].GetMappedChildNodes();
            int i = 0;
            foreach (var param in paramsNodes)
            {
                ParamList.Add(AddChild("Param: " + i, param));
                i++;
            }
            var stmtNodes = nodes[2].GetMappedChildNodes();
            i = 0;
            foreach (var stmt in stmtNodes)
            {
                Stmts.Add(AddChild("stmt: " + i, stmt));
                i++;
            }
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }

        public override NodeType GetNodeType()
        {
            return NodeType.Function;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

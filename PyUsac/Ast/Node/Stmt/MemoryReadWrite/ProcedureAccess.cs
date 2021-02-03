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

namespace _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.MemoryReadWrite
{
    public class ProcedureAccess : AstNode
    {
        public AstNode IdentifierNode { get; private set; } //Identifier
        public AstNodeList Arguments { get; private set; } = new AstNodeList();

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            IdentifierNode = AddChild("Identifier: ", nodes[0]);
            var argumentNodes = nodes[1].GetMappedChildNodes();
            var i = 0;
            foreach (var argument in argumentNodes)
            {
                Arguments.Add(AddChild("argument: " + i, argument));
                i++;
            }
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }
        public override NodeType GetNodeType()
        {
            return NodeType.FunctionAccess;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

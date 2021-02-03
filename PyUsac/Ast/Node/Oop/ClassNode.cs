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
    public class ClassNode : AstNode
    {
        public AstNode IdentifierNode { get; private set; }//Class name
        public AstNodeList DefinitonList { get; private set; } = new AstNodeList();
        public AstNodeList StmtList { get; private set; } = new AstNodeList();

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            IdentifierNode = AddChild("class name: ", nodes[0]);
            var definitionOrStmt = nodes[1].ChildNodes;
            var i = 0;
            var j = 0;
            foreach (var node in definitionOrStmt)
            {
                if (node.Term.Name[0] == '#')//Chapus medio alto para saber si es definition
                {
                    DefinitonList.Add(AddChild("Definition: " + j, node));
                    j++;
                }
                else
                {
                    StmtList.Add(AddChild("stmt " + i, node));
                    i++;
                }
            }
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }

        public override NodeType GetNodeType()
        {
            return NodeType.Class;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

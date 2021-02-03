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
    public class ProgramNode : AstNode
    {
        public AstNodeList ImportList { get; private set; } = new AstNodeList();
        public AstNodeList DefinitonList { get; private set; } = new AstNodeList();
        public AstNodeList StmtList { get; private set; } = new AstNodeList();

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            var definitionOrStmt = nodes[0].ChildNodes;
            var importCount = 0;
            var definitionCount = 0;
            var stmtCount = 0;
            foreach (var node in definitionOrStmt)
            {

                if (node.Term.Name[0] == '$')//Chapus medio alto para saber si es import
                {
                    ImportList.Add(AddChild("Import: " + importCount, node));
                    importCount++;
                }
                else if (node.Term.Name[0] == '#')//Chapus medio alto para saber si es definition
                {
                    DefinitonList.Add(AddChild("Definition: " + definitionCount, node));
                    definitionCount++;
                }
                else
                {
                    StmtList.Add(AddChild("stmt " + stmtCount, node));
                    stmtCount++;
                }
            }
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }

        public override NodeType GetNodeType()
        {
            return NodeType.ProgramNode;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

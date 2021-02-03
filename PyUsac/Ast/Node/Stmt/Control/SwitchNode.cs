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
    public class SwitchNode : AstNode
    {

        public AstNode Flag { get; private set; }//Expr
        public AstNodeList SwitchElements { get; private set; } = new AstNodeList();//Stmt o switch label, si stmt es declaration tirrar error

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Flag = AddChild("Flag: ", nodes[1]);
            var switchParseElements = nodes[2].GetMappedChildNodes();
            int i = 0;
            foreach (var switchParseElement in switchParseElements)
            {
                SwitchElements.Add(AddChild("Switch element " + i, switchParseElement));
                i++;
            }
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
            return NodeType.Switch;
        }
    }
}

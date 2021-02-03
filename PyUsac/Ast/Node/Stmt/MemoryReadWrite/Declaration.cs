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
    public class Declaration : AstNode
    {
        public AstNodeList LeftValues { get; private set; } = new AstNodeList();//IdentifierNodes
        public AstNodeList Indexes { get; private set; } = new AstNodeList();
        public AstNode RightValue { get; private set; }//expr (Puede ser null!)

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            var identifierNodes = nodes[0].GetMappedChildNodes();
            int i = 0;
            foreach (var identifierNode in identifierNodes)
            {
                LeftValues.Add(AddChild("left value : " + i, identifierNode));
                i++;
            }
            var indexes = nodes[1].GetMappedChildNodes();
            i = 0;
            foreach (var index in indexes)
            {
                Indexes.Add(AddChild("Index: " + i, index));
                i++;
            }
            if (nodes.Count > 2)
                RightValue = AddChild("right value: ", nodes[2]);
        }

        public override string DotLabel()
        {
            //Chapuz para considere las 3 posibilidades
            if (RightValue == null && !Indexes.IsEmpty())
                return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, "array declaration", "null declaration");
            if (RightValue == null)
                return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, "null declaration");
            if (!Indexes.IsEmpty())
                return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name, "array declaration");
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }

        public override NodeType GetNodeType()
        {
            return NodeType.Declaration;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

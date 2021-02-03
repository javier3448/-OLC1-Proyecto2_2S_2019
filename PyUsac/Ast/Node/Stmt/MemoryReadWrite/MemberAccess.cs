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
    public class MemberAccess : AstNode
    {
        public AstNode FirstSegment { get; private set; }//access segment
        public AstNodeList OptionalSegments { get; private set; } = new AstNodeList();//memberAccessOptionalSegment

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            FirstSegment = AddChild("First Segment", nodes[0]);
            //Codigo para que member_access_optional_segmentList sea transient en el ast
            var optionalSegments = nodes[1].ChildNodes;
            int i = 1;
            foreach (var node in optionalSegments)
            {
                OptionalSegments.Add(AddChild("segment " + i, node));
                i++;
            }
        }

        public override string DotLabel()
        {
            return Graphviz.DotUtilities.BuildDotLabel(this.GetType().Name);
        }

        public override NodeType GetNodeType()
        {
            return NodeType.MemberAccess;
        }

        public override Word Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}

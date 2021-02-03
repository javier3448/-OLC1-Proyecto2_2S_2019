using _Compi1_Proyecto2.PyUsac.Interfaces;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Ast.Base
{
    public class AstNodeList : List<AstNode> 
    { 
        public AstNodeList() { }
        public AstNodeList(AstNodeList nodeList) : base(nodeList) {   }
        public bool IsEmpty()
        {
            return Count < 1;
        }
    }

    public abstract class AstNode : IAstNodeInit
    {
        /// <summary>
        /// Padre del nodo, ojo! si setea como hijo de otro nodo se pierde la referencia al primer padre pero el padre no piede la referencia al hijo
        /// </summary>
        public AstNode Parent;
        public BnfTerm Term;
        public SourceSpan Span { get; set; }
        public PyPath NodePath { get; set; }

        public readonly AstNodeList ChildNodes = new AstNodeList();  //List of child nodes
        /// <summary>
        /// Seteado por el padre, sirve para describir el rol que como hijo del padre. Solo para debugging y el ToString
        /// </summary>
        public string Role;

        /// <summary>
        /// Init default: solo setea las propiedas tomadas del treeNode. Y le asigna al treeNode que su ast es this
        /// </summary>
        /// <param name="context"></param>
        /// <param name="treeNode"></param>
        public virtual void Init(AstContext context, ParseTreeNode treeNode)
        {
            this.Term = treeNode.Term;
            this.Span = treeNode.Span;
            treeNode.AstNode = this;
        }

        protected AstNode AddChild(string role, ParseTreeNode childParseNode)
        {
            var child = (AstNode)childParseNode.AstNode;
            if (child == null)
                throw new ArgumentNullException("childParseNode", "No se le puede agregar un hijo null ar AstNode");
            child.Role = role;
            child.Parent = this;
            ChildNodes.Add(child);
            return child;
        }

        public abstract NodeType GetNodeType();

        public abstract Word Accept(IAstVisitor visitor);

        /// <summary>
        /// Solo para debugging
        /// devuelve un string que puede ser utilizado como label html en un nodo de graphviz. Incluye el nombre tambien
        /// </summary>
        /// <returns></returns>
        public abstract string DotLabel();

        
    }
}

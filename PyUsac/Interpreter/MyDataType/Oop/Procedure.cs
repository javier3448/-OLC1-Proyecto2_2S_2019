using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop
{
    /// <summary>
    /// Basicamente Wrapper de Definiti
    /// </summary>
    public class Procedure : Definition
    {
        public readonly AstNodeList Stmts;

        public readonly string Name;
        public readonly int ParamCount;
        public readonly string[] ParamNames;

        public static Procedure EmptySignature(string name, int paramCount)
        {
            return new Procedure(name, paramCount);
        }

        public override DefinitionType GetDefinitionType()
        {
            return DefinitionType.NA;
        }

        private Procedure(string name, int paramCount)
        {
            name = name.ToLowerInvariant();
            Name = name;
            ParamCount = paramCount;
        }
        protected Procedure(AstNode definition)
        {
            if (definition.GetNodeType() == NodeType.Method)//Chapus medio alto. Mucho codigo copiado y pegado, deberia de existir alguno nodo que sea padre de MethodNode y FuncionNode para arreglar esto
            {
                ParamCount = ((MethodNode)definition).ParamList.Count;
                ParamNames = new string[ParamCount];
                var i = 0;
                foreach (var param in ((MethodNode)definition).ParamList)
                {
                    ParamNames[i] = ((IdentifierNode)param).Value;
                    i++;
                }
                Stmts = new AstNodeList(((MethodNode)definition).Stmts);
            }
            else if (definition.GetNodeType() == NodeType.Function)
            {
                ParamCount = ((FunctionNode)definition).ParamList.Count;
                ParamNames = new string[ParamCount];
                var i = 0;
                foreach (var param in ((FunctionNode)definition).ParamList)
                {
                    ParamNames[i] = ((IdentifierNode)param).Value;
                    i++;
                }
                Stmts = new AstNodeList(((FunctionNode)definition).Stmts);
            }
            else
            {
                throw new Exception("No se puede instanciar Function con un nodo tipo: " + (definition == null ? "null" : definition.GetNodeType().ToString()));
            }
            var name = ((IdentifierNode)definition.ChildNodes[0]).Value.ToLowerInvariant();
            Name = name;
        }
    }

    public class ProcedureSameSignature : EqualityComparer<Procedure>
    {
        public override bool Equals(Procedure p0, Procedure p1)
        {
            if (p0 == null && p1 == null)
                return true;
            else if (p0 == null || p1 == null)
                return false;

            return (p0.ParamCount == p1.ParamCount &&
                    p0.Name.Equals(p1.Name));
        }

        //No se si esta implementacion produce muchas o pocas colisiones :/
        public override int GetHashCode(Procedure obj)
        {
            return obj.Name.GetHashCode() ^ obj.ParamCount;
        }
    }
}

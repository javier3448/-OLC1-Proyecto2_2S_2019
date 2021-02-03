using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop;
using static _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker.ErrorHelper;
using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker
{
    /// <summary>
    /// Wrapper de StaticVisitor, representa un 'entorno estatico'
    /// </summary>
    public class StaticEntity
    {
        private StaticVisitor Visitor;
        private readonly ProgramNode Root; //Guarda esto porque es el unico Wrapper que hace parser, los demas lo tiene ya todo 'compilado'
        private List<StaticEntity> Imports = new List<StaticEntity>();

        /// <summary>
        /// Recive un parseTree si errores, de lo contrario tira exception
        /// </summary>
        /// <param name="parseTree"></param>
        public StaticEntity(ProgramNode programNode)
        {
            Root = programNode;
        }

        //Debe de corre antes de usar cualquier GetMember
        public void InitVisitor(bool run = false)
        {
            Visitor = new StaticVisitor(this);
            if (run)
                Visitor.Visit(Root);
        }

        public void AddImport(StaticEntity import)
        {
            if (import is null)
                throw new ArgumentNullException(nameof(import));

            Imports.Add(import);
        }

        //Retorna error o PyObj
        public Word GetMember(ProcedureSegment procedureSegment)
        {
            if (Visitor == null)
                throw new Exception("Tiene que inicializar visitor con InitVisitor antes de usar cualquier GetMember");
            var result = Visitor.InvokeProcedure(procedureSegment);
            var firstResult = result;
            if (!IsError(result))
                return result;
            foreach (var import in Imports)
            {
                result = import.GetMember(procedureSegment);
                if (!IsError(result))
                    return result;
            }
            return firstResult;
        }

        //Retorna error o memblock
        public Word GetMember(IdentifierSegment identifierSegment)
        {
            if (Visitor == null)
                throw new Exception("Tiene que inicializar visitor con InitVisitor antes de usar cualquier GetMember");
            var result = Visitor.GetGlobalAttribute(identifierSegment);
            var firstResult = result;
            if (!result.IsError())
                return result;
            foreach (var import in Imports)
            {
                result = import.GetMember(identifierSegment);
                if (!result.IsError())
                    return result;
            }
            return firstResult;
        }

        public ClassDefinition GetClass(ClassDefinition emptyClassDefinition)
        {
            ClassDefinition classDefinition;
            Visitor.ClassDefinitions.TryGetValue(emptyClassDefinition, out classDefinition);
            if (classDefinition != null)
                return classDefinition;
            foreach (var import in Imports)
            {
                classDefinition = import.GetClass(emptyClassDefinition);
                if (classDefinition != null)
                    return classDefinition;
            }
            return null;
        }

        public void InvokeMain()
        {
            var result = GetMember(ProcedureSegment.EmptyProcedureSegment("main", 0));
            if (IsError(result))
                ErrorFactory.MainNotFound(Root.NodePath, (MyError)result);
        }
    }
}

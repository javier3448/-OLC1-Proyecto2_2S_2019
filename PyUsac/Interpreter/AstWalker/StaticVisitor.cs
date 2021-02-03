using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.Memory;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using _Compi1_Proyecto2.PyUsac.Parser;
using Irony.Ast;
using Irony.Parsing;
using static _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker.ErrorHelper;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker
{
    public class StaticVisitor : BaseVisitor
    {
        public StaticVisitor(StaticEntity staticEntity)
        {
            StaticEntity = staticEntity;
        }

        public readonly HashSet<ClassDefinition> ClassDefinitions = new HashSet<ClassDefinition>(new ClassDefinitionSameName());//Contiene los metodos y funciones globales

        #region overrides
        public override Word Visit(ProgramNode programNode)
        {
            //Inicializa el scope
            GlobalScope = new Scope();
            CurrentScope = GlobalScope;

            Word result;

            foreach (var import in programNode.ImportList)
            {
                import.Accept(this);
            }

            foreach (var def in programNode.DefinitonList)
            {
                result = def.Accept(this);
                if (IsError(result))
                    continue;
                if (def.GetNodeType() == NodeType.Method || def.GetNodeType() == NodeType.Function)
                    Procedures.Add((Procedure)result);
                else//Tiene que ser class definition
                    ClassDefinitions.Add((ClassDefinition)result);
            }

            foreach (var stmt in programNode.StmtList)
            {
                result = stmt.Accept(this);
                if (IsJumper(result))
                    ErrorFactory.NotValidJumper(stmt, (Jumper)result);
            }

            return null;
        }

        public override Word Visit(ImportNode importNode)//chapuz bajo: NO retorna nada nunca para que no tengamos que hacer otra clase que herrede de Word, Este metodo como tal se encarga de agregar a sus imports
        {
            string path = ((MyString)importNode.Path.Accept(this)).StringValue;
            if (path.Length < 1)
                return ErrorFactory.PathNotValid(importNode.Path, path);
            if ((path[0] != Path.DirectorySeparatorChar) || (path[0] != 'C' && path[0] != 'D' && path[1] != ':'))//tiene que agregar al path la ruta del archivo con el que este nodo fue creado
                path = importNode.NodePath.GetParentPath() + Path.DirectorySeparatorChar + PyPath.ReplaceSepartors(path);
            if (!File.Exists(path))
            {
                ErrorFactory.FileNotFound(importNode, path);
                return null;
            }
                
            var import = CreateStaticEntity(path);
            if (import == null)
            {
                ErrorFactory.SyntaxErrorInImport(importNode, path);
                return null;
            }
            StaticEntity.AddImport(import);

            return null;
        }

        public override Word Visit(ClassNode classNode)
        {
            var className = ((MyString)classNode.IdentifierNode.Accept(this)).StringValue.ToLowerInvariant();
            var classId = TypeConstants.AddType(className);
            if (classId < 0)
                return ErrorFactory.AlreadyDefinedClass(classNode, className);

            return new ClassDefinition(classId, className, classNode, StaticEntity);
        }
        #endregion

        #region Utilities
        /// <summary>
        /// retorna null si ocurrio un error al leer el archivo.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private StaticEntity CreateStaticEntity(string path)
        {
            var src = File.ReadAllText(path);
            var grammar = new PyUsacGrammar();
            var langData = new LanguageData(grammar);
            var parser = new Irony.Parsing.Parser(langData);//Para evitar conflicto con el namespace Parser
            var parseTree = parser.Parse(src);

            bool hasErrors = false;
            foreach (var error in parseTree.ParserMessages)
            {
                if (error.Level == Irony.ErrorLevel.Error)
                    hasErrors = true;
                ErrorFactory.CreateParsingError(error, path);
            }
            if (hasErrors)
                return null;

            var astBuilder = new PyAstBuilder(new AstContext(langData), path);
            astBuilder.BuildAst(parseTree);
            var programNode = (ProgramNode)parseTree.Root.AstNode;

            var entity = new StaticEntity(programNode);
            entity.InitVisitor(true);

            return entity;
        }
        #endregion
    }
}

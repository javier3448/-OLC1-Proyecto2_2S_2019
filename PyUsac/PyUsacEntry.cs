using _Compi1_Proyecto2.Graphviz;
using _Compi1_Proyecto2.MyGui;
using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using _Compi1_Proyecto2.PyUsac.Parser;
using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac
{
    public static class PyUsacEntry
    {
        public static bool TestSyntax(string src)
        {
            var grammar = new PyUsacGrammar();
            var langData = new LanguageData(grammar);
            var parser = new Irony.Parsing.Parser(langData);//Para evitar conflicto con el namespace Parser
            var parseTree = parser.Parse(src);
            var root = parseTree.Root;

            if (parseTree.HasErrors())
            {
                ErrorHelper.ErrorFactory.CreateParsingErrors(parseTree, "_null");
                return false;
            }
            else
            {
                GetDot(root);


                Debug.WriteLine("");
                Debug.WriteLine("--------------------------------------------------");
                Debug.WriteLine("");

                return true;
            }
        }

        public static bool TestAst(string src)
        {
            var grammar = new PyUsacGrammar();
            var langData = new LanguageData(grammar);
            var parser = new Irony.Parsing.Parser(langData);//Para evitar conflicto con el namespace Parser
            var parseTree = parser.Parse(src);
            var root = parseTree.Root;

            if (parseTree.HasErrors())
            {
                ErrorHelper.ErrorFactory.CreateParsingErrors(parseTree, "_null");
                return false;
            }
            else
            {
                var dotCode = GetDot(root);

                var astBuilder = new AstBuilder(new AstContext(langData));
                astBuilder.BuildAst(parseTree);
                var astRoot = (AstNode)root.AstNode;

                Debug.WriteLine("");
                Debug.WriteLine("--------------------------------------------------");
                Debug.WriteLine("");

                GetDot(astRoot);

                return true;
            }
        }

        public static bool InterpretFromFileName(string filePath)
        {
            string src;
            try
            {
                src = File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                //Reprotar error de preprocesador
                return false;
            }
            var grammar = new PyUsacGrammar();
            var langData = new LanguageData(grammar);
            var parser = new Irony.Parsing.Parser(langData);//Para evitar conflicto con el namespace Parser
            var parseTree = parser.Parse(src);
            var root = parseTree.Root;

            bool hasErrors = false;
            foreach (var error in parseTree.ParserMessages)
            {
                if (error.Level == Irony.ErrorLevel.Error)
                    hasErrors = true;
                ErrorHelper.ErrorFactory.CreateParsingError(error, filePath);
            }
            if (hasErrors)
                return false;
            //var dotCode = GetDot(root);//Descomentar en debug mode! :)

            var astBuilder = new PyAstBuilder(new AstContext(langData), filePath);
            astBuilder.BuildAst(parseTree);
            var programNode = (ProgramNode)parseTree.Root.AstNode;
            //GetDot(programNode);//Descomentar en debug mode! :)

            var entity = new StaticEntity(programNode);
            entity.InitVisitor(true);
            entity.InvokeMain();
            TypeConstants.ClearTypeHashtable();

            return true;
        }

        //DEBUG ONLY
        private static LinkedList<string> GetDot(ParseTreeNode root)
        {
            LinkedList<string> lines = new LinkedList<String>();
            lines.AddLast("digraph CssAstTree{");
            lines.AddLast("style=invis;");

            IGetDot(root, lines);

            lines.AddLast("}");

            foreach (String line in lines)
            {
                Debug.WriteLine(line);
                //Chapuz maximo Solo para debugging:
                //Interpreter.RuntimeEnvironment.Console.Instance.PrintLine(Interpreter.MyDataType.PyUsacTypes.Primitives.MyPrimitiveFactory.Create(line));
            }

            return lines;
        }

        /// <summary>
        /// Metodo recursivo para generar la lista de strings del dot de un nodo
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        private static void IGetDot(ParseTreeNode node, LinkedList<String> container)
        {
            if (node == null)
            {
                return;
            }
            string term = Convert.ToString(node.Term);
            string val = Convert.ToString(node.Token);
            if (val.IndexOf('(') >= 0)
            {
                val = val.Substring(0, val.IndexOf('('));
            }
            container.AddLast(node.GetHashCode().ToString() + "[ label = \"" + term + "\\n" + val + "\" ];");
            foreach (var child in node.ChildNodes)
            {
                IGetDot(child, container);
                container.AddLast(node.GetHashCode().ToString() + "->" + child.GetHashCode() + ";");
            }
        }

        public static LinkedList<string> GetDot(AstNode root)
        {
            LinkedList<string> lines = new LinkedList<String>();
            lines.AddLast("digraph CssAstTree{");
            lines.AddLast("node[shape = record];");
            lines.AddLast("style=invis;");

            IGetDot(root, lines);

            lines.AddLast("}");

            foreach (String line in lines)
            {
                Debug.WriteLine(line);
            }

            return lines;
        }

        /// <summary>
        /// Metodo recursivo para generar la lista de strings del dot de un nodo
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        private static void IGetDot(AstNode node, LinkedList<String> container)
        {
            if (node == null)
            {
                return;
            }
            string label = node.DotLabel();
            container.AddLast(node.GetHashCode().ToString() + "[ label = <" + label + "> ];");
            foreach (var child in node.ChildNodes)
            {
                IGetDot(child, container);
                container.AddLast(node.GetHashCode().ToString() + "->" + child.GetHashCode() + ";");
            }
        }
    }
}

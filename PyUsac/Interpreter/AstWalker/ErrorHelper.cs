using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Expressions;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Control;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.MemoryReadWrite;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using Irony;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker
{
    /// <summary>
    /// Metodos de ayuda para el manejo de errores, lista de errores, y metodos de cumplen la funcion de un error factory
    /// </summary>
    public static class ErrorHelper
    {
        /// <summary>
        /// Tiene todos los errores semanticos de esta ejecucion
        /// </summary>
        public static List<MyError> ErrorList { get; private set; } = new List<MyError>();

        /// <summary>
        /// Null safe is error
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsError(Word word)
        {
            if (word == null)
                return false;
            return word.IsError();
        }

        /// <summary>
        /// Null save is Jumper
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsJumper(Word word)
        {
            if (word == null)
                return false;
            return word.IsJumper();
        }

        /// <summary>
        /// Null safe IsMemoryBlock
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsMemoryBlock(Word word)
        {
            if (word == null)
                return false;
            return word.IsMemoryBlock();
        }

        public static class ErrorFactory
        {
            /// <summary>
            /// Setea los valores de locacion al error y lo agrega a la lista de errores
            /// </summary>
            /// <returns>El error despues de ser editado</returns>
            public static MyError Create(AstNode node, MyError error)
            {
                error.Set(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            internal static Word AlreadyDefinedProcedure(AstNode node, string procedureName, int procedureParamCount)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Ya existe un metodo definido en este entorno con el nombre: " + procedureName + " y numero de parametros: " + procedureParamCount.ToString(),
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError SystemError(Exception exception, string path)
            {
                MyError error = new MyError(-1,
                    -1,
                    path,
                    "C# " + exception.GetType().Name + ". message: "  + exception.Message,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.System);
                AddError(error);
                return error;
            }

            public static void CreateParsingErrors(ParseTree parseTree, string path)//Necesita el path porque no se logro generar el Ast
            {
                foreach (var syntaxError in parseTree.ParserMessages)
                {
                    ErrorFactory.CreateParsingError(syntaxError, path);
                }
            }

            public static MyError CreateParsingError(LogMessage syntaxError, string path)
            {
                MyError.ErrorType errorType;
                //CHAPUZ MAXIMO PARA VER SI ES ERROR LEXICO
                var lexicErrorSubString = "Invalid character";
                if (syntaxError.Message.Substring(0, lexicErrorSubString.Length).Equals(lexicErrorSubString))
                    errorType = MyError.ErrorType.Lexic;
                else
                    errorType = MyError.ErrorType.Syntactic;

                var error = new MyError(syntaxError.Message);
                error.Set(syntaxError.Location.Line,
                    syntaxError.Location.Column,
                    path,
                    errorType,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError SyntaxErrorInImport(AstNode node, string path)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue, 
                    "Errores sintacticos al tratar de importar el archivo: " + path,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError FileNotFound(AstNode node, string path)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No existe el archivo con direccion: " + path,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError MainNotFound(PyPath nodePath, MyError error)
            {
                error.Message = "Main no definido";
                error.Set(-1,
                    -1,
                    nodePath.StringValue,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Warning);
                AddError(error);
                return error;
            }

            /// <summary>
            /// Crea un nuevo error con el mensaje indicado
            /// </summary>
            /// <returns>El error despues de ser editado</returns>
            public static MyError Create(AstNode node, string message)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    message,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError AlertError(AstNode node, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se puede hacer alert con un tipo: " + type,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word VoidExpr(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Expresion no valida, retorna void.",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError LogError(AstNode node, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se puede hacer log con un tipo: " + type,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError GraphError(AstNode node, string paramName, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    paramName + " de graph con un tipo: " + type,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError IncDecError(AstNode node, IncDecOperator op, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se puede hacer " + op.ToStringSymbol() + " con un tipo: " + type,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError ArrayDimensionError(AstNode node, Word indexWord)
            {
                string type;
                string value;
                if (indexWord.IsError())
                {
                    type = indexWord.GetType().Name;
                    value = "";
                }
                else
                {
                    type = TypeConstants.GetMyTypeName(((PyObj)indexWord).GetMyType());
                    value = ((PyObj)indexWord).MyToString();
                }
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Indice no valido en declaracion de Array: " + type + " valor: " + value,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError IncDecNonRefError(AstNode node, IncDecOperator op)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se puede hacer la operacion:" + op.ToStringSymbol() + " a un tipo no referencia",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError AssignmentNonRefError(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se puede hacer asignacion a un tipo no referencia",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError ArrayTypeError(AstNode node, int[] intIndices, Word word)
            {
                string type;
                string value;
                if (word.IsError())
                {
                    type = word.GetType().Name;
                    value = "";
                }
                else
                {
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                    value = ((PyObj)word).MyToString();
                }
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se le puede asignar a un array con las dimensiones: [ " + String.Join(", ", intIndices) + " ]. Un objeto tipo: " + type + " valor: " + value,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError NotValidJumper(AstNode node, Jumper jumper)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Jumper tipo: " + jumper.GetJumperType().ToString() + ". No es valido el call stack actual: " + ControlStack.MyToString(),
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError IfError(AstNode node, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Condicion de IF no es valida. Tipo: " + type + " se esperaba: " + TypeConstants.GetMyTypeName(TypeConstants.BOOLEAN),
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError DeclarationInSwitch(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se pueden hacer declaraciones dentro de un switch",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word AlreadyDefinedClass(AstNode node, string className)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Ya existe una clase definida con el nombre: " + className,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word NonDefinedClass(AstNode node, string className)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No existe una clase definida con el nombre: " + className,
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word MultipleDefaultInSwitch(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Solo puede tener una switchlabel: default",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word NotValidOperationInSwitch(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "La comparacion de uno de los casos del switch retorno un tipo no booleano",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError MultipleTrueCasesInSwitch(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Existe mas de un caso que retorna verdadero en el switch",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError WhileError(AstNode node, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Condicion de WHILE no es valida. Tipo: " + type + " se esperaba: " + TypeConstants.GetMyTypeName(TypeConstants.BOOLEAN),
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError CantReturnError(AstNode node, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se puede retornar un tipo: " + type + " se retorno null para poder continuar con la ejecucion",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError ForFlagTypeError(AstNode node, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Bandera del FOR no es valida. Tipo: " + type + " se esperaba: " + TypeConstants.GetMyTypeName(TypeConstants.INT) + " o " + TypeConstants.GetMyTypeName(TypeConstants.INT),
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word VoidGetMember(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "No se puede obtener el miembro de void",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static MyError ForConditionError(AstNode node, Word word)
            {
                string type;
                if (word.IsError())
                    type = word.GetType().Name;
                else
                    type = TypeConstants.GetMyTypeName(((PyObj)word).GetMyType());
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Condicion del FOR no es valida. Tipo: " + type + " se esperaba: " + TypeConstants.GetMyTypeName(TypeConstants.BOOLEAN),
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word NoValueReturnedInFunction(AstNode node)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Funcion no tiene retorno, se a retornado null para continuar la ejecucion ",
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            private static void AddError(MyError error)
            {
                ErrorList.Add(error);
                RuntimeEnvironment.Logger.Instance.Log(error);
            }

            public static Word PathNotValid(AstNode node, string path)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    "Direccion de graph o import: " + path + " no es valida",//Chapuz minimo, deberia de decir si es graph o import no los dos
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }

            public static Word DotError(AstNode node, string dotErrorMessage)
            {
                MyError error = new MyError(node.Span.Location.Line,
                    node.Span.Location.Column,
                    node.NodePath.StringValue,
                    dotErrorMessage,//Chapuz minimo, deberia de decir si es graph o import no los dos
                    MyError.ErrorType.Semantic,
                    MyError.ErrorLevel.Fatal);
                AddError(error);
                return error;
            }
        }
    }

}

using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Expressions;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Control;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Jumpers;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.MemoryReadWrite;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Natives;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Terminal;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interfaces
{
    public interface IAstVisitor
    {

        #region Expressions
        Word Visit(AtomicExpr atomicExpr);
        Word Visit(UnaryExpr unaryExpr);
        Word Visit(IncDecExpr incDecExpr);
        Word Visit(ArrayExpr arrayExpr);
        Word Visit(BinaryExpr binaryExpr);
        #endregion

        #region Oop
        Word Visit(ProgramNode programNode);
        Word Visit(ImportNode importNode);
        Word Visit(ClassNode classNode);
        Word Visit(MethodNode methodNode);
        Word Visit(FunctionNode functionNode);
        #endregion

        #region Stmt
        //Control
        Word Visit(Block block);
        Word Visit(IfNode ifNode);
        Word Visit(SwitchNode switchNode);
        Word Visit(SwitchLabelNode switchLabelNode);
        Word Visit(WhileNode whileNode);
        Word Visit(DoWhileNode doWhileNode);
        Word Visit(ForNode forNode);
        //Jumpers
        Word Visit(ContinueNode continueNode);
        Word Visit(BreakNode breakNode);
        Word Visit(ReturnNode returnNode);
        //MemoryReadWrite
        Word Visit(Assignment assignment);
        Word Visit(Declaration declaration);
        Word Visit(ProcedureAccess procedureAccess);
        Word Visit(IdentifierAccess identifierAccess);
        Word Visit(ExprAccess exprAccess);
        Word Visit(IndexAccess indexAccess);
        Word Visit(ObjectCreationAccess objectCreationAccess);
        Word Visit(MemberAccess memberAccess);
        //Natives
        Word Visit(Alert alert);
        Word Visit(Graph graph);
        Word Visit(LogNode logNode);
        #endregion

        #region Terminal
        Word Visit(BooleanLiteralNode boolLiteralNode);
        Word Visit(CharLiteralNode charLiteralNode);
        Word Visit(IdentifierNode identifierNode);
        Word Visit(NullLiteralNode nullLiteralNode);
        Word Visit(NumberLiteralNode numberLiteralNode);
        Word Visit(StringLiteralNode stringLiteralNode);
        #endregion
    }
}

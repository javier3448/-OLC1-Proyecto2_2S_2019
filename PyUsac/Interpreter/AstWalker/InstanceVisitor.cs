using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.Memory;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using System;
using System.Collections.Generic;
using static _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker.ErrorHelper;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker
{
    public class InstanceVisitor : BaseVisitor
    {
        public InstanceVisitor(StaticEntity entity, ClassNode classNode)
        {
            StaticEntity = entity;
            classNode.Accept(this);
        }

        #region Overrides
        public override Word Visit(ProgramNode programNode)
        {
            return null;
        }

        public override Word Visit(ImportNode importNode)
        {
            return null;
        }

        public override Word Visit(ClassNode classNode)
        {
            //Inicializa el scope
            GlobalScope = new Scope();
            CurrentScope = GlobalScope;

            Word result;

            foreach (var def in classNode.DefinitonList)
            {
                result = def.Accept(this);
                if (IsError(result))
                    continue;
                if (def.GetNodeType() == NodeType.Method || def.GetNodeType() == NodeType.Function)
                    Procedures.Add((Procedure)result);
                else//Tiene que ser class definition
                    throw new Exception(String.Format("Class node solo puede tener definiciones tipo: {0}, {1}. No: ", NodeType.Method.ToString(), NodeType.Function.ToString(), def.GetNodeType().ToString()));
            }

            foreach (var stmt in classNode.StmtList)
            {
                result = stmt.Accept(this);
                if (IsJumper(result))
                    ErrorFactory.NotValidJumper(stmt, (Jumper)result);
            }

            return null;
        }
        #endregion

        #region Utilities
        public List<PyObj> GetAttributeValues()
        {
            return GlobalScope.ConvertSymbolTableValuesToList();
        }
        #endregion
    }
}

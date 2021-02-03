using _Compi1_Proyecto2.PyUsac.Ast.Base;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Expressions;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Oop;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Control;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Jumpers;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.MemoryReadWrite;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Stmt.Natives;
using _Compi1_Proyecto2.PyUsac.Ast.Node.Terminal;
using _Compi1_Proyecto2.PyUsac.Interfaces;
using _Compi1_Proyecto2.PyUsac.Interpreter.Memory;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.Jumpers;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.MemberSegments;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes.SwitchLabels;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Oop;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Collections;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker.ErrorHelper;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.AstWalker
{
    public abstract class BaseVisitor : IAstVisitor
    {
        protected StaticEntity StaticEntity;
        protected Scope CurrentScope;

        //Ambos miembros son tentativos no se si se van a quedar asi cuando implementemos las clases
        protected Scope GlobalScope;//Contiene las variables globales solamente
        protected readonly HashSet<Procedure> Procedures = new HashSet<Procedure>(new ProcedureSameSignature());//Contiene los metodos y funciones globales

        #region Oop
        public abstract Word Visit(ProgramNode programNode);

        public abstract Word Visit(ImportNode importNode);

        public abstract Word Visit(ClassNode classNode);

        public Word Visit(MethodNode methodNode)
        {
            var method = new Method(methodNode);
            if (Procedures.Contains(method))
                return ErrorFactory.AlreadyDefinedProcedure(methodNode, method.Name, method.ParamCount);
            return method;
        }

        public Word Visit(FunctionNode functionNode)
        {
            var function = new Function(functionNode);
            if (Procedures.Contains(function))
                return ErrorFactory.AlreadyDefinedProcedure(functionNode, function.Name, function.ParamCount);
            return function;
        }
        #endregion

        #region Expressions
        public Word Visit(AtomicExpr atomicExpr)
        {
            var result = atomicExpr.Value.Accept(this);
            if (IsError(result))
                return result;
            //PyObj pyObj;//Quitar comentario de esta linea si se desea pasar por "copia de referencia" los argumentos (i.e. como en java)
            if (result == null)
                return ErrorFactory.VoidExpr(atomicExpr);
            //Quitar comentario de este bloque si se desea pasar por "copia de referencia" los argumentos (i.e. como en java)
            //if (IsMemoryBlock(result))
            //    pyObj = ((MemoryBlock)result).Value;
            //else
            //    pyObj = (PyObj)result;
            return result;
        }

        public Word Visit(UnaryExpr unaryExpr)
        {
            var result = unaryExpr.RightExpr.Accept(this);
            if (IsError(result))
                return result;
            PyObj pyObj;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            result = pyObj.UnaryOperation(unaryExpr.UnaryOperator);
            if (IsError(result))
                return ErrorFactory.Create(unaryExpr, (MyError)result);
            return (PyObj)result;
        }

        public Word Visit(ArrayExpr arrayExpr)
        {
            Word result;
            PyObj pyObj;
            MyArray array = new MyArray(arrayExpr.Expressions.Count);
            int i = 0;
            foreach (var expr in arrayExpr.Expressions)
            {
                result = expr.Accept(this);
                if (IsError(result))
                    return result;
                if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                    pyObj = ((MemoryBlock)result).Value;
                else
                    pyObj = (PyObj)result;
                //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
                array.Array[i].Value = pyObj;
                i++;
            }
            return array;
        }

        public Word Visit(IncDecExpr incDecExpr)
        {
            var result = incDecExpr.Value.Accept(this);
            if (IsError(result))
                return result;
            if (!IsMemoryBlock(result))
                return ErrorFactory.IncDecNonRefError(incDecExpr.Value, incDecExpr.IncDecOperator);
            var memoryBlock = (MemoryBlock)result;
            var pyObj = memoryBlock.Value;
            var pyObjType = pyObj.GetMyType();
            if (!TypeConstants.IsNumber(pyObjType))
                return ErrorFactory.IncDecError(incDecExpr, incDecExpr.IncDecOperator, pyObj);
            var pyObjCopy = MyPrimitiveFactory.CreateCopy((MyPrimitive)pyObj);
            switch (incDecExpr.IncDecOperator)
            {
                case IncDecOperator.PlusPlus:
                    memoryBlock.Value = (PyObj)pyObj.BinaryOperation(BinaryOperator.Plus, new MyInt(1));
                    break;
                case IncDecOperator.MinusMinus:
                    memoryBlock.Value = (PyObj)pyObj.BinaryOperation(BinaryOperator.Minus, new MyInt(1));
                    break;
                default:
                    throw new Exception("Operador Inc dec no valido: " + incDecExpr.IncDecOperator);
            }
            return pyObjCopy;
        }

        public Word Visit(BinaryExpr binaryExpr)
        {
            var result = binaryExpr.LeftExpr.Accept(this);
            PyObj pyObjLeft;
            if (IsError(result))
                return result;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObjLeft = ((MemoryBlock)result).Value;
            else
                pyObjLeft = (PyObj)result;
            //pyObjLeft = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.

            result = pyObjLeft.PreBinaryOperation(binaryExpr.BinaryOperator);//Revisa si es necesario computar el segundo argumento de la expresion. ej para (false && getbool()) no es necesario computar el segundo argumento
            if (result != null)
                return result;

            result = binaryExpr.RightExpr.Accept(this);
            PyObj pyObjRight;
            if (IsError(result))
                return result;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObjRight = ((MemoryBlock)result).Value;
            else
                pyObjRight = (PyObj)result;
            //pyObjRight = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.

            result = pyObjLeft.BinaryOperation(binaryExpr.BinaryOperator, pyObjRight);
            if (IsError(result))
                return ErrorFactory.Create(binaryExpr.LeftExpr, (MyError)result);
            return (PyObj)result;
        }
        #endregion

        #region MemoryReadWrite
        public Word Visit(Assignment assignment)
        {
            var lValue = assignment.LeftValue.Accept(this);
            if (IsError(lValue))
                return lValue;
            if (!IsMemoryBlock(lValue))
                return ErrorFactory.AssignmentNonRefError(assignment.LeftValue);

            var rValue = assignment.RightValue.Accept(this);
            if (IsError(rValue))
                return rValue;
            PyObj rPyObj;
            if (IsError(rValue))
                return rValue;
            if (IsMemoryBlock(rValue))//comentar ese if else si se hace la desereferencia en atomic expr.
                rPyObj = ((MemoryBlock)rValue).Value;
            else
                rPyObj = (PyObj)rValue;
            //rPyObj = (PyObj)rValue;//Descomentar esta linea si se hace la desereferencia en atomic expr.

            var lMemBlock = (MemoryBlock)lValue;
            lMemBlock.Value = rPyObj;

            return rPyObj;//Chapuz medio alto para facilitar el for
        }

        public Word Visit(Declaration declaration)
        {
            var idList = new List<string>();
            Word lResult;
            foreach (var lValue in declaration.LeftValues)
            {
                lResult = lValue.Accept(this);
                if (IsError(lResult))
                    return lResult;
                idList.Add(((MyString)lResult).StringValue);
            }
            Word rValue;
            PyObj rPyObj;
            int i;
            if (declaration.Indexes.IsEmpty())
            {
                if (declaration.RightValue == null)
                    rValue = MyNull.GetInstance();
                else
                    rValue = declaration.RightValue.Accept(this);

                if (IsError(rValue))
                    return rValue;
                if (IsMemoryBlock(rValue))//comentar ese if else si se hace la desereferencia en atomic expr.
                    rPyObj = ((MemoryBlock)rValue).Value;
                else
                    rPyObj = (PyObj)rValue;
                //rPyObj = (PyObj)rValue;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            }
            else //Si tiene indices la declaracion significal que es un array
            {
                Word indexResult;
                PyObj indexPyObj;
                var intIndices = new int[declaration.Indexes.Count];
                //Obtiene la lista de ints en sus indices.
                i = 0;
                foreach (var index in declaration.Indexes)
                {
                    indexResult = index.Accept(this);
                    if (IsError(indexResult))
                        return indexResult;
                    indexPyObj = ((IndexSegment)indexResult).Index;
                    //Chequea que sea del tipo correcto:
                    if (indexPyObj.GetMyType() == TypeConstants.INT)
                        intIndices[i] = ((MyInt)indexPyObj).Int;
                    else if (indexPyObj.GetMyType() == TypeConstants.CHAR)
                        intIndices[i] = ((MyChar)indexPyObj).CharValue;
                    else
                        return ErrorFactory.ArrayDimensionError(index, indexPyObj);
                    //Chequea que tenga un rango valido
                    if (intIndices[i] < 0)
                        return ErrorFactory.ArrayDimensionError(index, indexPyObj);
                    i++;
                }
                if (declaration.RightValue == null)
                    rValue = MyArrayFactory.Create(intIndices);
                else
                    rValue = declaration.RightValue.Accept(this);

                if (IsError(rValue))
                    return rValue;
                if (IsMemoryBlock(rValue))//comentar ese if else si se hace la desereferencia en atomic expr.
                    rPyObj = ((MemoryBlock)rValue).Value;
                else
                    rPyObj = (PyObj)rValue;
                //rPyObj = (PyObj)rValue;//Descomentar esta linea si se hace la desereferencia en atomic expr.
                //Verifica que rValue sea un array con las dimensiones especificadas en intIndices
                if (rPyObj.GetMyType() != TypeConstants.ARRAY)
                    return ErrorFactory.ArrayTypeError(declaration, intIndices, rPyObj);
                if (!((MyArray)rPyObj).IsNDimensionalArray(intIndices))
                    return ErrorFactory.ArrayTypeError(declaration, intIndices, rPyObj);
            }

            Word result;
            i = 0;//Chapus minimo
            foreach (var id in idList)
            {
                result = CurrentScope.Add(id, rPyObj);
                if (IsError(result))
                    ErrorFactory.Create(declaration.LeftValues[i], (MyError)result);
                i++;
            }

            return rPyObj;//Chapuz medio alto para facilitar el for
        }


        public Word Visit(ProcedureAccess procedureAccess)
        {
            //Conseguir id
            var result = procedureAccess.IdentifierNode.Accept(this);
            if (IsError(result))
                return result;
            var functionId = ((MyString)result).StringValue;
            //Conseguir argumentos
            var memBlockArguments = new List<MemoryBlock>(procedureAccess.Arguments.Count);
            MemoryBlock memBlock;
            foreach (var argument in procedureAccess.Arguments)
            {
                result = argument.Accept(this);
                if (IsError(result))
                    return result;
                if (!IsMemoryBlock(result))
                    memBlock = new MemoryBlock((PyObj)result);
                else
                    memBlock = (MemoryBlock)result;
                memBlockArguments.Add(memBlock);
            }
            return new ProcedureSegment(functionId, memBlockArguments);
        }

        public Word Visit(ObjectCreationAccess objectCreationAccess)
        {
            var myStringClassName = (MyString)objectCreationAccess.IdentifierNode.Accept(this);
            var className = myStringClassName.StringValue.ToLowerInvariant();
            var classDefinition = StaticEntity.GetClass(ClassDefinition.EmptyClass(className));
            if (classDefinition == null)
                return ErrorFactory.NonDefinedClass(objectCreationAccess, className);
            var classInstance = classDefinition.CreateInstance();
            return new ExprSegment(classInstance);
        }

        public Word Visit(IdentifierAccess identifierAccess)
        {
            var result = identifierAccess.IdentifierNode.Accept(this);
            if (IsError(result))
                return result;
            var idValue = ((MyString)result).StringValue;
            return new IdentifierSegment(idValue);
        }

        public Word Visit(ExprAccess exprAccess)
        {
            var result = exprAccess.Expr.Accept(this);
            if (IsError(result))
                return result;
            PyObj pyObj;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            return new ExprSegment(pyObj);
        }

        public Word Visit(IndexAccess indexAccess)
        {
            var result = indexAccess.Expr.Accept(this);
            if (IsError(result))
                return result;
            PyObj pyObj;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            //rPyObj = (PyObj)rValue;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            return new IndexSegment(pyObj);
        }

        public Word Visit(MemberAccess memberAccess)
        {
            var segmentResult = memberAccess.FirstSegment.Accept(this);
            if (IsError(segmentResult))
                return segmentResult;

            PyObj pyObj;
            var segment = (Segment)segmentResult;
            var segmentType = segment.GetSegmentType();
            Word getSegmentResult;
            switch (segmentType)
            {
                case SegmentType.Identifier:
                    {
                        string id = ((IdentifierSegment)segment).Id;
                        getSegmentResult = CurrentScope.Get(id);
                        if (IsError(getSegmentResult))//Revisa si esta entre las definiciones estaticas
                        {
                            getSegmentResult = StaticEntity.GetMember((IdentifierSegment)segment);
                            if (IsError(getSegmentResult))
                                return ErrorFactory.Create(memberAccess.FirstSegment, (MyError)getSegmentResult);
                        }
                        pyObj = ((MemoryBlock)getSegmentResult).Value;
                    }
                    break;
                case SegmentType.Procedure://pyObj en este caso es un pyObj o null (i.e. void)
                    {
                        getSegmentResult = InvokeProcedure((ProcedureSegment)segment);
                        if (IsError(getSegmentResult))//Revisa si esta entre las definiciones estaticas
                        {
                            getSegmentResult = StaticEntity.GetMember((ProcedureSegment)segment);
                            if (IsError(getSegmentResult))
                                return ErrorFactory.Create(memberAccess.FirstSegment, (MyError)getSegmentResult);
                        }
                        pyObj = (PyObj)getSegmentResult;
                    }
                    break;
                case SegmentType.Expr:
                    {
                        getSegmentResult = ((ExprSegment)segment).Expr;
                        if (IsError(getSegmentResult))
                            return getSegmentResult;
                        if (IsMemoryBlock(getSegmentResult))//comentar ese if else si se hace la desereferencia en atomic expr.
                            pyObj = ((MemoryBlock)getSegmentResult).Value;
                        else
                            pyObj = (PyObj)getSegmentResult;
                    }
                    break;
                default:
                    throw new Exception("Tipo de segmento no reconocido: " + segmentType.ToString());
            }

            MemberSegment memberSegment;
            foreach (var optionalSegment in memberAccess.OptionalSegments)
            {
                segmentResult = optionalSegment.Accept(this);
                if (IsError(segmentResult))
                    return segmentResult;
                memberSegment = (MemberSegment)segmentResult;
                if (pyObj == null)
                    return ErrorFactory.VoidGetMember(optionalSegment);
                getSegmentResult = pyObj.GetMember(memberSegment);
                if (IsError(getSegmentResult))
                    return ErrorFactory.Create(optionalSegment, (MyError)getSegmentResult);
                if (IsMemoryBlock(getSegmentResult))
                    pyObj = ((MemoryBlock)getSegmentResult).Value;
                else
                    pyObj = (PyObj)getSegmentResult;
            }
            return getSegmentResult;
        }
        #endregion

        #region Stmts
        public Word Visit(Block block)
        {
            PushScope();
            Word result;
            foreach (var stmt in block.StmtList)
            {
                result = stmt.Accept(this);
                if (IsJumper(result))
                {
                    var jumper = (Jumper)result;
                    if (jumper.WasPopped())//Chapuz Alto para que no haga pop until mas de una vez cuando hay bloques adentro de bloques
                        return jumper;
                    if (ControlStack.PopUntil(jumper))
                        return jumper;
                    else
                        ErrorFactory.NotValidJumper(stmt, jumper);
                }
            }
            PopScope();
            return null;
        }
        public Word Visit(IfNode ifNode)
        {
            var result = ifNode.Condition.Accept(this);
            if (IsError(result))
                return result;
            PyObj pyObj;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            if (pyObj.GetMyType() != TypeConstants.BOOLEAN)
                return ErrorFactory.IfError(ifNode.Condition, pyObj);
            var myBool = (MyBoolean)pyObj;
            if (myBool.Bool)
            {
                return ifNode.Block.Accept(this);
            }
            else if (ifNode.Else != null)
            {
                return ifNode.Else.Accept(this);
            }
            return null;
        }
        public Word Visit(SwitchNode switchNode)
        {
            ControlStack.Push(ControlType.Switch);//Push into control stack
            var result = switchNode.Flag.Accept(this);
            if (IsError(result))
            {
                ControlStack.Pop();
                return result;
            }
            PyObj pyObjFlag;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObjFlag = ((MemoryBlock)result).Value;
            else
                pyObjFlag = (PyObj)result;
            //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            var switchElements = new AstNode[switchNode.SwitchElements.Count];
            switchNode.SwitchElements.CopyTo(switchElements);
            //Caso que dio true a pyObjFlag cuando se comparar con ==
            int? trueCaseIndex = null;
            int? defaultIndex = null;
            SwitchLabel switchLabel;
            PyObj pyObjCase;
            MyBoolean caseEvaluationResult;
            for (int i = 0; i < switchElements.Length; i++)
            {
                //Quitar comentario si se desea que no puedan venir declaraciones dentro del if
                //if (switchElements[i].GetNodeType() == NodeType.Declaration)
                //{
                //    ControlStack.Pop();
                //    return ErrorFactory.DeclarationInSwitch(switchElements[i]);
                //}
                if (switchElements[i].GetNodeType() != NodeType.SwitchLabel)
                    continue;
                result = switchElements[i].Accept(this);
                if (IsError(result))
                {
                    ControlStack.Pop();
                    return result;
                }
                switchLabel = (SwitchLabel)result;
                if (switchLabel.GetLabelType() == LabelType.Default)
                {
                    if (defaultIndex != null)
                    {
                        ControlStack.Pop();
                        return ErrorFactory.MultipleDefaultInSwitch(switchElements[i]);
                    }
                    else
                    {
                        defaultIndex = i;
                    }
                }
                else
                {
                    pyObjCase = switchLabel.Value;
                    result = pyObjFlag.BinaryOperation(BinaryOperator.PyEquals, pyObjCase);
                    if (IsError(result))
                    {
                        ControlStack.Pop();
                        return ErrorFactory.Create(switchElements[i], (MyError)result);
                    }
                    if (((PyObj)result).GetMyType() != TypeConstants.BOOLEAN)
                    {
                        ControlStack.Pop();
                        return ErrorFactory.NotValidOperationInSwitch(switchElements[i]);
                    }
                    caseEvaluationResult = (MyBoolean)result;
                    if (caseEvaluationResult.Bool)
                    {
                        if (trueCaseIndex != null)
                        {
                            ControlStack.Pop();
                            return ErrorFactory.MultipleTrueCasesInSwitch(switchElements[i]);
                        }
                        else
                        {
                            trueCaseIndex = i;
                        }
                    }
                }
            }
            int startIndex = switchElements.Length;
            if (trueCaseIndex != null)
                startIndex = (int)trueCaseIndex;
            else if (defaultIndex != null)
                startIndex = (int)defaultIndex;

            for (int i = startIndex; i < switchElements.Length; i++)
            {
                if (switchElements[i].GetNodeType() == NodeType.SwitchLabel)
                    continue;
                result = switchElements[i].Accept(this);
                if (IsJumper(result))
                {
                    var jumper = (Jumper)result;
                    if (jumper.CanJump(ControlType.Switch))
                    {
                        if (!jumper.WasPopped())//Chapuz alto en el caso que retorne un jumper que no este metido dentro de ningun scope
                            ControlStack.Pop();
                        return null;
                    }
                    if (jumper.WasPopped())//Chapuz Alto para que no haga pop until mas de una vez cuando hay bloques adentro de bloques
                        return jumper;
                    if (ControlStack.PopUntil(jumper))
                        return jumper;
                    else
                        ErrorFactory.NotValidJumper(switchElements[i], jumper);
                }
            }
            return null;
        }
        public Word Visit(SwitchLabelNode switchLabelNode)
        {
            if (switchLabelNode.IsDefault())
                return new SwitchLabel();
            var result = switchLabelNode.Expr.Accept(this);
            if (IsError(result))
                return result;
            PyObj pyObj;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            return new SwitchLabel(pyObj);
        }
        public Word Visit(WhileNode whileNode)
        {
            ControlStack.Push(ControlType.Loop);//Push into control stack
            Word result;
            PyObj pyObj;
            MyBoolean myBool;
            while (true)
            {
                result = whileNode.Condition.Accept(this);
                if (IsError(result))
                {
                    ControlStack.Pop();
                    return result;
                }
                if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                    pyObj = ((MemoryBlock)result).Value;
                else
                    pyObj = (PyObj)result;
                //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
                if (pyObj.GetMyType() != TypeConstants.BOOLEAN)
                {
                    ControlStack.Pop();
                    return ErrorFactory.WhileError(whileNode, pyObj);
                }
                myBool = (MyBoolean)pyObj;
                if (!myBool.Bool)
                    break;
                result = whileNode.Block.Accept(this);
                if (IsJumper(result))
                {
                    var jumper = (Jumper)result;
                    if (jumper.GetJumperType() == JumperType.Breaker)
                        return null;
                    else if (jumper.GetJumperType() != JumperType.Continue)
                        return jumper;
                    //Si es continue no tiene que hacer nada porque el block ya interumpio la ejecucion 1 vez
                }
            }
            ControlStack.Pop();
            return null;
        }
        public Word Visit(DoWhileNode doWhileNode)
        {
            ControlStack.Push(ControlType.Loop);
            Word result;
            PyObj pyObj;
            MyBoolean myBool;
            while (true)
            {
                result = doWhileNode.Block.Accept(this);
                if (IsJumper(result))
                {
                    var jumper = (Jumper)result;
                    if (jumper.GetJumperType() == JumperType.Breaker)
                        return null;
                    else if (jumper.GetJumperType() != JumperType.Continue)
                        return jumper;
                    //Si es continue no tiene que hacer nada porque el block ya interumpio la ejecucion 1 vez
                }
                result = doWhileNode.Condition.Accept(this);
                if (IsError(result))
                {
                    ControlStack.Pop();
                    return result;
                }
                if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                    pyObj = ((MemoryBlock)result).Value;
                else
                    pyObj = (PyObj)result;
                //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
                if (pyObj.GetMyType() != TypeConstants.BOOLEAN)
                {
                    ControlStack.Pop();
                    return ErrorFactory.WhileError(doWhileNode, pyObj);
                }
                myBool = (MyBoolean)pyObj;
                if (!myBool.Bool)
                    break;
            }
            ControlStack.Pop();
            return null;
        }
        public Word Visit(ForNode forNode)
        {
            ControlStack.Push(ControlType.Loop);
            Word result;
            PyObj pyObjCond;
            MyBoolean myBoolCond;
            // Declara o asigna la flag en un scope intermedio entre el scope que contiene al for y el del bloque del for
            PushScope();
            result = forNode.Flag.Accept(this);
            if (IsError(result))
            {
                PopScope();
                ControlStack.Pop();
                return result;
            }

            while (true)
            {
                result = forNode.Condition.Accept(this);
                if (IsError(result))
                {
                    PopScope();
                    ControlStack.Pop();
                    return result;
                }
                if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                    pyObjCond = ((MemoryBlock)result).Value;
                else
                    pyObjCond = (PyObj)result;
                //pyObjCond = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
                if (pyObjCond.GetMyType() != TypeConstants.BOOLEAN)
                {
                    PopScope();
                    ControlStack.Pop();
                    return ErrorFactory.ForConditionError(forNode, pyObjCond);
                }
                myBoolCond = (MyBoolean)pyObjCond;
                if (!myBoolCond.Bool)
                    break;
                result = forNode.Block.Accept(this);
                if (IsJumper(result))
                {
                    var jumper = (Jumper)result;
                    if (jumper.GetJumperType() == JumperType.Breaker)
                    {
                        PopScope();
                        return null;
                    }
                    else if (jumper.GetJumperType() != JumperType.Continue)
                    {
                        PopScope();
                        return jumper;
                    }
                    //Si es continue no tiene que hacer nada porque el block ya interumpio la ejecucion 1 vez
                }
                result = forNode.Update.Accept(this);
                if (IsError(result))
                {
                    PopScope();
                    ControlStack.Pop();
                    return result;
                }
            }
            PopScope();
            ControlStack.Pop();
            return null;
        }

        //Jumpers
        public Word Visit(ContinueNode continueNode)
        {
            return new Continue();
        }
        public Word Visit(BreakNode breakNode)
        {
            return new Breaker();
        }
        public Word Visit(ReturnNode returnNode)
        {
            if (returnNode.Expr == null)
                return new Return();
            var result = returnNode.Expr.Accept(this);
            PyObj pyObj;
            if (IsError(result))
            {
                ErrorFactory.CantReturnError(returnNode.Expr, result);
                return new Return(MyNull.GetInstance());
            }
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            return new Return(pyObj);
        }
        //Natives
        public Word Visit(Alert alert)
        {
            var result = alert.Expr.Accept(this);
            if (IsError(result))
                return ErrorFactory.AlertError(alert.Expr, result);
            PyObj pyObj;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            //pyObj = (PyObj)result;//Descomentar esta linea si se hace la desereferencia en atomic expr.
            RuntimeEnvironment.Console.Instance.Alert(pyObj);
            return null;
        }

        public Word Visit(Graph graph)
        {
            var result = graph.FileName.Accept(this);
            if (IsError(result))
                return ErrorFactory.GraphError(graph.FileName, "FileName", result);
            PyObj pyObjFileName;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObjFileName = ((MemoryBlock)result).Value;
            else
                pyObjFileName = (PyObj)result;
            if (pyObjFileName.GetMyType() != TypeConstants.STRING)
                return ErrorFactory.GraphError(graph.DotSource, "FileName", result);

            result = graph.DotSource.Accept(this);
            if (IsError(result))
                return ErrorFactory.GraphError(graph.DotSource, "DotSource", result);
            PyObj pyObjDotSource;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObjDotSource = ((MemoryBlock)result).Value;
            else
                pyObjDotSource = (PyObj)result;
            if (pyObjDotSource.GetMyType() != TypeConstants.STRING)
                return ErrorFactory.GraphError(graph.DotSource, "DotSource", result);

            //Chapuz hacer un nuevo my string con el pyusac type porque graph solo recibe mystrings
            var path = ((MyString)pyObjFileName).StringValue;
            if (path.Length < 1)
                return ErrorFactory.PathNotValid(graph.FileName, path);
            if ((path[0] != Path.DirectorySeparatorChar) || (path[0] != 'C' && path[0] != 'D' && path[1] != ':'))//tiene que agregar al path la ruta del archivo con el que este nodo fue creado
                path = graph.NodePath.GetParentPath() + Path.DirectorySeparatorChar + PyPath.ReplaceSepartors(path);

            var pyPath = new PyPath(path);
            if (!Directory.Exists(pyPath.GetParentPath()))
                return ErrorFactory.PathNotValid(graph.FileName, pyPath.GetParentPath());

            var myStringPath = new MyString(path);
            var myStringDotSource = (MyString)pyObjDotSource;

            var environmentGraphResult = RuntimeEnvironment.Console.Instance.Graph(myStringPath, myStringDotSource);
            if (environmentGraphResult != null)
                return ErrorFactory.DotError(graph.DotSource, environmentGraphResult);
            return null;
        }

        public Word Visit(LogNode logNode)
        {
            var result = logNode.Expr.Accept(this);
            if (IsError(result))
                return ErrorFactory.LogError(logNode.Expr, result);
            PyObj pyObj;
            if (IsMemoryBlock(result))//comentar ese if else si se hace la desereferencia en atomic expr.
                pyObj = ((MemoryBlock)result).Value;
            else
                pyObj = (PyObj)result;
            RuntimeEnvironment.Console.Instance.PrintLine(pyObj);
            return null;
        }
        #endregion

        #region Terminals
        public Word Visit(BooleanLiteralNode boolLiteralNode)
        {
            return MyPrimitiveFactory.Create(boolLiteralNode.Value);
        }

        public Word Visit(CharLiteralNode charLiteralNode)
        {
            return MyPrimitiveFactory.Create(charLiteralNode.Value);
        }

        public Word Visit(IdentifierNode identifierNode)
        {
            return MyPrimitiveFactory.Create(identifierNode.Value);
        }

        public Word Visit(NullLiteralNode nullLiteralNode)
        {
            return MyNull.GetInstance();
        }

        public Word Visit(NumberLiteralNode numberLiteralNode)
        {
            return MyPrimitiveFactory.Create(numberLiteralNode.Value);
        }

        public Word Visit(StringLiteralNode stringLiteralNode)
        {
            return MyPrimitiveFactory.Create(stringLiteralNode.Value);
        }
        #endregion

        #region Utilities
        protected void PushScope()
        {
            CurrentScope = new Scope(CurrentScope);
        }
        protected void GotoBottomScope()
        {
            while (!CurrentScope.IsBottom())
                PopScope();
        }
        protected void PopScope()
        {
            CurrentScope = CurrentScope.OuterScope;
        }

        public Word GetGlobalAttribute(string identifier)
        {
            return GlobalScope.Get(identifier);
        }

        public Word GetGlobalAttribute(IdentifierSegment identifierSegment)
        {
            return GlobalScope.Get(identifierSegment.Id);
        }

        public Word InvokeProcedure(ProcedureSegment procedureSegment)
        {
            var name = procedureSegment.Id;
            name = name.ToLowerInvariant();
            var arguments = procedureSegment.Arguments;
            var argumentCount = arguments.Count;

            Procedure procedure;
            Procedures.TryGetValue(Procedure.EmptySignature(name, argumentCount), out procedure);
            if (procedure == null)
                return new MyError("No existe una funcion con el nombre: " + name + " y numero de parametros: " + argumentCount + " en las definiciones globales");
            //scope antes de llamar a la funcion
            var previousScope = CurrentScope;
            //Pasamos al scope global para llamar a la funcion
            CurrentScope = GlobalScope;
            ControlType controlType;
            if (procedure.GetDefinitionType() == DefinitionType.Function)
                controlType = ControlType.Function;
            else if (procedure.GetDefinitionType() == DefinitionType.Method)
                controlType = ControlType.Method;
            else
                throw new Exception("definicion no valida. tiene que ser funciton o method");
            ControlStack.Push(controlType);
            //hacemos las declaraciones del metodo en un nuevo scope
            PushScope();
            Word result;
            for (int i = 0; i < argumentCount; i++)
            {
                result = CurrentScope.Add(procedure.ParamNames[i], arguments[i]);
                if (IsError(result))
                {
                    ControlStack.Pop();
                    CurrentScope = previousScope;
                    return result;
                }
            }
            //Los stmt del metodo:
            foreach (var stmt in procedure.Stmts)
            {
                result = stmt.Accept(this);
                if (IsJumper(result))
                {
                    var jumper = (Jumper)result;
                    if (!jumper.WasPopped() && !ControlStack.PopUntil(jumper))
                    {
                        ErrorFactory.NotValidJumper(stmt, jumper);
                        continue;
                    }
                    CurrentScope = previousScope;
                    return ((Return)jumper).Obj;
                }
            }
            //Si termina de visitar los stmt, no entro jumper y es una funcion retorna MyNull y reporta un error
            ControlStack.Pop();
            CurrentScope = previousScope;
            if (controlType == ControlType.Method)
            {
                return null;
            }
            else
            {
                ErrorFactory.NoValueReturnedInFunction(procedure.Stmts.Last());
                return MyNull.GetInstance();
            }
        }
        #endregion
    }
}

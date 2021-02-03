using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.Memory
{
    /// <summary>
    /// mas o menos un Stack de symbolTables con funcionalidad adicional
    /// </summary>
    public class Scope
    {
        private readonly SymbolTable SymbolTable = new SymbolTable();

        public readonly Scope OuterScope;

        public Scope(Scope outerScope)
        {
            OuterScope = outerScope;
        }

        public Scope()
        {
            OuterScope = null;
        }

        public bool IsBottom()
        { 
            return OuterScope == null;
        }

        public Scope GetOuterScope()
        {
            return OuterScope;
        }

        #region SymbolTable delegation
        public MyError Add(string key, PyObj value)
        {
            if (SymbolTable.Contains(key))
                return ScopeErrorFactory.AlreadyDefined(key);
            SymbolTable.UnsafeAdd(key, value);
            return null;
        }

        //Chapuz maximo porque el enunciado es una mierda que no esta bien pensado, los parametros se pasa como referencia, no como copia de referencia. Solo Dios sabe como putas se tratan las referencias cuando estan en return o cuando son rValue en una asignacion o declaracion. La usac es una mierda :(
        public MyError Add(string key, MemoryBlock memoryBlock)
        {
            if (SymbolTable.Contains(key))
                return ScopeErrorFactory.AlreadyDefined(key);
            SymbolTable.UnsafeAdd(key, memoryBlock);
            return null;
        }

        /// <summary>
        /// SOLO BORRA EN EL TOPE DEL STACK DE SCOPES!
        /// </summary>
        /// <param name="key"></param>
        public void UnsafeDelete(string key)
        {
            SymbolTable.UnsafeDelete(key);
        }

        public MyError Set(string key, PyObj value)
        {
            if (SymbolTable.Set(key, value))
            {
                return null;
            }
            if (!IsBottom())
            {
                return OuterScope.Set(key, value);
            }
            return ScopeErrorFactory.NotFound(key);
        }

        public Word Get(string key)
        {
            MemoryBlock memoryBlock = SymbolTable.Get(key);
            if (memoryBlock != null)
            {
                return memoryBlock;
            }
            if (!IsBottom())
            {
                return OuterScope.Get(key);
            }
            return ScopeErrorFactory.NotFound(key);
        }
        #endregion

        /// <summary>
        /// Revisa si no exite la referencia en el stack
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            if (SymbolTable.Contains(key))
            {
                return true;
            }
            if (!IsBottom())
            {
                return OuterScope.Contains(key);
            }
            return false;
        }

        /// <summary>
        /// retorna una copia de los elementos contenidos en este scope (no en todo el "stack") 
        /// </summary>
        /// <returns></returns>
        public List<PyObj> ConvertSymbolTableValuesToList()
        {
            var list = new List<PyObj>();
            MemoryBlock memoryBlock;
            foreach (var pair in SymbolTable.Table)
            {
                memoryBlock = pair.Value;
                list.Add(memoryBlock.Value);
            }
            return list;
        }

        private static class ScopeErrorFactory
        {
            public static MyError NotFound(string key)
            {
                return new MyError("No se pudo encontrar la llave: " + key + " en la tabla de simbolos.");
            }

            public static MyError AlreadyDefined(string key)
            {
                return new MyError("Ya existe una referencia con la llave: " + key + " en esta tabla de simbolos");
            }
        }
    }
}

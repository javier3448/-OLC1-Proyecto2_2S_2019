using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.Memory
{
    public class SymbolTable
    {
        public readonly Dictionary<string, MemoryBlock> Table = new Dictionary<string, MemoryBlock>();//Chapuz medio: esto deberia de ser private. Pero se dejo en public para que scope pueda retorna una copia de atributos al instance visitor

        public SymbolTable() { }

        /// <summary>
        /// Retorna false si no agrego el valor a la tabla de simbolos. Solo se agregan valores que no esten presentes
        /// </summary>
        /// <param name="type"></param
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(string key, PyObj value)
        {
            key = key.ToLowerInvariant();
            if (Table.ContainsKey(key))
            {
                return false;
            }
            if (value.IsNull())
                Table[key] = new MemoryBlock(MyNull.GetInstance());
            if (value.IsPrimitive())
                Table[key] = new MemoryBlock(MyPrimitiveFactory.CreateCopy((MyPrimitive)value));
            else//Only option left is for value to be customInstance
                Table[key] = new MemoryBlock(value);
            return true;
        }

        public void UnsafeDelete(string key)
        {
            Table.Remove(key);
        }

        /// <summary>
        /// Agrega sin verficar que no contenga la llave. Usarse solo si se verifico que no esten presentes los valores antes
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UnsafeAdd(string key, PyObj value)
        {
            key = key.ToLowerInvariant();
            if (value.IsNull())
                Table[key] = new MemoryBlock(MyNull.GetInstance());
            if (value.IsPrimitive())
                Table[key] = new MemoryBlock(MyPrimitiveFactory.CreateCopy((MyPrimitive)value));
            else//Only option left is for value to be customInstance
                Table[key] = new MemoryBlock(value);
        }

        //Chapuz maximo porque el enunciado es una mierda que no esta bien pensado, los parametros se pasa como referencia, no como copia de referencia. Solo Dios sabe como putas se tratan las referencias cuando estan en return o cuando son rValue en una asignacion o declaracion. La usac es una mierda :(
        public void UnsafeAdd(string key, MemoryBlock memoryBlock)
        {
            key = key.ToLowerInvariant();
            Table[key] = memoryBlock;
        }

        /// <summary>
        /// Retorna false si no se cambio el valor de la llave en la tabla de simbolos. Solo se cambian valores que ya estan presentes
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set(string key, PyObj value)
        {
            key = key.ToLowerInvariant();
            MemoryBlock memoryBlock;
            if (!Table.TryGetValue(key, out memoryBlock))
            {
                return false;
            }
            if (value.IsNull())
                memoryBlock.Value = MyNull.GetInstance();
            if (value.IsPrimitive())
                memoryBlock.Value = MyPrimitiveFactory.CreateCopy((MyPrimitive)value);
            else//Only option left is for value to be customInstance
                memoryBlock.Value = value;
            return true;
        }

        //Retorna null si no existe la llave en la tabla
        public MemoryBlock Get(string key)
        {
            key = key.ToLowerInvariant();
            MemoryBlock memoryBlock;
            if (Table.TryGetValue(key, out memoryBlock))
            {
                return memoryBlock;
            }
            return null;
        }

        public bool Contains(string key)
        {
            key = key.ToLowerInvariant();
            return Table.ContainsKey(key);
        }
    }
}

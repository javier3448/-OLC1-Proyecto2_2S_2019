using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Collections
{
    public static class MyArrayFactory
    {
        public static MyArray Create(params int[] dimensions)
        {
            var myArray = new MyArray(dimensions.First());
            if (dimensions.Length < 2)
                return myArray;
            dimensions = dimensions.RemoveFirst();
            foreach (var memBlock in myArray.Array)
            {
                memBlock.Value = Create(dimensions);
            }
            return myArray;
        }

        //Chapuz minimo para simular un stack. OJO: no cambia la referencia(el parametro array) hay que hacer como si fuera un strign
        public static int[] RemoveFirst(this int[] array)
        {
            var newSize = array.Length - 1;
            var result = new int[newSize];
            Array.Copy(array, 1, result, 0, newSize);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.Utilities
{

    //Solo soporta hasta 
    public static class TypeConstants
    {
        public const int NULL = 0;
        public const int BOOLEAN = 1;
        public const int INT = 2;
        public const int CHAR = 3;
        public const int DOUBLE = 4;
        public const int STRING = 5;
        public const int ARRAY = 6;

        public static int CurrentNumberOfTypes = 7;
        private static Dictionary<string, int> TypeHashtable = new Dictionary<string, int>();
        
        /// <summary>
        /// Agrega un tipos a la tabla de tipos y luego retorna el int positivo que se asigno. CUIDADO: Si ya existe un tipo con ese nombre retorna -1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int AddType(string name)
        {
            if (TypeHashtable.ContainsKey(name))
            {
                return -1;
            }
            int typeId = CurrentNumberOfTypes;
            TypeHashtable.Add(name, typeId);
            CurrentNumberOfTypes++;
            return typeId;
        }
        /// <summary>
        /// Tira Exception para  si no encontro el tipo. NO TINE LOS TIPOS PRIMITIVOS, Usar Primitive type para conseguir los numeros de los primitivos
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetMyTypeId(string name)
        {
            return TypeHashtable[name];
        }

        public static void ClearTypeHashtable()
        {
            TypeHashtable.Clear();
        }

        /// <summary>
        /// TIRA EXCEPTION SI NO ESTA EL NUMERO EN LA TABLA.
        /// Solo usar este metodo para reportar errores.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetMyTypeName(int type)
        {
            switch (type)
            {
                case NULL:
                    return "NULL";
                case BOOLEAN:
                    return "BOOLEAN";
                case INT:
                    return "INT";
                case CHAR:
                    return "CHAR";
                case DOUBLE:
                    return "DOUBLE";
                case STRING:
                    return "STRING";
                case ARRAY:
                    return "ARRAY";
            }
            foreach (KeyValuePair<string, int> entry in TypeHashtable)
            {
                if (entry.Value == type)
                {
                    return entry.Key;
                }
            }
            throw new Exception("No existe el tipo: " + type + ". en la tabla de tipos");
        }

        public static bool IsNumber(int pyObjType)
        {
            return pyObjType == TypeConstants.INT || pyObjType == TypeConstants.CHAR || pyObjType == TypeConstants.DOUBLE;
        }
    }
}

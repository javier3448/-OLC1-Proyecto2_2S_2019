using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType
{
    /// <summary>
    /// Tipo base del interprete. No puede exister ningun tipo de dato en el interprete que no herede de el. Incluye los tipos no validos como Error
    /// Esta clase fue creada para que los metodos del interprete puedan retornar un valor que pueda ser MyObj o MyError
    /// </summary>
    public abstract class Word
    {
        public abstract bool IsError();

        //Chapuz medio alto porque no podemos usar instanceof (as and is in c#)
        public abstract bool IsMemoryBlock();

        //Chapuz medio alto porque no podemos usar instanceof (as and is in c#)
        public abstract bool IsJumper();
    }
}

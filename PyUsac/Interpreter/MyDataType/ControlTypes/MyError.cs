using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes
{
    public class MyError : Word
    {
        public ErrorLocation Location;

        public string Message;
        public ErrorType Type;
        public ErrorLevel Level;

        public MyError(string message)
        {
            Init(new ErrorLocation(), message, ErrorType.None, ErrorLevel.None);
        }

        public MyError(ErrorLocation location, string message)
        {
            Init(location, message, ErrorType.None, ErrorLevel.None);
        }

        public MyError(int line, int column, string path, string message)
        {
            Init(new ErrorLocation(line, column, path), message, ErrorType.None, ErrorLevel.None);
        }

        public MyError(int line, int column, string path, string message, ErrorType type, ErrorLevel level)
        {
            Init(new ErrorLocation(line, column, path), message, type, level);
        }

        private void Init(ErrorLocation location, string message, ErrorType type, ErrorLevel level)
        {
            Location = location;
            Message = message;
            Type = type;
            Level = level;
        }

        /// <summary>
        /// Setea todas las propiedades excepto message
        /// </summary>
        public void Set(int line, int column, string path, ErrorType type, ErrorLevel level)
        {
            Location = new ErrorLocation(line, column, path);
            Type = type;
            Level = level;
        }

        public sealed override bool IsError()
        {
            return true;
        }

        public override bool IsMemoryBlock()
        {
            return false;
        }

        public override bool IsJumper()
        {
            return false;
        }
        public class ErrorLocation
        {
            public int? Line;
            public int? Column;
            public string FilePath;

            public ErrorLocation() { }

            public ErrorLocation(int? line, int? column)
            {
                Line = line;
                Column = column;
            }

            public ErrorLocation(int? line, int? column, string filePath)
            {
                Line = line + 1;
                Column = column + 1;//Chapuz bajo para que muestre el error empezando en 1
                FilePath = filePath;
            }
        }

        public enum ErrorType
        {
            None,
            Preprocess,
            Lexic,
            Syntactic,
            Semantic,
            Linking
        }

        public enum ErrorLevel
        {
            None = 0,
            Info = 1,
            Warning = 2,
            Fatal = 3,
            System = 4
        }
    }
}

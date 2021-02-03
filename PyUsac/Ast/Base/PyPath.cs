using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Ast.Base
{
    /// <summary>
    /// Wrapper de string. Deberia de ahorarnos memoria, pero no estoy seguro si mi razonamiento es correcto:/
    /// </summary>
    public class PyPath
    {
        private string _stringValue;
        public string StringValue {
            get
            {
                return _stringValue;
            }
            set 
            {
                _stringValue = ReplaceSepartors(value);
            } 
        }

        private static readonly Regex AcceptedFileSepartorsRegex = new Regex("[\\|/]");

        public static string ReplaceSepartors(string path)
        {
            return AcceptedFileSepartorsRegex.Replace(path, Path.DirectorySeparatorChar.ToString());
        }

        public PyPath(string path)
        {
            this.StringValue = path;
        }

        public string GetParentPath()
        {
            var lastIndexOfSeparator = StringValue.LastIndexOf(Path.DirectorySeparatorChar);
            return StringValue.Substring(0, lastIndexOfSeparator);
        }

        public string GetPathName()
        {
            var lastIndexOfSeparator = StringValue.LastIndexOf(Path.DirectorySeparatorChar);
            var stringLength = StringValue.Length;
            return StringValue.Substring(lastIndexOfSeparator + 1, stringLength - (lastIndexOfSeparator + 1));
        }
    }
}

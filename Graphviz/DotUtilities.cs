using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.Graphviz
{
    public static class DotUtilities
    {
        /// <summary>
        /// Construlle el label html para un nodo en graphviz
        /// </summary>
        /// <param name="title"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string BuildDotLabel(string title, params string[] attributes)
        {
            var sb = new StringBuilder();
            sb.Append("<B>" + title + "</B>");
            foreach (var item in attributes)
            {
                sb.Append("<BR/>" + item);
            }
            return sb.ToString();
        }
    }
}

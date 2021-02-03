using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.Graphviz
{
    public class DotCompiler
    {
        public static Image SavePng(string filepath, IEnumerable<string> srcCode)
        {
            var stringSourceCode = ConcatStringList(srcCode);
            return SavePng(filepath, stringSourceCode);
        }

        /// <summary>
        /// Guarda el resultado de compilar con dot el string srcCode en name
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="srcCode"></param>
        public static Image SavePng(string filepath, string srcCode)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);
            var indexOfDot = filepath.LastIndexOf('.');
            if (indexOfDot < 0 || indexOfDot + 1 >= filepath.Length)
                return null;
            var stringFormat = filepath.Substring(indexOfDot + 1);
            Enums.GraphReturnType returnType;

            switch (stringFormat)
            {
                case "jpg":
                    returnType = Enums.GraphReturnType.Jpg;
                    break;
                case "jpeg":
                    returnType = Enums.GraphReturnType.Jpg;
                    break;
                case "png":
                    returnType = Enums.GraphReturnType.Png;
                    break;
                default:
                    return null;
            }

            var imgBytes = wrapper.GenerateGraph(srcCode, returnType);
            if (imgBytes.Length < 1)//Error al compilar el graphviz
                return null;
            var ms = new MemoryStream(imgBytes, 0, imgBytes.Length);
            var img = Image.FromStream(ms, true);
            img.Save(filepath);
            return img;
        }

        private static string ConcatStringList(IEnumerable<string> srcCode)
        {
            var sb = new StringBuilder();
            foreach (var s in srcCode)
            {
                sb.Append(s);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}

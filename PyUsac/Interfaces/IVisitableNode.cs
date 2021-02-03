using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Compi1_Proyecto2.PyUsac.Interfaces
{
    public interface IVisitableNode
    {
        Word Accept(IAstVisitor visitor);
    }
}

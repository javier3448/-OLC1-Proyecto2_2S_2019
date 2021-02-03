using _Compi1_Proyecto2.PyUsac.Ast.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi1_Proyecto2.MyGui
{
    public class PyTabPage : TabPage
    {
        public readonly PyPath PyPath;
        public readonly FastColoredTextBoxNS.FastColoredTextBox FastColoredTextBox;

        public PyTabPage(string name, PyPath pyPath) : base(name)
        {
            PyPath = pyPath;
            FastColoredTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            FastColoredTextBox.Dock = DockStyle.Fill;
            FastColoredTextBox.AutoIndent = true;
            FastColoredTextBox.AutoIndentChars = false;
            this.Controls.Add(FastColoredTextBox);
        }

        public string GetTextBoxContent()
        {
            return FastColoredTextBox.Text;
        }
    }
}

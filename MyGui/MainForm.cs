using _Compi1_Proyecto2.Graphviz;
using _Compi1_Proyecto2.PyUsac;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.ControlTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes;
using _Compi1_Proyecto2.PyUsac.Interpreter.MyDataType.PyUsacTypes.Primitives;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Compi1_Proyecto2.MyGui
{
    public partial class MainForm : Form
    {
        DataTable ErrorTable = new DataTable();

        public MainForm()
        {
            InitializeComponent();
            InitializeRuntimeEnvironment();
            InitializeErrorTable();
        }

        private void InitializeErrorTable()
        {
            var dC = new DataColumn("#", typeof(string));
            ErrorTable.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("#", typeof(int)),
                new DataColumn("Nivel", typeof(string)),
                new DataColumn("Tipo", typeof(string)),
                new DataColumn("Linea", typeof(int)),
                new DataColumn("Columna", typeof(int)),
                new DataColumn("Direccion", typeof(string)),
                new DataColumn("Mensaje", typeof(string))
            });
            dataGridView1.DataSource = ErrorTable;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[0].Width = 20;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[2].Width = 85;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[3].Width = 45;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[4].Width = 60;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[5].Width = 220;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[6].Width = 282;

            dataGridView1.Columns[6].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (DataGridViewColumn c in dataGridView1.Columns)
                c.DefaultCellStyle.Font = new Font("Courier New", 10);
        }

        /// <summary>
        /// Setea las propiedades del runtime environment para que imprima en la consola de este form y Haga las alert
        /// </summary>
        private void InitializeRuntimeEnvironment()
        {
            PyUsac.Interpreter.RuntimeEnvironment.Console.Instance.Print = ConsolePrint;
            PyUsac.Interpreter.RuntimeEnvironment.Console.Instance.PrintLine = ConsolePrintLine;
            PyUsac.Interpreter.RuntimeEnvironment.Logger.Instance.Log = LoggerLog;
            PyUsac.Interpreter.RuntimeEnvironment.Console.Instance.Graph = ImageDisplayGraph;
        }

        private string ConvertToTextBoxFormat(string s)
        {
            Regex regex = new Regex("[\r|\n]");
            return regex.Replace(s, "\r\n");
        }

        public void ConsolePrint(PyObj pyObj)
        {
            var message = ConvertToTextBoxFormat(pyObj.MyToString());
            textBox1.Text += message;
        }

        public void ConsolePrintLine(PyObj pyObj)
        {
            var message = ConvertToTextBoxFormat(pyObj.MyToString());
            textBox1.AppendText(Environment.NewLine + message);
        }

        public int errorCount = 1;
        public void LoggerLog(MyError error)
        {
            ErrorTable.Rows.Add(errorCount, 
                error.Level.ToString(),
                error.Type.ToString(),
                error.Location.Line,
                error.Location.Column,
                error.Location.FilePath,
                error.Message);
            errorCount++;
            textBox2.Text += Environment.NewLine + error.Message;
            textBox2.Text += Environment.NewLine + "    Line: " + error.Location.Line;
            textBox2.Text += Environment.NewLine + "    Column: " + error.Location.Column;
        }

        public string ImageDisplayGraph(MyString name, MyString dotSrc)
        {
            var image = DotCompiler.SavePng(name.MyToString(), dotSrc.MyToString());
            if (image == null)
                return "Error al comipilar el codigo dot";
            var fileName = Path.GetFileName(name.StringValue);
            var imgDisplay = new ImageDisplay(fileName, image);
            imgDisplay.Visible = true;
            return null;
        }

        private void interpretarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl2.TabCount < 1)
            {
                MessageBox.Show("Abrir o crear un archivo antes!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveAll();
            //TODO: Quitar comentario de try y catch en realease
            try
            {
                label2.Text = PyUsacEntry.InterpretFromFileName(((PyTabPage)tabControl2.SelectedTab).PyPath.StringValue).ToString();
            }
            catch (Exception exception)
            {
                var path = ((PyTabPage)tabControl2.SelectedTab).PyPath;
                MessageBox.Show("C# " + exception.GetType().Name + ". message: " + exception.Message);
                LoggerLog(_Compi1_Proyecto2.PyUsac.Interpreter.AstWalker.ErrorHelper.ErrorFactory.SystemError(exception, path.StringValue));
            }
        }

        private void SaveAll()
        {
            foreach (var tab in tabControl2.TabPages)
            {
                var fileName = ((PyTabPage)tab).PyPath.StringValue;
                var fileContent = ((PyTabPage)tab).FastColoredTextBox.Text;
                if (File.Exists(fileName))
                    File.WriteAllText(fileName, fileContent);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Consola" + Environment.NewLine +
                            "----------------------------------------------------------------------------------";
        }

        private void nuevoArToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog1 = new SaveFileDialog())
            {
                Stream myStream;
                saveFileDialog1.Filter = "PyUsac files (*.pyusac)|*.pyusac|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                var dialogResult = saveFileDialog1.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        var pyPath = new PyUsac.Ast.Base.PyPath(saveFileDialog1.FileName);
                        var newPage = new PyTabPage(pyPath.GetPathName(), pyPath);
                        tabControl2.TabPages.Add(newPage);
                        tabControl2.SelectedTab = newPage;
                        myStream.Close();
                    }
                }
            }
        }

        private void manualTecnicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "PyUsac files (*.pyusac)|*.pyusac|All files (*.*)|*.*";
                openFileDialog1.Multiselect = true;
                openFileDialog1.FilterIndex = 0;
                openFileDialog1.RestoreDirectory = true;
                var dialogResult = openFileDialog1.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    string fileName, content;
                    PyUsac.Ast.Base.PyPath pyPath;
                    PyTabPage pyTabPage = null;
                    foreach (var file in openFileDialog1.FileNames)
                    {
                        fileName = file;
                        content = File.ReadAllText(fileName);
                        pyPath = new PyUsac.Ast.Base.PyPath(fileName);
                        pyTabPage = new PyTabPage(pyPath.GetPathName(), pyPath);
                        pyTabPage.FastColoredTextBox.Text = content;
                        tabControl2.TabPages.Add(pyTabPage);
                    }
                    if (pyTabPage != null)
                        tabControl2.SelectedTab = pyTabPage;
                }
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl2.TabCount < 1)
            {
                MessageBox.Show("Abrir o crear un archivo antes!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var fileName = ((PyTabPage)tabControl2.SelectedTab).PyPath.StringValue;
            var fileContent = ((PyTabPage)tabControl2.SelectedTab).FastColoredTextBox.Text;
            if (File.Exists(fileName))
                File.WriteAllText(fileName, fileContent);
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl2.TabCount < 1)
            {
                MessageBox.Show("Abrir o crear un archivo antes!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var saveFileDialog1 = new SaveFileDialog())
            {
                string fileName;
                saveFileDialog1.Filter = "PyUsac files (*.pyusac)|*.pyusac|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                var dialogResult = saveFileDialog1.ShowDialog();
                var fileContent = ((PyTabPage)tabControl2.SelectedTab).FastColoredTextBox.Text;
                if (dialogResult == DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                    ((PyTabPage)tabControl2.SelectedTab).PyPath.StringValue = fileName;//Actualizar el path del tab
                    if (File.Exists(fileName))
                        File.WriteAllText(fileName, fileContent);
                }
            }
        }

        private void clearToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Logger" + Environment.NewLine +
                            "----------------------------------------------------------------------------------";
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ErrorTable.Clear();
            errorCount = 1;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Javier Antonio Alvarez Gonzalez\n201612383");
        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl2.TabCount < 1)
                return;
            tabControl2.TabPages.Remove(tabControl2.SelectedTab);
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Consola" + Environment.NewLine +
                            "---------------------------------------------------------------------------------";
            ErrorTable.Clear();
            errorCount = 1;
            textBox2.Text = "Logger" + Environment.NewLine +
                            "---------------------------------------------------------------------------------";
        }
    }
}

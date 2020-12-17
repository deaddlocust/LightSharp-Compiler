using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LightSharp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string MyProperty { get; set; }

        public void Build(string file)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            string exename = filename + ".exe";
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compars = new CompilerParameters();

            compars.GenerateExecutable = true;
            compars.OutputAssembly = exename;
            compars.GenerateInMemory = false;
            compars.TreatWarningsAsErrors = false;
            string[] clist = listBox1.Items.OfType<string>().ToArray();
            compars.ReferencedAssemblies.AddRange(clist);

            CompilerResults res = provider.CompileAssemblyFromFile(compars, file);

            if (res.Errors.Count > 0)
            {
                richTextBox1.Clear();
                MessageBox.Show("Build failed.", "Error");
                foreach (CompilerError ce in res.Errors)
                {
                    MessageBox.Show(ce.ToString(), "Error");
                    richTextBox1.Text = richTextBox1.Text +
                        "Line number " + ce.Line +
                        ", Error Number: " + ce.ErrorNumber +
                        ", '" + ce.ErrorText + ";" +
                        Environment.NewLine + Environment.NewLine;

                }
            }
            else
            {
                foreach (string item in listBox1.Items)
                {
                    string itemname = Path.GetFileName(item);
                    System.IO.File.Copy(item, Environment.CurrentDirectory + "/" + itemname, true);
                }
                MessageBox.Show("Code compiled!", "Success");
            }
        }

        public void BuildRun(string file)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            string exename = filename + ".exe";
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compars = new CompilerParameters();

            compars.GenerateExecutable = true;
            compars.OutputAssembly = exename;
            compars.GenerateInMemory = false;
            compars.TreatWarningsAsErrors = false;
            string[] clist = listBox1.Items.OfType<string>().ToArray();
            compars.ReferencedAssemblies.AddRange(clist);

            CompilerResults res = provider.CompileAssemblyFromFile(compars, file);

            if (res.Errors.Count > 0)
            {
                MessageBox.Show("Build failed.", "Error");
                foreach (CompilerError ce in res.Errors)
                {
                    richTextBox1.Clear();
                    MessageBox.Show(ce.ToString(), "Error");
                    richTextBox1.Text = richTextBox1.Text +
                        "Line number " + ce.Line +
                        ", Error Number: " + ce.ErrorNumber +
                        ", '" + ce.ErrorText + ";" +
                        Environment.NewLine + Environment.NewLine;

                }
            }
            else
            {
                foreach (string item in listBox1.Items)
                {
                    string itemname = Path.GetFileName(item);
                    System.IO.File.Copy(item, Environment.CurrentDirectory + "/" + itemname, true);
                }
                Process.Start(filename + ".exe");
                MessageBox.Show("Code compiled!", "Success");
            }
        }

        public void ErrorCheck(string file)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            string exename = filename + ".exe";
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compars = new CompilerParameters();

            compars.GenerateExecutable = false;
            compars.OutputAssembly = exename;
            compars.GenerateInMemory = true;
            compars.TreatWarningsAsErrors = false;
            string[] clist = listBox1.Items.OfType<string>().ToArray();
            compars.ReferencedAssemblies.AddRange(clist);

            CompilerResults res = provider.CompileAssemblyFromFile(compars, file);

            if (res.Errors.Count > 0)
            {
                richTextBox1.Clear();
                foreach (CompilerError ce in res.Errors)
                {
                    richTextBox1.Text = richTextBox1.Text +
                        "Line number " + ce.Line +
                        ", Error Number: " + ce.ErrorNumber +
                        ", '" + ce.ErrorText + ";" +
                        Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string opened = this.MyProperty;
            if (String.IsNullOrEmpty(opened))
            {
                string template = File.ReadAllText(Environment.CurrentDirectory + "/templates/" + "template.cs");
                fastColoredTextBox1.Text = template;
            }
            else
            {
                textBox1.Text = opened;
                string extension = Path.GetExtension(opened);
                if (extension == ".cs")
                {
                    fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.CSharp;
                }
                else
                {
                    fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Custom;
                };
                string openedfile = File.ReadAllText(opened);
                fastColoredTextBox1.Text = openedfile;
            }
        }

        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.CSharp;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string opened = this.MyProperty;
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save File";
                saveFileDialog1.DefaultExt = "cs";
                saveFileDialog1.Filter = "C# Files (*.cs)|*.cs";
                saveFileDialog1.FilterIndex = 2;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter txtoutput = new StreamWriter(saveFileDialog1.FileName);
                    txtoutput.Write(fastColoredTextBox1.Text);
                    txtoutput.Close();
                    textBox1.Text = saveFileDialog1.FileName;
                }

            }
            else
            {
                System.IO.File.WriteAllText(textBox1.Text, fastColoredTextBox1.Text);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Redo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Paste();
        }

        private void buildOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("You need to save the file or open a file to compile.", "Error");
            }
            else
            {
                System.IO.File.WriteAllText(textBox1.Text, fastColoredTextBox1.Text);
                Build(textBox1.Text);
            }
        }

        private void buildAndRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("You need to save the file or open a file to compile.", "Error");
            }
            else
            {
                System.IO.File.WriteAllText(textBox1.Text, fastColoredTextBox1.Text);
                BuildRun(textBox1.Text);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save File";
            saveFileDialog1.DefaultExt = "cs";
            saveFileDialog1.Filter = "C# Files (*.cs)|*.cs";
            saveFileDialog1.FilterIndex = 2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter txtoutput = new StreamWriter(saveFileDialog1.FileName);
                txtoutput.Write(fastColoredTextBox1.Text);
                txtoutput.Close();
                textBox1.Text = saveFileDialog1.FileName;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "cs",
                Filter = "All files (*.*)|*.*|C# Files (*.cs)|*.cs",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string readed = File.ReadAllText(openFileDialog1.FileName);
                textBox1.Text = openFileDialog1.FileName;
                fastColoredTextBox1.Text = readed;
            }
        }

        private void addReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "dll",
                Filter = "Dynamic Link Library (*.dll)|*.dll",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(openFileDialog1.FileName);
            }

        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("deaddlocust (c) 2020", "Credits");
        }

        private void checkErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("You need to save the file or open a file first!", "Error");
            }
            else
            {
                System.IO.File.WriteAllText(textBox1.Text, fastColoredTextBox1.Text);
                ErrorCheck(textBox1.Text);
            }
        }
    }
}
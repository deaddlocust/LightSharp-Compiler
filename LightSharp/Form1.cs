using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://dotnet.microsoft.com/download/dotnet-framework");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("LightSharp: Version 0.5.0", "LightSharp");
        }

        private void button1_Click(object sender, EventArgs e)
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
                string opened = openFileDialog1.FileName;
                Form2 frm2 = new Form2();
                frm2.MyProperty = opened;
                this.Hide();
                frm2.Closed += (s, args) => this.Close();
                frm2.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();
        }
    }
}
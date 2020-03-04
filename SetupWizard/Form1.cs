using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using Microsoft.CSharp;


namespace SetupWizard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BuildBtn_Click(object sender, EventArgs e)
        {
            CompilerParameters Params = new CompilerParameters();
            Params.GenerateExecutable = true;

            Params.ReferencedAssemblies.Add("System.dll");
            Params.ReferencedAssemblies.Add("System.Drawing.dll");
            Params.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            Params.ReferencedAssemblies.Add("System.Runtime.InteropServices.dll");
            Params.ReferencedAssemblies.Add("System.Threading.dll");
            Params.ReferencedAssemblies.Add("System.IO.dll");
            Params.OutputAssembly = "output.exe";
            Params.CompilerOptions = " /target:winexe";
            //string Source = GetSource();
            string Source = System.IO.File.ReadAllText(@"C:\Source.cs");

            CompilerResults results = new CSharpCodeProvider().CompileAssemblyFromSource(Params, Source);
            if (results.Errors.Count < 0)
            {
                MessageBox.Show("nice");
            }
            else
            {
                foreach (var error in results.Errors)
                {
                    MessageBox.Show(error.ToString());
                }
            }
            MessageBox.Show("build successfully");
        }
    }
    
}

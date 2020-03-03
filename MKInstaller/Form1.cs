using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;

namespace MKInstaller
{
    public partial class Form1 : Form
    {
        //int indexpanel = 0;
        List<Panel> panel = new List<Panel>();

        int maxbytes = 0;
        int copied = 0;
        int total = 0;



        public Form1()
        {
            InitializeComponent();
            textBox1.Text = @"d:\dest";


            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(checkBox2);


            panel2.Controls.Add(label2);
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button5);

            panel.Add(panel1);
            panel.Add(panel2);

            this.Load += new EventHandler(Form1_Load);


            /// load config file
            //
            XmlDocument doc = new XmlDocument();
            doc.Load("config.xml");

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/cs/app");
           

               foreach (XmlNode node in nodes)
                {
           
                    label1.Text = node.SelectSingleNode("title").InnerText;
                      
                }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("ddd");

            panel2.Visible = false;
            panel1.Visible = true;
            //textBox1.Text = Environment.GetEnvironmentVariable("PROGRAMFILES");
        }

        private void Download()
        {

           

            using (WebClient wcDownload = new WebClient())
            {
                try
                {
                    string url = "https://download.microsoft.com/download/B/A/4/BA4A7E71-2906-4B2D-A0E1-80CF16844F5F/dotNetFx45_Full_setup.exe";



                    using (var client = new WebClient())
                    {
                        client.DownloadFile(url, "dotNetFx45_Full_setup.exe");
                    }
                    Process.Start("dotNetFx45_Full_setup.exe");

                }
                catch { }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "video files(*.mp4; *.avi)| *.mp4; *.avi";
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {



                textBox1.Text = folderBrowserDialog1.SelectedPath;

                // sr.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;




        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string ver = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
            
            string lastWord = ver.Split('=').Last();
            string replacement = lastWord.Replace("v", "");
            var regex = new Regex(@"\d{1}.\d");
            Match m = regex.Match(ver);
            //MessageBox.Show(m.Value);
            float rep = float.Parse(m.Value);
            if (rep < 4.6)
            {
                //MessageBox.Show("dddd");
                Download();

            }
            
        }

        private void shortcutex()
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Notepad.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "New shortcut for a Notepad";
            shortcut.Hotkey = "Ctrl+Shift+N";
            shortcut.TargetPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\notepad.exe";
            shortcut.Save();
        }


        public void Copy1(string sourceDirectory, string targetDirectory)
        {

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
            //Gets size of all files present in source folder.
            GetSize(diSource, diTarget);
            maxbytes = maxbytes / 1024;

            progressBar1.Maximum = maxbytes;
            CopyAll(diSource, diTarget);
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }
            foreach (FileInfo fi in source.GetFiles())
            {

                //fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);

                total += (int)fi.Length;

                copied += (int)fi.Length;
                copied /= 1024;
                progressBar1.Step = copied;

                progressBar1.PerformStep();

                label2.Text = (total / 1048576).ToString() + "MB of " + (maxbytes / 1024).ToString() + "MB copied";
                label2.Refresh();



            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {



                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public void GetSize(DirectoryInfo source, DirectoryInfo target)
        {


            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }
            foreach (FileInfo fi in source.GetFiles())
            {
                maxbytes += (int)fi.Length;//Size of File


            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                GetSize(diSourceSubDir, nextTargetSubDir);

            }

        }
    }
}

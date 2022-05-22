using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Diagnostics;



namespace WinFormsApp1
{
    public partial class UninstallForm : Form
    {
        public DriveInfo[] drives = DriveInfo.GetDrives();
        public string programFilesPath = "Program Files";
        public string programFilesPathx86 = "Program Files (x86)";
        public string programData = "ProgramData";
        public string programName;
        public string publisherName;
        public Queue queueName,queueUninstall;
        public Queue QueueName { get; set; }
        public Queue QueueUninstall { get; set; }
        public Queue QueuePublisher { get; set; }
        public UninstallForm(Queue queueName,Queue queueUninstall,Queue queuePublisher)
        {
            InitializeComponent();
            //this.label1.Text = "Selected";

            QueueName = queueName;
            QueueUninstall=queueUninstall;
            QueuePublisher = queuePublisher;
            foreach(string x in queueName)
            {
                listBox1.Items.Add(x);  
            }
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
              
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            button1.ForeColor = Color.Gray;
            button2.ForeColor = Color.Gray;
            button3.ForeColor = Color.Gray;
            string check, command;
            int progress = 0;
            int progressDiv = 100 /listBox1.Items.Count;
            int rest = 100 % listBox1.Items.Count;
            foreach (string x in listBox1.Items)
            {
                
                //new ToastContentBuilder().AddText("Debugging").AddText(QueueUninstall.Peek().ToString()).Show();
                SetText($"Uninstalling {x}");
                
                check = "";
                Process process2 = new Process();
                ProcessStartInfo start2 = new ProcessStartInfo();
                start2.WindowStyle = ProcessWindowStyle.Hidden;

                for (int i = 0; i < 18; i++)
                {
                    check += QueueUninstall.Peek().ToString()[i];
                }

                if (check == "Remove-AppxPackage")
                {
                    start2.FileName = @"powershell.exe";
                    //command = @$"{x.Trim()}";
                    command = "explorer.exe .";
                    new ToastContentBuilder().AddText($"MSstore").AddText($"{QueueUninstall.Peek().ToString()}").Show();
                    start2.Arguments = command;
                    start2.CreateNoWindow = true;
                    process2.StartInfo = start2;

                    Process p = Process.Start(start2);
                    //p.WaitForInputIdle();
                    p.WaitForExit();

                }
                else
                {
                    Process pr=Process.Start("cmd.exe",$@"/C ""{QueueUninstall.Peek().ToString()}"" ");
                    pr.CloseMainWindow();
                    pr.WaitForExit();
                }

                QueueUninstall.Dequeue();
                progress += progressDiv;
                backgroundWorker1.ReportProgress(progress);         

            }
            backgroundWorker1.ReportProgress(rest+progress);
    

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label2.Text = e.ProgressPercentage.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetText("Uninstall Completed");
            string message=string.Empty;
            button1.Enabled = true;
            new ToastContentBuilder().AddText("WinPurge").AddText("Finished Uninstalling!").Show();
            for(int i = 3; i != 0; i--)
            {
                label2.Text=$"Closing in {i} seconds";
                Thread.Sleep(1000);
            }

            ArrayList list = new ArrayList();
            foreach (string x in listBox1.Items)
            {
                CheckLeftovers(x, list);
            }

            foreach(string x in QueuePublisher)
            {
                CheckLeftovers(x, list);
                message += Environment.NewLine;
                message += x;
            }
            
            ArrayList temp = new();
            
            for(int i = 0; i < list.Count; i++)
            {
                if (i > 1)
                {
                    string a=list[i].ToString();
                    string b=list[i-1].ToString();
                    if(a.Trim()!=b.Trim())
                    {
                        temp.Add(list[i]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
         
            MessageBox.Show(message);
            this.Close();
            list.Sort();
            LeftoversForm leftoversForm = new LeftoversForm(list);
            leftoversForm.ShowDialog();
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            string message="";
            foreach(string x in listBox1.Items)
            {
                CheckLeftovers(x,list);
            }

            foreach(var y in list)
            {
                message += Environment.NewLine;
                message += y;
            }
            MessageBox.Show(message);
        }

        private ArrayList CheckLeftovers(string programName,ArrayList returnList)
        {
            ArrayList list = new ArrayList();
            
            foreach (var x in drives)
            {
                //Console.WriteLine(x);
                if (CheckPaths(x + programFilesPath))
                {
                    list=PrintDirectories(x + programFilesPath, programName);
                    if(list.Count > 0)
                    {
                        foreach(var y in list)
                        {
                            returnList.Add(y.ToString());
                        }
                    }
                    
                    //PrintDirectories(x + programFilesPath, publisherName);
                }
                if (CheckPaths(x + programFilesPathx86))
                {
                    list=PrintDirectories(x + programFilesPathx86, programName);
                    if (list.Count > 0)
                    {
                        foreach (var y in list)
                        {
                            returnList.Add(y.ToString());
                        }
                    }
                    //PrintDirectories(x + programFilesPathx86, publisherName);
                }
                if (CheckPaths(x + programData))
                {
                    list=PrintDirectories(x + programData, programName);
                    if (list.Count > 0)
                    {
                        foreach (var y in list)
                        {
                            returnList.Add(y.ToString());
                        }
                    }
                    //PrintDirectories(x + programData, publisherName);
                }
            }
            return returnList;

        }

        private bool CheckPaths(string path)
        {
            bool IsDirectory = Directory.Exists(path);
            if (IsDirectory == false)
            {
                //Console.WriteLine($"Path {path} doesn't exist.");
                return false;
            }
            else
            {
                //Console.WriteLine($"Path {path} exists.");
                return true;
            }
        }

        private void UninstallForm_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (this.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.Text = text;
            }
        }

        private static ArrayList PrintDirectories(string path, string program)
        {
            ArrayList coincidences=new();
            int minor;
            string temp1, temp2;
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var y in dir.GetDirectories())
            {
                temp1 = string.Empty;
                temp2 = string.Empty;
                if (program.Length > y.Name.Length)
                {
                    minor = y.Name.Length;
                }
                else
                {
                    minor = program.Length;
                }

                for (int i = 0; i < minor; i++)
                {
                    //Console.WriteLine($"{y.Name[i]} | {program[i]}");
                    temp1 += program[i];
                    temp2 += y.Name[i];
                }
                if (temp1 == temp2)
                {
                    coincidences.Add(y);
                }

            }
            
            return coincidences;
        }
    }
}

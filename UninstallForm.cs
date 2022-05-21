﻿using System;
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
        public UninstallForm(Queue queueName,Queue queueUninstall)
        {
            InitializeComponent();
            this.label1.Text = "Selected";

            QueueName = queueName;
            QueueUninstall=queueUninstall;
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

            int progress = 0;
            int progressDiv = 100 / listBox1.Items.Count;
            int rest = 100 % listBox1.Items.Count;
            foreach (string x in listBox1.Items)
            {
                
                new ToastContentBuilder().AddText("Debugging").AddText(QueueUninstall.Peek().ToString()).Show();
                QueueUninstall.Dequeue();
                //listBox1.Items.Remove(x);
                progress += progressDiv;
                backgroundWorker1.ReportProgress(progress);         
                System.Threading.Thread.Sleep(7000);

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
            string message=string.Empty;
            button1.Enabled = true;
            new ToastContentBuilder().AddText("Debugging").AddText("Finished Uninstalling!").Show();
            for(int i = 3; i != 0; i--)
            {
                label2.Text=$"Closing this Window in {i} seconds";
                Thread.Sleep(1000);
            }

            foreach(string x in listBox1.Items)
            {
                message += Environment.NewLine;
                message += x;
            }

            MessageBox.Show(message);
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
                            returnList.Add(y);
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
                            returnList.Add(y);
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
                            returnList.Add(y);
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
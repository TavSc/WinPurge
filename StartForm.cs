using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualBasic.Logging;
using System.Security.Principal;

namespace WinFormsApp1
{
    public partial class StartForm : Form
    {

        
        public StartForm()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }

        public static bool IsAdministrator =>
        new WindowsPrincipal(WindowsIdentity.GetCurrent())
           .IsInRole(WindowsBuiltInRole.Administrator);

        private void Form2_Load(object sender, EventArgs e)
        {
            if (IsAdministrator)
            {
                pictureBox1.Image = WinPurge.Properties.Resources.WinPurgeAdmin;
            }
            else
            {
                pictureBox1.Image = WinPurge.Properties.Resources.WinPurge;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (File.Exists(".\\AppxName.txt"))
            {
                if (File.Exists(".\\AppxFullName.txt"))
                {
                    if (File.Exists(".\\Publisher.txt"))
                    {
                        if (File.Exists(".\\Version.txt"))
                        {
                            SetText("Loading Files");
                        }
                        else
                        {
                            File.Create(".\\Version.txt");
                        }
                    }
                    else
                    {
                        File.Create(".\\Publisher.txt");
                    }
                }
                else
                {
                    File.Create(".\\AppxFullName.txt");
                }
            }
            else
            {
                File.Create(".\\AppxName.txt");
            }

            SetText("Loading Microsoft Store Apps");
            int j = 0;
            for(int i = 0; i < 9; i++)
            {
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i*2);
                j=j+(i*2);
            }
            Process process3 = new Process();
            ProcessStartInfo start3 = new ProcessStartInfo();
            start3.WindowStyle = ProcessWindowStyle.Hidden;
            start3.FileName = @"powershell.exe";
            start3.CreateNoWindow = true;
            ;
            string command2 = "cd .. ;cd ..;Get-AppxPackage | Where-Object { $_.NonRemovable -eq $false -and $_.IsFramework -eq $false }|Select PackageFullName | Out-File -FilePath .\\AppxFullName.txt;Get-AppxPackage | Where-Object { $_.NonRemovable -eq $false -and $_.IsFramework -eq $false }|Select Version | Out-File -FilePath .\\Version.txt";
            string command3 = "cd .. ;cd ..;Get-AppxPackage | Where-Object { $_.NonRemovable -eq $false -and $_.IsFramework -eq $false }|Select Name | Out-File -FilePath .\\AppxName.txt;Get-AppxPackage | Where-Object { $_.NonRemovable -eq $false -and $_.IsFramework -eq $false }|Select Publisher | Out-File -FilePath .\\Publisher.txt";

            start3.Arguments = command2;
            process3.StartInfo = start3;
            process3.Start();
            backgroundWorker1.ReportProgress(j);
            Process process4 = new Process();
            ProcessStartInfo start4 = new ProcessStartInfo();
            start4.WindowStyle = ProcessWindowStyle.Hidden;
            start4.FileName = @"powershell.exe";
            start4.CreateNoWindow = true;
            start4.Arguments = command3;
            process4.StartInfo = start4;
            process4.Start();


            process4.WaitForExit();
            backgroundWorker1.ReportProgress(j +2 );
            process3.WaitForExit();

            SetText("Loading Win32 Apps");
            Thread.Sleep(700);
            backgroundWorker1.ReportProgress(90);
            SetText("Loading Done");
            Thread.Sleep(1500);
            SetText("Opening Window");
            Thread.Sleep(500);
            backgroundWorker1.ReportProgress(100);
            new ToastContentBuilder().AddText("WinPurge").AddText("Loading Done!").Show();
            process4.Kill();
            process3.Kill();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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
                this.label1.Text = text;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

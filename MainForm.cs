using System.Collections;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.Notifications;
using System.ComponentModel;
using System.Security.Principal;
using System.Reflection;
using System.Drawing;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        public List<string> list=new List<string>();
        string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        string key2 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
        public MainForm()
        {
            InitializeComponent();
            dataGridView1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            //this.Text = String.Empty;

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.SetProperty, null,
            dataGridView1, new object[] { true });
            //toolStripStatusLabel3.Alignment = Tool;
            dataGridView1.Enabled = true;
            dataGridView1.ReadOnly = false;
            dataGridView1.RowHeadersVisible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartForm form2 = new StartForm();
            form2.ShowDialog();
            displayItems();
            displayStoreItems();
            toolStripStatusLabel3.Text = $"{dataGridView1.RowCount} Installers found";
            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
            if (IsAdministrator == true)
            {
                this.Text = "WinPurge (Administrator)";
                this.Icon = Icon.ExtractAssociatedIcon(@"..\..\..\Resources\WinPurgeAdmin.ico");
            }
            Debug.WriteLine("Test");

        }

        private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["nameColumn"].Value);
                if (isSelected)
                {
                    message += Environment.NewLine;
                    message += row.Cells["typeColumn"].Value.ToString();
                }
            }

            MessageBox.Show("Selected Values" + message);
        }

        private void displayItems()
        {
            using (RegistryKey key1 = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key1.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key1.OpenSubKey(subkey_name))
                    {
                        string displayName = subkey.GetValue("DisplayName", 0).ToString();
                        string systemComponent = subkey.GetValue("SystemComponent", 0).ToString();
                        string publisher = subkey.GetValue("Publisher", "Unknown").ToString();
                        string version = subkey.GetValue("DisplayVersion", "Unknown").ToString();
                        string uninstallString = subkey.GetValue("UninstallString", "Unknown").ToString();
                        //Console.WriteLine(subkey.GetValue("DisplayName"));
                        if (displayName == "0")
                        {
                            continue;
                        }
                        else
                        {
                            if (systemComponent == "0")
                            {
                               
                                dataGridView1.Rows.Add(false,Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), displayName,publisher, version,"Win32",uninstallString);
                                
                                //hey.Add(subkey_name);
                            }
                        }

                    }
                }
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(key2))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        string displayName = subkey.GetValue("DisplayName", 0).ToString();
                        string systemComponent = subkey.GetValue("SystemComponent", 0).ToString();
                        string publisher = subkey.GetValue("Publisher", "Unknown").ToString();
                        string version = subkey.GetValue("DisplayVersion", "Unknown").ToString();
                        string uninstallString = subkey.GetValue("UninstallString", "Unknown").ToString();
                        //Console.WriteLine(subkey.GetValue("DisplayName"));
                        if (displayName == "0")
                        {
                            continue;
                        }
                        else
                        {
                            if (systemComponent == "0")
                            {
                                if(displayName == "Microsoft Edge Update")
                                {
                                    continue;
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), displayName, publisher, version, "Win32", uninstallString);

                                }
                                //hey.Add(subkey_name);
                            }
                        }

                    }
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        string displayName = subkey.GetValue("DisplayName", 0).ToString();
                        string systemComponent = subkey.GetValue("SystemComponent", 0).ToString();
                        string publisher = subkey.GetValue("Publisher", "Unknown").ToString();
                        string version = subkey.GetValue("DisplayVersion", "Unknown").ToString();
                        string uninstallString= subkey.GetValue("UninstallString", "Unknown").ToString();

                        //Console.WriteLine(subkey.GetValue("DisplayName"));
                        if (displayName == "0")
                        {
                            continue;
                        }
                        else
                        {
                            if (systemComponent == "0")
                            {

                                //dataGridView1.Rows.Add(false, Icon.ExtractAssociatedIcon(@"C:\Users\Public\Desktop\League of Legends.lnk"), displayName, publisher, "Win32");
                                dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), displayName, publisher, version,"Win32", uninstallString);
                                //hey.Add(subkey_name);
                            }
                        }
                    }
                }
            }
        }

        private void displayStoreItems()
        {
            string[] lines = File.ReadAllLines(@"..\..\AppxName.txt");
            string[] lines2 = File.ReadAllLines(@"..\..\AppxFullName.txt");
            string[] publisherFile = File.ReadAllLines(@"..\..\Publisher.txt");
            string[] versionInfo = File.ReadAllLines(@"..\..\Version.txt");
            string[] publisherFileExclusive = new string[publisherFile.Length];
            int count = 0;

            foreach (string x in publisherFile)
            {
                string y = "";
                if (x == "Publisher                                                                       " || x == "---------                                                                       ")
                {
                    continue;
                }
                else
                {
                    for (int i = 3; i < x.Length; i++)
                    {
                        if (x[i] == char.Parse(","))
                        {
                            break;
                        }
                        else
                        {
                            y += x[i];
                        }
                    }
                }
                publisherFileExclusive[count] = y;
                count++;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim() == "Name" || lines[i] == "" || lines[i].Trim() == "----")
                {
                    continue;
                }
                else
                {
                    //hey.Add("@" + lines[i].Trim() + "$[AppxP] " + publisherFileExclusive[i - 2].Trim());

                    switch (publisherFileExclusive[i - 2].Trim())
                    {
                        case "5BD5593D-A41B-4F89-884E-B4F3E0FBAA75":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "Apple Inc.", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        case "4975D53F-AA7E-49A5-8B49-EA4FDC1BB66B":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "Python Software Foundation", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        case "D6816951-877F-493B-B4EE-41AB9419C326":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "NVIDIA Corporation", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        case "58D26209-1D57-482C-B403-B655571B5C7B":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "Dolby Laboratories", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        case "EB51A5DA-0E72-4863-82E4-EA21C1F8DFE3":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "Intel Corporation", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        case "453637B3-4E12-4CDF-B0D3-2A3C863BF6EF":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "Spotify AB", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        case "83564403-0B26-46B8-9D84-040F43691D31":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "Realtek Semiconductor Corp.", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        case "52120C15-ACFA-47FC-A7E3-4974DBA79445":
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), "Netflix, Inc.", "Unknown", "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                        default:
                            dataGridView1.Rows.Add(false, Image.FromFile(@"D:\salas\Desktop\Microsoft_Store_2021_Light.ico"), lines[i].Trim(), publisherFileExclusive[i - 2].Trim(), versionInfo[i-2].Trim(), "Microsoft Store", "Remove-AppxPackage " + lines2[i].Trim());
                            break;
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Queue selectedDisplayName=new Queue();
            Queue selectedUninstallString = new Queue();
            Queue selectedPublisher=new Queue();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["nameColumn"].Value);
                if (isSelected)
                {
                    //message += Environment.NewLine;
                    selectedDisplayName.Enqueue(row.Cells["displayNameColumn"].Value.ToString());
                    selectedUninstallString.Enqueue(row.Cells["uninstallStringColumn"].Value.ToString());
                    selectedPublisher.Enqueue(row.Cells["publisherColumn"].Value.ToString());
                    //message += ;
                }
            }

           // MessageBox.Show();
            UninstallForm form3 = new UninstallForm(selectedDisplayName,selectedUninstallString,selectedPublisher);
            form3.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (toolStripButton2.Checked)
            {
                toolStripButton2.Text = "Unselect All";
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells["nameColumn"].Value = true;
                }
            }
            else
            {
                toolStripButton2.Text = "Select All";
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells["nameColumn"].Value = false;
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (toolStripButton3.Checked)
            {
                toolStripButton3.Text = "Enable Published by Microsoft";
                toolStripButton3.Image = WinPurge.Properties.Resources.Microsoft_logo__201e2__svg___Copy;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["publisherColumn"].Value.ToString().Trim() == "Microsoft Corporation"  || row.Cells["publisherColumn"].Value.ToString().Trim() == "Microsoft Windows")
                    {
                        row.Cells["nameColumn"].Value = false;
                        row.Cells["nameColumn"].ReadOnly = true;                       
                    }
                }
            }
            else
            {
                toolStripButton3.Text = "Disable Published by Microsoft";
                toolStripButton3.Image=WinPurge.Properties.Resources.Microsoft_logo__2012_e_svg;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["publisherColumn"].Value.ToString().Trim() == "Microsoft Corporation" || row.Cells["publisherColumn"].Value.ToString().Trim() == "Microsoft Windows")
                    {
                        row.Cells["nameColumn"].Value = false;
                        row.Cells["nameColumn"].ReadOnly = false;
                    }
                    
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (IsAdministrator == true)
            {
                var result=MessageBox.Show("The application will reload as Administrator. If you want to run it without administrator privileges, close the app and open it again. Do you want to continue anyways?", "Warning", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                if(result == DialogResult.OK)
                {
                    Application.Restart();
                }
            }
            else
            {
                Application.Restart();
            }
            
        }

        

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AboutForm form4 = new AboutForm();
            //form4.ShowDialog();

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["nameColumn"].Value);
                if (isSelected)
                {
                    count++;
                }
            }
            toolStripStatusLabel2.Text = $"{count} Uninstallers Selected.";
            if(count> 0)
            {
                toolStripButton1.Enabled=true;
            }
            else
            {
                toolStripButton1.Enabled=false;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = Application.ExecutablePath;
            proc.Verb = "runas";

         
            try
            {
                Process.Start(proc);
            }
            catch
            {
                return;
            }
            Application.Exit();
        }

            
        

        public static bool IsAdministrator =>
        new WindowsPrincipal(WindowsIdentity.GetCurrent())
           .IsInRole(WindowsBuiltInRole.Administrator);
        private void toolStripButton1_MouseEnter(object sender, EventArgs e)
        {
            toolStripButton1.ForeColor = Color.Black;
        }

        private void toolStripButton1_MouseLeave(object sender, EventArgs e)
        {
            toolStripButton1.ForeColor = Color.White;
        }
        private void toolStripButton2_MouseEnter(object sender, EventArgs e)
        {
            toolStripButton2.ForeColor = Color.Black;
        }

        private void toolStripButton2_MouseLeave(object sender, EventArgs e)
        {
            toolStripButton2.ForeColor = Color.White;
        }

        private void toolStripButton3_MouseEnter(object sender, EventArgs e)
        {
            toolStripButton3.ForeColor = Color.Black;
        }

        private void toolStripButton3_MouseLeave(object sender, EventArgs e)
        {
            toolStripButton3.ForeColor = Color.White;
        }

        private void toolStripButton4_MouseEnter(object sender, EventArgs e)
        {
            toolStripButton4.ForeColor = Color.Black;
        }

        private void toolStripButton4_MouseLeave(object sender, EventArgs e)
        {
            toolStripButton4.ForeColor = Color.White;
        }

        private void toolStripButton6_MouseEnter(object sender, EventArgs e)
        {
            toolStripButton6.ForeColor = Color.Black;
        }

        private void toolStripButton6_MouseLeave(object sender, EventArgs e)
        {
            toolStripButton6.ForeColor = Color.White;
        }

        private void fileToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = Color.Black;
        }

        private void fileToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (fileToolStripMenuItem.Enabled) {
                fileToolStripMenuItem.ForeColor = Color.Black;
            }
            else
            {
                fileToolStripMenuItem.ForeColor = Color.White;
            }
            
        }

        private void viewToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {

            viewToolStripMenuItem.ForeColor = Color.Black;
        }

        private void viewToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            viewToolStripMenuItem.ForeColor = Color.White;
        }

        private void helpToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {

            helpToolStripMenuItem.ForeColor = Color.Black;
        }

        private void helpToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            helpToolStripMenuItem.ForeColor = Color.White;
        }

        private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = Color.White;
        }









        /*private void Uninstall()
        {
            string name;
            bool isFound;
            do
            {
                Console.Write("\nApp to Uninstall: ");
                name = Console.ReadLine();
                isFound = false;
                while (isFound == false)
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                    {
                        foreach (string subkeyName in key.GetSubKeyNames())
                        {
                            using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                            {
                                string displayName = subkey.GetValue("DisplayName", 0).ToString();
                                if (displayName == name)
                                {
                                    string systemComponent = subkey.GetValue("SystemComponent", 0).ToString();
                                    string publisher = subkey.GetValue("Publisher", 0).ToString();
                                    string uninstallString = subkey.GetValue("UninstallString", 0).ToString();
                                    Console.WriteLine($"Found: {displayName} | {uninstallString} | {registry_key}");
                                    isFound = true;
                                    queue2.Enqueue(name);
                                    queue.Enqueue(uninstallString);
                                }
                            }
                        }
                    }
                    if (isFound == true)
                    {
                        break;
                    }
                    else
                    {
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(key2))
                        {
                            foreach (string subkey_name in key.GetSubKeyNames())
                            {
                                using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                                {
                                    string displayName = subkey.GetValue("DisplayName", 0).ToString();
                                    if (displayName == name)
                                    {
                                        string systemComponent = subkey.GetValue("SystemComponent", 0).ToString();
                                        string publisher = subkey.GetValue("Publisher", 0).ToString();
                                        string uninstallString = subkey.GetValue("UninstallString", 0).ToString();
                                        Console.WriteLine($"Found: {displayName} | {uninstallString} | {key2}");
                                        isFound = true;
                                        queue2.Enqueue(name);
                                        queue.Enqueue(uninstallString);
                                    }
                                }
                            }
                        }
                        if (isFound == true)
                        {
                            break;
                        }
                        else
                        {
                            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
                            {
                                foreach (string subkey_name in key.GetSubKeyNames())
                                {
                                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                                    {
                                        string displayName = subkey.GetValue("DisplayName", 0).ToString();
                                        if (displayName == name)
                                        {
                                            string systemComponent = subkey.GetValue("SystemComponent", 0).ToString();
                                            string publisher = subkey.GetValue("Publisher", 0).ToString();
                                            string uninstallString = subkey.GetValue("UninstallString", 0).ToString();
                                            Console.WriteLine($"Found: {displayName} | {uninstallString} | {registry_key}");
                                            isFound = true;
                                            queue2.Enqueue(name);
                                            queue.Enqueue(uninstallString);
                                        }
                                    }
                                }
                            }
                            if (isFound == true)
                            {
                                break;
                            }
                            else
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    if (lines[i].Trim() == name)
                                    {
                                        Console.WriteLine("Found: " + lines2[i]);
                                        isFound = true;
                                        string uninstallString = "Remove-AppxPackage " + lines2[i].Trim();
                                        queue.Enqueue(uninstallString);
                                        queue2.Enqueue(name);
                                    }
                                }
                            }
                        }
                    }
                    if (isFound == false)
                    {
                        if (name == "quit")
                        {
                            break;
                        }
                        Console.WriteLine("Error");
                        break;
                    }


                }
            } while (name != "quit");

            if (queue.Count > 0)
            {
                Console.WriteLine("\nUninstall Queue");
                foreach (string x in queue2)
                {
                    Console.WriteLine(x);
                }


                string check, command;
                Console.Write("�Est� seguro que desea continuar? (yes/no)");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "yes":
                        foreach (string x in queue)
                        {
                            Console.Title = "Removing " + queue2.Peek();
                            check = "";
                            Console.WriteLine(x);
                            Process process2 = new Process();
                            ProcessStartInfo start2 = new ProcessStartInfo();
                            start2.WindowStyle = ProcessWindowStyle.Hidden;

                            for (int i = 0; i < 18; i++)
                            {
                                check += x[i];
                            }

                            if (check == "Remove-AppxPackage")
                            {
                                start2.FileName = @"powershell.exe";
                                command = @$"{x.Trim()}";
                            }
                            else
                            {
                                start2.FileName = @"cmd.exe";
                                command = $@"/C ""{x}"" ";
                            }
                            start2.Arguments = command;
                            process2.StartInfo = start2;
                            process2.StartInfo.RedirectStandardOutput = true;
                            process2.Start();
                            Console.ForegroundColor = ConsoleColor.Yellow;

                            process2.WaitForExit();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Finished {0}", x);
                            process2.Kill();
                            queue2.Dequeue();
                            System.Threading.Thread.Sleep(500);
                        }
                        break;
                    case "no":
                        Console.WriteLine("Operation cancelled by user");
                        break;
                    default:
                        Console.WriteLine("Error.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("No Uninstallers Selected!");
            }
        }*/
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class LeftoversForm : Form
    {
        public LeftoversForm(ArrayList list)
        {
            InitializeComponent();
            foreach(var x in list)
            {
                checkedListBox1.Items.Add(x);
            }
        }

        private void LeftoversForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message=string.Empty;
            foreach(string x in checkedListBox1.CheckedItems)
            {
                message += Environment.NewLine;
                message += x;
            }
            MessageBox.Show(message);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(checkedListBox1.CheckedItems.Count > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled=false;
            }
        }
    }
}

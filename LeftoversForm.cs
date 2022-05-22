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
    }
}

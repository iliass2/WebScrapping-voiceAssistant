using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web
{
    public partial class page1 : Form
    {
        public page1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Form1 f = new Form1();
                f.Show();
                this.Hide();
            }
            if (radioButton2.Checked == true)
            {
                Form2 f = new Form2();
                f.Show();
                this.Hide();
            }
        }

        private void page1_Load(object sender, EventArgs e)
        {

        }
    }
}

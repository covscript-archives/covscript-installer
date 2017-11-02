using System;
using System.Windows.Forms;

namespace Covariant_Script_Installer
{
    public partial class Form2 : Form
    {
        private Form1 parent;
        public Form2(Form1 form)
        {
            InitializeComponent();
            parent = form;
            textBox1.Text = Properties.Resources.license;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.Close();
            // TODO
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

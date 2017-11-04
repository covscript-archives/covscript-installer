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
            textBox1.Select(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked;
        }
    }
}

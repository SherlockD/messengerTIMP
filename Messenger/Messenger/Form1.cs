using System;
using System.Windows.Forms;

namespace Messenger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var messengerForm = new Form2(textBox7.Text, int.Parse(textBox8.Text), int.Parse(textBox6.Text));
            messengerForm.CallOnTabBack += () => Show();
            messengerForm.CallOnTabClose += () => Close();
            messengerForm.Show();
            Hide();
        }
    }
}

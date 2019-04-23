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

        private bool InputChecker()
        {
            if (string.IsNullOrEmpty(textBox7.Text) || !textBox7.Text.Contains("."))
            {
                return false;
            }
            if(!int.TryParse(textBox8.Text,out int result1) || !int.TryParse(textBox6.Text, out int result2))
            {
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (InputChecker())
            {
                var messengerForm = new Form2(textBox7.Text, int.Parse(textBox8.Text), int.Parse(textBox6.Text));
                messengerForm.CallOnTabBack += () => Show();
                messengerForm.CallOnTabClose += () => Close();
                messengerForm.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Неверно написанные данные");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var hackerForm = new HackerForm(textBox7.Text, int.Parse(textBox6.Text));
            hackerForm.Show();
            Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

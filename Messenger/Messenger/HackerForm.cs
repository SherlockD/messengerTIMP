using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Messenger
{
    public partial class HackerForm : Form
    {
        private string port;

        public HackerForm()
        {
            FormClosed += new FormClosedEventHandler(OnFormClosed);
            InitializeComponent();
            var _reciveThread = new Thread(new ThreadStart(ReceiveMessage));
            _reciveThread.Start();
        }

        public void OnFormClosed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ReceiveMessage()
        {
            UdpClient udpClient = new UdpClient(8000);
            IPEndPoint remoteIp = null;
            try
            {
                while (true)
                {

                    byte[] data = udpClient.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);

                    if (message.Substring(5) == "Exit")
                    {
                        if (message.Substring(0, 4) == port)
                        {
                            Invoke((MethodInvoker)(() => label3.Text = "Status: offline"));
                        }
                        LastExitSerialization.Serialize(message.Substring(0, 4),DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString());
                    }

                    if (message.Substring(5) == "Open")
                    {
                        if (message.Substring(0, 4) == port)
                        {
                            Invoke((MethodInvoker)(() => label3.Text = "Status: online"));
                            //return;
                        }
                    }

                    if (!string.IsNullOrEmpty(message) && message.Substring(0, 4) == port)
                    {
                        Invoke((MethodInvoker)(() => label3.Text = "Status: online"));

                        Invoke((MethodInvoker)(() => listBox2.Items.Add($"{message}")));
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "Status: offline";
            port = textBox1.Text;
            var lastData = LastExitSerialization.GetPortData(port);

            label2.Text = $"Дата последнего входа: {(lastData != string.Empty ? lastData : "Невозможно определить")}";
        }

        private void HackerForm_Load(object sender, EventArgs e)
        {

        }
    }
}

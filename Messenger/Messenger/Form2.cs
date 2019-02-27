using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Messenger
{
    public partial class Form2 : Form
    {
        public event Action CallOnTabBack;
        public event Action CallOnTabClose;

        private string _remoteAddress;
        private int _remotePort;
        private int _localPort;

        private Thread _reciveThread;

        public Form2(string remoteAddress, int remotePort, int localPort)
        {
            InitializeComponent();
            Text = localPort.ToString();
            _remoteAddress = remoteAddress;
            _remotePort = remotePort;
            _localPort = localPort;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            FormClosed += new FormClosedEventHandler(OnFormClosed);
            _reciveThread = new Thread(new ThreadStart(ReceiveMessage));
            _reciveThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UdpClient messageSender = new UdpClient();
            try
            {
                string message = textBox1.Text;
                textBox1.Clear();
                byte[] data = Encoding.Unicode.GetBytes(message);
                messageSender.Send(data, data.Length, _remoteAddress, _remotePort);
                listBox1.Items.Add($"Вы: {message}");               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                messageSender.Close();
            }
        }

        private void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(_localPort); 
            IPEndPoint remoteIp = null; 
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);
                    if (message != "")
                    {
                        Invoke((MethodInvoker)(() => listBox1.Items.Add($"Собеседник: {message}")));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }

        public void OnFormClosed(object sender, EventArgs e)
        {
            _reciveThread.Abort();
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CallOnTabBack?.Invoke();
            Close();
        }
    }
}

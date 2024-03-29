using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Process.Start("TCPLab.exe");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect("192.168.0.108", 11000);
                NetworkStream stream = client.GetStream();
                stream.Write(Encoding.UTF8.GetBytes(textBox1.Text));
                textBox1.Clear();
            }
            catch (SocketException ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
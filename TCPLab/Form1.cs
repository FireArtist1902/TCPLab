using System.Net.Sockets;
using System.Net;
using System.Text;

namespace TCPLab
{
    public partial class Form1 : Form
    {
        Thread thread;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread != null)
                return;
            thread = new Thread(ServerFunc);
            thread.IsBackground = true;
            thread.Start();
            Text = "Working";
        }

        private void ServerFunc(object? obj)
        {
            List<string> words = new List<string>();
            TcpListener listener = new TcpListener(IPAddress.Parse("192.168.0.108"), 11000);
            try
            {
                listener.Start();
                do
                {
                    if (listener.Pending())
                    {
                        string word;
                        TcpClient client = listener.AcceptTcpClient();
                        byte[] bytes = new byte[1024];
                        NetworkStream ns = client.GetStream();
                        int len = ns.Read(bytes, 0, bytes.Length);
                        StringBuilder sb = new StringBuilder();
                        word = Encoding.UTF8.GetString(bytes, 0, len);
                        if (word != "Exit")
                        {
                            words.Add(word);
                            sb.AppendLine($"{len} bytr received from {client.Client.RemoteEndPoint}");
                            sb.AppendLine(word);
                            textBox1.BeginInvoke(new Action<string>(AddText), sb.ToString());
                            client.Client.Shutdown(SocketShutdown.Receive);
                        }
                        else
                        {
                            listener.Stop();
                            this.Close();
                        }
                    }
                } while (true);
            }
            catch (SystemException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                listener.Stop();
            }
        }

        private void AddText(string obj)
        {
            StringBuilder sb = new StringBuilder(textBox1.Text);
            sb.AppendLine(obj);
            textBox1.Text = sb.ToString();
        }
    }
}
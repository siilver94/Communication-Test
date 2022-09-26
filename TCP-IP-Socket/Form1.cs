using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace tcpServer
{
    public partial class Form1 : Form
    {
        private byte[] data = new byte[1024];
        private int size = 1024;
        private Socket mainSocket = null;
        List<Socket> connectedClients = new List<Socket> { };
        IPAddress thisAddress;

        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;

        public Form1()
        {
            InitializeComponent();
            SetIp();
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _textAppender = new AppendTextDelegate(AppendText);
        }


        private void SetIp()

        {

            IPHostEntry he = Dns.GetHostEntry(Dns.GetHostName());
            richTextBox2.Text = "8088";

            // 처음으로 발견되는 ipv4 주소를 사용한다.

            foreach (IPAddress addr in he.AddressList)

            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    thisAddress = addr;
                    break;
                }
            }
          
            if (thisAddress == null)
                thisAddress = IPAddress.Loopback;// 로컬호스트 주소를 사용한다.
            richTextBox1.Text = thisAddress.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            startServer();
            button1.Enabled = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            StopServer();
        }

        public void StopServer()
        {
            if (this.mainSocket != null || this.mainSocket.Connected)
            {
                try
                {
                    this.mainSocket.Shutdown(SocketShutdown.Both);
                }

                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            mainSocket.Close();
        }

        public void startServer()

        {
            int port;
            if (!int.TryParse(richTextBox2.Text, out port))
            {

                MessageBox.Show("포트 번호가 잘못 입력되었거나 입력되지 않았습니다.");

                richTextBox2.Focus();

                richTextBox2.SelectAll();

                return;

            }

            IPEndPoint iep = new IPEndPoint(thisAddress, port);

            mainSocket.Bind(iep);

            mainSocket.Listen(5);

            mainSocket.BeginAccept(AcceptConn, null);

            richTextBox3.Text += "Server Listen";
            richTextBox3.SelectionStart = richTextBox3.TextLength;
            richTextBox3.ScrollToCaret();

        }

        private void AcceptConn(IAsyncResult ar)

        {

            // 클라이언트의 연결 요청을 수락한다.

            Socket client = mainSocket.EndAccept(ar);

            // 또 다른 클라이언트의 연결을 대기한다.

            mainSocket.BeginAccept(AcceptConn, null);

            // 연결된 클라이언트 리스트에 추가해준다.

            connectedClients.Add(client);

            // 텍스트박스에 클라이언트가 연결되었다고 써준다.
            AppendText(richTextBox3, string.Format("\r\nServer Connect"));

            // AppendText(richTextBox3, string.Format("Server Connect"));
            AppendText(richTextBox4, string.Format("클라이언트 (@ {0})가 연결되었습니다.", client.RemoteEndPoint));

            // 클라이언트의 데이터를 받는다.

            client.BeginReceive(data, 0, size, 0, ReceiveData, client);

        }


        private void SendData(IAsyncResult iar)

        {

            Socket client = (Socket)iar.AsyncState;

            int sent = client.EndSend(iar);

            client.BeginReceive(data, 0, size, SocketFlags.None, ReceiveData, client);

        }



        private void ReceiveData(IAsyncResult iar)

        {

            Socket client = (Socket)iar.AsyncState;

            if (!client.Connected)

                return;

            int recv = client.EndReceive(iar);

            if (recv <= 0)

            {

                client.Shutdown(SocketShutdown.Both);

                client.Close();

                return;

            }

            string recvData = Encoding.UTF8.GetString(data, 0, recv);

            AppendText(this.richTextBox4, recvData);

            byte[] message2 = Encoding.UTF8.GetBytes(recvData);

            client.BeginSend(message2, 0, message2.Length, SocketFlags.None, SendData, client);


        }


        void AppendText(Control ctrl, string s)

        {

            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);

            else

            {

                string source = ctrl.Text;

                ctrl.Text = source + Environment.NewLine + s;

            }

        }



        private void txtSend_KeyUp(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Enter)

            {

                SendMessaage();

            }

        }



        private void SendMessaage()

        {

            byte[] message = Encoding.UTF8.GetBytes(richTextBox5.Text.Trim());

            foreach (var client in this.connectedClients)

            {

                client.Send(message);

            }

            AppendText(this.richTextBox4, richTextBox5.Text.Trim());

            richTextBox5.Clear();

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SendMessaage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rs232commTest
{
    public partial class Form1 : Form
    {

        SerialPort sp = new SerialPort();

        public  delegate void dele();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            sp.PortName = "COM1";          //  포트넘버
            sp.BaudRate = 9600;             //  통신속도
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.Handshake = Handshake.None;

            sp.DataReceived += Sp_DataReceived;

        }


        string indata = "";
        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
            this.Invoke(new dele(() =>
            {
                indata = sp.ReadExisting();
                richTextBox1.AppendText(indata);

            }));
        }

        private void button1_Click(object sender, EventArgs e)  //연결
        {
            sp.Open();
            if (sp.IsOpen == true) MessageBox.Show("연결되었습니다.");
        }

        private void button2_Click(object sender, EventArgs e)  //열결 끊기
        {
            sp.Close();
            if (sp.IsOpen == false) MessageBox.Show("연결이 끊겼습니다.");

        }

        private void button3_Click(object sender, EventArgs e)  //보내기
        {
            if (richTextBox_received.Text.Length > 0)
            {
                //포트 오픈 여부 체크
                if (sp == null || sp.IsOpen == false)
                {
                    MessageBox.Show("포트를 열어 주세요");
                    return;
                }
                else
                {
                    sp.Write(richTextBox_received.Text);
                }
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sp.Close();
        }
    }
}

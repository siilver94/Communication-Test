using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Text;
using Ken2.Util;
using System.Diagnostics;
using System.IO;

namespace EthernetTest
{
    public partial class Form1 : Form
    {
        TcpClient tc;
        public TCPClient_PLC1 tck;

        string msg = "";

        NetworkStream stream;



        private delegate void dele(); // delegate


        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }
        public Form1()
        {
            InitializeComponent();
        }

        //fffffffffff
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }




        private void button1_Click(object sender, EventArgs e)   //접속
        {
            // (1) IP 주소와 포트를 지정하고 TCP 연결 
            tc = new TcpClient(textBox3.Text, Convert.ToInt32(textBox1.Text));
            tck = new TCPClient_PLC1(textBox3.Text, Convert.ToInt32(textBox1.Text), 100, this);

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)  // 종료
        {
            tc.Dispose();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)  //전송
        {

            tck.MCWrite(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text));

            richTextBox1.AppendText(Dtime.Now(Dtime.StringType.CurrentTime) + " : " + textBox5.Text + Environment.NewLine);
            richTextBox1.ScrollToCaret();
            Delay(100);
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        //cccccccccccc
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tc.Dispose();
        }
    }

}

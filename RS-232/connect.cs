SerialPort sp = new SerialPort();

sp.PortName = "COM37";          //  포트넘버
sp.BaudRate = 9600;             //  통신속도
sp.DataBits = 8;
sp.Parity = Parity.None;
sp.StopBits = StopBits.One;
sp.Handshake = Handshake.None;

sp.Open();

sp.DataReceived += Sp_DataReceived;

string indata = "";

indata = sp.ReadExisting();
richTextBox_received.AppendText(indata);

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 三菱3E帧项目
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Mitsubishi_message mitsubishi_Message = new Mitsubishi_message();
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Connect(new IPEndPoint(IPAddress.Parse("192.168.3.2"), int.Parse("4999")));
            //var t= mitsubishi_Message.Write_Word(message_Word.D, 0, new byte[] { 21 });
            //socket.Send(mitsubishi_Message.Write_Word(message_Word.D, 0, new byte[] { 01,02 }));
            //byte[] vs = new byte[100];
            //socket.Receive(vs);
            //socket.Send(mitsubishi_Message.Write_Word(message_Word.D, 1, new byte[] { 01, 02 ,01}));          
            //byte[] vs6 = new byte[100];
            //socket.Receive(vs6);
            //socket.Send(mitsubishi_Message.Read_bit(message_bit.Y,0,3));
            //byte[] vs2 = new byte[100];
            //socket.Receive(vs2);
            //socket.Send(BytesLHToBytesHL("500000FFFF03000C00010001040000000000A80100"));
            //byte[] vs1 = new byte[100];
            //socket.Receive(vs1);           

            Mitsubishi_realize mitsubishi_Realize = new Mitsubishi_realize(new IPEndPoint(IPAddress.Parse("192.168.3.2"), int.Parse("4999")));
            mitsubishi_Realize.Open();
            var DS = mitsubishi_Realize.Read_Byet(message_Word.D, 0);
            var DS1 = mitsubishi_Realize.Read_short(message_Word.D, 0);
            var DS2 = mitsubishi_Realize.Read_int(message_Word.D, 1);
            var DS3 = mitsubishi_Realize.Read_uint(message_Word.D, 1);
            var DS4 = mitsubishi_Realize.Read_float(message_Word.D, 4);
            var DS5 = mitsubishi_Realize.Write_Byte(message_Word.D, 10, 66);
            var DS6 = mitsubishi_Realize.Write_short(message_Word.D, 11, 66);
            var DS7 = mitsubishi_Realize.Write_ushort(message_Word.D, 12, 66);
            var DS8 = mitsubishi_Realize.Write_int(message_Word.D, 13, 66666);
            var DS9 = mitsubishi_Realize.Write_Int64(message_Word.D, 15, 55555555);

            for (int I = 0; I < 10; I++)
            {
                var SD = mitsubishi_Realize.write_Bool(message_bit.Y, 0, false);
                var SD11 = mitsubishi_Realize.write_multi_Bool(message_bit.Y, 10, new bool[] { true, true, true });
                Thread.Sleep(600);
                var SD12 = mitsubishi_Realize.write_multi_Bool(message_bit.Y, 10, new bool[] { false, false, false });
                var SD10 = mitsubishi_Realize.write_Bool(message_bit.Y, 0, true);
                Thread.Sleep(600);
            }
            var SD1 = mitsubishi_Realize.Read_Bool(message_bit.Y, 0);

            var DS22 = mitsubishi_Realize.Read_Byet(message_Word.D, 0);

            var SD20 = mitsubishi_Realize.Read_Bool(message_bit.Y, 0,3);
            var SD21 = mitsubishi_Realize.Read_Bool(message_bit.Y, 10, 3);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using 三菱3E帧项目;
using System.Threading;
using Modbus;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //对PLC进行远程操作
            //Mitsubishi_PLC_remote mitsubishi_PLC_Remote = new Mitsubishi_PLC_remote(new IPEndPoint(IPAddress.Parse("192.168.3.2"), int.Parse("4999")));
            //mitsubishi_PLC_Remote.Open();
            //Console.WriteLine(mitsubishi_PLC_Remote.);
            //mitsubishi_PLC_Remote.Write_PLC_Rrr_Rest();
            //Thread.Sleep(5000);
            //mitsubishi_PLC_Remote.Write_PLC_Stop();
            //Thread.Sleep(5000);
            //mitsubishi_PLC_Remote.Write_PLC_Run();
            //Thread.Sleep(5000);
            //mitsubishi_PLC_Remote.Write_PLC_Stop();
            //Thread.Sleep(5000);
            //mitsubishi_PLC_Remote.Write_PLC_Run();
            //modbus_tcp测试
            //Modbus.Modbus_TCPrealize modbus_TCPrealize = new Modbus_TCPrealize(new IPEndPoint(IPAddress.Parse("192.168.3.2"), int.Parse("502")));
            //modbus_TCPrealize.Open();
            //var t = Task.Run(() =>
            //{
            //    for (int I = 0; I < 9999; I++)
            //    {
            //        //复位
            //        Console.WriteLine(modbus_TCPrealize.write_multi_Bool(20, 3, 0).Content);
            //        Console.WriteLine(modbus_TCPrealize.Read_Bool(0).Content);
            //        Console.WriteLine(modbus_TCPrealize.Read_Byet(0).Content);
            //        Console.WriteLine(modbus_TCPrealize.Read_multi_Bool(10, 3).Content);
            //        Console.WriteLine(modbus_TCPrealize.Read_uint(13).Content);
            //        Console.WriteLine(modbus_TCPrealize.write_Bool(20, coil.ON).Content);
            //        Thread.Sleep(300);
            //        Console.WriteLine(modbus_TCPrealize.write_multi_Bool(20, 3, 15).Content);
            //        Console.WriteLine(modbus_TCPrealize.Write_int(20, 666).Content);
            //        Thread.Sleep(300);
            //    }
            //});
            ////三菱3E帧报文测试
            Mitsubishi_realize mitsubishi_Realize = new Mitsubishi_realize(new IPEndPoint(IPAddress.Parse("192.168.3.251"), int.Parse("501")));
            mitsubishi_Realize.Open();
            Console.WriteLine(mitsubishi_Realize.write_Bool(message_bit.Y, 37, false).Content.ToString() ?? "00");
            //var D = mitsubishi_Realize.Read_Bool(message_bit.Y, 42);
            //mitsubishi_Realize.write_Bool(message_bit.Y, 27, false);
            //var t1 = Task.Run(() =>
            //  {
            //      for (int I = 0; I < 9999; I++)
            //      {
            //          Console.WriteLine(mitsubishi_Realize.Read_Byet(message_Word.D, 0).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Read_short(message_Word.D, 0).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Read_int(message_Word.D, 1).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Read_uint(message_Word.D, 1).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Read_float(message_Word.D, 4).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Write_Byte(message_Word.D, 10, 66).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Write_short(message_Word.D, 11, 66).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Write_ushort(message_Word.D, 12, 66).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Write_int(message_Word.D, 13, 66666).Content.ToString() ?? "00");
            //          Console.WriteLine(mitsubishi_Realize.Write_Int64(message_Word.D, 15, 55555555).Content.ToString() ?? "00");
            //      }

            //  });
            //for (int I = 0; I < 9999; I++)
            //{
            //    Console.WriteLine(mitsubishi_Realize.write_Bool(message_bit.Y, 0, false).Content.ToString() ?? "00");
            //    Console.WriteLine(mitsubishi_Realize.write_multi_Bool(message_bit.Y, 10, new bool[] { true, true, true }).Content.Length);
            //    Thread.Sleep(600);
            //    Console.WriteLine(mitsubishi_Realize.write_multi_Bool(message_bit.Y, 10, new bool[] { false, false, false }).Content);
            //    Console.WriteLine(mitsubishi_Realize.write_Bool(message_bit.Y, 0, true).Content.ToString() ?? "00");
            //    Thread.Sleep(600);
            //    Console.WriteLine(mitsubishi_Realize.Read_Bool(message_bit.Y, 0).Content.ToString() ?? "00");
            //    Console.WriteLine(mitsubishi_Realize.Read_Byet(message_Word.D, 0).Content);
            //    Console.WriteLine(mitsubishi_Realize.Read_Bool(message_bit.Y, 0, 3).Content);
            //    Console.WriteLine(mitsubishi_Realize.Read_Bool(message_bit.Y, 10, 3).Content);
            //}
            
             Console.Read();
        }
        public static byte[] BytesLHToBytesHL(string str)//字符串转换二进制
        {
            byte[] bytes = new byte[str.Length / 2];

            for (int i = 0; i < str.Length; i += 2)

                bytes[i / 2] = (byte)Convert.ToByte(str.Substring(i, 2), 16);
            return bytes;
        }
}
}

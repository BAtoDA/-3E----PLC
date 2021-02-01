using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 三菱3E帧项目
{
    /// <summary>
    /// 实现对三菱设备通过3E帧进行数据交换访问
    /// 三菱3E帧实现继承与Mitsubishi_message
    /// </summary>
    public class Mitsubishi_realize :Mitsubishi_message
    {
        /// <summary>
        /// 实例化一个套接字
        /// </summary>
        Socket socket { get; set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /// <summary>
        /// 实例化一个IP地址与端口
        /// </summary>
        IPEndPoint IPEnd { get; set; } = new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse("4999"));
        /// <summary>
        /// 指示着是否访问成功
        /// </summary>
        public bool Socket_ready { get; set; }
        /// <summary>
        /// 互斥锁 预防多线程进入导致 数据错乱
        /// </summary>
        static Mutex mutex { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iPEnd"></param>
        public Mitsubishi_realize(IPEndPoint iPEnd)
        {

            this.IPEnd = iPEnd;//获取地址
            //this.IPEnd.Port = 4999;//强行更改端口
            this.socket.ReceiveTimeout = 8000;//超时
            this.socket.SendTimeout = 8000;//超时
            mutex = new Mutex();//实例化
        }
        /// <summary>
        /// 无参数构造函数
        /// IP默认是127.0.0.1--端口是4999
        /// </summary>
        public Mitsubishi_realize()
        {

            //this.IPEnd.Port = 4999;//强行更改端口
            this.socket.ReceiveTimeout = 8000;//超时
            this.socket.SendTimeout = 8000;//超时
            mutex = new Mutex();//实例化
        }
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public Operating<int> Open()
        {
            try
            {
                this.socket.Connect(this.IPEnd);//链接远程设备
                this.Socket_ready = true;//标志着访问成功
                return new Operating<int>() { Content = 0, ErrorCode = "0", IsSuccess = true };//返回本次访问的结果
            }
            catch (Exception e)
            {
                return Err<int>(e);//推出异常
            }
        }
        /// <summary>
        /// 切断套接字
        /// </summary>
        public void Close()
        {
            if (this.socket.EnableBroadcast)
                this.socket.Close();//切断一个套接字
            else
                return;
        }
        /// <summary>
        /// 读取一个字节--不推荐使用
        /// 由于三菱寄存器全是字寄存器会导致高位无法访问
        /// </summary>
        /// <param name="message_Word">要读取的软元件地址</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public Operating<byte> Read_Byet(message_Word message_Word, int address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Read_Word(message_Word,address,1));//发送报文
                    return (Operating<byte>)Read_return(numerical_format.Byet);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<byte>(e);
            }
        }
        /// <summary>
        /// 读取一个字
        /// </summary>
        /// <param name="message_Word">要读取的软元件地址</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public Operating<short> Read_short(message_Word message_Word, int address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Read_Word(message_Word,address,1));//发送报文
                    return (Operating<short>)Read_return(numerical_format.Signed_16_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<short>(e);
            }
        }
        /// <summary>
        /// 读取一个无符号字
        /// </summary>
        /// <param name="message_Word">要读取的软元件地址</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public Operating<ushort> Read_ushort(message_Word message_Word, int address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Read_Word(message_Word,address,1));//发送报文
                    return (Operating<ushort>)Read_return(numerical_format.Unsigned_16_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<ushort>(e);
            }
        }
        /// <summary>
        /// 读取有符号双字
        /// </summary>
        /// <param name="message_Word">要读取的软元件地址</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public Operating<int> Read_int(message_Word message_Word, int address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Read_Word(message_Word,address,2));//发送报文
                    return (Operating<int>)Read_return(numerical_format.Signed_32_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<int>(e);
            }
        }
        /// <summary>
        /// 读取无符号双字
        /// </summary>
        /// <param name="message_Word">要读取的软元件地址</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public Operating<uint> Read_uint(message_Word message_Word, int address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Read_Word(message_Word,address,2));//发送报文
                    return (Operating<uint>)Read_return(numerical_format.Unsigned_32_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<uint>(e);
            }
        }
        /// <summary>
        /// 读取浮点小数
        /// </summary>
        /// <param name="message_Word">要读取的软元件地址</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public Operating<float> Read_float(message_Word message_Word, int address)
        {
            try
            {               
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Read_Word(message_Word, address, 2));//发送报文
                    return (Operating<float>)Read_return(numerical_format.Float_32_Bit);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<float>(e);
            }
        }
        /// <summary>
        /// 用于读取返回指定类型
        /// </summary>
        /// <param name="numerical">起始地址</param>
        /// <returns></returns>
        private object Read_return(numerical_format numerical)
        {
            object message = new Operating<short>() { Content = 0, ErrorCode = "0", IsSuccess = true };
            try
            {
                byte[] data = new byte[100];//定义数据接收数组  
                this.socket.Receive(data);//接收数据到data数组  
                switch (numerical)
                {
                    case numerical_format.Byet:
                        message = new Operating<byte>() { Content = data[11], ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_16_Bit:
                        message = new Operating<short>() { Content = BitConverter.ToInt16(new byte[] { data[11], data[12] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Unsigned_16_Bit:
                        message = new Operating<ushort>() { Content = BitConverter.ToUInt16(new byte[] { data[11], data[12] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_32_Bit:
                        message = new Operating<int>() { Content = BitConverter.ToInt32(new byte[] { data[11], data[12], data[13], data[14] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Unsigned_32_Bit:
                        message = new Operating<uint>() { Content = BitConverter.ToUInt32(new byte[] { data[11], data[12], data[13], data[14] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Float_32_Bit:
                        message = new Operating<float>() { Content = BitConverter.ToSingle(new byte[] { data[11], data[12], data[13], data[14] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                }
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<int>(e);
            }
            mutex.ReleaseMutex();
            return message;//返回内容
        }
        /// <summary>
        /// 向设备写入一个字节--不推荐使用只能写入低位
        /// </summary>
        /// <param name="message_Word">需要写入的软元件类型</param>
        /// <param name="address">起始地址</param>
        /// <param name="Data">需要写入的数据</param>
        /// <returns></returns>
        public Operating<byte> Write_Byte(message_Word message_Word, int address, byte Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (Operating<byte>)Write_return(message_Word,numerical_format.Byet, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<byte>(e);
            }
        }
        /// <summary>
        /// 向设备写入一个字
        /// </summary>
        /// <param name="message_Word">需要写入的软元件类型</param>
        /// <param name="address">起始地址</param>
        /// <param name="Data">需要写入的数据</param>
        /// <returns></returns>
        public Operating<short> Write_short(message_Word message_Word, int address, short Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (Operating<short>)Write_return(message_Word, numerical_format.Signed_16_Bit, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<short>(e);
            }
        }
        /// <summary>
        /// 向设备写入一个无符号字
        /// </summary>
        /// <param name="message_Word">需要写入的软元件类型</param>
        /// <param name="address">起始地址</param>
        /// <param name="Data">需要写入的数据</param>
        /// <returns></returns>
        public Operating<ushort> Write_ushort(message_Word message_Word, int address, ushort Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (Operating<ushort>)Write_return(message_Word, numerical_format.Unsigned_16_Bit, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<ushort>(e);
            }
        }
        /// <summary>
        /// 向设备写入一个双字
        /// </summary>
        /// <param name="message_Word">需要写入的软元件类型</param>
        /// <param name="address">起始地址</param>
        /// <param name="Data">需要写入的数据</param>
        /// <returns></returns>
        public Operating<int> Write_int(message_Word message_Word, int address, int Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (Operating<int>)Write_return(message_Word, numerical_format.Signed_32_Bit, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<int>(e);
            }
        }
        /// <summary>
        /// 向设备写入一个long长整形
        /// </summary>
        /// <param name="message_Word">需要写入的软元件类型</param>
        /// <param name="address">起始地址</param>
        /// <param name="Data">需要写入的数据</param>
        /// <returns></returns>
        public Operating<long> Write_Int64(message_Word message_Word, int address, long Data)
        {
            try
            {
                if (this.Socket_ready)
                {
                    return (Operating<long>)Write_return(message_Word, numerical_format.Signed_64_Bit, Data, address);
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<long>(e);
            }
        }
        /// <summary>
        /// 用于写入的操作结果
        /// </summary>
        /// <typeparam name="T">要写入的类型--约束泛型T</typeparam>
        /// <param name="message_Word">要写入软元件的类型</param>
        /// <param name="numerical">要写入的格式</param>
        /// <param name="Data">要写入的数据--类型是约束T的类型</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        private object Write_return<T>(message_Word message_Word,numerical_format numerical, T Data, int address)
        {
            mutex.WaitOne(3000);
            object message = new Operating<short>() { Content = 0, ErrorCode = "0", IsSuccess = true };
            try
            {
                byte[] Write_Data = new byte[10];//创建默认要写入的数据
                byte[] transition = BitConverter.GetBytes(Convert.ToInt64(Data));
                for (int i = 0; i < transition.Length; i++)//获得要写入的数据
                    Write_Data[i] = transition[i];
                switch (numerical)
                {
                    case numerical_format.Byet:
                        this.socket.Send(this.Write_Word(message_Word,address,new byte[] { Write_Data[0]}));
                        message = new Operating<byte>() { Content = Write_Data[0], ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_16_Bit:
                        this.socket.Send(this.Write_Word(message_Word, address, new byte[] { Write_Data[1], Write_Data[0] }));
                        message = new Operating<short>() { Content = BitConverter.ToInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Unsigned_16_Bit:
                        this.socket.Send(this.Write_Word(message_Word, address, new byte[] { Write_Data[1], Write_Data[0] }));
                        message = new Operating<ushort>() { Content = BitConverter.ToUInt16(new byte[] { Write_Data[0], Write_Data[1] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_32_Bit:
                        this.socket.Send(this.Write_Word(message_Word, address, new byte[] { Write_Data[3], Write_Data[2], Write_Data[1], Write_Data[0] }));
                        message = new Operating<int>() { Content = BitConverter.ToInt32(new byte[] { Write_Data[0], Write_Data[1], Write_Data[2], Write_Data[3] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                    case numerical_format.Signed_64_Bit:
                        this.socket.Send(this.Write_Word(message_Word, address, new byte[] {Write_Data[7], Write_Data[6], Write_Data[5], Write_Data[4], Write_Data[3], Write_Data[2], Write_Data[1], Write_Data[0] }));
                        message = new Operating<long>() { Content = BitConverter.ToInt64(new byte[] { Write_Data[0], Write_Data[1], Write_Data[2], Write_Data[3], Write_Data[4], Write_Data[5] , Write_Data[6], Write_Data[7] }, 0), ErrorCode = "0", IsSuccess = true };
                        break;
                }
                //统一等待回复报文
                byte[] Data_1 = new byte[50];
                this.socket.Receive(Data_1);//接收回复
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<T>(e);
            }
            mutex.ReleaseMutex();
            return message;//返回内容
        }
        /// <summary>
        /// 读取一个位类型
        /// </summary>
        /// <param name="message_Bit">需要读取的软元件</param>
        /// <param name="address">起始地址</param>
        /// <returns></returns>
        public Operating<bool> Read_Bool(message_bit message_Bit,int address)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    var XA = this.Read_bit(message_Bit, address, 1);
                    this.socket.Send(this.Read_bit(message_Bit,address,1));//发送报文
                    byte[] Data = new byte[50];
                    this.socket.Receive(Data);//接收回复
                    mutex.ReleaseMutex();
                    return new Operating<bool>() { Content = Analysis(Data,address), ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 解析Y点线圈状态
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private bool Analysis(byte[] Data, int address)
        {
            int len = 0;//Y点读取位置
            int inx = 0;//尾部位置
            switch (address.ToString().Length)
            {
                case 1:
                    len = 1;
                    inx = address;
                    break;
                case 2:
                    len = Convert.ToInt32(address.ToString().Remove(1, 1)) % 2 > 0 ? 2 : 1;
                    inx = Convert.ToInt32(address.ToString().Remove(0, 1));
                    break;
                case 3:
                    len = Convert.ToInt32(address.ToString().Remove(2, 2)) % 2 > 0 ? 2 : 1;
                    inx = Convert.ToInt32(address.ToString().Remove(0, 2));
                    break;
            }
            if(len>1)
            {
                int a = 15 + (inx / 2);
                return Y_ysis(Data[15 + (inx / 2)],inx);
            }
            return Y_ysis(Data[11 + (inx / 2)],inx);
        }

        private bool Y_ysis(byte Data, int inx)
        {
            switch (Data)
            {
                case 1:
                    if (inx % 2 == 1)
                        return true;
                    else
                        return false;
                case 16:
                    if (inx % 2 == 1)
                        return false;
                    else
                        return true;
                case 17:
                    if (inx % 2 == 1)
                        return true;
                    else
                        return true;
                case 0:
                    if (inx % 2 == 1)
                        return false;
                    else
                        return false;
            }
            return false;
        }
        /// <summary>
        /// 读取多个位
        /// </summary>
        /// <param name="message_Bit">需要读取的软元件</param>
        /// <param name="address">起始地址</param>
        /// <param name="number">读取个数</param>
        /// <returns></returns>
        public Operating<bool[]> Read_Bool(message_bit message_Bit, int address, byte number)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Read_bit(message_Bit, address, number));//发送报文
                    byte[] Data = new byte[50];
                    this.socket.Receive(Data,SocketFlags.None);//接收回复
                    byte[] Data_1 = new byte[Data[7] - 2];
                    for (int i = 0; i < (Data[7] - 2); i++)
                        Data_1[i] = Data[11 + i];
                    mutex.ReleaseMutex();
                    return new Operating<bool[]>() { Content = Byet_to_bool(Data_1,number), ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool[]>(e);
            }
        }
        /// <summary>
        /// 写入单线圈位
        /// </summary>
        /// <param name="message_Bit">需要读取的软元件类型</param>
        /// <param name="address">起始地址</param>
        /// <param name="coil">要写入的状态</param>
        /// <returns></returns>
        public Operating<bool> write_Bool(message_bit message_Bit,int address, bool coil)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Write_bit(message_Bit,address, coil));//发送报文
                    byte[] Data = new byte[50];
                    this.socket.Receive(Data);//接收回复
                    mutex.ReleaseMutex();
                    return new Operating<bool>() { Content = Convert.ToBoolean(coil), ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 写入多线圈
        /// </summary>
        /// <param name="message_Bit">需要写入的软软件类型</param>
        /// <param name="address">起始地址</param>
        /// <param name="coil">要写入状态的bool数组长度决定了要写入的个数</param>
        /// <returns></returns>
        public Operating<bool[]> write_multi_Bool(message_bit message_Bit,byte address, bool[] coil)
        {
            try
            {
                mutex.WaitOne(3000);
                if (this.Socket_ready)
                {
                    this.socket.Send(this.Write_multi_bit(message_Bit,address,coil));//发送报文
                    byte[] Data = new byte[50];
                    this.socket.Receive(Data);//接收回复
                    mutex.ReleaseMutex();
                    return new Operating<bool[]>() { Content =coil, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return Err<bool[]>(e);
            }
        }
        /// <summary>
        /// 把接收到的字节数组转换成bool数组
        /// </summary>
        /// <param name="Data">字节长度</param>
        /// <param name="multi">要转换的个数</param>
        /// <returns></returns>
        private bool[] Byet_to_bool(byte[] Data, byte multi)
        {
            List<bool> Data_1 = new List<bool>();
            int rul = 0;
            for(int i=0;i<multi;i+=2)
            {
                switch(Data[rul])
                {
                    case 16:
                        Data_1.Add(true);
                        Data_1.Add(false);
                        break;
                    case 17:
                        Data_1.Add(true);
                        Data_1.Add(true);
                        break;
                    case 1:
                        Data_1.Add(false);
                        Data_1.Add(true);
                        break;
                    case 0:
                        Data_1.Add(false);
                        Data_1.Add(false);
                        break;
                }
                rul += 1;
            }
            return Data_1.Take(multi).ToArray();
        }
        /// <summary>
        /// 处理Err异常  返回结果
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private Operating<T> Err<T>(Exception exception)
        {
            this.Socket_ready = false;//表示链接已经切断
            return new Operating<T>() { ErrorCode = exception.Message, IsSuccess = false };//返回本次访问的结果
        }
    }
}

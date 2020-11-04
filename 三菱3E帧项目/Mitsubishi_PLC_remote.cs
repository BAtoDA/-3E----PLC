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
    /// 本类是实现对三菱PLC进行远程操作
    /// 将SLMP对应设备及CPU模块置为RUN状态及STOP状态等
    /// </summary>
    public class Mitsubishi_PLC_remote:Mitsubishi_message
    {
        /// <summary>
        /// 实例化一个IP地址与端口
        /// </summary>
        IPEndPoint IPEnd { get; set; } = new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse("4999"));
        /// <summary>
        /// 创建一个UDP对象
        /// </summary>
        UdpClient Udp { get; set; }
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
        public Mitsubishi_PLC_remote(IPEndPoint iPEnd)
        {
            this.IPEnd = iPEnd;
            Udp = new UdpClient(this.IPEnd.Address.ToString(),IPEnd.Port);
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
                this.Udp.Connect(this.IPEnd);//链接远程设备
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
            if (this.Udp.EnableBroadcast)
                this.Udp.Close();//切断一个套接字
            else
                return;
        }
        /// <summary>
        /// 对PLC进行远程RUN操作
        /// 没有远程RUN指令的响应数据
        /// </summary>
        /// <returns></returns>
        public Operating<bool> Write_PLC_Run()
        {
            try
            {
                if (this.Socket_ready)
                {
                    byte[] Data = this.PLC_Run_remote();
                    this.Udp.Send(Data, Data.Length);
                    return new Operating<bool>() { Content = true, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 对PLC进行远程Stop操作
        /// 没有远程STOP指令的响应数据
        /// </summary>
        /// <returns></returns>
        public Operating<bool> Write_PLC_Stop()
        {
            try
            {
                if (this.Socket_ready)
                {
                    byte[] Data = this.PLC_Stop_remote();
                    this.Udp.Send(Data, Data.Length);
                    return new Operating<bool>() { Content = true, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 对PLC进行远程Pause操作
        /// 没有远程Pause指令的响应数据
        /// </summary>
        /// <returns></returns>
        public Operating<bool> Write_PLC_Pause()
        {
            try
            {
                if (this.Socket_ready)
                {
                    byte[] Data = this.PLC_Pause_remote();
                    this.Udp.Send(Data, Data.Length);
                    return new Operating<bool>() { Content = true, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 对PLC进行远程Rest操作--清除寄存器-慎用
        /// 没有远程Rest指令的响应数据
        /// </summary>
        /// <returns></returns>
        public Operating<bool> Write_PLC_Rest()
        {
            try
            {
                if (this.Socket_ready)
                {
                    byte[] Data = this.PLC_Rest_remote();
                    this.Udp.Send(Data, Data.Length);
                    return new Operating<bool>() { Content = true, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<bool>(e);
            }
        }
        /// <summary>
        /// 对PLC进行远程Rrr_Rest操作复位出错
        /// 没有远程Rrr_Rest指令的响应数据
        /// </summary>
        /// <returns></returns>
        public Operating<bool> Write_PLC_Rrr_Rest()
        {
            try
            {
                if (this.Socket_ready)
                {
                    byte[] Data = this.PLC_Rrr_Rest_remote();
                    this.Udp.Send(Data, Data.Length);
                    return new Operating<bool>() { Content = true, ErrorCode = "0", IsSuccess = true };
                }
                else
                    throw new AggregateException("未连接设备");
            }
            catch (Exception e)
            {
                return Err<bool>(e);
            }
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

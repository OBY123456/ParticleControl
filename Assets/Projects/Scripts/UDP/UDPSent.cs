using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPSent : MonoBehaviour
{
    public static UDPSent Instance;

    private string ip = "192.168.6.194";     //主机ip地址
    private int port = 8001;
    private IPAddress ipAddress;
    private IPEndPoint endPoint;
    private Socket socket;
    private EndPoint server;
    private byte[] sendData;                //发送内容，转化为byte字节

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init();
    }

    //发送函数
    public void Send(string value)             //参数不是字符串时转化为string
    {
        string msg = value;      //传递的值转化为string
        try
        {
            sendData = new byte[1024];                  //定义发送字节大小
            sendData = Encoding.Default.GetBytes(msg);  //对msg编码
            socket.SendTo(sendData, sendData.Length, SocketFlags.None, endPoint);    //发送信息
            LogMsg.Instance.Log("发送消息==" + msg);
        }
        catch
        {
            LogMsg.Instance.Log("发送失败！");
        }
    }

    public void Init()
    {
        //if (Config.Instance)
        //{
        //    ip = Config.Instance.configData.ip;
        //    port = Config.Instance.configData.Port;
        //}
        ipAddress = IPAddress.Parse(ip);            //ip地址
        endPoint = new IPEndPoint(ipAddress, port);  //自定义端口号
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 8060);
        server = (EndPoint)sender;
    }
}

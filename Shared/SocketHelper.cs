using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Shared;

public class SocketHelper
{
    public const char Eom = '␙';
    public const char Ack = '␆';

    public static async Task<string> Receive(Socket socket)
    {
        byte[] buffer = new byte[1024];
        int received = await socket.ReceiveAsync(buffer);
        string response = Encoding.UTF8.GetString(buffer, 0, received);
        return response;
    }

    public static async Task Send(Socket socket, string message)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes($"{message}{Eom}");
        _ = await socket.SendAsync(messageBytes);
    }

    public static async Task SendAck(Socket socket)
    {
        await Send(socket, $"{Ack}");
    }

    public static async Task<bool> ReceiveAck(Socket socket)
    {
        return (await Receive(socket))[0] == Ack;
    }

    public static async Task<IPEndPoint> GetIpEndPoint(string hostName, int port)
    {
        IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(hostName);
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        return new(ipAddress, port);
    }

    public static Socket GetSocket(IPEndPoint ipEndPoint)
    {
        return new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }
}
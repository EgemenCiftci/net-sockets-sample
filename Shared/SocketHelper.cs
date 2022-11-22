using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Shared;

public class SocketHelper
{
    public const char Eom = '␙';
    public const char Ack = '␆';

    public static async Task<string> ReceiveAsync(Socket socket)
    {
        byte[] buffer = new byte[1024];
        int received = await socket.ReceiveAsync(buffer);
        return Encoding.UTF8.GetString(buffer, 0, received);
    }

    public static async Task SendAsync(Socket socket, string message)
    {
        _ = await socket.SendAsync(Encoding.UTF8.GetBytes($"{message}{Eom}"));
    }

    public static async Task SendAckAsync(Socket socket)
    {
        await SendAsync(socket, $"{Ack}");
    }

    public static async Task<bool> ReceiveAckAsync(Socket socket)
    {
        return (await ReceiveAsync(socket))[0] == Ack;
    }

    public static async Task<IPEndPoint> GetIpEndPointAsync(string hostName, int port)
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
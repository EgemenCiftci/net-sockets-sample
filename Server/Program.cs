using Shared;
using System.Net;
using System.Net.Sockets;

const string hostName = "localhost";
const int port = 11000;

IPEndPoint ipEndPoint = await SocketHelper.GetIpEndPoint(hostName, port);
using Socket listener = SocketHelper.GetSocket(ipEndPoint);
listener.Bind(ipEndPoint);
listener.Listen(100);
Socket socket = await listener.AcceptAsync();

Console.WriteLine($"Listening...");

while (true)
{
    Console.WriteLine($"Waiting...");

    string response = await SocketHelper.Receive(socket);

    if (response.EndsWith(SocketHelper.Eom))
    {
        Console.WriteLine($"Message received: {response.TrimEnd(SocketHelper.Eom)}");
        await SocketHelper.SendAck(socket);
        Console.WriteLine($"ACK sent.");
        break;
    }
}

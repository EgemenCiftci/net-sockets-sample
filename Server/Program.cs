using Shared;
using System.Net;
using System.Net.Sockets;

const string hostName = "localhost";
const int port = 11000;

IPEndPoint ipEndPoint = await SocketHelper.GetIpEndPointAsync(hostName, port);
using Socket listener = SocketHelper.GetSocket(ipEndPoint);
listener.Bind(ipEndPoint);
listener.Listen(100);
Socket socket = await listener.AcceptAsync();

Console.WriteLine($"Listening to {hostName}:{port}");

while (true)
{
    Console.WriteLine("Waiting for a message...");

    string response = await SocketHelper.ReceiveAsync(socket);

    if (string.IsNullOrEmpty(response))
    {
        Console.WriteLine("Connection shutdown by client.");
        break;
    }

    if (response.EndsWith(SocketHelper.Eom))
    {
        Console.WriteLine($"Message received: {response.TrimEnd(SocketHelper.Eom)}");
        await SocketHelper.SendAckAsync(socket);
    }
}

Console.ReadKey();

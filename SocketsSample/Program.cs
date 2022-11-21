using Shared;
using System.Net;
using System.Net.Sockets;

const string hostName = "localhost";
const int port = 11000;

IPEndPoint ipEndPoint = await SocketHelper.GetIpEndPoint(hostName, port);
using Socket socket = SocketHelper.GetSocket(ipEndPoint);
await socket.ConnectAsync(ipEndPoint);

while (true)
{
    string message = "Hi friends 👋!";
    await SocketHelper.Send(socket, message);
    bool result = await SocketHelper.ReceiveAck(socket);

    if (result)
    {
        Console.WriteLine("Message sent.");
        break;
    }
}

Console.ReadKey();

socket.Shutdown(SocketShutdown.Both);
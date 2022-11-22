using Shared;
using System.Net;
using System.Net.Sockets;

const string hostName = "localhost";
const int port = 11000;

IPEndPoint ipEndPoint = await SocketHelper.GetIpEndPointAsync(hostName, port);
using Socket socket = SocketHelper.GetSocket(ipEndPoint);
await socket.ConnectAsync(ipEndPoint);

string message;
int maxRetryCount = 3;
int retryCount = 0;

Console.WriteLine($"Client connected to {hostName}:{port}");

while (true)
{
    Console.WriteLine("Please type a message. Type DC to disconnect...");

    message = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrEmpty(message))
    {
        continue;
    }

    if (message == "DC")
    {
        break;
    }

    while (retryCount < maxRetryCount)
    {
        await SocketHelper.SendAsync(socket, message);
        bool result = await SocketHelper.ReceiveAckAsync(socket);

        if (result)
        {
            retryCount = 0;
            break;
        }
        else
        {
            retryCount++;
        }
    }
}

socket.Shutdown(SocketShutdown.Both);

Console.WriteLine($"Disconnected.");
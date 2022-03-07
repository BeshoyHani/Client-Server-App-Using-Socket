using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set IP and Port of Server to communicate with
            IPAddress host = IPAddress.Parse("127.0.0.1");
            IPEndPoint hostEndpoint = new IPEndPoint(host, 8000);

            Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            clientSock.Connect(hostEndpoint);

            string serverMessage = "";

            //wait for server to send File
            byte[] receivedData = new byte[1024];
            int receivedBytesLen = clientSock.Receive(receivedData);
            int fileNameLen = BitConverter.ToInt32(receivedData);
            string fileName = Encoding.ASCII.GetString(receivedData, 4, fileNameLen);
            serverMessage = Encoding.ASCII.GetString(receivedData, 4 + fileNameLen, receivedBytesLen);
            // Store Received Data in a file
            File.WriteAllText("received file.txt", serverMessage);
            Console.WriteLine("Server Sent: {0}\n{1}", fileName, serverMessage);

            //Close Socket
            Console.WriteLine("Transfer Ended");
            clientSock.Shutdown(SocketShutdown.Both);
            clientSock.Close();
        }
    }
}

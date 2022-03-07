using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set Server IP, Port and Protocol
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(serverIP, 8000);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            sock.Bind(ipEnd);
            sock.Listen(100);
            
            //Personal Data
            Console.WriteLine("Beshoy Hani Badee");
            Console.WriteLine("CS");
            Console.WriteLine("2");
            Console.WriteLine("Listening at IP: 127.0.0.1, Port: 8000");

            //Getting File Data ready to be sent
            string fileName = args.Length>0 && args[0]!="fileName"? args[0] : "test.txt";
            byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
            byte[] fileNameLength = BitConverter.GetBytes(fileName.Length);
            byte[] fileContent = File.ReadAllBytes(fileName);
            int FileLength = 4 + fileNameByte.Length + fileContent.Length;
            byte[] file = new byte[FileLength];

            fileNameLength.CopyTo(file, 0);
            fileNameByte.CopyTo(file, 4);
            fileContent.CopyTo(file, 4 + fileNameByte.Length);

            //Wait for Client
            Socket clientSock = sock.Accept();
            Console.WriteLine("Client Accepted");

            //Send Client a message

            Console.WriteLine("Sending File: {0}",fileName);
            clientSock.Send(file);
            Console.WriteLine("File Sent");
            byte[] messageByteArray = new byte[1024];
            int receivedBytesLen = clientSock.Receive(messageByteArray);

            //Close Socket
            Console.WriteLine("Transfer Ended");
            clientSock.Shutdown(SocketShutdown.Both);
            clientSock.Close();


        }
    }
}
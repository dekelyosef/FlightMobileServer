using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightMobileWeb.Model
{
    public class MyClientModel : IClientModel
    {
        private TcpClient client = null;
        private NetworkStream stream = null;
        private readonly object lockObj;


        /**
         * Constructor
         **/
        public MyClientModel()
        {
            this.lockObj = new object();
        }


        /**
         * Open a new Tcp Client connection to simulator
         **/
        public void Connect(string ip, int port)
        {
            client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("Connected to client");
            stream = client.GetStream();
            stream.Flush();
        }

        /**
         * Close the client and the network stream
         **/
        public void Disconnect()
        {
            stream.Close();
            client.Close();
        }


        /**
         * Read from server
         **/
        public string Read()
        {
            lock (lockObj)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                byte[] bytes = new byte[1024];
                int bytesRead = this.stream.Read(bytes, 0, bytes.Length);
                // Checks if 10 seconds pass
                if (stopWatch.ElapsedMilliseconds > 10000)
                {
                    return Encoding.ASCII.GetString(bytes, 0, bytesRead);
                }
                else
                {
                    return null;
                }
            }
        }


        /**
         * Send the string to the server
         **/
        public void Write(string command)
        {
            lock (lockObj)
            {
                //convert the command string to an array of bytes and sent to the server
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                this.stream.Write(buffer, 0, buffer.Length);
                this.stream.Flush();
            }
        }

    }
}

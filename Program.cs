using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;






namespace instrumentBE_NET_Framework
{
    internal class Program
    {
        static void Main(string[] args)

        {
            bool logging = false;
            bool background = false;

            //TCP Server start


            // make an endpoint for communication:

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 5000);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //bind to endpoint and start server

            server.Bind(endpoint);
            server.Listen(10);

            //output info

            Console.WriteLine("Server started. Waiting for connection..");

            // Keep Connection Open

            while (true)
            {
                //accept connection

                Socket client = server.Accept();
                Console.WriteLine("Client connected.");

                //data received
                byte[] buffer = new byte[1024];
                int bytesReceived = client.Receive(buffer);

                string commandReceived = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                Console.WriteLine("Received: " + commandReceived);

                string commandResponse = SerialCommand("COM3", commandReceived);

                // log data received

                if (logging == true && commandReceived != "EMPTY")
                {
                    using (StreamWriter writer = new StreamWriter("log.txt", true))
                    {
                        writer.WriteLine("[" + DateTime.Now + "] " + "Received:" + commandReceived);
                    }
                }

                // return received data to server

                if (commandReceived != "")
                {
                    client.Send(Encoding.ASCII.GetBytes(commandResponse));
                }

                else
                {
                    client.Send(Encoding.ASCII.GetBytes("Empty message recieved"));
                }
            }




            // define port 

            





            /*

            while (serialPort.IsOpen ) 
            
            {
                // get user input

                Console.WriteLine("Enter message:");

                string serialMessage = Console.ReadLine();

                // send user input to arduino

                serialPort.WriteLine(serialMessage);

                // get response

                Console.WriteLine("Message sendt. Waiting for response");
                string serialResponse = serialPort.ReadLine();

                Console.WriteLine("Arduino response: " + serialResponse);

                // flush memory

                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                


                // close port

                if (serialMessage == "close")
                {
                    serialPort.Close();
                }


            }
            */
        }

        static string SerialCommand(string portName, string command)
        {
            int baudRate = 9600;
            SerialPort serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            Console.WriteLine("Connected to arduino. Write close to disconnect");
            serialPort.WriteLine("readscaled");
            string serialResponse = serialPort.ReadLine();

            Console.WriteLine(serialResponse);
            Console.ReadKey();
            serialPort.Close();

            return serialResponse;

        }
    }
}

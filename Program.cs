using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;
using System.IO;

//example: writeconf>password>arduinoSensor;0.0;400.0;35;350

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

                string commandReceivedFE = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                Console.WriteLine("Received: " + commandReceivedFE);

                

                string serialResponse = SerialCommand("COM3", commandReceivedFE);

                // log data received

                if (logging)
                {
                    using (StreamWriter writer = new StreamWriter("log.txt", true))
                    {
                        writer.WriteLine("[" + DateTime.Now + "] " + "Received:" + commandReceivedFE);
                    }
                }

                // return received data to server

                

                if (commandReceivedFE != "")
                {
                    client.Send(Encoding.ASCII.GetBytes(serialResponse));
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

            try
            {
                serialPort.Open();

            }
            catch (IOException e)
            {
                
                Console.WriteLine("No connection found on USB port");
                serialPort.Close();
                return "No connection found on USB port";

            }

            catch (UnauthorizedAccessException e)
            {

                Console.WriteLine("Unauthorized Access to USB port");
                serialPort.Close();
                return "No connection found on USB port";

            }



            Console.WriteLine("Connected to arduino. Write close to disconnect");
            serialPort.WriteLine(command);
            string serialResponse = serialPort.ReadLine();

            Console.WriteLine(serialResponse);
           
            serialPort.Close();

            return serialResponse;

        }
        
        
    }
}

using System;
using System.Runtime.InteropServices;
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
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {

            // Arguments

            bool logging = false;
            int com = 3;
            bool background = false;
            int port = 5000;
            bool skipSetup = false;

            // Setup Variables

            bool settingsUpdatedInArgs = false;

            // Check for command line arguments

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-logging"))
                {
                    if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool result))
                    {
                        logging = result;
                        settingsUpdatedInArgs = true;
                    }
                }
                else if (args[i].Equals("-com"))
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int result))
                    {
                        com = result;
                        settingsUpdatedInArgs = true;
                    }
                }
                else if (args[i].Equals("-background"))
                {
                    if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool result))
                    {
                        background = result;
                        skipSetup = result;
                        settingsUpdatedInArgs = true;
                    }
                }
                else if (args[i].Equals("-port"))
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int result))
                    {
                        port = result;
                        settingsUpdatedInArgs = true;
                    }
                }
                else if (args[i].Equals("-skipSetup"))
                {
                    if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool result))
                    {
                        skipSetup = result;
                        
                    }
                }
            }

            // Feedback on commandline arguments used

            if (settingsUpdatedInArgs == true)
            {
                Console.WriteLine("\nSettings updated from command line arguments");
            }

            else if (skipSetup == true)
            {
                Console.WriteLine("\nSetup procedure skipped");
            }
            else
            {
                Console.WriteLine("No command line arguments used on startup");
            }

            // User Setup

            bool userSetup = false;
            bool userIsAskedWithChangingSettings = true;

            if (skipSetup == true)
            {
                userIsAskedWithChangingSettings = false;
            }



            // Give user feedback on settings

            Console.WriteLine("\r\n"+"------ Settings -------");
            Console.WriteLine("Logging: " + logging);
            Console.WriteLine("Running in background?: " + background);
            Console.WriteLine("COM port: " + com);
            Console.WriteLine("Port: " + port);
            Console.WriteLine("-----------------------");

            // Ask if user want to change settings

            while (userIsAskedWithChangingSettings)
            {
                Console.WriteLine("Do you want to change these settings for this session? Y/N");
                string userChangeSettings = Console.ReadLine();

                if (userChangeSettings == "Y" || userChangeSettings == "y")
                {
                    userSetup = true;
                    userIsAskedWithChangingSettings = false;
                }
                
                else if (userChangeSettings == "N" || userChangeSettings == "n")
                {
                    userSetup = false;
                    userIsAskedWithChangingSettings = false;
                }

                else
                {
                    Console.WriteLine("Invalid input. Please enter Y or N.");
                }
            }

            // User setup

            while (userSetup)
            {
                // Select what settings to change

                Console.WriteLine("Which settings do you want to change? 1: Logging, 2: Background, 3: COM port, 4: Port");
                string userSettingsToChange = Console.ReadLine();
               
                // Change logging settings

                if (userSettingsToChange == "1")
                {
                    Console.WriteLine("Do you want to enable logging? Y/N");
                    string userLogging = Console.ReadLine();
                    
                    if (userLogging == "Y" || userLogging == "y")
                    {
                        logging = true;
                        Console.WriteLine("Logging enabled");
                    }
                    else if (userLogging == "N" || userLogging == "n")
                    {
                        logging = false;
                        Console.WriteLine("Logging disabled");
                    }

                    else
                    {
                        Console.WriteLine("Invalid input. Settings not changed.");
                    }
                }

                // Change background settings

                else if (userSettingsToChange == "2")
                {
                    
                    Console.WriteLine("Do you want to run in background? Y/N");
                    string userBackground = Console.ReadLine();
                    
                    if (userBackground == "Y" || userBackground == "y")
                    {
                        background = true;
                        Console.WriteLine("Running in background");
                    }
                    
                    else if (userBackground == "N" || userBackground == "n")
                    {
                        background = false;
                        Console.WriteLine("Running in foreground");
                    }
                    
                    else
                    {
                        Console.WriteLine("Invalid input. Settings not changed.");
                    }
                }

                // Change COM port settings


                else if (userSettingsToChange == "3")
                {

                    bool userChangeComSettings = true;

                    while (userChangeComSettings)
                    {
                        Console.WriteLine("Which COM port do you want to use?");
                        string userCom = Console.ReadLine();

                        try
                        {
                            int userComInt = int.Parse(userCom);
                            com = userComInt;
                            Console.WriteLine("COM port changed to " + com + "!");
                            userChangeComSettings = false;
                        }
                        catch (FormatException e)
                        {
                           
                            Console.WriteLine("Invalid input. Please enter an integer value for the COM port.");
                        }
                    }
                }

                // Change port settings

                else if (userSettingsToChange == "4")
                {

                    bool userChangePortSettings = true;

                    while (userChangePortSettings)
                    {
                        Console.WriteLine("Which Port do you want to use?");
                        string userPort = Console.ReadLine();

                        try
                        {
                            int userPortInt = int.Parse(userPort);
                            port = userPortInt;
                            Console.WriteLine("Port changed to " + port + "!");
                            userChangePortSettings = false;
                        }
                        catch (FormatException e)
                        {

                            Console.WriteLine("Invalid input. Please enter an integer value for the Port.");
                        }
                    }
                }

                // Invalid input

                else
                {
                    Console.WriteLine("Invalid input");
                }

                // Ask user if they want to see the updated settings

                Console.WriteLine("Do you want to see the updated settings? Y/N");
                string userSeeUpdatedSettings = Console.ReadLine();

                if (userSeeUpdatedSettings == "Y" || userSeeUpdatedSettings == "y")
                {
                 

                    Console.WriteLine("\r\n" + "------ Settings -------");
                    Console.WriteLine("Logging: " + logging);
                    Console.WriteLine("Running in background?: " + background);
                    Console.WriteLine("COM port: " + com);
                    Console.WriteLine("Port: " + port);
                    Console.WriteLine("-----------------------");
                }

                // Ask if user wants to change more settings

                Console.WriteLine("Do you want to change more settings? Y/N");
                string userChangeMoreSettings = Console.ReadLine();

                if (userChangeMoreSettings == "Y" || userChangeMoreSettings == "y")
                {
                    userSetup = true;
                }
                else
                {
                    userSetup = false;
                }
            }

            // If background is true, hide the console window
            if (background)
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, 0); // Hide
            }

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

                // choose userinput for COM port

                string COMstring = "COM" + com;

                string serialResponse = SerialCommand(COMstring, commandReceivedFE);


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
                    try 
                    {
                        client.Send(Encoding.ASCII.GetBytes(serialResponse));
                    }



                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
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

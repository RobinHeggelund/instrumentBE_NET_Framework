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
        // Hide console window
        
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {

            // Arguments

            bool logging = false;
            bool background = false;
            int TCPPort = 5000;
            IPAddress databaseIP = null;
            bool skipSetup = false;
            bool autoConfigure = false;

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

                else if (args[i].Equals("-background"))
                {
                    if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool result))
                    {
                        background = result;
                        skipSetup = result;
                        settingsUpdatedInArgs = true;
                    }
                }
                else if (args[i].Equals("-TCPPort"))
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int result))
                    {
                        TCPPort = result;
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

                else if (args[i].Equals("-autoConfigure"))
                {
                    if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool result))
                    {
                        autoConfigure = result;

                    }
                }

                else if (args[i].Equals("-IP"))
                {
                    if (i + 1 < args.Length && IPAddress.TryParse(args[i + 1], out IPAddress result))
                    {
                        databaseIP = result;
                        settingsUpdatedInArgs = true;
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

            Console.WriteLine("\r\n"+"------------------ Settings -------------------");
            Console.WriteLine("Logging?                  -logging       " + logging);
            Console.WriteLine("Running in background?    -background    " + background);
            Console.WriteLine("Database IP:              -IP            " + databaseIP);
            Console.WriteLine("TCP Port                  -TCPPort       " + TCPPort);
            Console.WriteLine("Skip Setup?               -skipSetup     " + skipSetup);
            Console.WriteLine("Auto Configure?           -autoConfigure " + autoConfigure);
            Console.WriteLine("-----------------------------------------------");

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

                Console.WriteLine("Which settings do you want to change? 1: Logging, 2: Background, 3: Database IP, 4: TCP-Port, 5: Auto Configure");
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

                // Change database IP

                else if (userSettingsToChange == "3")
                {

                    bool userChangePortSettings = true;

                    while (userChangePortSettings)
                    {
                        Console.WriteLine("Which IP do you want to use?");
                        string userIP = Console.ReadLine();

                        try
                        {
                            IPAddress userIPConvert = IPAddress.Parse(userIP);
                            databaseIP = userIPConvert;
                            Console.WriteLine("database IP changed to " + databaseIP + "!");
                            userChangePortSettings = false;
                        }
                        catch (FormatException e)
                        {

                            Console.WriteLine("Invalid input. Please enter an IP address.");
                        }
                    }
                }



                // Change TCP port settings

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
                            TCPPort = userPortInt;
                            Console.WriteLine("TCP Port changed to " + TCPPort + "!");
                            userChangePortSettings = false;
                        }
                        catch (FormatException e)
                        {

                            Console.WriteLine("Invalid input. Please enter an integer value for the TCP Port.");
                        }
                    }
                }

                // Change auto configure settings

                else if (userSettingsToChange == "5")
                {

                    Console.WriteLine("Do you want program to auto configure instrument with default settings? Y/N");
                    string userAutoConfigure = Console.ReadLine();

                    if (userAutoConfigure == "Y" || userAutoConfigure == "y")
                    {
                        autoConfigure = true;
                        Console.WriteLine("Auto configure enabled");
                    }

                    else if (userAutoConfigure == "N" || userAutoConfigure == "n")
                    {
                        autoConfigure = false;
                        Console.WriteLine("Auto configure disabled");
                    }

                    else
                    {
                        Console.WriteLine("Invalid input. Settings not changed.");
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

                    // Give user feedback on settings

                    Console.WriteLine("\r\n" + "------------------ Settings -------------------");
                    Console.WriteLine("Logging?                  -logging       " + logging);
                    Console.WriteLine("Running in background?    -background    " + background);
                    Console.WriteLine("Database IP:              -IP            " + databaseIP);
                    Console.WriteLine("TCP Port                  -TCPPort       " + TCPPort);
                    Console.WriteLine("Skip Setup?               -skipSetup     " + skipSetup);
                    Console.WriteLine("Auto Configure?           -autoConfigure " + autoConfigure);
                    Console.WriteLine("-----------------------------------------------");
                }

                // Ask if user wants to change more settings

                Console.WriteLine("Do you want to change more settings? Y/N");
                string userChangeMoreSettings = Console.ReadLine();

                if (userChangeMoreSettings == "Y" || userChangeMoreSettings == "y")
                {
                    userSetup = true;
                }
                else if (userChangeMoreSettings == "N" || userChangeMoreSettings == "n")
                {
                    userSetup = false;
                }

                else
                {
                    userSetup = true;
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

            try
            {
                server.Bind(endpoint);
                server.Listen(10);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error trying to Bind to endpoint. Another instance of instrumentBE running?");
            }


            if (autoConfigure)
            {

                //Write default config and get response from instrument

                string serialFirstResponse = SerialCommand("COM3", "writeconf>password>arduinoSensor;0.0;400.0;35;350", logging, 9600);

                string[] serialFirstResponseArray = serialFirstResponse.Split(';');

                if (serialFirstResponseArray[0] == "writeconf")
                {
                    int configured = int.Parse(serialFirstResponseArray[1]);

                    if (configured == 1)
                    {
                        Console.WriteLine("\r\nConnection with instrument established");
                    }
                    else
                    {
                        Console.WriteLine("\r\nError: Connected to instrument but could not configure");
                    }
                }
                else
                {
                    Console.WriteLine("\r\nError: Could not connect to instrument");
                }

            }


            Console.WriteLine("Server open. Waiting for connection from FE..");

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

                if (commandReceivedFE != "ping")
                {
                    // Split command

                    string[] commandRecievedFESplit = commandReceivedFE.Split(',');

                    // Convert command

                    string commandRecievedFEcommand = commandRecievedFESplit[0];
                    string commandReceivedFECOM = "COM"+commandRecievedFESplit[1];
                    int commandReceivedFEbaudRate = int.Parse(commandRecievedFESplit[2]);


                    // Get response from instrument and send to FE

                    string serialResponse = SerialCommand(commandReceivedFECOM, commandRecievedFEcommand, logging, commandReceivedFEbaudRate);

                    // Return error

                    if (serialResponse == "No connection found on USB port")

                    { try
                        {
                            client.Send(Encoding.ASCII.GetBytes("error;USB"));
                        }

                    catch (Exception e)
                        {
                            Console.WriteLine("Error sending data to FE");
                        }
                    }
                    
                    // Return Response

                    try
                    {
                        client.Send(Encoding.ASCII.GetBytes(serialResponse));
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Error sending data to FE");
                    }

                    // log to file

                    if (logging)
                    {
                        using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\log.txt", true))
                        {
                            writer.WriteLine("[" + DateTime.Now + "] " + "From FE:" + commandReceivedFE);
                        }
                    }
                }

                else if (commandReceivedFE == "ping")
                {
                    client.Send(Encoding.ASCII.GetBytes("ping"));
                }



                // log data received

                if (logging)
                {
                    using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory+"\\log.txt", true))
                    {
                        writer.WriteLine("[" + DateTime.Now + "] " + "From FE:" + commandReceivedFE);
                    }
                }
                
            }

        }

        // Send command to instrument

        static string SerialCommand(string portName, string command, bool logging, int baudRate)
        {
            
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

            Console.WriteLine("Command sendt to instrument: " + command );
            serialPort.WriteLine(command);
            try
            {
                
                string serialResponseInternal = serialPort.ReadLine();

                Console.WriteLine("Command recieved from instrument: " + serialResponseInternal);

                if (logging)
                {
                    using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\log.txt", true))
                    {
                        writer.WriteLine("[" + DateTime.Now + "] " + "From Instrument:" + serialResponseInternal);
                    }
                }

                serialPort.Close();

                return serialResponseInternal;
            }
            catch (System.Net.Sockets.SocketException ex)

            {
                Console.WriteLine("No response from instrument");
                serialPort.Close();
                return "No response from instrument";
            }

            

        }
        
        
    }
}

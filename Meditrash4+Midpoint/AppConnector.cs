using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace Meditrash4_Midpoint
{
    class AppConnector
    {
        TcpListener listener;
        Thread serverThread;
        static RNGCryptoServiceProvider rg;
        MySqlHandle mySqlHandle;
        List<uint> resevedKeys;
        //List<KeyValuePair<uint, string>> registeredList;
        public AppConnector(MySqlHandle _mySqlHandle)
        {
            this.mySqlHandle = _mySqlHandle;
            listener = new TcpListener(16246);
            listener.Start();
            //registeredList = new List<KeyValuePair<uint, string>>();
            resevedKeys = new List<uint>();
            rg = new RNGCryptoServiceProvider();

            //listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback),listener);
            serverThread = new Thread(()=> {
                while (true) // Add your exit flag here
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        ThreadPool.QueueUserWorkItem(ProccesConnection, client);
                    }
                    catch (SocketException e)
                    {
                        break;
                    }
                }
            });
            serverThread.Start();

        }
        public static void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;
            Console.WriteLine(listener.ToString());
           

            // End the operation and display the received data on
            // the console.
            TcpClient client = listener.EndAcceptTcpClient(ar);
            NetworkStream  networkStream = client.GetStream();
            
            
            MemoryStream memoryStream = new MemoryStream();
            networkStream.CopyTo(memoryStream);
            String message = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            try
            {
                XDocument doc = XDocument.Parse(message);
                Console.WriteLine(doc);
            }
            catch (Exception e)
            {
                Console.WriteLine("parse error: " + e.Message);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            Console.WriteLine("Client connected completed");
        }

        private void ProccesConnection(Object stateInflo)
        {
            TcpClient client = (TcpClient)stateInflo;
            Console.WriteLine(client.Client.RemoteEndPoint.ToString());
            NetworkStream networkStream = client.GetStream();
            String message = "";
            try
            {
                message = read(networkStream, 5000);
                XDocument doc = XDocument.Parse(message);
                switch (doc.Root.Name.LocalName)
                {
                    case "Login":
                        uint key = processLogin(doc);

                        XElement body = new XElement("Login",
                            new XElement("uniqueToken", key.ToString("X"))
                            );
                        Console.WriteLine(body);
                        networkStream.Write(
                            System.Text.Encoding.UTF8.GetBytes(body.ToString()),
                            0,
                            body.ToString().Length);
                        break;
                    case "Request":




                    default:
                        break;
                }
                Console.WriteLine(doc);
            }
            
            catch (Exception e)
            {
                Console.WriteLine("parse error: " + e.Message);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            client.Close();
            Console.WriteLine("Client connected completed");
        }

        private uint processLogin(XDocument doc)
        {
            XElement name = doc.Root.Element("name");
            XElement password = doc.Root.Element("password");

            String testName = MySqlHelper.EscapeString(name.Value);
            List<User> users = mySqlHandle.GetObjectList<User>("name=" + "'" + testName + "'", 2);
            if (users.Count == 0)
            {
                return 0;
            }
            

            if (password.Value == "password")
            {
                uint newRandKey = 0;
                while (newRandKey==0)
                {
                    byte[] rno = new byte[5];
                    rg.GetBytes(rno);
                    uint randomvalue = BitConverter.ToUInt32(rno, 0);
                    if (!resevedKeys.Contains(randomvalue)) 
                    {
                        newRandKey = randomvalue;
                    }
                }
                return newRandKey;
            }
            return 0U;
        }

        public void stop()
        {      
            listener.Stop();
        }
        ~AppConnector()
        {
            listener.Stop();
        }


        private static string read(NetworkStream networkStream, long timeout = 3000)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while (!networkStream.DataAvailable)
            {
                if (sw.ElapsedMilliseconds > timeout) throw new TimeoutException();
            }

            byte[] myReadBuffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = 0;
            do
            {
                numberOfBytesRead = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
            }
            while (networkStream.DataAvailable);
            String message = myCompleteMessage.ToString();
            return message;
        }
    }
}

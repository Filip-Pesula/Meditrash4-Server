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
using Meditrash4_Midpoint.mysqlTables;

namespace Meditrash4_Midpoint
{
    class AppConnector
    {
        TcpListener listener;
        Thread serverThread;
        static RNGCryptoServiceProvider rg;
        MySqlHandle mySqlHandle;
        List<uint> resevedKeys;
        List<KeyValuePair<uint, User>> registeredList;
        
        //List<KeyValuePair<uint, string>> registeredList;
        public AppConnector(MySqlHandle _mySqlHandle)
        {
            this.mySqlHandle = _mySqlHandle;
            listener = new TcpListener(16246);
            listener.Start();
            registeredList = new List<KeyValuePair<uint, User>>();
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
                        {
                            XElement name = doc.Root.Element("name");
                            XElement password = doc.Root.Element("password");

                            String testName = MySqlHelper.EscapeString(name.Value);
                            List<User> users = mySqlHandle.GetObjectList<User>("name=" + "'" + testName + "'", 2);
                            if (users.Count == 0)
                            {
                                return;
                            }
                            User user = users[0];
                            uint key = processLogin(doc, user);
                            XElement body = null;
                            if (key != 0)
                            {
                                body = new XElement("Login",
                                    new XElement("uniqueToken", key.ToString("X")),
                                    new XElement("firstName", user.firstName),
                                    new XElement("lastName", user.lastName),
                                    new XElement("rights", user.rights)
                                    );
                            }
                            else
                            {
                                body = new XElement("Login",
                                   new XElement("uniqueToken", key.ToString("0"))
                                   );
                            }
                            Console.WriteLine(body);
                            networkStream.Write(
                                System.Text.Encoding.UTF8.GetBytes(body.ToString()),
                                0,
                                body.ToString().Length);
                            break;
                        }
                    case "Request":
                        {
                            XElement keyX = doc.Root.Element("uniqueToken");
                            uint key = uint.Parse(keyX.Value,System.Globalization.NumberStyles.HexNumber);
                            User user = null;
                            foreach(KeyValuePair<uint,User> userPair in registeredList)
                            {
                                if(userPair.Key == key)
                                {
                                    user = userPair.Value;
                                }
                            }
                            XElement response = null;
                            if(user == null)
                            {
                                response = new XElement("RequestError",
                                        "wrong Login"
                                    );
                            }
                            else
                            {
                                response = processRequest(doc, user);
                            }
                            networkStream.Write(
                                System.Text.Encoding.UTF8.GetBytes(response.ToString()),
                                0,
                                response.ToString().Length);
                            break;
                        }
                    default:
                        break;
                }
                Console.WriteLine(doc);
            }
            
            catch (Exception e)
            {
                Logger.LogE("parse error:", e);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            client.Close();
            Console.WriteLine("Client connected completed");
        }

        private XElement processRequest(XDocument doc, User opUser)
        {
            XElement response;
            XElement requestCommand = doc.Root.Element("requestCommand");
            XAttribute commandName = requestCommand.Attribute("name");
            switch (commandName.Value)
            {   
                case "addUser":
                    if (opUser.rights < 2)
                    {
                        return genIncorrectResponse("notPermitted", "not Permitted to this operation");
                    }
                    try
                    {
                        string department = requestCommand.Element("department").Value;
                        List<Department> departments = mySqlHandle.GetObjectList<Department>("name='" + department + "'");
                        if(departments.Count == 0)
                        {
                            return genIncorrectResponse("incorrectName", "department has incorrect name");
                        }
                        User user = new User(
                            requestCommand.Element("name").Value,
                            requestCommand.Element("password").Value,
                            Int32.Parse(requestCommand.Element("rodCislo").Value),
                            departments[0].id,
                            Int32.Parse(requestCommand.Element("rights").Value),
                            requestCommand.Element("firstName").Value,
                            requestCommand.Element("lastName").Value
                            );
                        mySqlHandle.saveObject(user);
                        return new XElement("Request", "user was Added");
                    }
                    catch(Exception ex)
                    {
                        return genIncorrectResponse("addingError","could not add user");
                    }
                    break;
                case "addDepartment":
                    if (opUser.rights < 2)
                    {
                        return genIncorrectResponse("notPermitted", "not Permitted to this operation");
                    }
                    try
                    {
                        Department department = new Department(requestCommand.Element("name").Value);
                        mySqlHandle.saveObject(department);
                        return new XElement("Request", "department was Added");
                    }
                    catch (Exception ex)
                    {
                        return genIncorrectResponse("addingError", "could not add department");
                    }
                    break;

            }
            return genIncorrectResponse("UnknownCommand", "Unknown Command");
        }
        private XElement genIncorrectResponse(string errorType,string message)
        {
            XElement response = new XElement("RequestError", message);
            response.SetAttributeValue("type", errorType);
            return response;
        }
        private uint processLogin(XDocument doc, User user)
        {
            XElement name = doc.Root.Element("name");
            XElement password = doc.Root.Element("password");

            String testName = MySqlHelper.EscapeString(name.Value);
            List<User> users = mySqlHandle.GetObjectList<User>("name=" + "'" + testName + "'", 2);
            if (users.Count == 0)
            {
                return 0;
            }
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(password.Value));

            if (hash.SequenceEqual(user.password))
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
                registeredList.Add(new KeyValuePair<uint, User>(newRandKey,user));
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

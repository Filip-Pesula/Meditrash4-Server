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
        List<UserLogin> registeredList;
        
        //List<KeyValuePair<uint, string>> registeredList;
        public AppConnector(MySqlHandle _mySqlHandle)
        {
            this.mySqlHandle = _mySqlHandle;
            listener = new TcpListener(16246);
            listener.Start();
            registeredList = new List<UserLogin>();
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
            Logger.Log(client.Client.RemoteEndPoint.ToString(), ConsoleColor.Cyan);

            NetworkStream networkStream = client.GetStream();
            String message = "";
            try
            {
                message = read(networkStream, 5000);
                XDocument doc = XDocument.Parse(message);
                Logger.Log(doc.ToString(),ConsoleColor.DarkBlue);
               
                switch (doc.Root.Name.LocalName)
                {
                    case "Login":
                        {

                            XElement name = doc.Root.Element("name");
                            XElement password = doc.Root.Element("password");

                            List<User> users = mySqlHandle.GetObjectList<User>(
                                "name= BINARY @name",
                                new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                                        { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "@name",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.VarChar,name.Value))},
                                2);
                            XElement body = null;
                            

                            if (users.Count == 0)
                            {
                                body = new XElement("Login",
                                    new XElement("uniqueToken", "0"));
                            }
                   
                            else
                            {
                                User user = users[0];
                                uint key = processLogin(doc, user);
                                clearTokens(key);
                                List<Department> departments = mySqlHandle.GetObjectList<Department>(
                                    "uid= @uid",
                                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                                        { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "@uid",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.Int32,users[0].department_id))});
                                if (departments.Count == 0)
                                {
                                    departments.Add(new Department(""));
                                }

                                if (key != 0)
                                {
                                    body = new XElement("Login",
                                        new XElement("uniqueToken", key.ToString("X")),
                                        new XElement("firstName", user.firstName),
                                        new XElement("lastName", user.lastName),
                                        new XElement("userName", user.userName),
                                        new XElement("department", departments[0].name),
                                        new XElement("rodCislo", user.rod_cislo),
                                        new XElement("rights", user.rights)
                                        );
                                }
                                else
                                {
                                    body = new XElement("Login",
                                        new XElement("uniqueToken", "0"));
                                }
                            }
                                
                            XDocument rootDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null), body);
                            Logger.Log(rootDoc.ToString(), ConsoleColor.Magenta);
                            byte[] ansB = System.Text.Encoding.UTF8.GetBytes(rootDoc.ToString());
                            networkStream.Write(
                                ansB,
                                0,
                                ansB.Length);
                            break;
                        }
                    case "Request":
                        {
                            XElement keyX = doc.Root.Element("uniqueToken");
                            uint key = uint.Parse(keyX.Value,System.Globalization.NumberStyles.HexNumber);
                            User user = null;
                            foreach(UserLogin userPair in registeredList)
                            {
                                if(userPair.token == key)
                                {
                                    user = userPair.user;
                                }
                            }
                            XElement response = null;
                            if(user == null)
                            {
                                response = new XElement("RequestError",
                                        "wrong Login");
                            }
                            else
                            {
                                response = processRequest(doc, user);
                            }
                            XDocument rootDoc = new XDocument(new XDeclaration("1.0","UTF-8",null), response);
                            Logger.Log(rootDoc.ToString(), ConsoleColor.Magenta);
                            byte[] ansB = System.Text.Encoding.UTF8.GetBytes(rootDoc.ToString());
                            networkStream.Write(
                                ansB,
                                0,
                                ansB.Length);
                            break;
                        }
                    default:
                        break;
                }
            }
            
            catch (Exception e)
            {
                Logger.LogE("parse error:", e);
                Logger.Log(message, ConsoleColor.Blue);
            }

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            client.Close();
            Logger.Log("Client connected completed", ConsoleColor.Green);
           
        }

        private XElement processRequest(XDocument doc, User opUser)
        {
            XElement requestCommand = doc.Root.Element("requestCommand");
            XAttribute commandName = requestCommand.Attribute("name");
            switch (commandName.Value)
            {   
                case "addUser":
                    return ReqResolver.addUser(requestCommand,opUser,mySqlHandle);
                    break;
                case "removeUser":
                    return ReqResolver.removeUser(requestCommand,opUser,mySqlHandle);
                    break;
                case "editPassword":
                    return ReqResolver.editPassword(requestCommand, opUser, mySqlHandle);
                    break;
                case "addDepartment":
                    return ReqResolver.addDepartment(requestCommand, opUser, mySqlHandle);
                    break;
                case "removeDepartment":
                    return ReqResolver.removeDepartment(requestCommand, opUser, mySqlHandle);
                    break;
                case "getDepartments":
                    return ReqResolver.getDepartments(requestCommand, opUser, mySqlHandle);
                    break;
                //only extention
                case "addCathegory":
                    return  ReqResolver.addCathegory(requestCommand, opUser, mySqlHandle);
                    break;  
                case "removeCathegory":
                    return  ReqResolver.removeCathegory(requestCommand, opUser, mySqlHandle);
                    break;
                case "getCathegories":
                    return ReqResolver.getCathegories(requestCommand, opUser, mySqlHandle);
                    break;
                case "addItem":
                    return ReqResolver.addItem(requestCommand, opUser, mySqlHandle);
                    break;
                case "removeItem":
                    return ReqResolver.removeItem(requestCommand, opUser, mySqlHandle);
                    break;
                case "getItems":
                    return ReqResolver.getItems(requestCommand, opUser, mySqlHandle);
                    break;
                case "addFavItem":
                    return ReqResolver.addFavItem(requestCommand, opUser, mySqlHandle);
                    break;
                case "removeFavItem":
                    return ReqResolver.removeFavItem(requestCommand, opUser, mySqlHandle);
                    break;
                case "getFavList":
                    return ReqResolver.getFavList(requestCommand, opUser, mySqlHandle);
                    break;
                case "trashItem":
                    return ReqResolver.trashItem(requestCommand, opUser, mySqlHandle);
                    break;
                case "removeRecord":
                    return ReqResolver.removeRecord(requestCommand, opUser, mySqlHandle);
                    break;
                case "getTrashItem":
                    return ReqResolver.getTrashItem(requestCommand, opUser, mySqlHandle);
                    break;
                case "addRespPerson":
                    return ReqResolver.addRespPerson(requestCommand, opUser, mySqlHandle);
                    break;
                //TODO
                case "exportTrashByCathegory":
                    return ReqResolver.exportTrashByCathegory(requestCommand, opUser, mySqlHandle);
                    break;
            }
            return ReqResolver.genIncorrectResponse(commandName.Value,"UnknownCommand", "Unknown Command");
        }
        private uint processLogin(XDocument doc, User user)
        {
            XElement name = doc.Root.Element("name");
            XElement password = doc.Root.Element("password");

            String testName = MySqlHelper.EscapeString(name.Value);
            List<User> users = mySqlHandle.GetObjectList<User>("name = @name",
                new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                                        { new(
                                            "@name",
                                            new(MySqlDbType.VarChar,name.Value))},
                2);
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
                registeredList.Add(new UserLogin(newRandKey,user, DateTime.Now));
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
        private void clearTokens(uint token)
        {
            for(int i = 0; i < registeredList.Count; i++)
            {
                if(registeredList[i].token == token)
                {
                    registeredList[i].timeStamp = DateTime.Now;
                }
                if(registeredList[i].timeStamp < DateTime.Now.AddMinutes(-30))
                {
                    registeredList.RemoveAt(i);
                }
            }
        }
        private User getUserByKey(uint key)
        {
            User user = null;
            foreach (UserLogin userPair in registeredList)
            {
                if (userPair.token == key)
                {
                    user = userPair.user;
                }
            }
            return user;
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

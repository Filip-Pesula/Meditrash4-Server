using Meditrash4_Midpoint.mysqlTables;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Meditrash4_Midpoint
{
    internal class ReqResolver
    {
        public static XElement genIncorrectResponse(string reqCommand, string errorType, string message)
        {
            XElement requestCommand = new XElement("requestCommand");
            requestCommand.SetAttributeValue("name", reqCommand);
            XElement response = new XElement("RequestError", requestCommand, new XElement("Message", message));
            response.SetAttributeValue("type", errorType);
            return response;
        }
        public static XElement addUser(XElement requestCommand,User opUser,MySqlHandle mySqlHandle)
        {
            if (opUser.rights < 2)
            {
                return genIncorrectResponse("addUser", "notPermitted", "not Permitted to this operation");
            }
            try
            {
                string department = requestCommand.Element("department").Value;
                List<Department> departments = mySqlHandle.GetObjectList<Department>("name= @name",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                                { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "@name",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.VarChar,department))});
                if (departments.Count == 0)
                {
                    return genIncorrectResponse("addUser", "incorrectName", "department has incorrect name");
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

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "addUser");

                return new XElement("Request", rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("addUser", "addingError", "could not add user");
            }
        }
        public static XElement removeUser(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            if (opUser.rights < 2)
            {
                return genIncorrectResponse("removeUser", "notPermitted", "not Permitted to this operation");
            }
            XElement nameEl = requestCommand.Element("name");
            if (nameEl == null)
            {
                return genIncorrectResponse("removeUser", "missingArgument", "missing name");
            }
            String name = nameEl.Value;
            try
            {
                User user = mySqlHandle.GetObjectList<User>(
                    "name = @name",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "@name",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.VarChar,name))})[0];
                mySqlHandle.removeObject(user);

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "removeUser");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "user was Removed"));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("removeUser", "removingError", "could not add department");
            }
        }
        public static XElement editPassword(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {

                mySqlHandle.setObjectParam<User>(
                    opUser, 
                    "passwd", 
                    MySqlDbType.Binary, 
                    SHA256.HashData(
                        Encoding.UTF8.GetBytes(
                            requestCommand.Element("password").Value)
                        )
                    );
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "editPassword");

                return new XElement("Request", rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("addUser", "addingError", "could not add user");
            }
        }
        public static XElement addDepartment(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            if (opUser.rights < 2)
            {
                return genIncorrectResponse("addDepartment", "notPermitted", "not Permitted to this operation");
            }
            try
            {
                Department department = new Department(requestCommand.Element("name").Value);
                mySqlHandle.saveObject(department);

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "addDepartment");
                return new XElement("Request", 
                    rootRes, 
                    new XElement("Message", "department was Added"));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("addDepartment", "addingError", "could not add department");
            }
        }
        public static XElement removeDepartment(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            if (opUser.rights < 2)
            {
                return genIncorrectResponse("removeDepartment", "notPermitted", "not Permitted to this operation");
            }
            XElement nameEl = requestCommand.Element("name");
            if (nameEl == null)
            {
                return genIncorrectResponse("removeDepartment", "missingArgument", "missing name");
            }
            String name = nameEl.Value;
            try
            {
                Department department = mySqlHandle.GetObjectList<Department>(
                    "name = @name",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                        "@name",
                                        new KeyValuePair<MySqlDbType, object>(MySqlDbType.VarChar,name))})[0];
                mySqlHandle.removeObject(department);

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "removeDepartment");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "department was Removed"));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("removeDepartment", "removingError", "could not add department");
            }
        }
        public static XElement getDepartments(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                List<Department> departmentList = mySqlHandle.GetObjectList<Department>("true", new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>());
                
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "getDepartments");
                foreach (Department department in departmentList)
                {
                    XElement item = new XElement("deepartment");
                    item.SetAttributeValue("name", department.name);
                    item.SetAttributeValue("id", department.id);
                    rootRes.Add(item);
                }
                return new XElement("Request",
                    rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("getDepartments", "addingError", "could not get department List");
            }
        }
        public static XElement addCathegory(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            string errormsg = "";
            try
            {
                List<Cathegory> cathegories = new List<Cathegory>();
                requestCommand.Elements("cathegory").ToList().ForEach(x => cathegories.Add(
                    new Cathegory(
                        int.Parse(x.Element("id").Value),
                        x.Element("name").Value)
                    ));
                cathegories.ForEach(x =>
                {
                    try
                    {
                        mySqlHandle.saveObject(x);
                    }
                    catch (Exception e)
                    {
                        errormsg += "addError: " + x.name + '\n';
                    }
                });
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "addCathegory");
                return new XElement("Request",
                    rootRes, new XElement("message", "cathegory were added" + errormsg));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("addCathegory", "addingError", "could not add cathegory\n" + errormsg);
            }
        }
        public static XElement removeCathegory(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            if (opUser.rights < 2)
            {
                return genIncorrectResponse("removeDepartment", "notPermitted", "not Permitted to this operation");
            }
            XElement nameEl = requestCommand.Element("id");
            if (nameEl == null)
            {
                return genIncorrectResponse("removeCathegory", "missingArgument", "missing name");
            }
            try
            {
                int id = int.Parse(nameEl.Value);
                Cathegory cathegory = mySqlHandle.GetObjectList<Cathegory>(
                    "id = @id",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "@id",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.VarChar,id))})[0];
                mySqlHandle.removeObject(cathegory);

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "removeCathegory");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "cathegiry was Removed"));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("removeCathegory", "removingError", "could not add department");
            }
        }
        public static XElement getCathegories(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                List<Cathegory> trashList = mySqlHandle.GetObjectList<Cathegory>("true", new());
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "getCathegories");
                foreach (Cathegory cathegory in trashList)
                {
                    XElement item = new XElement("cathegory");
                    item.SetAttributeValue("name", cathegory.name);
                    item.SetAttributeValue("id", cathegory.id);
                    rootRes.Add(item);
                }
                return new XElement("Request",
                    rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("getCathegories", "addingError", "could not add item");
            }
        }
        public static XElement addItem(XElement requestCommand, User opUser, MySqlHandle mySqlHandle) {
            string errormsg = "";
            try
            {
                List<Trash> trash = new List<Trash>();
                requestCommand.Elements("trash").ToList().ForEach(x => trash.Add(
                    new Trash(
                        x.Element("name").Value,
                        int.Parse(x.Element("cathegory").Value),
                        int.Parse(x.Element("weight").Value))
                    ));
                trash.ForEach(x =>
                {
                    try
                    {
                        mySqlHandle.saveObject(x);
                    }
                    catch (Exception e)
                    {
                        errormsg += "addError" + x.name + '\n';
                    }
                });
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "removeDepartment");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "items were added" + errormsg));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("addItem", "addingError", "could not add item\n" + errormsg);
            }
        }
        public static XElement removeItem(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            if (opUser.rights < 2)
            {
                return genIncorrectResponse("removeItem", "notPermitted", "not Permitted to this operation");
            }
            XElement nameEl = requestCommand.Element("id");
            if (nameEl == null)
            {
                return genIncorrectResponse("removeItem", "missingArgument", "missing name");
            }
            try
            {
                int id = int.Parse(nameEl.Value);
                Trash trash = mySqlHandle.GetObjectList<Trash>(
                    "uid = @uid",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "@uid",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.VarChar,id))})[0];
                mySqlHandle.removeObject(trash);

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "removeItem");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "trash was Removed"));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("removeItem", "removingError", "could not add department");
            }
        }
        public static XElement getItems(XElement requestCommand, User opUser, MySqlHandle mySqlHandle) {
            try
            {
                List<Trash> trashList = mySqlHandle.GetObjectList<Trash>("true", new());
                
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "getItems");
                foreach (Trash trash in trashList)
                {
                    XElement item = new XElement("item");
                    item.SetAttributeValue("name", trash.name);
                    item.SetAttributeValue("id", trash.uid);
                    item.SetAttributeValue("cathegory", trash.cathegory);
                    item.SetAttributeValue("weight", trash.weight);
                    rootRes.Add(item);
                }
                return new XElement("Request",
                    rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("getItems", "addingError", "could not add item");
            }
        }
        public static XElement addFavItem(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            string errormsg = "";
            try
            {
                requestCommand.Elements("id").ToList().ForEach(x =>
                {
                    int trashId = int.Parse(x.Value);
                    List<Trash> trash = mySqlHandle.GetObjectList<Trash>("uid = @uid",
                        new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new(
                                            "@uid",
                                            new(MySqlDbType.VarChar,trashId))});
                    TrashFaw trashFaw = new TrashFaw(opUser.rod_cislo, trash[0].uid);
                    mySqlHandle.saveObject(trashFaw);
                    try
                    {
                        mySqlHandle.saveObject(trashFaw);
                    }
                    catch (Exception e)
                    {
                        errormsg += "addError: " + trashFaw.odpad_uid +" " + trashFaw.user_rod_c+ '\n';
                    }
                });
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "addFavItem");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "items were added" + errormsg));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("addFavItem", "addingError", "could not add item" + errormsg);
            }
        }
        public static XElement removeFavItem(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            string errormsg = "";
            try
            {
                requestCommand.Elements("id").ToList().ForEach(x =>
                {
                    int trashId = int.Parse(x.Value);
                    List<TrashFaw> trashFaw = mySqlHandle.GetObjectList<TrashFaw>(
                        "Odpad_uid = @Odpad_uid",
                        new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "@Odpad_uid",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.Int32,trashId))});
                    trashFaw.ForEach(x =>
                    {
                        try
                        {
                            mySqlHandle.removeObject(x);
                        }
                        catch (Exception e)
                        {
                            errormsg += "addError: " + trashFaw[0].odpad_uid + " " + trashFaw[0].user_rod_c + '\n';
                        }
                    });
                });
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "removeFavItem");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "items were removed" + errormsg));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("deleteFavItem", "addingError", "could not remove item" + errormsg);
            }
        }
        public static XElement getFavList(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                List<Trash> trashList = mySqlHandle.GetObjectList<Trash>(
                    "uid in (select Odpad_uid from Odpad_User_Settings where User_rodCislo = @User_rodCislo )",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new(
                                            "@User_rodCislo",
                                            new(MySqlDbType.Int64,opUser.rod_cislo))});

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "getFavList");
                foreach (Trash trash in trashList)
                {
                    XElement item = new XElement("item");
                    item.SetAttributeValue("name", trash.name);
                    item.SetAttributeValue("id", trash.uid);
                    rootRes.Add(item);
                }
                return new XElement("Request",
                    rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("getFavList", "addingError", "could not add item");
            }
        }
        public static XElement trashItem(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                int trashId = int.Parse(requestCommand.Element("id").Value);
                int trashCout = int.Parse(requestCommand.Element("count").Value);
                List<Trash> trash = mySqlHandle.GetObjectList<Trash>(
                    "uid = @uid",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new(
                                            "@uid",
                                            new(MySqlDbType.Int32,trashId))});
                Records record = new Records(trashCout, trash[0].uid, opUser.rod_cislo);
                mySqlHandle.saveObject(record);

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "trashItem");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "record was added"));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("trashItem", "addingError", "could not add record");
            }
        }
        public static XElement removeRecord(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            string errormsg = "";
            try
            {
                requestCommand.Elements("id").ToList().ForEach(x =>
                {
                    int recordId = int.Parse(x.Value);
                    List<Records> record = mySqlHandle.GetObjectList<Records>(
                        "uid = @uid",
                        new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                                            "uid",
                                            new KeyValuePair<MySqlDbType, object>(MySqlDbType.Int32,recordId))});
                    record.ForEach(x =>
                    {
                        try
                        {
                            mySqlHandle.removeObject(x);
                        }
                        catch (Exception e)
                        {
                            errormsg += "addError: " + record[0].uid + '\n';
                        }
                    });
                });
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "removeRecord");
                return new XElement("Request",
                    rootRes,
                    new XElement("Message", "items were removed" + errormsg));
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("removeRecord", "addingError", "could not remove item" + errormsg);
            }
        }
        public static XElement getTrashItem(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                int cathegory = int.Parse(requestCommand.Element("catheory").Value);
                DateTime timeStart = new DateTime(
                    int.Parse(requestCommand.Element("yearStart").Value),
                    int.Parse(requestCommand.Element("monthStart").Value),
                    int.Parse(requestCommand.Element("dayStart").Value));
                DateTime timeEnd = new DateTime(
                    int.Parse(requestCommand.Element("yearEnd").Value),
                    int.Parse(requestCommand.Element("monthEnd").Value),
                    int.Parse(requestCommand.Element("dayEnd").Value));
                var data = mySqlHandle.querry(
                   @"select Records.uid,Rec_Odp_User_Trc.id,Rec_Odp_User_Trc.name,storageDate,amount,Odpad_uid,Rec_Odp.name,Rec_Odp_User.name from Records 
	                                LEFT JOIN Odpad Rec_Odp on Records.Odpad_uid = Rec_Odp.uid 
                                    LEFT JOIN User Rec_Odp_User on Rec_Odp_User.rodCislo = Records.User_rodCislo
                                    LEFT JOIN TrashCathegody Rec_Odp_User_Trc on Rec_Odp_User_Trc.id = Rec_Odp.TrashCathegody_id
		                                where storageDate >= @dateStart AND storageDate <= @dateEnd AND Rec_Odp_User_Trc.id = @catheory"
               , new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>> 
               { 
                   new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                       "@dateStart", 
                       new KeyValuePair<MySqlDbType, object>(MySqlDbType.Date, 
                       timeStart)),
                   new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                       "@dateEnd", 
                       new KeyValuePair<MySqlDbType, object>(MySqlDbType.Date, 
                       timeEnd)),
                    new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>(
                       "@catheory", 
                       new KeyValuePair<MySqlDbType, object>(MySqlDbType.Int32,
                       cathegory)) 
               });
                //time.ToString("yyyy-mm-dd")

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "getTrashItem");
                data.ForEach(row =>
                {
                    XElement item = new XElement("record");
                    item.SetAttributeValue("recordId", row[0].Value);
                    item.SetAttributeValue("cathegoryId", row[1].Value);
                    item.SetAttributeValue("cathegoryName", row[2].Value);
                    item.SetAttributeValue("storageDate", row[3].Value);
                    item.SetAttributeValue("amount", row[4].Value);
                    item.SetAttributeValue("odpadId", row[5].Value);
                    item.SetAttributeValue("odpadName", row[6].Value);
                    item.SetAttributeValue("userName", row[7].Value);
                    rootRes.Add(item);
                });
                return new XElement("Request",rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("getTrashItem", "addingError", "could get records");
            }
        }
        //TODO need check
        public static XElement deleteTrashItem(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                int trashId = int.Parse(requestCommand.Element("id").Value);
                List<Records> records = mySqlHandle.GetObjectList<Records>(
                    "uid = @uid",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>
                            { new(
                                            "@uid",
                                            new(MySqlDbType.Int32,trashId))});
                if (records.Count == 0)
                {
                    return genIncorrectResponse("deleteTrashItem", "removingError", "item does not exist");
                }
                mySqlHandle.removeObject(records[0]);
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "deleteTrashItem");
                return new XElement("Request",rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("deleteTrashItem", "removingError", "could not remove exist");
            }
        }
        public static XElement addRespPerson(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                long ico = long.Parse(requestCommand.Element("ico").Value);
                string name = requestCommand.Element("name").Value;
                string ulice = requestCommand.Element("ulice").Value;
                int cislo_popisne = int.Parse(requestCommand.Element("cislo_popisne").Value);
                string mesto = requestCommand.Element("mesto").Value;
                int psc = int.Parse(requestCommand.Element("psc").Value);
                int zuj = int.Parse(requestCommand.Element("zuj").Value);

                RespPerson respPerson = new RespPerson(ico, name, ulice, cislo_popisne, mesto, psc, zuj);
                mySqlHandle.saveObject(respPerson);
                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "addRespPerson");
                return new XElement("Request", rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("addRespPerson", "addingError", "could not add record");
            }
        }
        //TODO need reshape
        public static XElement exportTrashByCathegory(XElement requestCommand, User opUser, MySqlHandle mySqlHandle)
        {
            try
            {
                XElement respPerson = requestCommand.Element("respPerson");
                int respPersonIco = int.Parse(respPerson.Element("ico").Value);
                List<RespPerson> respPerson1 = mySqlHandle.GetObjectList<RespPerson>(
                    "ico = @ico",
                    new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>>{
                                    new(
                                        "@ico",
                                        new(MySqlDbType.Int64,respPersonIco))}
                );
                if (respPerson1.Count == 0)
                {
                    throw new Exception("Resp person does not exist");
                }
                ExportRecords exportRecords = new ExportRecords(respPerson1[0].ico);
                mySqlHandle.saveObject(exportRecords);
                exportRecords = mySqlHandle.GetObjectList<ExportRecords>("uid = (SELECT MAX(uid) FROM exportrecords)", new())[0];
                requestCommand.Elements("cathegory").ToList().ForEach(cathegory =>
                {
                    int intVal = int.Parse(cathegory.Element("id").Value);
                    mySqlHandle.querry(
                        @"update records
                        SET DeStoreRecords_uid = (SELECT uid FROM exportrecords WHERE uid=(SELECT MAX(uid) FROM exportrecords)) where uid in (select * from (select R.uid  from odpad  LEFT join records R on odpad.uid = R.Odpad_uid where TrashCathegody_id = @cathegory AND DeStoreRecords_uid = null ) AS X);"
                        ,
                        new List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>> { new KeyValuePair<string, KeyValuePair<MySqlDbType, object>>("@cathegory", new KeyValuePair<MySqlDbType, object>(MySqlDbType.Int32, intVal)) });
                });

                XElement rootRes = new XElement("requestCommand");
                rootRes.SetAttributeValue("name", "exportTrashByCathegory");
                return new XElement("Request", rootRes);
            }
            catch (Exception ex)
            {
                Logger.LogE(ex);
                return genIncorrectResponse("exportTrashByCathegory", "addingError", "could not add record");
            }
        }


    }
}

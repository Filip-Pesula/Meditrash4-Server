using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Meditrash4_Midpoint.tests")]

namespace Meditrash4_Midpoint
{
    class MySqlHandle
    {
        private static object connLock = new object();
        MySqlConnection conn;
        public MySqlHandle()
        {
            conn = new MySqlConnection();
        }
        public void connect(ServerSetup sdata)
        {
            conn.Close();
            conn.ConnectionString = sdata.getConnectionString();
            conn.Open();
        }
        public void close()
        {
            conn.Close();
        }
        public bool checkSql()
        {
            /*
            MySqlCommand cmd = new MySqlCommand(querry, conn);
            MySqlDataReader resultReader = cmd.ExecuteReader();
            List<object> returnVals = new List<object>();
            Console.WriteLine("usersFCount" + resultReader.FieldCount);
            if (resultReader.HasRows)
            {
                while (resultReader.Read())
                {
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        Console.WriteLine("usrType: " + resultReader.GetFieldType(i));
                    }
                }
            }
            resultReader.Close();
            */
            return false;
        }
        public List<object> querry(string querry, int max = -1)
        {
            MySqlCommand cmd = new MySqlCommand(querry, conn);
            MySqlDataReader resultReader = cmd.ExecuteReader();
            List<object> returnVals = new List<object>();
            Console.WriteLine("usersFCount" + resultReader.FieldCount);
            if (resultReader.HasRows) {
                while (resultReader.Read())
                {
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        Console.WriteLine("usrType: " + resultReader.GetFieldType(i));
                    }
                }
            }
            resultReader.Close();
            return returnVals;
        }
        public void setObjectParam<T>(T _object, string collum, string value) where T : MysqlReadable
        {
            List<DbVariable> vallist = _object.getObjectData();
            List<int> pkList = _object.getPrimaryIndex();
            string cond = "";
            foreach(int obj in _object.getPrimaryIndex())
            {
                cond += vallist[obj].name+ " '" + MySqlHelper.EscapeString(vallist[obj].value.ToString()) + "' AND";
            }
            value = MySqlHelper.EscapeString(value);
            MySqlCommand cmd = new MySqlCommand("UPDATE " + _object.getMyName() +  " SET " + collum + "='" +value+"'"+ " where " , conn);
            int execution = cmd.ExecuteNonQuery();
        }
        
        public static string genSelectCommandParamQ<T>(T _object) where T : MysqlReadable
        {
            StringBuilder values = new StringBuilder("(");
            List<DbVariable>  dataPairs = _object.getObjectData();
            foreach (DbVariable obj in dataPairs)
            {
                values.Append('@');
                values.Append(obj.name);
                values.Append(',');
            }
            values.Remove(values.Length-1,1);
            values.Append(")");
            return values.ToString();
        }
        public static void fillSelectCommandParamQ<T>(ref MySqlCommand cmd, T _object) where T : MysqlReadable
        {
            List<DbVariable> dataPairs = _object.getObjectData();
            foreach (DbVariable obj in dataPairs)
            {
                cmd.Parameters.Add('@' + obj.name, obj.type).Value = obj.value;
            }
        }
        public void saveObject<T>(T _object) where T : MysqlReadable
        {
            Exception e = null;


            Monitor.Enter(connLock);
            try
            {
                string valText = _object.valueQuerry();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO " + _object.getMyName() + _object.writeQuerry() + " VALUES " + valText + ";", conn);
                fillSelectCommandParamQ(ref cmd, _object);
                int execution = cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                e = ex;
            }
            finally
            {
                Monitor.Exit(connLock);
            }
            
            if (e != null)
            {
                throw e;
            }
        }
        public List<T> GetObjectList<T>(string condition, int max = -1) where T : MysqlReadable, new()
        {
            Exception e = null;
            Monitor.Enter(connLock);
            MySqlDataReader resultReader = null;
            try
            {
                T t = new T();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + t.getMyName() + " WHERE " + condition, conn);
                resultReader = cmd.ExecuteReader();
                List<T> returnVals = new List<T>();

                List<KeyValuePair<string, Type>> typelist = t.getMyTypeList();


                if (resultReader.FieldCount != typelist.Count)
                {
                    throw new UnmatchingTypeListException();
                }
                if (resultReader.HasRows)
                {
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        //TODO předělat na hashmap based on key
                        resultReader.GetName(i);
                        if (typelist[i].Value != resultReader.GetFieldType(i))
                        {
                            throw new UnmatchingTypeListException();
                        }
                    }

                    while (resultReader.Read())
                    {
                        List<Object> data = new List<Object>();
                        for (int i = 0; i < resultReader.FieldCount; i++)
                        {
                            object holderObj = resultReader.GetValue(i);
                            if (resultReader.IsDBNull(i))
                            {
                                holderObj = null;
                            }
                            data.Add(holderObj);
                        }
                        T _object = new T();
                        _object.setMyData(data);
                        returnVals.Add(_object);
                    }
                }
                return returnVals;
            }catch (Exception ex)
            {
                e = ex;
            }
            finally
            {
                if (resultReader != null)
                {
                    resultReader.Close();
                }
                Monitor.Exit(connLock);
            }
            if (e != null)
            {
                throw e;
            }
            return new List<T>();
        }
        public void reset()
        {
            MySqlCommand cmd = new MySqlCommand("drop schema if exists " + MySqlHelper.EscapeString(conn.Database), conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("create schema " + MySqlHelper.EscapeString(conn.Database), conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("use " + MySqlHelper.EscapeString(conn.Database), conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(
@"-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2021-12-03 14:45:38.455

-- tables
-- Table: DeStoreRecords
CREATE TABLE DeStoreRecords (
    uid int NOT NULL AUTO_INCREMENT,
    deStoreDate date NOT NULL,
    RespPerson_ico int NOT NULL,
    CONSTRAINT DeStoreRecords_pk PRIMARY KEY (uid)
);

-- Table: Department
CREATE TABLE Department (
    uid int NOT NULL AUTO_INCREMENT,
    name varchar(50) NOT NULL,
    UNIQUE INDEX Department_ak_1 (name),
    CONSTRAINT Department_pk PRIMARY KEY (uid)
);

-- Table: Odpad
CREATE TABLE Odpad (
    uid int NOT NULL,
    name varchar(50) NOT NULL,
    category int NOT NULL,
    weight_g int NOT NULL,
    CONSTRAINT Odpad_pk PRIMARY KEY (uid)
);

-- Table: Odpad_User_Settings
CREATE TABLE Odpad_User_Settings (
    User_rodCislo bigint NOT NULL,
    Odpad_uid int NOT NULL,
    CONSTRAINT Odpad_User_Settings_pk PRIMARY KEY (User_rodCislo,Odpad_uid)
);

-- Table: Records
CREATE TABLE Records (
    uid int NOT NULL AUTO_INCREMENT,
    storageDate date NOT NULL,
    amount int NOT NULL,
    Odpad_uid int NOT NULL,
    User_rodCislo bigint NOT NULL,
    DeStoreRecords_uid int NULL,
    CONSTRAINT Records_pk PRIMARY KEY (uid)
);

-- Table: RespPerson
CREATE TABLE RespPerson (
    ico int NOT NULL,
    name int NOT NULL,
    ulice varchar(50) NOT NULL,
    cislo_popisne int NOT NULL,
    mesto varchar(50) NOT NULL,
    PSC int NOT NULL,
    CONSTRAINT RespPerson_pk PRIMARY KEY (ico)
);

-- Table: User
CREATE TABLE User (
    rodCislo bigint NOT NULL,
    userRights int NOT NULL,
    name varchar(50) NOT NULL,
    firstName varchar(50) NOT NULL,
    lastName varchar(50) NOT NULL,
    passwd binary(32) NOT NULL,
    Department_uid int NULL,
    UNIQUE INDEX User_ak_1 (name),
    CONSTRAINT User_pk PRIMARY KEY (rodCislo)
);

-- foreign keys
-- Reference: DeStoreRecords_RespPerson (table: DeStoreRecords)
ALTER TABLE DeStoreRecords ADD CONSTRAINT DeStoreRecords_RespPerson FOREIGN KEY DeStoreRecords_RespPerson (RespPerson_ico)
    REFERENCES RespPerson (ico);

-- Reference: Odpad_User_Settings_Odpad (table: Odpad_User_Settings)
ALTER TABLE Odpad_User_Settings ADD CONSTRAINT Odpad_User_Settings_Odpad FOREIGN KEY Odpad_User_Settings_Odpad (Odpad_uid)
    REFERENCES Odpad (uid);

-- Reference: Odpad_User_Settings_User (table: Odpad_User_Settings)
ALTER TABLE Odpad_User_Settings ADD CONSTRAINT Odpad_User_Settings_User FOREIGN KEY Odpad_User_Settings_User (User_rodCislo)
    REFERENCES User (rodCislo);

-- Reference: Records_DeStoreRecords (table: Records)
ALTER TABLE Records ADD CONSTRAINT Records_DeStoreRecords FOREIGN KEY Records_DeStoreRecords (DeStoreRecords_uid)
    REFERENCES DeStoreRecords (uid);

-- Reference: Records_Odpad (table: Records)
ALTER TABLE Records ADD CONSTRAINT Records_Odpad FOREIGN KEY Records_Odpad (Odpad_uid)
    REFERENCES Odpad (uid);

-- Reference: Records_User (table: Records)
ALTER TABLE Records ADD CONSTRAINT Records_User FOREIGN KEY Records_User (User_rodCislo)
    REFERENCES User (rodCislo);

-- Reference: User_Department (table: User)
ALTER TABLE User ADD CONSTRAINT User_Department FOREIGN KEY User_Department (Department_uid)
    REFERENCES Department (uid);

-- End of file.
", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(@"insert into user
values(254751000,2,'root','ROOT','admin',unhex(SHA2('root',256)),null);", conn);
            cmd.ExecuteNonQuery();
        }
    }
    
}

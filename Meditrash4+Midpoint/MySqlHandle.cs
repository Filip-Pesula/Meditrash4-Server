using Meditrash4_Midpoint.mysqlTables;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

[assembly: InternalsVisibleTo("Meditrash4_Midpoint.tests")]

namespace Meditrash4_Midpoint
{
    class MySqlHandle
    {
        private static object connLock = new object();
        private MySqlConnection conn { get; set; }
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
        public List<string> checkSql()
        {
            List<string> errorList = new List<string>();
            List<MysqlReadable> typelist = new List<MysqlReadable>
            {
                new Cathegory(),
                new Department(),
                new ExportRecords(),
                new Records(),
                new RespPerson(),
                new Trash(),
                new TrashFaw(),
                new User()
            };
           
            typelist.ForEach((x) =>
            {
                MySqlDataReader resultReader = null;
                try
                {

                    MySqlCommand cmd = new MySqlCommand("describe " + x.getMyName(), conn);
                    resultReader = cmd.ExecuteReader();

                    List<KeyValuePair<string, Type>> typelist = x.getMyTypeList();

                    if (resultReader.FieldCount != typelist.Count)
                    {
                        errorList.Add(x.getMyName() + ": " + "incorrect Fielt count");
                    }
                   
                    if (resultReader.HasRows)
                    {
                        while (resultReader.Read())
                        {
                            for (int i = 0; i < ((resultReader.FieldCount > typelist.Count)? typelist.Count: resultReader.FieldCount); i++)
                            {;
                                if (typelist[i].Value != resultReader.GetFieldType(i))
                                {
                                    errorList.Add(x.getMyName() + ": " + resultReader.GetFieldValue<string>(i));
                                }
                            }
                        }

                    }
                    else
                    {
                        errorList.Add("database is empty");
                    }
                    resultReader.Close();
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                    if (resultReader != null)
                    {
                        resultReader.Close();
                    }
                    
                }
            });
            return errorList;
        }

        public List<List<KeyValuePair<Type, object>>> querry(string querry,List<KeyValuePair<string, KeyValuePair<MySqlDbType,object>>> parms, int max = -1)
        {
            MySqlCommand cmd = new MySqlCommand(querry, conn);
            foreach (KeyValuePair<string,KeyValuePair<MySqlDbType,object>> param in parms)
            {
                cmd.Parameters.Add(param.Key,param.Value.Key).Value = param.Value.Value;
            }
            MySqlDataReader resultReader = cmd.ExecuteReader();
            List < List <KeyValuePair<Type, object>>> returnVals = new List<List<KeyValuePair<Type, object>>>();
            if (resultReader.HasRows) {
                while (resultReader.Read())
                {
                    List<KeyValuePair<Type, object>> row = new List<KeyValuePair<Type, object>>();
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        row.Add(new KeyValuePair<Type, object>(
                            resultReader.GetFieldType(i),
                            resultReader.GetValue(i)));
                    }
                    returnVals.Add(row);
                }
            }
            resultReader.Close();
            return returnVals;
        }
        public void setObjectParam<T>(T _object, string collum, MySqlDbType type, object value) where T : MysqlReadable
        {
            Exception e = null;
            List<DbVariable> vallist = _object.getObjectData();
            List<int> pkList = _object.getPrimaryIndex();

            MySqlCommand cmd = new MySqlCommand("", conn);
            string cond = genKeySelectCond(_object, cmd);

            Monitor.Enter(connLock);

            try
            {
                cmd.CommandText = "UPDATE " + _object.getMyName() + " SET " + collum + "=@" + collum + "_edit " + " WHERE " + cond + ";";
                fillSelectCommandParamQ(ref cmd, _object);
                cmd.Parameters.Add("@" + collum + "_edit", type).Value = value;
                int execution = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
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
                MySqlCommand cmd = new MySqlCommand("INSERT INTO " + _object.getMyName() + _object.contentQuerry() + " VALUES " + valText + ";", conn);
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
        private void fillParrms(ref MySqlCommand cmd, List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>> parms, int max = -1)
        {
            foreach (KeyValuePair<string, KeyValuePair<MySqlDbType, object>> param in parms)
            {
                cmd.Parameters.Add(param.Key, param.Value.Key).Value = param.Value.Value;
            }
        }
        public List<T> GetObjectList<T>(string condition, List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>> parms, int max = -1) where T : MysqlReadable, new()
        {
            Exception e = null;
            Monitor.Enter(connLock);
            MySqlDataReader resultReader = null;
            try
            {
                T t = new T();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + t.getMyName() + " WHERE " + condition, conn);
                fillParrms(ref cmd, parms);
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
        public static string genKeySelectCond<T>(T _object, MySqlCommand cmd) where T : MysqlReadable
        {
            string cond = "";
            List<DbVariable> vallist = _object.getObjectData();
            int k = 0;
            foreach (int obj in _object.getPrimaryIndex())
            {
                cond += vallist[obj].name + " = @cond" + k + " AND ";
                cmd.Parameters.Add("@cond"+k, vallist[obj].type).Value = vallist[obj].value;
                k++;
            
            }
            cond = cond.Remove(cond.Length - 4);
            return cond;
        }
        internal void removeObject<T>(T _object) where T : MysqlReadable
        {
            Exception e = null;
            MySqlCommand cmd = new MySqlCommand("", conn);
            string cond = genKeySelectCond(_object,cmd);
            Monitor.Enter(connLock);
            try
            {
                cmd.CommandText = "DELETE FROM " + _object.getMyName() + " WHERE "+ cond + ";";
                fillSelectCommandParamQ(ref cmd, _object);
                int execution = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
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
        public void reset()
        {
            MySqlCommand cmd;

            cmd = new MySqlCommand(@"-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2022-01-09 11:16:09.841

-- foreign keys
ALTER TABLE ExportRecords
    DROP FOREIGN KEY DeStoreRecords_RespPerson;

ALTER TABLE Odpad
    DROP FOREIGN KEY Odpad_TrashCathegody;

ALTER TABLE Odpad_User_Settings
    DROP FOREIGN KEY Odpad_User_Settings_Odpad;

ALTER TABLE Odpad_User_Settings
    DROP FOREIGN KEY Odpad_User_Settings_User;

ALTER TABLE Records
    DROP FOREIGN KEY Records_DeStoreRecords;

ALTER TABLE Records
    DROP FOREIGN KEY Records_Odpad;

ALTER TABLE Records
    DROP FOREIGN KEY Records_User;

ALTER TABLE User
    DROP FOREIGN KEY User_Department;

-- tables
DROP TABLE Department;

DROP TABLE ExportRecords;

DROP TABLE Odpad;

DROP TABLE Odpad_User_Settings;

DROP TABLE Records;

DROP TABLE RespPerson;

DROP TABLE TrashCathegody;

DROP TABLE User;

-- End of file.

", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(
@"-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2022-01-09 11:16:09.841

-- tables
-- Table: Department
CREATE TABLE Department (
    uid int NOT NULL AUTO_INCREMENT,
    name varchar(50) NOT NULL,
    UNIQUE INDEX Department_ak_1 (name),
    CONSTRAINT Department_pk PRIMARY KEY (uid)
);

-- Table: ExportRecords
CREATE TABLE ExportRecords (
    uid int NOT NULL AUTO_INCREMENT,
    exportDate date NOT NULL,
    RespPerson_ico bigint NOT NULL,
    CONSTRAINT ExportRecords_pk PRIMARY KEY (uid)
);

-- Table: Odpad
CREATE TABLE Odpad (
    uid int NOT NULL AUTO_INCREMENT,
    name varchar(50) NOT NULL,
    TrashCathegody_id int NOT NULL,
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
    ico bigint NOT NULL,
    name varchar(50) NOT NULL,
    ulice varchar(50) NOT NULL,
    cislo_popisne int NOT NULL,
    mesto varchar(50) NOT NULL,
    PSC int NOT NULL,
    ZUJ int NOT NULL,
    CONSTRAINT RespPerson_pk PRIMARY KEY (ico)
);

-- Table: TrashCathegody
CREATE TABLE TrashCathegody (
    id int NOT NULL,
    name varchar(250) NOT NULL,
    CONSTRAINT TrashCathegody_pk PRIMARY KEY (id)
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
-- Reference: DeStoreRecords_RespPerson (table: ExportRecords)
ALTER TABLE ExportRecords ADD CONSTRAINT DeStoreRecords_RespPerson FOREIGN KEY DeStoreRecords_RespPerson (RespPerson_ico)
    REFERENCES RespPerson (ico)
    ON DELETE RESTRICT
    ON UPDATE CASCADE;

-- Reference: Odpad_TrashCathegody (table: Odpad)
ALTER TABLE Odpad ADD CONSTRAINT Odpad_TrashCathegody FOREIGN KEY Odpad_TrashCathegody (TrashCathegody_id)
    REFERENCES TrashCathegody (id)
    ON DELETE RESTRICT
    ON UPDATE CASCADE;

-- Reference: Odpad_User_Settings_Odpad (table: Odpad_User_Settings)
ALTER TABLE Odpad_User_Settings ADD CONSTRAINT Odpad_User_Settings_Odpad FOREIGN KEY Odpad_User_Settings_Odpad (Odpad_uid)
    REFERENCES Odpad (uid)
    ON DELETE CASCADE
    ON UPDATE CASCADE;

-- Reference: Odpad_User_Settings_User (table: Odpad_User_Settings)
ALTER TABLE Odpad_User_Settings ADD CONSTRAINT Odpad_User_Settings_User FOREIGN KEY Odpad_User_Settings_User (User_rodCislo)
    REFERENCES User (rodCislo)
    ON DELETE CASCADE
    ON UPDATE CASCADE;

-- Reference: Records_DeStoreRecords (table: Records)
ALTER TABLE Records ADD CONSTRAINT Records_DeStoreRecords FOREIGN KEY Records_DeStoreRecords (DeStoreRecords_uid)
    REFERENCES ExportRecords (uid)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT;

-- Reference: Records_Odpad (table: Records)
ALTER TABLE Records ADD CONSTRAINT Records_Odpad FOREIGN KEY Records_Odpad (Odpad_uid)
    REFERENCES Odpad (uid)
    ON DELETE RESTRICT
    ON UPDATE CASCADE;

-- Reference: Records_User (table: Records)
ALTER TABLE Records ADD CONSTRAINT Records_User FOREIGN KEY Records_User (User_rodCislo)
    REFERENCES User (rodCislo)
    ON DELETE RESTRICT
    ON UPDATE CASCADE;

-- Reference: User_Department (table: User)
ALTER TABLE User ADD CONSTRAINT User_Department FOREIGN KEY User_Department (Department_uid)
    REFERENCES Department (uid)
    ON DELETE SET NULL
    ON UPDATE CASCADE;

-- End of file.
", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(@"insert into User
values(254751000,2,'root','ROOT','admin',unhex(SHA2('root',256)),null);", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180101,'Ostré předměty');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180102,'Části těla a orgány včetně krevních vaků a krevních konzerv');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180103,'Odpady, na jejichž sběr a odstraňování jsou kladeny zvláštní požadavky s ohledem na prevenci infekce');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180104,'Odpady, na jejichž sběr a odstraňování nejsou kladeny zvláštní požadavky s ohledem na prevenci infekce');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180106,'Chemikálie, které jsou nebo obsahují nebezpečné látky');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180107,'Chemikálie neuvedené pod číslem 18 01 06');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180108,'Nepoužitelná cytostatika');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180109,'Jiná nepoužitelná léčiva neuvedená pod číslem 18 01 08');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180110,'Odpadní amalgám ze stomatologické péče');", conn);
            cmd.ExecuteNonQuery();
            /*
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180201,'Ostré předměty (kromě čísla 18 02 02)');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180202,'Odpady, na jejichž sběr a odstraňování jsou kladeny zvláštní požadavky s ohledem na prevenci infekce');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180203,'Odpady, na jejichž sběr a odstraňování nejsou kladeny zvláštní požadavky s ohledem na prevenci infekce');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180205,'Chemikálie sestávající z nebezpečných látek nebo tyto látky obsahující');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180206,'Jiné chemikálie neuvedené pod číslem 18 02 05');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180207,'Nepoužitelná cytostatika');", conn);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("insert into TrashCathegody\nvalues(180208,'Jiná nepoužitelná léčiva neuvedená pod číslem 18 02 07');", conn);
            cmd.ExecuteNonQuery();
            */
        }
    }
    
}

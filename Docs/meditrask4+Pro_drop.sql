-- Created by Vertabelo (http://vertabelo.com)
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


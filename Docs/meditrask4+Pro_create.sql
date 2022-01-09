-- Created by Vertabelo (http://vertabelo.com)
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


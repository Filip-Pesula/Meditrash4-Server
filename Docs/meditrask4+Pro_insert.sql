insert into odpad(name,TrashCathegody_id,weight_g)
values
('jehla kratka',180101,10),
('jehla dlouha',180101,20),
('krevní vak',180102,100),
('část těla',180102,50),
('obvaz',180104,200),
('náplast',180104,50),
('rukavice',180104,20),
('rukavice nitrilové',180104,20),
('lékařské lopatky',180104,20),
('leky',180109,100);

insert into department(name)
values
('jip'),
('stomatologie'),
('sál'),
('rengen'),
('čekárna'),
('url'),
('ambulance'),
('sono'),
('pohotovost'),
('laboratoř');

insert into user(rodCislo,userRights,name,firstName,lastName,passwd,Department_uid)
values
(0106065355,1, 'Josef.Novák' , 'Josef', 'Novák',unhex(SHA2('Josef.Novák',256)),1),
(8709185573,1, 'Petr.Novák' , 'Pert', 'Novák',unhex(SHA2('Petr.Novák',256)),1),
(8209044063,1, 'David.Novák' , 'David', 'Novák',unhex(SHA2('David.Novák',256)),4),
(6603167516,1, 'Ivan.Novák' , 'Ivan', 'Novák',unhex(SHA2('Ivan.Novák',256)),5),
(6408060054,1, 'Patrik.Novák' , 'Patrik', 'Novák',unhex(SHA2('Patrik.Novák',256)),3),
(6906135192,1, 'Oto.Novák' , 'Oto', 'Novák',unhex(SHA2('Oto.Novák',256)),1),
(9502043969,1, 'Oldřich.Novák' , 'Oldřich', 'Novák',unhex(SHA2('Oldřich.Novák',256)),2),
(6509162374,1, 'Jana.Novák' , 'Jana', 'Novák',unhex(SHA2('Jana.Novák',256)),1),
(6603113671,1, 'Petra.Novák' , 'Petra', 'Novák',unhex(SHA2('Petra.Novák',256)),1),
(9409072112,2, 'Karla.Novák' , 'Karla', 'Novák',unhex(SHA2('Karla.Novák',256)),2),
(7112108388,2, 'Karel.Novák' , 'Karla', 'Novák',unhex(SHA2('Karel.Novák',256)),1);

insert into odpad_user_settings(User_rodCislo,Odpad_uid)
values
(9409072112,1),
(9409072112,2),
(9409072112,3),
(9409072112,4),
(9409072112,5),
(7112108388,3),
(7112108388,4),
(7112108388,5),
(7112108388,6),
(7112108388,7),
(7112108388,8);

insert into respperson(ico,name,ulice,cislo_popisne,mesto,PSC,ZUJ)
values
(52174116,'odpadPodnik1', 'ulice1' , '1', 'Liberec','46340',1),
(54480366,'odpadPodnik2', 'ulice2' , '2', 'Liberec','46340',1),
(63967905,'odpadPodnik3', 'ulice3' , '3', 'Liberec','46340',1),
(58015296,'odpadPodnik4', 'ulice4' , '4', 'Liberec','46340',1),
(88562827,'odpadPodnik5', 'ulice5' , '5', 'Jablonec','46348',1),
(44649774,'odpadPodnik6', 'ulice6' , '6', 'Jablonec','46349',1),
(53754220,'odpadPodnik7', 'ulice7' , '7', 'Jablonec','46348',1),
(57067709,'odpadPodnik8', 'ulice8' , '8', 'Jablonec','46348',1),
(31239698,'odpadPodnik9', 'ulice9' , '9', 'Jablonec','46346',1),
(10844595,'odpadPodnik10', 'ulice10' , '10', 'Jablonec','46348',1);

insert into records(storageDate,amount,Odpad_uid,User_rodCislo,DeStoreRecords_uid)
values
(2020-03-01,'10', '1' , '9409072112', 'Liberec',null),
(2020-03-01,'3', '2' , '9409072112', 'Liberec',null),
(2021-03-01,'8', '2' , '9409072112', 'Liberec',null),
(2021-03-01,'1', '1' , '9409072112', 'Liberec',null),
(2020-03-01,'1', '1' , '7112108388', 'Liberec',null),
(2020-03-01,'4', '1' , '7112108388', 'Liberec',null),
(2021-03-01,'1', '2' , '7112108388', 'Liberec',null),
(2021-03-01,'1', '1' , '7112108388', 'Liberec',null),
(2021-03-01,'8', '1' , '7112108388', 'Liberec',null),
(2020-03-01,'6', '2' , '0106065355', 'Liberec',null),
(2021-03-01,'1', '1' , '0106065355', 'Liberec',null),
(2021-03-01,'1', '3' , '0106065355', 'Liberec',null);

insert into exportrecords(exportDate,RespPerson_ico)
values
(2020-03-05,'52174116'),
(2020-03-05,'58015296'),
(2020-03-05,'10844595');

SELECT * FROM exportrecords WHERE uid=(SELECT MAX(uid) FROM exportrecords);

update records
SET DeStoreRecords_uid = 1;

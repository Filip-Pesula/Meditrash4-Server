# Meditrash4-Server
![link](/Docs/Logo.png "Logo")<br/>
# Zadani

# Projekt 3: Meditrash +

### Cíl

Legislativa vyžaduje evidovat zdravotnický odpad na straně lékařů.

Chceme vytvořit program na evidenci odpadu a na včasné, rychlé a snadné odesílání formulářů do systém ISPOP. Cílem je snížit byrokratickou zátěž zdravotnického personálu v menších zdravotnických zařízeních.

### Úvod do problematiky

#### Legislativa

Zákon č. [541/2020 S](https://www.zakonyprolidi.cz/cs/2020-541/zneni-20210101#p158_p158-1)b. o odpadech a vyhláška č. 383/2001 Sb. o podrobnostech nakládání s odpady, č. 93/2016 Sb. (katalog odpadů). Nejvíce se pak odpady ze zdravotnictví zabývá vyhláška č. 306/2012 Sb. o podmínkách předcházení vzniku a šíření infekčních onemocnění a o hygienických požadavcích na provoz zdravotnických zařízení a ústavů sociální péče.

Zdravotnická zařízení produkují odpady, a proto musí vést průběžnou evidenci odpadů. Pokud přesáhnou zákonné limity v produkci a nakládání s odpady (100 kg nebezpečných (N) nebo 100 t ostatních (O) odpadů), musí elektronicky podávat Roční hlášení do systému ISPOP.

Pro malá zdravotnická zařízení je v pracovní době náročné ještě vést evidenci odpadů. Existuje katalog všech odpadů a je zdlouhavé v něm hledat konkrétní katalogová čísla zdravotnického odpadu.

Je potřeba zjednodušit proces evidování odpadů a vytvořit program s personalizovanou databází nejčastějších odpadů z konkrétního zdravotnického zařízení, tak aby si zaměstnanci jednou vyfiltrovali nejčastější odpady z jejich zařízení, průběžně případně mohli přidat další a poté už jen zadávali kolik ks konkrétního odpadu, který den vyhodili.

#### Existující řešení

Inisoft - EVI 8, ENVITA.cz - obecné programy pro evidenci odpadů - všechny katalogy, velký objem dat

### 5W otázky

#### Why

- Úspora času
- Snažší práce

#### Who

- zdravotnický personál

#### What

- aplikace s databází
- výběr a tvorba specifického personalizovaného katalogu
- vysouvací menu pro výběr častých odpadů
- upozornění na deadline odeslání ročního hlášení při překročení zákonných limitů (100 kg nebezpečných (N) nebo 100 t ostatních (O) odpadů)
- export do systému ISPOP
- databáze skladových zásob nádob na odpad
- upozornění na nutnou objednávku nádob
- statistický přehled vyprodukovaného odpadu

#### When

- funkční aplikace s instruktáží

#### Where

- místní úložiště (případně cloud)

#### for What

- 80 000 Kč

#### SLA

- Podpora v pracovní době

### Use case diagram

(Diagram případů užití)

![](RackMultipart20220109-4-5tlaox_html_f4b98d3aa6b440f8.png)

### Specifikace případů užití

#### Případ užití: Nastavení profilu (informace o zdravotnické zařízení a typech odpadu)

| **Krátký popis:** Systém umožňuje uživateli vytvořit a spravovat profil, ve kterém si nastaví informace o zdravotnickém zařízení a nejčastějších typech odpadu |
| --- |
| **Aktéři:** Uživatel, systém |
| **Podmínky spuštění:** Při spuštění dotaz na přihlášení |
| **Základní tok:** 1. Uživatel spravuje profil 2. Systém validuje uživatele a upraví proměnné podle uživatele |
| **Alternativní tok:** Pokud uživatel nevyplní profil musí všechny informace vyplňovat ručně bez našeptávače |
| **Podmínky pro dokončení:** Uživatel je zvalidován a jeho osobní údaje a předvolby jsou doplňovány do odesílaných pdf |

#### Případ užití: Zadává druh a počet odpadů

| **Krátký popis:** Zdravotnický personál v aplikaci zadává odpad, který je vybírán z personalizovaného menu. |
| --- |
| **Aktéři:** Uživatel, Systém |
| **Podmínky spuštění:** Nastavené personalizované menu |
| **Základní tok:** 1. Uživatel zadá kolik jakého odpadu bylo vyhozeno 2. Systém uloží zadané údaje do vnitřní databáze |
| **Podmínky pro dokončení:** Data jsou uloženy v databázi. |

#### Případ užití: Exportovaní

| **Krátký popis:** Databáze nabízí export hodnot do tabulkových formátů |
| --- |
| **Aktéři:** Uživatel, Systém |
| **Podmínky spuštění:** Uživatel zvolí export |
| **Základní tok:** 1. Uživatel zvolí export 2. Systém exportuje do zvoleného tabulkového formátu |
| **Podmínky pro dokončení:** Soubor byl uložen na zvolené místo ve zvoleném formátu |

#### Případ užití: Vyplňování a export pdf formulářů do ISPOP

| **Krátký popis:** Systém vyplní pdf formulář připravený k odeslání do ISPOP, upozorní uživatele na překročení zákonných limitů |
| --- |
| **Aktéři:** Systém |
| **Podmínky spuštění:** Systém detekoval přesáhnutí zákonných limitů |
| **Základní tok:** Systém notifikuje vyplnění pdf formuláře, předvyplní hodnoty název odpadu, katalogové číslo, počet, datum a exportuje data do pdf |
| **Podmínky pro dokončení:** Vyplněný formulář exportovaný do pdf |

#### Případ užití: Systém nabízí statistiku databáze

| **Krátký popis:** Systém umožňuje náhled statistik. Počet zaevidovaných odpadů, druhy odpadů, grafy, hmotnost, cena a podobně |
| --- |
| **Aktéři:** Uživatel, systém |
| **Podmínky spuštění:** Uživatel zvolí generování statistiky |
| **Základní tok:** 1. Uživatel spravuje zvolí generování statistiky 2. Systém zobrazí statistiky |
| **Alternativní tok:** Pokud uživatel zvolí statistiky, ale nemá žádná data je třeba uživatele upozornit |
| **Podmínky pro dokončení:** Uživatel se zobrazily statistiky nebo byl upozorněn na nedostatek dat |

### Požadavky na data

#### Entita: Uživatel

| **Právnická/fyzická osoba** | Zvolit zda li je uživatel právnická/fyzická osoba |
| --- | --- |
| **Jméno** | Jméno osoby |
| **Zdravotnické zařízení** | Oficiální jméno zdravotnického zařízení dle registru |
| **Ulice** | Ulice |
| **Číslo popisné** | číslo popisné |
| **Město** | Název města |
| **PSČ** | poštovní směrovací číslo |
| **IČO** | Identifikační číslo 8 čísel |
| **Telefonní číslo** | Telefonní číslo |
| **E-mail** | e-mail |
| **Fax** | fax |

#### Entita: Odpad

| **Název** | Název odpadu dle katalogu odpadů vyhláška č. 8/2021 Sb. |
| --- | --- |
| **Kód odpadu** | Dle katalogu odpadů Vyhláška č. 8/2021 Sb.příloha č. 1Šestimístná / osmimístná katalogová, první dvojčíslí označuje skupinu odpadů, druhé dvojčíslí podskupinu odpadů a třetí dvojčíslí druh odpadu. |
| **Kategorie** | Nebezpečný / ostatní |
| **Množství** | hmotnost (kg) |
| **Datum uskladnění** | Datum uskladnění odpadu |
| **Datum odevzdání** | Datum odevzdání odpadu oprávněné osobě |

#### Entita: Oprávněná osoba

| **Jméno** | Jméno oprávněné osoby zvolené pro zneškodnění odpadu |
| --- | --- |
| **IČO** | Identifikační číslo pracovníka nebo zařízení (8 čísel) |
| **Ulice** | Ulice |
| **Číslo popisné** | číslo popisné |
| **Město** | Název města |
| **PSČ** | poštovní směrovací číslo |
| **Datum** | Datum odevzdání odpadu oprávněné osobě |

###


### Návrh uživatelského rozhraní

![](RackMultipart20220109-4-5tlaox_html_571880fd0bfb11a5.jpg) ![](RackMultipart20220109-4-5tlaox_html_a744970517fea319.jpg)

![](RackMultipart20220109-4-5tlaox_html_a33974829ec1a7db.jpg)

![](RackMultipart20220109-4-5tlaox_html_d0aa43cf5349939d.jpg)

### Otázky

#### 1) Jak funguje evidence odpadů teď u malých zařízení?

Prakticky vůbec. Maximálně mají papírové podklady, vážní lístky, dodací listy, příjemky, … Subjekt, který se jim o evidenci stará, pak musí vše přepisovat. Cílem je, aby alespoň základní údaje o produkci odpadů vedli elektronicky na základě předem nadefinovaných šablon. (lepší excel).

#### 2) Proč nejde používat u malých zdravotnických zařízení program ENVITA, EVI 8 ?

Protože je pro ně drahý, zbytečně složitý a robustní, musí se instalovat a pro zdravotnická zařízení je evidence odpadů okrajová záležitost.

#### 3) Jak vypadá konkrétní formulář do ISPOP malého zdravotnického zařízení (příklad) ? (uživatel prý musí být registrován aby mu byl poskytnut formulář)

Ano, to je pravda a především jde o to, že ISPOP nesbírá průběžnou evidenci, ale pouze roční. Tzn. že do formulářů se musí data za rok sečíst a doplnit. Pro malá zdravotnická zařízení tuto činnost vykonává zpravidla nějaký poradce, oprávněná osoba, která potřebuje pouze dobré podklady. Pak jim vše zařídí a ohlásí. Někdy ani nemusí malé zdravotnické zařízení hlášení do ISPOP podávat (nenavrší limity stanovené zákonem – nenakládá s více než 100 tun ostatních odpadů, 600 kg nebezpečných odpadů). Pak stačí, když vede pouze průběžnou evidenci odpadů.

### Podklady

![](RackMultipart20220109-4-5tlaox_html_9debba4978dc8a.jpg)

![](RackMultipart20220109-4-5tlaox_html_9debba4978dc8a.jpg) ![](RackMultipart20220109-4-5tlaox_html_e75db8e8b2dbbaf7.jpg)




# Řešení
aplikace je složena ze 3 modulů<br/>
UserApp - aplikace řídící načítání dat a zobrazování dat uživately<br/>
AppConnector - zpracovává požadavky od UserApp a komunikuje s databází<br/>
Mysql Server - databáze držící data<br/>

![link](/Docs/App.png "App Diagram")<br/>

# Návrh databázového schématu

![link](/Docs/meditrask4+Pro-2022-01-09_15-59.png "Database Diagram")

# Použité technologie
C# a MySql API - backend<br/>
(Node)JS elektron - frontend<br/>
MySql - databáze<br/>

Komunikace mezi serverem a Ui je přez tcp, pomocí předem stanovených Xml requestů.


# Připojení k databáze

připojení k databázi je pomocí MySql API, která je uzavřená v MySqlHandle. Ta umožnuje volat jednotlivé requesty a kontrolu data a sql injection.
MySqlHandle dale obsahuje Drop a Create skripty které společně s 

```C#
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
    public List<List<KeyValuePair<Type, object>>> querry(string querry,List<KeyValuePair<string, KeyValuePair<MySqlDbType,object>>> parms, int max = -1) ...
    public void setObjectParam<T>(T _object, string collum, MySqlDbType type, object value) where T : MysqlReadable ...
    public void saveObject<T>(T _object) where T : MysqlReadable ...
    public List<T> GetObjectList<T>(string condition, List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>> parms, int max = -1) where T : MysqlReadable, new()...
    public void removeObject<T>(T _object) where T : MysqlReadable ...
    public void reset() ...
```


# SQL Injection
Řešení sql injection je pomocí vnitřní struktury api která umožnňuje vytovření sql reqesutu se zvolenýmy proměnnými a ty jsou poté vezpečně nahrazeny daty, kdy API vždy kontroluje datové typy.<br/>
Pokud pracujo s celýmy objekty je jeich uládání enkapsulvýno pomocí inteface MySqlReadable, který umožňuje uniformní kontrolované zabezpečení dat.
```C#
var data = mySqlHandle.querry(
   @"select records.uid,Rec_Odp_User_Trc.id,Rec_Odp_User_Trc.name,storageDate,amount,Odpad_uid,Rec_Odp.name,Rec_Odp_User.name from records 
                  LEFT JOIN odpad Rec_Odp on records.Odpad_uid = Rec_Odp.uid 
                    LEFT JOIN user Rec_Odp_User on Rec_Odp_User.rodCislo = records.User_rodCislo
                    LEFT JOIN trashcathegody Rec_Odp_User_Trc on Rec_Odp_User_Trc.id = Rec_Odp.TrashCathegody_id
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
               
private void fillParrms(ref MySqlCommand cmd, List<KeyValuePair<string, KeyValuePair<MySqlDbType, object>>> parms)
{
    foreach (KeyValuePair<string, KeyValuePair<MySqlDbType, object>> param in parms)
    {
        cmd.Parameters.Add(param.Key, param.Value.Key).Value = param.Value.Value;
    }
}
```

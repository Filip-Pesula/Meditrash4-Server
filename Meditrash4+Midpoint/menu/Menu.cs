using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Meditrash4_Midpoint.menu
{
    internal class Menu
    {
        enum MenuState
        {
            mainMenu,
            settingMenu,
            resetMenu,
            settingCompanyMenu
        }
        enum SettingState
        {
            choiceState,
            inputState
        }
        MainMenu mainMenu;
        SettingsMenu settingsMenu;
        CompanySettingsMenu companySettingsMenu;
        ResetMenu resetMenu;

        MenuState menuState;
        SettingState settingState;

        ServerData serverData;
        CompanyData companyData;

        //callbacks
        ConnectCallBack connectCallBack;
        ResetCallBack resetCallBack;
        SaveCallBack saveCallBack;
        TestCallBack testCallBack;
        SaveCompanyCallBack saveCompanyCallBack;

        public delegate void ConnectCallBack();
        public delegate void ResetCallBack();
        public delegate void SaveCallBack();
        public delegate void TestCallBack();
        public delegate void SaveCompanyCallBack();
        public Menu(ServerData serverData, CompanyData companyData)
        {
            this.serverData = serverData;
            this.companyData = companyData;
            menuState = MenuState.mainMenu;
            settingState = SettingState.choiceState;
            mainMenu = new MainMenu();
            settingsMenu = new SettingsMenu(serverData);
            companySettingsMenu = new CompanySettingsMenu(companyData);
            resetMenu = new ResetMenu();
        }

        public void setConnectCallBack(ConnectCallBack connectCallBack)
        {
            this.connectCallBack = connectCallBack;
        }
        public void setResetCallBack(ResetCallBack resetCallBack)
        {
            this.resetCallBack = resetCallBack;
        }
        public void setSaveCallBack(SaveCallBack saveCallBack)
        {
            this.saveCallBack = saveCallBack;
        }
        public void setTestCallBack(TestCallBack testCallBack)
        {
            this.testCallBack = testCallBack;
        }
        public void setSaveCompanyCallBack(SaveCompanyCallBack saveCompanyCallBack)
        {
            this.saveCompanyCallBack = saveCompanyCallBack;
        }
        private bool connectoed = false;

        internal void setNotConnected()
        {
            connectoed = false;
        }
        public void setConnected()
        {
            connectoed=true;
        }
        public bool run()
        {
            
            switch (menuState)
            {
                case MenuState.mainMenu:
                    {
                        mainMenu.draw(mainMenu.active,connectoed);
                        ConsoleKey key = Console.ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.DownArrow:
                                mainMenu.down();
                                break;
                            case ConsoleKey.UpArrow:
                                mainMenu.up();
                                break;
                            case ConsoleKey.Enter:
                                switch (mainMenu.active)
                                {
                                    case 0:
                                        menuState = MenuState.settingMenu;
                                        break;
                                    case 1:
                                        menuState = MenuState.resetMenu;
                                        break;
                                    case 2:
                                        menuState = MenuState.settingCompanyMenu;
                                        break;
                                    case 3:
                                        return true;
                                        break;
                                }
                                break;
                            case ConsoleKey.Escape:
                                break;
                        }
                        break;
                    }
                case MenuState.settingMenu:
                    {
                        settingsMenu.draw(connectoed);
                        switch (settingState) {
                            case SettingState.choiceState:
                                {
                                    ConsoleKey key = Console.ReadKey(true).Key;
                                    switch (key)
                                    {
                                        case ConsoleKey.DownArrow:
                                            settingsMenu.down();
                                            break;
                                        case ConsoleKey.UpArrow:
                                            settingsMenu.up();
                                            break;
                                        case ConsoleKey.Enter:
                                            switch (settingsMenu.active)
                                            {
                                                case 0:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            serverData.ServerAddres = ins;
                                                            saveCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            serverData.User = ins;
                                                            saveCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            serverData.Database = ins;
                                                            saveCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            serverData.Port = ins;
                                                            saveCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 4:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            serverData.Password = ins;
                                                            saveCallBack();
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        case ConsoleKey.Escape:
                                            menuState = MenuState.mainMenu;
                                            break;
                                    }
                                }
                                break;
                            case SettingState.inputState:
                                {
                                    Console.Clear();
                                    bool end = false;
                                    Console.WriteLine("Vložte nastavení:");
                                    string mline = Console.ReadLine();
                                    settingState = SettingState.choiceState;
                                    break;
                                }
                        }

                        break;
                    }
                case MenuState.settingCompanyMenu:
                    {
                        companySettingsMenu.draw(connectoed);
                        switch (settingState) {
                            case SettingState.choiceState:
                                {
                                    ConsoleKey key = Console.ReadKey(true).Key;
                                    switch (key)
                                    {
                                        case ConsoleKey.DownArrow:
                                            companySettingsMenu.down();
                                            break;
                                        case ConsoleKey.UpArrow:
                                            companySettingsMenu.up();
                                            break;
                                        case ConsoleKey.Enter:
                                            switch (companySettingsMenu.active)
                                            {
                                                case 0:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            companyData.ico = ins;
                                                            saveCompanyCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            companyData.name = ins;
                                                            saveCompanyCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            companyData.id = ins;
                                                            saveCompanyCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            companyData.ulice = ins;
                                                            saveCompanyCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 4:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            companyData.mesto = ins;
                                                            saveCompanyCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 5:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            companyData.psc = ins;
                                                            saveCompanyCallBack();
                                                        }
                                                        break;
                                                    }
                                                case 6:
                                                    {
                                                        string ins = input();
                                                        if (ins == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            companyData.zuj = ins;
                                                            saveCompanyCallBack();
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        case ConsoleKey.Escape:
                                            menuState = MenuState.mainMenu;
                                            break;
                                    }
                                }
                                break;
                            case SettingState.inputState:
                                {
                                    Console.Clear();
                                    bool end = false;
                                    Console.WriteLine("Vložte nastavení:");
                                    string mline = Console.ReadLine();
                                    settingState = SettingState.choiceState;
                                    break;
                                }
                        }

                        break;
                    }
                case MenuState.resetMenu:
                    {
                        resetMenu.draw(connectoed);
                        ConsoleKey key = Console.ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.DownArrow:
                                resetMenu.down();
                                break;
                            case ConsoleKey.UpArrow:
                                resetMenu.up();
                                break;
                            case ConsoleKey.Enter:
                                switch (resetMenu.active)
                                {
                                    case 0:
                                        connectCallBack();       
                                        break;
                                    case 1:
                                        if (connectoed)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Tímto nastavením uplně vymažete obsah databáze, jste si jsití?");
                                            Console.WriteLine("Resetovat Databázy: y/n");
                                            bool reset = false;
                                            bool end = false;
                                            while (!end)
                                            {
                                                switch (Console.ReadKey().Key)
                                                {
                                                    case ConsoleKey.Y:
                                                        reset = true;
                                                        end = true;
                                                        break;
                                                    case ConsoleKey.Escape:
                                                    case ConsoleKey.N:
                                                        end = true;
                                                        break;
                                                }
                                            }
                                            resetMenu.draw(connectoed);
                                            if (reset)
                                            {
                                                resetCallBack();
                                            }

                                            settingState = SettingState.choiceState;
                                        }
                                        break;
                                }
                                break;
                            case ConsoleKey.Escape:
                                menuState = MenuState.mainMenu;
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        private string input()
        {
            Console.Clear();
            Console.WriteLine("Vložte nastavení:");
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        return null;
                        break;
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.Backspace:
                        sb.Remove(sb.Length-1,1);
                        break;
                    default:
                        sb.Append(key.KeyChar);
                        break;
                }
                if(key.Key == ConsoleKey.Enter)
                {
                    return sb.ToString();
                }
            }
        }

    }
}

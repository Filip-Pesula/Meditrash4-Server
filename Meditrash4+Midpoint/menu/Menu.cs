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
            resetMenu
        }
        enum SettingState
        {
            choiceState,
            inputState
        }
        MainMenu mainMenu;
        SettingsMenu settingsMenu;
        ResetMenu resetMenu;
        MenuState menuState;
        SettingState settingState;
        ServerData serverData;
        ConnectCallBack connectCallBack;
        ResetCallBack resetCallBack;
        SaveCallBack saveCallBack;
        public delegate void ConnectCallBack();
        public delegate void ResetCallBack();
        public delegate void SaveCallBack();
        public Menu(ServerData serverData)
        {
            this.serverData = serverData;
            
            menuState = MenuState.mainMenu;
            settingState = SettingState.choiceState;
            mainMenu = new MainMenu();
            settingsMenu = new SettingsMenu(serverData);
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
                        Logger.MainMenu(mainMenu.active,connectoed);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.menu
{
    internal class SettingsMenu
    {
        public const int ITEM_COUNT = 5;
        public int active { get; private set; }
        ServerData serverData;
        public SettingsMenu(ServerData serverData)
        {
            active = 0;
            this.serverData = serverData;
        }
        public void up()
        {
            if (active > 0)
                active--;
        }
        public void down()
        {
            if (active < ITEM_COUNT - 1)
                active++;
        }
        public void draw(bool connected)
        {
            Console.Clear();
            Logger.drawMenuHeader(false, connected);
            if (active == 0)
            {
                Logger.drawSelectItem("ServerAddres".PadRight(20) + serverData.ServerAddres.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("ServerAddres".PadRight(20) + serverData.ServerAddres.PadRight(80));
            }
            if (active == 1)
            {
                Logger.drawSelectItem("User".PadRight(20) + serverData.User.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("User".PadRight(20) + serverData.User.PadRight(80));
            }
            if (active == 2)
            {
                Logger.drawSelectItem("Database".PadRight(20) + serverData.Database.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("Database".PadRight(20) + serverData.Database.PadRight(80));
            }
            if (active == 3)
            {
                Logger.drawSelectItem("Port".PadRight(20) + serverData.Port.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("Port".PadRight(20) + serverData.Port.PadRight(80));
            }if (active == 4)
            {
                Logger.drawSelectItem("Password".PadRight(20) + serverData.Password.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("Password".PadRight(20) + serverData.Password.PadRight(80));
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}

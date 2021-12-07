using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.menu
{
    internal class MainMenu
    {
        public const int ITEM_COUNT = 4;
        public int active { get; private set; }

        public MainMenu()
        {
            active = 0;
        }
        public void up(){
            if (active > 0)
                active--;
        }
        public void down()
        {
            if(active < ITEM_COUNT-1)
                active++;
        }
        public void draw(int index, bool connectoed)
        {
            Console.Clear();
            Logger.drawMenuHeader(true, connectoed);
            if (index == 0)
            {
                Logger.drawSelectItem("Nastavení".PadRight(20));
            }
            else
            {
                Logger.drawUnselectItem("Nastavení".PadRight(20));
            }
            if (index == 1)
            {
                Logger.drawSelectItem("Restartovat".PadRight(20));
            }
            else
            {
                Logger.drawUnselectItem("Restartovat".PadRight(20));
            }
            if (index == 2)
            {
                if (connectoed)
                    Logger.drawSelectItem("TestDatabase".PadRight(20));
                else
                    Logger.drawSelectItem("TestDatabase".PadRight(20), ConsoleColor.Gray);
            }
            else
            {

                if (connectoed)
                    Logger.drawUnselectItem("TestDatabase".PadRight(20));
                else
                    Logger.drawUnselectItem("TestDatabase".PadRight(20), ConsoleColor.Gray);
            }
            if (index == 3)
            {
                Logger.drawSelectItem("Exit".PadRight(20));
            }
            else
            {
                Logger.drawUnselectItem("Exit".PadRight(20));
            }
           
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}

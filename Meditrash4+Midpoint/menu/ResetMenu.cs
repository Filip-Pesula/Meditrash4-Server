using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.menu
{
    internal class ResetMenu
    {
        public const int ITEM_COUNT = 2;
        public int active { get; private set; }
        public ResetMenu()
        {
            active = 0;
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
                if (!connected)
                {
                    
                }
                Logger.drawSelectItem("Restart Připojení".PadRight(20));
            }
            else
            {
                Logger.drawUnselectItem("Restart Připojení".PadRight(20));
            }
            if (active == 1)
            {
                if (connected)
                {
                    Logger.drawSelectItem("Resetovat databázi".PadRight(20));
                }
                else
                {
                    Logger.drawSelectItem("Resetovat databázi".PadRight(20),ConsoleColor.DarkGray);
                }
               
            }
            else
            {
                if (connected)
                {
                    Logger.drawUnselectItem("Resetovat databázi".PadRight(20));
                }
                else
                {
                    Logger.drawUnselectItem("Resetovat databázi".PadRight(20), ConsoleColor.DarkGray);
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}

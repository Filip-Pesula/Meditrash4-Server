using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.menu
{
    internal class CompanySettingsMenu
    {
        public const int ITEM_COUNT = 7;
        public int active { get; private set; }
        private CompanyData _data;

        public CompanySettingsMenu(CompanyData data)
        {
            active = 0;
            _data = data;
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
        public void draw(bool connectoed)
        {
            Console.Clear();
            Logger.drawMenuHeader(true, connectoed);
            if (active == 0)
            {
                Logger.drawSelectItem("ICO".PadRight(20) + _data.ico.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("ICO".PadRight(20) + _data.ico.PadRight(80));
            }
            if (active == 1)
            {
                Logger.drawSelectItem("Jméno".PadRight(20) + _data.name.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("Jméno".PadRight(20) + _data.name.PadRight(80));
            }
            if (active == 2)
            {
                Logger.drawSelectItem("ID".PadRight(20) + _data.id.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("ID".PadRight(20) + _data.id.PadRight(80));
            }
            if (active == 3)
            {
                Logger.drawSelectItem("Ulice".PadRight(20) + _data.ulice.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("Ulice".PadRight(20) + _data.ulice.PadRight(80));
            }
            if (active == 4)
            {
                Logger.drawSelectItem("Město".PadRight(20) + _data.mesto.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("Město".PadRight(20) + _data.mesto.PadRight(80));
            }
            if (active == 5)
            {
                Logger.drawSelectItem("PSČ".PadRight(20) + _data.psc.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("PSČ".PadRight(20) + _data.psc.PadRight(80));
            }
            if (active == 6)
            {
                Logger.drawSelectItem("ZUJ".PadRight(20) + _data.zuj.PadRight(80));
            }
            else
            {
                Logger.drawUnselectItem("ZUJ".PadRight(20) + _data.zuj.PadRight(80));
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}

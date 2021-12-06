using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.menu
{
    internal class MainMenu
    {
        public const int ITEM_COUNT = 3;
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
    }
}

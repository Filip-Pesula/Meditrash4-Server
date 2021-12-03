using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint
{
    internal class AppLogin
    {
        public DateTime lastLogin { get; private set; }
        public uint unuqueToken { get; private set; }
        public User user { get; private set; }
        public string address { get; private set; }

        public AppLogin(User user, uint unuqueToken, string address)
        {
            this.lastLogin = lastLogin;
            this.unuqueToken = unuqueToken;
            this.user = user;
            this.address = address;
        }
    }
}

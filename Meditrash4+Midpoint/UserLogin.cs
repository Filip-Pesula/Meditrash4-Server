using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint
{
    internal class UserLogin
    {
        public uint token { get; private set; }
        public User user { get; private set; }
        public DateTime timeStamp { get; set; }
        public UserLogin(uint token,User user, DateTime timeStamp)
        {
            this.token = token;
            this.user = user;
            this.timeStamp = timeStamp;
        }
    }
}

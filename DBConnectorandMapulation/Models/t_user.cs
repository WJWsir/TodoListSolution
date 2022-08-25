using System;
using System.Collections.Generic;
using System.Text;

namespace DBConnectorandMapulation.Models
{
    public class t_user
    {
        public int user_identity { set; get; }
        public string user_username { get; set; }
        public string user_password { get; set; }
    }
}

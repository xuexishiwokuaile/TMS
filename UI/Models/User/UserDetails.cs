using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.User
{
    public class UserDetails
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string classification { get; set; }
        public string sex { get; set; }
        public string phone { get; set; }
        public string apartment { get; set; }
        //public string pwd { get; set; }
        public Guid workcellid { get; set; }//workcellid
        public string email { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//已更新--2020.4.17
namespace UI.Models.Workcell
{
    public class Workcell
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string manager { get; set; }
        public string date { get; set; }
        public int count { get; set; }
        public int classification { get; set; }
        
    }
}
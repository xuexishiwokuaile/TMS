using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.User
{
    public class ModuleStereotype
    {
        public string title { get; set; }//模块标题
        public string icon { get; set; }//图标
        public bool spread { get; set; }//是否展开
        public string href { get; set; }//对应链接
        public List<ModuleStereotype> children { get; set; }

    }
}
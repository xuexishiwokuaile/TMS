using System;
using System.Web;

namespace UI.Models
{
    public class ScrapOrderDetailModel
    {
        public Guid scrapId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? supervisorId { get; set; }

        public Guid? managerId { get; set; }

        public Guid? toolId { get; set; }

        public string code { get; set; }
        
        //这里存储的是toolId
        public Guid? seqId { get; set; }

        public int? lifeCount { get; set; }

        public string description { get; set; }

        public int? state { get; set; }

        
    }
}

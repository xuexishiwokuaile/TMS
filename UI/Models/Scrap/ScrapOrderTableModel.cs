using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class ScrapOrderTableModel
    {
        public Guid scrapId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? supervisorId { get; set; }

        public Guid? managerId { get; set; }

        public Guid? toolId { get; set; }

        public string code { get; set; }

        public int? seqId { get; set; }

        public int? lifeCount { get; set; }

        public string description { get; set; }

        public int? state { get; set; }

        public string proposerName { get; set; }

        public string supervisorName { get; set; }

        public string managerName { get; set; }
    }
}
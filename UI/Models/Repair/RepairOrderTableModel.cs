using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class RepairOrderTableModel
    {
        public string proposerName { get; set; }
        public Guid repairOrderId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? toolId { get; set; }

        public string code { get; set; }

        public int? seqId { get; set; }

        public Guid? dealerId { get; set; }

        public string faultPicUrl { get; set; }

        public string description { get; set; }

        public int? state { get; set; }

        public string name { get; set; }
    }
}
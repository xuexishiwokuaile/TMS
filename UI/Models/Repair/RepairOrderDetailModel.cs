using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class RepairOrderDetailModel
    {
        public Guid repairOrderId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? toolId { get; set; }

        public Guid? seqId { get; set; }

        public Guid? dealerId { get; set; }

        public string faultPicUrl { get; set; }

        public string description { get; set; }

        public int? state { get; set; }
    }
}
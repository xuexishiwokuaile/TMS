using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class PurchaseOrderDetailModel
    {
        public Guid purchaseId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? toolType { get; set; }

        public string code { get; set; }

        //public Guid? toolId { get; set; }

        //public Guid? supervisorId { get; set; }

        //public Guid? managerId { get; set; }

        //public int? state { get; set; }

        public int purchaseNum { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class PurchaseOrderTableModel
    {
        public Guid purchaseId { get; set; }

        public Guid? proposerId { get; set; }

        public Guid? typeId { get; set; }

        public Guid? toolId { get; set; }

        public Guid? supervisorId { get; set; }

        public Guid? managerId { get; set; }

        public int? state { get; set; }

        public string code { get; set; }

        public string proposerName { get; set; }

        public string supervisorName { get; set; }

        public string managerName { get; set; }
    }
}
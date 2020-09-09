using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class ToolTypeDetailModel
    {
   
        public Guid typeId { get; set; }

        //public string workcellId { get; set; }

        //public string familyId { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public string model { get; set; }

        public string partNo { get; set; }

        public string usedFor { get; set; }

        public int? ulp { get; set; }

        //public Guid? ownerId { get; set; }

        public int? pmPeriod { get; set; }

        //public DateTime? recOn { get; set; }

        //public Guid? recById { get; set; }

        //public DateTime? editOn { get; set; }

        //public Guid? editById { get; set; }
    }
}
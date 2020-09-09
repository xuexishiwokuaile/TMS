using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class inoutListModel
    {
        public Guid toolId { get; set; }

        public Guid? typeId { get; set; }

        public Guid? toolType { get; set; }

        public String code { get; set; }

        public int? seqId { get; set; }

        public string billNo { get; set; }

        public Guid? operatorId { get; set; }

        public DateTime? regDate { get; set; }

        public int? usedCount { get; set; }

        public string location { get; set; }

        public DateTime? proDate { get; set; }

        public string picUrl { get; set; }

        public bool? _out { get; set; }
    }
}
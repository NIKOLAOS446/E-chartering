using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Echartering.Models
{
    public class Ship : CommonModel
    {
        public int DWCC { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string Flag { get; set; }

        public int year { get; set; }

   
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}

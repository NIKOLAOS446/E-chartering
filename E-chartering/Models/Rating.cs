using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Echartering.Models
{
    public class Rating : CommonModel
    {
        public string ShipOwnerId { get; set; }
        [ForeignKey("ShipOwnerId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


        public string Description { get; set; }


        public int Score { get; set; }
    }
}

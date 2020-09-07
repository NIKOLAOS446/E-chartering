using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Echartering.Models
{
    public class ShipCargoRelation:CommonModel
    {

        
        public Int64 ShipId { get; set; }

        [ForeignKey("ShipId")]
        public virtual Ship Ship { get; set;}


        public Int64 CargoId { get; set; }

        [ForeignKey("CargoId")]
        public virtual Cargo Cargo { get; set; }

        public Boolean IsApproved { get; set; }

        public Boolean IsApproved1 { get; set; }

        public string ShipUserId { get; set; }
        [ForeignKey("ShipUserId")]
        public virtual ApplicationUser ShipUser { get; set; }

        public string CargoUserId { get; set; }
        [ForeignKey("CargoUserId")]
        public virtual ApplicationUser CargoUser { get; set; }

        public double FixedFreight { get; set; }

        public double Commission { get; set; }
        public string AcceptorRole { get; set; }
    }
}

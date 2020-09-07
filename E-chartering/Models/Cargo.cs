using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Echartering.Models
{
    public class Cargo : CommonModel
    {
        public int Quantity { get; set; }

        public string CargoType { get; set; }

        public string DeparturePort { get; set; }

        public string DestinationPort { get; set; }

        public  DateTime DateFrom { get; set; }

        public  DateTime DateTo { get; set; }

        public string DischargingRate { get; set; }

        public string LoadingRate { get; set; }

        public double FreightIdea { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echartering.Models
{
    public class ApplicationUser:IdentityUser
    {
        //public ICollection<Ship> Ships { get; set; }
        // public List<Ship> ships { get; set; }
        // public ICollection<Cargo> Cargos { get; set; }

        public Boolean IsApproved { get; set; }
    }
}

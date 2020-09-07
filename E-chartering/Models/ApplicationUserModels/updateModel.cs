using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echartering.Models.ApplicationUserModels
{
    public class UpdateModel
    {
        public string id { get; set; }
        public string userName { get; set; }

        public string email { get; set; }

        public string CurrentPass { get; set; }
        public string password { get; set; }

        public string phoneNumber { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Madoors.Models
{
    public class CarPassModel
    {
        public Guid carPassID { get; set; }

        public String entranceID { get; set; }

        public String entranceName { get; set; }

        public String plateNo { get; set; }

        public int? state { get; set; }

        public byte[] image { get; set; }

        public String time { get; set; } 

    }
}

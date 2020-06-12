using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Madoors.Models
{
    public class AccountModel
    {
        public String tokenID { get; set; }
        public Guid? entranceID { get; set; }
        public Guid? branchGuid { get; set; }
        public String branchID { get; set; }
        public String branchName { get; set; }
        public String entranceName { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String deviceKey { get; set; }
        public String entranceNames { get; set; }
    }
}
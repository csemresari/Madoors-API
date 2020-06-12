using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Madoors.Models
{
    public class AccountResultModel
    {
        public AccountResultStatus Status { get; set; }
        public AccountModel Account { get; set; }
    }
}
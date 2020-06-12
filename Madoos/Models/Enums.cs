using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Madoors.Models
{
    public enum AccountResultStatus
    {
        VALID_LOGIN = 1,
        INVALID_SERVER = 2,
        INVALID_USERNAME_PASSWORD = 3,
        UNKNOWN_ERROR
    }
}
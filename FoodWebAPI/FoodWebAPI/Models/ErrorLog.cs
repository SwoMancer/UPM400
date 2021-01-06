using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodWebAPI.Models
{
    public class ErrorLog
    {
        public string messages { get; set; }
        public Exception exception { get; set; }
    }
}
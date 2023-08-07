using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class Cimast
    {
        public string afacctno { get; set; }
        public string acctno { get; set; }
        public double balance { get; set; }
        public double pp { get; set; }
        public double cidepofeeacr { get; set; }
        public double depofeeamt { get; set; }
        public double currentdebt { get; set; }
        public string lastchange { get; set; }
        public string status { get; set; }
    }
}
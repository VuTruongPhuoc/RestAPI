using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class Afmast
    {
        public string custid { get; set; }
        public string acctno { get; set; }
        public string martype { get; set; }
        public string mrcrlimitmax { get; set; }
    }
    public class MaxAcctnoAfmast
    {
        public string maxacctno { get; set; }

        public MaxAcctnoAfmast() { }
        public MaxAcctnoAfmast(string maxAcctno)
        {
            this.maxacctno = maxAcctno;
        }
    }
    public class DuyetCfmast
    {
        public string custid { get; set; }
        public string acctno { get; set; }
        public string martype { get; set; }
        public double mrcrlimitmax { get; set; }
        public string afacctno { get; set; }
        public double balance { get; set; }
        public double pp { get; set; }
        public double cidepofeeacr { get; set; }
        public double depofeeamt { get; set; }
        public double currentdebt { get; set; }
        public string lastchange { get; set; }
        public string status { get; set;}

    }
}
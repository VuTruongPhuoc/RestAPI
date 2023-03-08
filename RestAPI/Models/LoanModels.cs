using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{

    public class getLoanType
    {
        public string lnTypeId = "";
        public string name = "";
        public string loanType = "";
        public string sourceType = "";
        public string custBank = "";
        public string autoApply = "";
        public string complyApply = "";
        public string basis = "";
        public string loanCalendar = "";
        public Int64 preferentialDays = 0;
        public Int64 term = 0;
        public double rate1 = 0;
        public double rate2 = 0;
        public double rate3 = 0;
        public double cfRate1 = 0;
        public double cfRate2 = 0;
        public double cfRate3 = 0;
        public string autoPrepay = "";
        public string autoRenew = "";
        public double prepayFee = 0;
        public Int64 warningDays = 0;
        public string status = "";
        public string note = "";
    }

}


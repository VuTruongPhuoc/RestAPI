using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class Cfmast
    {
        public string custid;

        public string fullname;

        public string custodycd;

        public string idtype;

        public string idcode;

        public string iddate;

        public string address;

        public string phone;

        public string mobile;

        public string email;
    }
    public class MaxCustIdCfmast
    {
        public string maxCustid;

        public MaxCustIdCfmast(string mCustid)
        {
            this.maxCustid = mCustid;
        }
    }
}
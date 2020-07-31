using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Bussiness
{
    public class BussinessObject
    {
    }

    public class list
    {
        public string s = "";
        public object[] d;
    }

    public class BoResponse
    {
        public string s = "";
        public string errmsg = "";
    }
    public class BoResponse1
    {
        public long s = 0;
        public string errmsg = "";
        public object d = null;
    }
    public class orderResponse
    {
        public string orderid = "";
    }

    public class KeyField
    {
        public string keyName = "";
        public string keyValue = "";
        public string keyType = "";   //VARCHAR2 OR NUMBER
    }


}
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

    public class KeyField
    {
        public string keyName = "";
        public string keyValue = "";
        public string keyType = "";   //VARCHAR2 OR NUMBER
    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{

    #region execution
    public class Execution
    {
        public string id = "";
        public string instrument = "";
        public long price = 0;
        public string time = "";
        public long qty = 0;
        public string side = "";
    }
    #endregion

    #region orders
    public class orders
    {
        public string id = "";
        public string instrument = "";
        public long qty = 0;
        public string side = "";
        public string type = "";
        public long filledqty = 0;
        public long avgprice = 0;
        public long limitprice = 0;
        public long stopprice = 0;
        public string parentid = "";
        public string parenttype = "";
        public string duration = "";
        public string status = "";
        public long lastModified = 0;
    }
    #endregion

    #region ordersHistory
    public class ordersHistory
        {
        public string id = "";
        public string instrument = "";
        public long qty = 0;
        public string side = "";
        public string type = "";
        public long filledqty = 0;
        public long avgprice = 0;
        public long limitprice = 0;
        public long stopprice = 0;
        public string parentid = "";
        public string parenttype = "";
        public string duration = "";
        public string status = "";
        public long lastModified = 0;
    }
    #endregion
}


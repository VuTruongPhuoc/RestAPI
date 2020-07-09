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
}
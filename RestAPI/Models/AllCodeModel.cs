using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class AllCode
    {
        public string cdtype { get; set; }
        public string cdname { get; set; }
        public string cdval { get; set; }
        public string cdcontent { get; set; }
        public long lstodr { get; set; }
        public string cduser { get; set; }
        public string en_content { get; set; }
        public string chstatus { get; set; }
    }
}
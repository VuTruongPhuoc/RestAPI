using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using RestAPI.Models;
using RestAPI.Bussiness;
using log4net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace RestAPI.Controllers
{
    public class RootController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        
        [Route()]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getAccountExecutions(HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.OK, "Welcom To DNSE RestApi System");
                
        }
    }
}

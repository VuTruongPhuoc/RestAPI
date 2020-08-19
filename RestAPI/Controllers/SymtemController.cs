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
using System.Data;

namespace RestAPI.Controllers
{
    public class SymtemController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region symtem
        //Huy lenh
        [Route("symtem/HealthCheck")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage HealthCheck(HttpRequestMessage request)
        {
            string preFixlogSession = "symtem/HealthCheck/";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                    var result = Bussiness.SymtemProcess.HealthCheck();
                    if (result.GetType() == typeof(HealthCheck) && ((HealthCheck)result).errorCode == "200" )
                    {
                        var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.OK, result);
                        Log.Info(preFixlogSession + "======================END");
                        return responses;
                    }
                    else
                    {
                        var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.InternalServerError, result);
                        Log.Info(preFixlogSession + "======================END");
                        return responses;
                    }

            }
            catch (Exception ex)
            {
                Log.Error(preFixlogSession, ex);
                var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.InternalServerError, ex);
                Log.Info(preFixlogSession + "======================END");
                return responses;
            }
        }
        #endregion
    }
}

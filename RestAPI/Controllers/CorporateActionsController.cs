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
    public class CorporateActionsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Su kien quyen chia co tuc bang co phieu DNSE-1580: DNS.2021.10.1.44 
        [Route("corporate-actions/stock-dividend")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage stockdividend(HttpRequestMessage request)
        {
            string preFixlogSession = "corporate-actions/stock-dividend ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.CorporateActionsProcess.stockdividend(request.Content.ReadAsStringAsync().Result, ipaddress);
                    if (result.GetType() == typeof(BoResponse) && ((BoResponse)result).s == Constants.Result_OK)
                    {
                        var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.OK, result);
                        Log.Info(preFixlogSession + "======================END");
                        return responses;
                    }
                    else
                    {
                        var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.BadRequest, result);
                        Log.Info(preFixlogSession + "======================END");
                        return responses;
                    }
                }
                else
                {
                    var v_err_request = JObject.Parse("{'error': 400, 'message': 'Invalid Input Content-Type'}");
                    Log.Info(preFixlogSession + "======================END");
                    return request.CreateResponse(HttpStatusCode.BadRequest, v_err_request);
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

    }
}

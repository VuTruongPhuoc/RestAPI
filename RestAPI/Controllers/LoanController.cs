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
    public class LoanController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Thong tin mon vay DNS.2022.01.1.02
        [Route("loan/{id}/loanInfo")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getloaninfo(HttpRequestMessage request, string id)
        {
            string preFixlogSession = "loan/" + id + "/loanInfo " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.LoanProcess.getloaninfo(request.Content.ReadAsStringAsync().Result, id);
                    if (result.GetType() == typeof(Models.loanInfo) || result.GetType() == typeof(Bussiness.list))
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

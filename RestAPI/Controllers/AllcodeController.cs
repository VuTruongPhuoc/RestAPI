using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace RestAPI.Controllers
{
    public class AllcodeController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [System.Web.Http.Route("api/allcode")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAllcodes(HttpRequestMessage request)
        {
            string preFixlogSession = "cfmast";
            log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);
            try
            {
                if (request.Content.Headers.ContentType == null || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AllcodeProcess.getAllCodes();
                    if (result.GetType() == typeof(Models.AllCode) || result.GetType() == typeof(Bussiness.list))
                    {
                        var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.OK, result);
                        log.Info(preFixlogSession + "======================END");
                        return responses;
                    }
                    else
                    {
                        var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.BadRequest, result);
                        log.Info(preFixlogSession + "======================END");
                        return responses;
                    }
                }
                else
                {
                    var v_err_request = JObject.Parse("{'error': 400, 'message': 'Invalid Input Content-Type'}");
                    log.Info(preFixlogSession + "======================END");
                    return request.CreateResponse(HttpStatusCode.BadRequest, v_err_request);
                }
            }
            catch (Exception ex)
            {
                log.Error(preFixlogSession, ex);
                var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.InternalServerError, ex);
                log.Info(preFixlogSession + "======================END");
                return responses;
            }
        }
    }
}
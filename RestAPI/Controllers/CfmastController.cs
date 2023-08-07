using log4net;
using Newtonsoft.Json.Linq;
using RestAPI.Bussiness;
using RestAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestAPI.Controllers
{

    public class CfmastController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("api/cfmast")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getAllCfmast(HttpRequestMessage request)
        {
            string preFixlogSession = "cfmast";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.CfmastProcess.getAllCfmasts();
                    if (result.GetType() == typeof(Models.Cfmast) || result.GetType() == typeof(Bussiness.list))
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

        [Route("api/cfmast/{custid}")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getCfmastbyCustid(HttpRequestMessage request, string custid)
        {
            string preFixlogSession = "cfmast/" + custid;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.CfmastProcess.getCfmastByCustId(custid);
                    if (result.GetType() == typeof(Models.Cfmast) || result.GetType() == typeof(Bussiness.list))
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

        [Route("api/cfmast")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage postCfmast(HttpRequestMessage request)
        {
            string preFixlogSession = "cfmasts" + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");
                    var result = Bussiness.CfmastProcess.postCfmast(request.Content.ReadAsStringAsync().Result, ipaddress);
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
        [Route("api/cfmast/{custid}")]
        [System.Web.Http.HttpPut]
        public HttpResponseMessage putCfmast(HttpRequestMessage request, string custid)
        {
            string preFixlogSession = "cfmast/" + custid + " " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.CfmastProcess.putCfmast(request.Content.ReadAsStringAsync().Result, custid, ipaddress);
                    if (result.GetType() == typeof(BoResponse) && ((BoResponse)result).s == "ok")
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

        [Route("api/cfmast/{custid}")]
        [System.Web.Http.HttpDelete]
        public HttpResponseMessage deleteCfmast(HttpRequestMessage request, string custid)
        {
            string preFixlogSession = "cfmast/" + custid + " " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");
                    var result = Bussiness.CfmastProcess.deleteCfmast(request.Content.ReadAsStringAsync().Result,custid, ipaddress);
                    if (result.GetType() == typeof(BoResponse) && ((BoResponse)result).s == "ok")
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
        [Route("api/cfmast/getmaxcustid")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getMaxCustId(HttpRequestMessage request)
        {
            string preFixlogSession = "cfmast/getmaxcustid";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.CfmastProcess.getMaxCustidByCfmast();
                    if (result.GetType() == typeof(Models.MaxCustIdCfmast) || result.GetType() == typeof(Bussiness.list))
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

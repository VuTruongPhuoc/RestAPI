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
    public class AccountsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region accounts
        //Chi tiết khớp
        [Route("accounts/{accountNo}/executions")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getAccountExecutions(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/executions " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.getAccountExecutions(request.Content.ReadAsStringAsync().Result, accountNo);
                    if (result.GetType() == typeof(Models.Execution) || result.GetType() == typeof(Bussiness.list))
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

        
        //ordersHistory
        [Route("accounts/{accountNo}/ordersHistory")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getAccountordersHistory(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/ordersHistory " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.getAccountordersHistory(request.Content.ReadAsStringAsync().Result, accountNo);
                    if (result.GetType() == typeof(Models.Execution) || result.GetType() == typeof(Bussiness.list))
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


        //POST /accounts/{accountId}/deposit
        [Route("accounts/{accountNo}/deposit")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage postAccountsDeposit(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/postAccountsDeposit " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.bankDeposit(request.Content.ReadAsStringAsync().Result, accountNo);
                    if (result.GetType() == typeof(BoResponse))
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


        [Route("accounts/{accountNo}/getAvailableTrade")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getAvailableTrade(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/getAvailableTrade " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.getAvailableTrade(request.Content.ReadAsStringAsync().Result, accountNo);
                    if (result.GetType() == typeof(Models.PPSE) || result.GetType() == typeof(Bussiness.list))
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

        [Route("accounts/{accountNo}/summaryAccount")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getsummaryAccount(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/summaryAccount " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.getsummaryAccount(request.Content.ReadAsStringAsync().Result, accountNo);
                    if (result.GetType() == typeof(Models.PPSE) || result.GetType() == typeof(Bussiness.list))
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

        [Route("accounts/{accountNo}/securitiesPortfolio")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getsecuritiesPortfolio(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/securitiesPortfolio " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.getsecuritiesPortfolio(request.Content.ReadAsStringAsync().Result, accountNo);
                    if (result.GetType() == typeof(Models.PPSE) || result.GetType() == typeof(Bussiness.list))
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

        [Route("accounts/bankTransactions")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage trfMoney2Bank(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/bankTransactions " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");
                    var result = Bussiness.AccountProcess.trfMoney2Bank(request.Content.ReadAsStringAsync().Result, ipaddress);
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

        #endregion
    }
}

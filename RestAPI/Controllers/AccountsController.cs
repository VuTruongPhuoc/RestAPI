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
        #region "Api P1"
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
                    if (result.GetType() == typeof(Models.orders) || result.GetType() == typeof(Bussiness.list))
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
                    if (result.GetType() == typeof(BoResponse) && ((BoResponse) result).s == "ok")
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


        //POST API gửi tiết kiệm: DNSE-1666 DNS.2022.03.1.13
        [Route("accounts/savings")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage accountSaving(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/savings" + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.accountSaving(request.Content.ReadAsStringAsync().Result);
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
                    if (result.GetType() == typeof(Models.summaryAccount) || result.GetType() == typeof(Bussiness.list))
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
                    if (result.GetType() == typeof(Models.securitiesPortfolio) || result.GetType() == typeof(Bussiness.list))
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
        #endregion "Api P1"

        #region "Api P2"
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

        [Route("accounts/advancePayment")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage advancePayment(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/advancePayment ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.AccountProcess.advancePayment(request.Content.ReadAsStringAsync().Result, ipaddress);
                    if (result.GetType() == typeof(BoResponse) && ((BoResponse) result).s == Constants.Result_OK)
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

        [Route("accounts/cashTransfer")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage internalCashTranfer(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/cashTranfer";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);            

            try
            {
                if (request.Content.Headers.ContentType == null || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");
                    var result = Bussiness.AccountProcess.internalCashTranfer(request.Content.ReadAsStringAsync().Result, ipaddress);
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

        // API Chuyen tien tren suc mua thang du: DNSE-1584 DNS.2021.10.1.44
        [Route("accounts/CashWithdrawPPse")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage CashWithdrawPPse(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/CashWithdrawPPse";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");
                    var result = Bussiness.AccountProcess.CashWithdrawPPse(request.Content.ReadAsStringAsync().Result, ipaddress);
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

        [Route("accounts/stockTransfer")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage internalStockTranfer(HttpRequestMessage request)
        {
            string preFixlogSession = "internalStockTranfer";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");
                    var result = Bussiness.AccountProcess.internalStockTranfer(request.Content.ReadAsStringAsync().Result, ipaddress);
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

        [Route("accounts/rightRegister")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage rightRegister(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/rightRegister ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.AccountProcess.rightRegister(request.Content.ReadAsStringAsync().Result, ipaddress);
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

        [Route("accounts/receiveSecurities")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Inward_SE_Transfer(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/receiveSecurities ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.AccountProcess.Inward_SE_Transfer(request.Content.ReadAsStringAsync().Result, ipaddress);
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

        [Route("accounts/withdrawSecurities")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage OutwardSETransfer(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/withdrawSecurities ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.AccountProcess.OutwardSETransfer(request.Content.ReadAsStringAsync().Result, ipaddress);
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
        [Route("accounts/withdrawSecuritiesToCloseSubaccount")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage sendSecuritiesrightToClose(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/withdrawSecuritiesToCloseSubaccount ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.AccountProcess.sendSecuritiesrightToClose(request.Content.ReadAsStringAsync().Result, ipaddress);
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
        [Route("accounts/blockSecurities")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SecuritiesBlock(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/blockSecurities ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.AccountProcess.SecuritiesBlock(request.Content.ReadAsStringAsync().Result, ipaddress);
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

        [Route("accounts/updatecareby")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage updateCareby(HttpRequestMessage request)
        {
            string preFixlogSession = "accounts/updatecareby ";
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType == null
                    || request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string ipaddress = modCommon.getRequestHeaderValue(request, "client-ip");

                    var result = Bussiness.AccountProcess.updateCareby(request.Content.ReadAsStringAsync().Result, ipaddress);
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
        #endregion "Api P2"


        #endregion
    }
}

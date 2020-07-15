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
    public class TradingController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region trading
        //Chi tiết khớp
        [Route("accounts/{accountNo}/orders/{orderId}")]
        [System.Web.Http.HttpDelete]
        public HttpResponseMessage DeleteTradingOrders(HttpRequestMessage request, string accountNo, string orderId)
        {
            string preFixlogSession = "accounts/" + accountNo + "/orders" + orderId +" " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                object v_objResult;
                long v_lngErrorCode = 0;
                string v_strerrorMessage = string.Empty;
                string errorType = string.Empty;
                string returnKey = string.Empty;

                if (request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string strErrorMesage = string.Empty;
                    DataSet pv_dataSet = null;

                    string strValue = string.Empty;
                    strValue = "{\"accountNo\":\"" + accountNo + "\" }";

                    string v_strTxmessage = modCommon.buildTransMessage(request, strValue);

                    //v_lngErrorCode = TransactionProcess.doTransaction_Dataset("pr_delete_orders", v_strTxmessage, ref pv_dataSet, ref strErrorMesage, request.Method.ToString());
                    Log.Info("preFixlogSession: returnKey:" + strErrorMesage);

                    Log.Info("preFixlogSession: returnKey:" + strErrorMesage);
                    if (v_lngErrorCode == 0) //success
                    {
                        v_objResult = TradingProcess.getTrading_delt_orders(pv_dataSet);
                    }
                    else
                    {
                        v_strerrorMessage = strErrorMesage;
                        if (v_strerrorMessage == string.Empty || v_strerrorMessage == "")
                        {
                            v_strerrorMessage = "bad request!";
                        }
                        v_objResult = new ErrorMapHepper().getResponsesForType(v_lngErrorCode.ToString(), v_strerrorMessage, ref errorType);
                    }

                    if (v_objResult.GetType() == typeof(Models.OrderDelete) || v_objResult.GetType() == typeof(Bussiness.list))
                    {
                        var responses = Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.OK, v_objResult);
                        Log.Info(preFixlogSession + "======================END");
                        return responses;
                    }
                    else
                    {
                        var responses = Bussiness.ErrorMapHepper.CreateResponseError(request, v_objResult, errorType);
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

        //Dat lenh
        [Route("accounts/{accountNo}/orders")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage postTradingOrders(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/executions " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    string via = modCommon.getRequesetHeaderValue(request, "xvia");
                    string ipaddress = modCommon.getRequesetHeaderValue(request, "client-ip");

                    var result = Bussiness.TradingProcess.postTradingorders(request.Content.ReadAsStringAsync().Result, accountNo, ipaddress, via);
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

        //Orders
        [Route("accounts/{accountNo}/orders")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage getAccountorders(HttpRequestMessage request, string accountNo)
        {
            string preFixlogSession = "accounts/" + accountNo + "/orders " + request.Method;
            Log.Info(preFixlogSession + "======================BEGIN");
            Bussiness.modCommon.LogFullRequest(request);

            try
            {
                if (request.Content.Headers.ContentType.MediaType.ToLower() == "application/json")
                {
                    var result = Bussiness.AccountProcess.getAccountorders(request.Content.ReadAsStringAsync().Result, accountNo);
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
        #endregion
    }
}

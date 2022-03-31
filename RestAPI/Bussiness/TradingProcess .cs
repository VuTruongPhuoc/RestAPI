using System;
using RestAPI.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using CommonLibrary;
using System.Net.Sockets;
using System.Net;
namespace RestAPI.Bussiness
{
    public static class TradingProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string COMMAND_DEL_ORDERS = "fopks_restapi.pr_delete_orders";
        private static string COMMAND_POST_ORDERS = "fopks_restapi.pr_post_orders";
        private static string COMMAND_PUT_ORDERS = "fopks_restapi.pr_put_orders";
        private static string COMMAND_DIVIDEND_ORDERS = "fopks_restapi.pr_dividendorder";
        #region orders
        //huy lenh
        public static object delTradingorders(string strRequest, string accountNo, string orderId, string p_ipAddress)
        {
            try
            {
                string ipAddress = p_ipAddress;

                JObject request = JObject.Parse(strRequest);
                JToken jToken;

                string v_channel = "";
                string v_maker = "";

                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                if (request.TryGetValue("channel", out jToken))
                {
                    v_channel = jToken.ToString();
                }
                if (request.TryGetValue("maker", out jToken))
                {
                    v_maker = jToken.ToString();
                }

                string errparam = "";

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[8];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_orderid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = orderId;
                v_objParam.ParamSize = orderId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_userName";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = String.Empty;
                v_objParam.ParamSize = 0;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_ipaddress";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = ipAddress;
                v_objParam.ParamSize = ipAddress.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_via";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = v_channel;
                v_objParam.ParamSize = v_channel.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_maker";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = v_maker;
                v_objParam.ParamSize = v_maker.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                string v_strerrorMessage = string.Empty;
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DEL_ORDERS, ref v_arrParam, 6);
                errparam = (string)v_arrParam[7].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("delTradingorders: ", ex);
                return 1;
            }
        }

        //sua lenh
        public static object putTradingorders(string strRequest, string accountNo, string orderId, string p_ipAddress)
        {
            try
            {
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string limitprice = "" , errparam = "";
                long qty = 0;
                string v_channel = "";
                string v_maker = "";

                if (request.TryGetValue("channel", out jToken))
                {
                    v_channel = jToken.ToString();
                }
                if (request.TryGetValue("maker", out jToken))
                {
                    v_maker = jToken.ToString();
                }

                if (request.TryGetValue("qty", out jToken))
                    Int64.TryParse(jToken.ToString(), out qty);
                if (request.TryGetValue("limitprice", out jToken))
                    limitprice = jToken.ToString();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[10];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_orderid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = orderId;
                v_objParam.ParamSize = orderId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_userName";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = String.Empty;
                v_objParam.ParamSize = 0;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qty";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = qty;
                v_objParam.ParamSize = qty.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "limitPrice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = limitprice;
                v_objParam.ParamSize = limitprice.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_ipaddress";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = ipAddress;
                v_objParam.ParamSize = ipAddress.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_via";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = v_channel;
                v_objParam.ParamSize = v_channel.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_maker";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = v_maker;
                v_objParam.ParamSize = v_maker.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                string v_strerrorMessage = string.Empty;
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_PUT_ORDERS, ref v_arrParam, 8);
                errparam = (string)v_arrParam[9].ParamValue;

                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("putTradingorders: ", ex);
                return 1;
            }
        }

        //dat lenh
        public static object postTradingorders(string strRequest, string accountNo, string p_ipAddress, string p_via)
        {
            string logStr = "postTradingorders";

            Log.Info(logStr + "strRequest:" + strRequest.ToString());

            try
            {
                string via = p_via, ipAddress = p_ipAddress;
                if (via == null || via.Length == 0)
                    via = modCommon.getConfigValue("DEFAULT_VIA", "O");
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();


                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string instrument = "", requestid = "", side = "", type = "", limitprice = "", durationtype = "", timeInForce = "";
                string errcode = "", errparam = "";
                object v_orderid = "";
                string v_channel = "";
                string v_maker   = "";

                long qty = 0;

                if (request.TryGetValue("instrument", out jToken))
                    instrument = jToken.ToString();
                if (request.TryGetValue("requestid", out jToken))
                    requestid = jToken.ToString();
                if (request.TryGetValue("side", out jToken))
                    side = jToken.ToString();
                if (request.TryGetValue("type", out jToken))
                    type = jToken.ToString();
                if (request.TryGetValue("timeInForce", out jToken))
                    timeInForce = jToken.ToString();
                if (request.TryGetValue("limitprice", out jToken))
                    limitprice = jToken.ToString();
                if (request.TryGetValue("durationtype", out jToken))
                    durationtype = jToken.ToString();
                if (request.TryGetValue("channel", out jToken)){
                    v_channel = jToken.ToString();
                }
                if (request.TryGetValue("maker", out jToken))
                {
                    v_maker = jToken.ToString();
                }

                Log.Info(logStr + "v_channel:" + v_channel.ToString());
                Log.Info(logStr + "v_maker:" + v_maker.ToString());


                if (request.TryGetValue("qty", out jToken))
                    Int64.TryParse(jToken.ToString(), out qty);

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[15];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestid;
                v_objParam.ParamSize = requestid.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_userName";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = String.Empty;
                v_objParam.ParamSize = 0;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_instrument";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = instrument;
                v_objParam.ParamSize = instrument.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qty";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = qty;
                v_objParam.ParamSize = qty.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_side";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = side;
                v_objParam.ParamSize = side.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_type";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = type;
                v_objParam.ParamSize = type.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "timeInForce";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = timeInForce;
                v_objParam.ParamSize = timeInForce.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "limitPrice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = limitprice;
                v_objParam.ParamSize = limitprice.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_ipaddress";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = ipAddress;
                v_objParam.ParamSize = ipAddress.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_via";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = v_channel;
                v_objParam.ParamSize = v_channel.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_marker";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = v_maker;
                v_objParam.ParamSize = v_maker.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_orderid";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[14] = v_objParam;


                string v_strerrorMessage = string.Empty;
                string returnMsg = "";

                long returnErr = TransactionProcess.doTransaction(COMMAND_POST_ORDERS, ref v_arrParam, 13);
                errparam = (string) v_arrParam[14].ParamValue;

                if (returnErr == 0)
                {
                    v_orderid = new orderResponse() { orderid = (string)v_arrParam[12].ParamValue };
                    return modCommon.getBoResponseWithData(returnErr, v_orderid, errparam);
                }

                Log.Info(logStr + "returnErr:" + returnErr.ToString());
                return modCommon.getBoResponse(returnErr, returnMsg);

            }
            catch (Exception ex)
            {
                Log.Error("postTradingorders: ", ex);
                return modCommon.getBoResponse(-1);
            }
        }

        public static object dividendorder(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", foRefId = "", confirmNumber = "";
                long dealQuantity = 0, dividendQuantity = 0, dealPrice = 0;

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("foRefId", out jToken))
                    foRefId = jToken.ToString();
                if (request.TryGetValue("confirmNumber", out jToken))
                    confirmNumber = jToken.ToString();
                if (request.TryGetValue("dealQuantity", out jToken))
                    Int64.TryParse(jToken.ToString(), out dealQuantity);
                if (request.TryGetValue("dividendQuantity", out jToken))
                    Int64.TryParse(jToken.ToString(), out dividendQuantity);
                if (request.TryGetValue("dealPrice", out jToken))
                    Int64.TryParse(jToken.ToString(), out dealPrice);
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[8];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_foRefId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = foRefId;
                v_objParam.ParamSize = foRefId.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_confirmNumber";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = confirmNumber;
                v_objParam.ParamSize = confirmNumber.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dealQuantity";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dealQuantity;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.Int64").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dividendQuantity";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dividendQuantity;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.Int64").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dealPrice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dealPrice;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.Int64").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_DIVIDEND_ORDERS, ref v_arrParam, 6);
                string v_strerrorMessage = (string)v_arrParam[7].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("dividendorder " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        #endregion

    }
}
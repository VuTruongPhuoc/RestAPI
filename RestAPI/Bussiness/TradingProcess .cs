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
        #region orders
        //huy lenh
        public static object delTradingorders(string strRequest, string accountNo, string orderId, string p_ipAddress)
        {
            try
            {
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                string errparam = "";

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[6];

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
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                string v_strerrorMessage = string.Empty;
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DEL_ORDERS, ref v_arrParam, 4);
                errparam = (string)v_arrParam[5].ParamValue;


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

                if (request.TryGetValue("qty", out jToken))
                    Int64.TryParse(jToken.ToString(), out qty);
                if (request.TryGetValue("limitprice", out jToken))
                    limitprice = jToken.ToString();

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
                returnErr = TransactionProcess.doTransaction(COMMAND_PUT_ORDERS, ref v_arrParam, 6);
                errparam = (string)v_arrParam[7].ParamValue;

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
                

                if (request.TryGetValue("qty", out jToken))
                    Int64.TryParse(jToken.ToString(), out qty);

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[14];

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
                v_objParam.ParamValue = via;
                v_objParam.ParamSize = via.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_orderid";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                string v_strerrorMessage = string.Empty;
                string returnMsg = "";
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_POST_ORDERS, ref v_arrParam, 12);
                errparam = (string) v_arrParam[13].ParamValue;

                if (returnErr == 0)
                {
                    v_orderid = new orderResponse() { orderid = (string)v_arrParam[11].ParamValue };
                    return modCommon.getBoResponseWithData(returnErr, v_orderid, errparam);
                }

                return modCommon.getBoResponse(returnErr, returnMsg);

            }
            catch (Exception ex)
            {
                Log.Error("postTradingorders: ", ex);
                return modCommon.getBoResponse(-1);
            }
        }
        #endregion

    }
}
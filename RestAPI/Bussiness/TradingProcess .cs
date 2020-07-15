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
        #region orders
        public static object getTrading_delt_orders(DataSet data)
        {
            try
            {

                Models.OrderDelete[] orderDelete = null;
                if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    orderDelete = new OrderDelete[data.Tables[0].Rows.Count];
                    for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        orderDelete[i] = new OrderDelete()
                        {
                            errorCode = data.Tables[0].Rows[i]["ID"].ToString(),
                            errorMesage = data.Tables[0].Rows[i]["INSTRUSMENT"].ToString(),
                        };
                    }
                }
                return new list() { s = "ok", d = orderDelete };
            }
            catch (Exception ex)
            {
                Log.Error("get_accounts: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
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
                string orderid = "", errcode = "", errparam = "";

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
                if (request.TryGetValue("orderid", out jToken))
                    orderid = jToken.ToString();
                if (request.TryGetValue("errcode", out jToken))
                    errcode = jToken.ToString();
                if (request.TryGetValue("errparam", out jToken))
                    errparam = jToken.ToString();

                if (request.TryGetValue("qty", out jToken))
                    Int64.TryParse(jToken.ToString(), out qty);

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[13];

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
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                string v_strerrorMessage = string.Empty;
                string returnKey = "";
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_POST_ORDERS, ref v_arrParam, 11);
                errparam = (string )v_arrParam[12].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("get_accounts: ", ex);
                return 1;
            }
        }
        #endregion

    }
}
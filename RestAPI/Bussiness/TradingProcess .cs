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
        public static long postTradingorders(string strRequest, string accountNo)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string instrument = "";
                string requestid = "";
                string side = "";
                string type = "";
                string limitprice = "";
                string durationtype = "";
                string orderid = "";
                string errcode = "";
                string errparam = "";

                int qty = 0;
                int stopprice = 0;
                int stoploss = 0;
                int takeprofit = 0;

                if (request.TryGetValue("instrument", out jToken))
                    instrument = jToken.ToString();
                if (request.TryGetValue("requestid", out jToken))
                    requestid = jToken.ToString();
                if (request.TryGetValue("side", out jToken))
                    side = jToken.ToString();
                if (request.TryGetValue("type", out jToken))
                    type = jToken.ToString();
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
                    Int32.TryParse(jToken.ToString(), out qty);
                if (request.TryGetValue("stopprice", out jToken))
                    Int32.TryParse(jToken.ToString(), out stopprice);
                if (request.TryGetValue("stoploss", out jToken))
                    Int32.TryParse(jToken.ToString(), out stoploss);
                if(request.TryGetValue("takeprofit", out jToken))
                    Int32.TryParse(jToken.ToString(), out takeprofit);

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[15];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountid";
                v_objParam.ParamDirection = "0"; 
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1"; 
                v_objParam.ParamValue = requestid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_userName";
                v_objParam.ParamDirection = "1"; 
                v_objParam.ParamValue = System.Net.Dns.GetHostName();
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_instrument";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = instrument;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qty";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Convert.ToString(qty);
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_side";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = side;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_type";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = type;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_limitprice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = limitprice;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_stopprice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Convert.ToString(stopprice);
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_durationtype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = durationtype;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_stoploss";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Convert.ToString(stoploss);
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_ipaddress";
                v_objParam.ParamDirection = "1";
                foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork) // filter out ipv4
                    {
                        v_objParam.ParamValue = ipAddress.ToString();
                    }
                }
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_orderid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[14] = v_objParam;

                string v_strerrorMessage = string.Empty;
                string returnKey = "";
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_POST_ORDERS, v_strerrorMessage, ref returnKey, v_arrParam);

                
                    return returnErr;

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
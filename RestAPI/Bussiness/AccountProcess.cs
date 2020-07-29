using System;
using RestAPI.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using CommonLibrary;

namespace RestAPI.Bussiness
{
    public static class AccountProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string COMMAND_GET_EXECUTIONS = "fopks_restapi.pr_get_executions";
        private static string COMMAND_GET_ORDERS = "fopks_restapi.pr_get_orders";
        private static string COMMAND_GET_ORDERSHISTORY = "fopks_restapi.pr_get_ordersHistory";
        private static string COMMAND_GET_PPSE = "fopks_restapi.pr_getAvailableTrade";
        private static string COMMAND_DO_BANKDEPOSIT = "fopks_restapi.pr_post_deposit";
        #region execution
        public static object getAccountExecutions(string strRequest, string accountNo)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string instrument = "";
                int maxCount = 0;
                if (request.TryGetValue("instrument", out jToken))
                    instrument = jToken.ToString();

                if (request.TryGetValue("maxCount", out jToken))
                    Int32.TryParse(jToken.ToString(), out maxCount);

                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldAccountNo = new KeyField();
                fieldAccountNo.keyName = "p_accountid";
                fieldAccountNo.keyValue = accountNo;
                fieldAccountNo.keyType = "VARCHAR2";
                keyField.Add(fieldAccountNo);

                KeyField fieldIntrument = new KeyField();
                fieldIntrument.keyName = "p_instrument";
                fieldIntrument.keyValue = instrument;
                fieldIntrument.keyType = "VARCHAR2";
                keyField.Add(fieldIntrument);

                KeyField fieldMaxcount = new KeyField();
                fieldMaxcount.keyName = "p_maxcount";
                fieldMaxcount.keyValue = Convert.ToString(maxCount);
                fieldMaxcount.keyType = "NUMBER";
                keyField.Add(fieldMaxcount);

                //KeyField[] fields = new KeyField[3];
                //fields[0] = new KeyField() { keyName = "p_accountid", keyType = "VARCHAR2", keyValue = accountNo };
                //fields[1] = new KeyField() { keyName = "p_instrument", keyType = "VARCHAR2", keyValue = instrument };
                //fields[2] = new KeyField() { keyName = "p_maxcount", keyType = "NUMBER", keyValue = Convert.ToString(maxCount) };
                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_EXECUTIONS, keyField);

                Models.Execution[] execution = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    execution = new Execution[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        execution[i] = new Execution()
                        {
                            id = ds.Tables[0].Rows[i]["ID"].ToString(),
                            instrument = ds.Tables[0].Rows[i]["INSTRUSMENT"].ToString(),
                            price = Convert.ToInt64(ds.Tables[0].Rows[i]["PRICE"].ToString()),
                            time = ds.Tables[0].Rows[i]["TIME"].ToString(),
                            qty = Convert.ToInt32(ds.Tables[0].Rows[i]["QTY"].ToString()),
                            side = ds.Tables[0].Rows[i]["SIDE"].ToString()
                        };
                    }
                }
                return new list() { s = "ok", d = execution};
            }
            catch (Exception ex)
            {
                Log.Error("get_accounts: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        #endregion

        #region orders
        public static object getAccountorders(string strRequest, string accountNo)
        {
            try
            {
              
                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldAccountNo = new KeyField();
                fieldAccountNo.keyName = "p_accountid";
                fieldAccountNo.keyValue = accountNo;
                fieldAccountNo.keyType = "VARCHAR2";
                keyField.Add(fieldAccountNo);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_ORDERS, keyField);

                Models.orders[] order = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    order = new orders[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        order[i] = new orders()
                        {
                            id = ds.Tables[0].Rows[i]["ID"].ToString(),
                            instrument = ds.Tables[0].Rows[i]["INSTRUMENT"].ToString(),
                            qty = Convert.ToInt64(ds.Tables[0].Rows[i]["QTY"].ToString()),
                            side = ds.Tables[0].Rows[i]["SIDE"].ToString(),
                            type = ds.Tables[0].Rows[i]["TYPE"].ToString(),
                            filledqty = Convert.ToInt64(ds.Tables[0].Rows[i]["FILLEDQTY"].ToString()),
                            avgprice = Convert.ToInt64(ds.Tables[0].Rows[i]["AVGPRICE"].ToString()),
                            limitprice = Convert.ToInt64(ds.Tables[0].Rows[i]["LIMITPRICE"].ToString()),
                            stopprice = Convert.ToInt64(ds.Tables[0].Rows[i]["STOPPRICE"].ToString()),
                            parentid = ds.Tables[0].Rows[i]["PARENTID"].ToString(),
                            parenttype = ds.Tables[0].Rows[i]["PARENTTYPE"].ToString(),
                            duration = ds.Tables[0].Rows[i]["DURATION"].ToString(),
                            status = ds.Tables[0].Rows[i]["STATUS"].ToString(),
                            lastModified = Convert.ToInt64(ds.Tables[0].Rows[i]["LASTMODIFIED"].ToString())
                        };
                    }
                }
                
                return new list() { s = "ok", d = order };
            }
            catch (Exception ex)
            {
                Log.Error("get_accounts: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
       

        #endregion

        #region ordersHistory
        public static object getAccountordersHistory(string strRequest, string accountNo)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                int maxCount = 0;
                if (request.TryGetValue("maxCount", out jToken))
                    Int32.TryParse(jToken.ToString(), out maxCount);

                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldAccountNo = new KeyField();
                fieldAccountNo.keyName = "p_accountid";
                fieldAccountNo.keyValue = accountNo;
                fieldAccountNo.keyType = "VARCHAR2";
                keyField.Add(fieldAccountNo);

                KeyField fieldMaxcount = new KeyField();
                fieldMaxcount.keyName = "p_maxcount";
                fieldMaxcount.keyValue = Convert.ToString(maxCount);
                fieldMaxcount.keyType = "NUMBER";
                keyField.Add(fieldMaxcount);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_ORDERSHISTORY, keyField);

                Models.orders[] execution = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    execution = new orders[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        execution[i] = new orders()
                        {
                            id = ds.Tables[0].Rows[i]["ID"].ToString(),
                            instrument = ds.Tables[0].Rows[i]["INSTRUMENT"].ToString(),
                            qty = Convert.ToInt64(ds.Tables[0].Rows[i]["QTY"].ToString()),
                            side = ds.Tables[0].Rows[i]["SIDE"].ToString(),
                            type = ds.Tables[0].Rows[i]["TYPE"].ToString(),
                            filledqty = Convert.ToInt64(ds.Tables[0].Rows[i]["FILLEDQTY"].ToString()),
                            avgprice = Convert.ToInt64(ds.Tables[0].Rows[i]["AVGPRICE"].ToString()),
                            limitprice = Convert.ToInt64(ds.Tables[0].Rows[i]["LIMITPRICE"].ToString()),
                            stopprice = Convert.ToInt64(ds.Tables[0].Rows[i]["STOPPRICE"].ToString()),
                            parentid = ds.Tables[0].Rows[i]["PARENTID"].ToString(),
                            parenttype = ds.Tables[0].Rows[i]["PARENTTYPE"].ToString(),
                            duration = ds.Tables[0].Rows[i]["DURATION"].ToString(),
                            status = ds.Tables[0].Rows[i]["STATUS"].ToString(),
                            lastModified = Convert.ToInt64(ds.Tables[0].Rows[i]["LASTMODIFIED"].ToString())
                        };
                    }
                }

                return new list() { s = "ok", d = execution };
            }
            catch (Exception ex)
            {
                Log.Error("get_accounts: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        public static object getAvailableTrade(string strRequest, string accountNo)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string symbol = "", via = "";
                long price = 0;

                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                //if (request.TryGetValue("via", out jToken))
                //    via = jToken.ToString();
                via = modCommon.getConfigValue("DEFAULT_VIA", "B");
                if (request.TryGetValue("price", out jToken))
                    Int64.TryParse(jToken.ToString(), out price);

                List<KeyField> keyField = new List<KeyField>();

                keyField.Add(new KeyField() { keyName = "p_accountId", keyType = "VARCHAR2", keyValue = accountNo });
                keyField.Add(new KeyField() { keyName = "p_symbol", keyType = "VARCHAR2", keyValue = symbol });
                keyField.Add(new KeyField() { keyName = "p_quoteprice", keyType = "VARCHAR2", keyValue = price.ToString() });
                keyField.Add(new KeyField() { keyName = "p_via", keyType = "VARCHAR2", keyValue = via });
                
                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_PPSE, keyField);

                Models.PPSE[] ppse = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ppse = new PPSE[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ppse[i] = new PPSE()
                        {
                            ppse = Int64.Parse(ds.Tables[0].Rows[i]["PPSE"].ToString()),
                            maxQtty = Int64.Parse(ds.Tables[0].Rows[i]["MAXQTTY"].ToString()),
                            trade = Int64.Parse(ds.Tables[0].Rows[i]["TRADE"].ToString())
                        };
                    }
                }

                return new list() { s = "ok", d = ppse };
            }
            catch (Exception ex)
            {
                Log.Error("getAvailableTrade: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        public static object getOrder(string accountNo, string orderid)
        {
            try
            {
                Log.Info(String.Format("Begin getOrder: acctno={0} orderid={1}", accountNo,orderid));
                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldAccountNo = new KeyField();
                fieldAccountNo.keyName = "p_accountid";
                fieldAccountNo.keyValue = accountNo;
                fieldAccountNo.keyType = "VARCHAR2";
                keyField.Add(fieldAccountNo);

                KeyField fieldOrderId = new KeyField();
                fieldOrderId.keyName = "p_orderid";
                fieldOrderId.keyValue = orderid;
                fieldOrderId.keyType = "VARCHAR2";
                keyField.Add(fieldOrderId);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_ORDERS, keyField);

                Models.ordersInfo[] orders = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    orders = new ordersInfo[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        orders[i] = new ordersInfo()
                        {
                            txdate = ds.Tables[0].Rows[i]["TXDATE"].ToString(),
                            txtime = ds.Tables[0].Rows[i]["TXTIME"].ToString(),
                            action = ds.Tables[0].Rows[i]["ACTION"].ToString(),
                            orderId =  ds.Tables[0].Rows[i]["ORDERID"].ToString(),
                            matchQtty = Convert.ToInt64(ds.Tables[0].Rows[i]["MATCHQTTY"].ToString()),
                            matchPrice = Convert.ToInt64(ds.Tables[0].Rows[i]["MATCHPRICE"].ToString()),
                            quoteQtty = Convert.ToInt64(ds.Tables[0].Rows[i]["QUOTEQTTY"].ToString()),
                            quotePrice = Convert.ToInt64(ds.Tables[0].Rows[i]["QUOTEPRICE"].ToString())
                        };
                    }
                }
                Log.Info(String.Format("End getOrder: acctno={0} orderid={1}", accountNo, orderid));
                return new list() { s = "ok", d = orders };
            }
            catch (Exception ex)
            {
                Log.Error("getOrder: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        #endregion

        #region "Account Transaction"
        public static object bankDeposit(string strRequest, string accountNo)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string bankAccount = "", bankTxnum = "", desc = "";
                long amt = 0;

                if (request.TryGetValue("bankAccount", out jToken))
                    bankAccount = jToken.ToString();
                if (request.TryGetValue("bankTxnum", out jToken))
                    bankTxnum = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    desc = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    Int64.TryParse(jToken.ToString(), out amt);



                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[7];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_bankacctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankAccount;
                v_objParam.ParamSize = bankAccount.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_refnum";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankTxnum;
                v_objParam.ParamSize = bankTxnum.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amt;
                v_objParam.ParamSize = amt.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_desc";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = desc;
                v_objParam.ParamSize = desc.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_BANKDEPOSIT, ref v_arrParam, 5);
                string errparam = (string) v_arrParam[6].ParamValue;


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
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
        private static string COMMAND_DO_SUMMARY = "fopks_restapi.pr_getSummaryAccount";
        private static string COMMAND_DO_SECURITIES = "fopks_restapi.pr_getSecuritiesPortfolio";
        private static string COMMAND_TRF_MONEY_2_BANK = "fopks_restapi.pr_trfMoney2Bank";

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
                Log.Error("getAccountExecutions: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        #endregion

        #region orders
        public static object getOrders(string accountNo)
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
                            timeInForce = ds.Tables[0].Rows[i]["TIMEINFORCE"].ToString(),
                            filledqty = Convert.ToInt64(ds.Tables[0].Rows[i]["FILLEDQTY"].ToString()),
                            avgprice = Convert.ToDouble(ds.Tables[0].Rows[i]["AVGPRICE"].ToString()),
                            limitprice = Convert.ToInt64(ds.Tables[0].Rows[i]["LIMITPRICE"].ToString()),
                            stopprice = Convert.ToInt64(ds.Tables[0].Rows[i]["STOPPRICE"].ToString()),
                            parentid = ds.Tables[0].Rows[i]["PARENTID"].ToString(),
                            parenttype = ds.Tables[0].Rows[i]["PARENTTYPE"].ToString(),
                            duration = ds.Tables[0].Rows[i]["DURATION"].ToString(),
                            status = ds.Tables[0].Rows[i]["STATUS"].ToString(),
                            lastModified = ds.Tables[0].Rows[i]["LASTMODIFIED"].ToString(),
                            createdDate = ds.Tables[0].Rows[i]["CREATETIME"].ToString(),
                            type = ds.Tables[0].Rows[i]["TYPE"].ToString(),
                            txdate = ds.Tables[0].Rows[i]["TXDATE"].ToString()
                        };
                    }
                }
                
                return new list() { s = "ok", d = order };
            }
            catch (Exception ex)
            {
                Log.Error("getOrders: ", ex);
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
                string frDate, toDate, symbol = "", side = "";
                frDate = request.GetValue("frDate").ToString();
                toDate = request.GetValue("toDate").ToString();

                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("side", out jToken))
                    side = jToken.ToString();

                List<KeyField> lstField = new List<KeyField>();

                lstField.Add(new KeyField() { keyName = "p_accountid", keyValue = accountNo, keyType = "VARCHAR2" });
                lstField.Add(new KeyField() { keyName = "p_frDate", keyValue = frDate, keyType = "VARCHAR2" });
                lstField.Add(new KeyField() { keyName = "p_toDate", keyValue = toDate, keyType = "VARCHAR2" });
                lstField.Add(new KeyField() { keyName = "p_symbol", keyValue = symbol, keyType = "VARCHAR2" });
                lstField.Add(new KeyField() { keyName = "p_side", keyValue = side, keyType = "VARCHAR2" });
                

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_ORDERSHISTORY, lstField);

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
                            avgprice = Convert.ToDouble(ds.Tables[0].Rows[i]["AVGPRICE"].ToString()),
                            limitprice = Convert.ToInt64(ds.Tables[0].Rows[i]["LIMITPRICE"].ToString()),
                            stopprice = Convert.ToInt64(ds.Tables[0].Rows[i]["STOPPRICE"].ToString()),
                            parentid = ds.Tables[0].Rows[i]["PARENTID"].ToString(),
                            parenttype = ds.Tables[0].Rows[i]["PARENTTYPE"].ToString(),
                            duration = ds.Tables[0].Rows[i]["DURATION"].ToString(),
                            status = ds.Tables[0].Rows[i]["STATUS"].ToString(),
                            lastModified = ds.Tables[0].Rows[i]["LASTMODIFIED"].ToString(),
                            createdDate = ds.Tables[0].Rows[i]["CREATETIME"].ToString(),
                            timeInForce = ds.Tables[0].Rows[i]["TIMEINFORCE"].ToString(),
                            txdate = ds.Tables[0].Rows[i]["TXDATE"].ToString()
                        };
                    }
                }

                return new list() { s = "ok", d = execution };
            }
            catch (Exception ex)
            {
                Log.Error("getAccountordersHistory: ", ex);
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

        public static object getOrders(string accountNo, string orderid)
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
                Log.Error("getOrders: ", ex);
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
                Log.Error("bankDeposit: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
        public static object trfMoney2Bank(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string custodycd = "", bankAccountNumber = "", feeType = "", accountId = "", feeCode = "";
                long withdrawAmount = 0;
                string refId = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                custodycd = request.GetValue("custodyCode").ToString();
                accountId = request.GetValue("accountId").ToString();
                bankAccountNumber = request.GetValue("bankAccountNumber").ToString();
                feeType = request.GetValue("feeType").ToString();
                refId = request.GetValue("refId").ToString();

                jToken = request.GetValue("withdrawAmount");
                if (!Int64.TryParse(jToken.ToString(), out withdrawAmount))
                    return modCommon.getBoResponse(-10020);
                if (request.TryGetValue("feeCode", out jToken))
                    feeCode = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[10];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custodycd;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_withdrawAmount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = withdrawAmount;
                v_objParam.ParamSize = withdrawAmount.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_bankAccountNumber";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankAccountNumber;
                v_objParam.ParamSize = bankAccountNumber.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeType";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeType;
                v_objParam.ParamSize = feeType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeCOde";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeCode;
                v_objParam.ParamSize = feeCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_refId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = refId;
                v_objParam.ParamSize = refId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_ipAddress";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = ipAddress;
                v_objParam.ParamSize = ipAddress.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_TRF_MONEY_2_BANK, ref v_arrParam, 8);
                string v_strerrorMessage = (string)v_arrParam[9].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("trfMoney2Bank:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        #endregion

        #region summaryAccount
        public static object getsummaryAccount(string strRequest, string accountNo)
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
                ds = GetDataProcess.executeGetData(COMMAND_DO_SUMMARY, keyField);

                Models.summaryAccount[] summary = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    summary = new summaryAccount[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        summary[i] = new summaryAccount()
                        {
                            balance = Convert.ToInt64(ds.Tables[0].Rows[i]["BALANCE"].ToString()),
                            cibalance = Convert.ToInt64(ds.Tables[0].Rows[i]["CIBALANCE"].ToString()),
                            tdbalance = Convert.ToInt64(ds.Tables[0].Rows[i]["TDBALANCE"].ToString()),
                            interestbalance = Convert.ToInt64(ds.Tables[0].Rows[i]["INTERESTBALANCE"].ToString()),
                            receivingt1 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT1"].ToString()),
                            receivingt2 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT2"].ToString()),
                            receivingt3 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT3"].ToString()),
                            securitiesamt = Convert.ToInt64(ds.Tables[0].Rows[i]["SECURITIESAMT"].ToString()),
                            marginqttyamt = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINQTTYAMT"].ToString()),
                            nonmarginqttyamt = Convert.ToInt64(ds.Tables[0].Rows[i]["NONMARGINQTTYAMT"].ToString()),
                            dfqttyamt = Convert.ToInt64(ds.Tables[0].Rows[i]["DFQTTYAMT"].ToString()),
                            totaldebtamt = Convert.ToInt64(ds.Tables[0].Rows[i]["TOTALDEBTAMT"].ToString()),
                            secureamt = Convert.ToInt64(ds.Tables[0].Rows[i]["SECUREAMT"].ToString()),
                            trfbuyamt = Convert.ToInt64(ds.Tables[0].Rows[i]["TRFBUYAMT"].ToString()),
                            marginamt = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINAMT"].ToString()),
                            t0debtamt = Convert.ToInt64(ds.Tables[0].Rows[i]["T0DEBTAMT"].ToString()),
                            advancedamt = Convert.ToInt64(ds.Tables[0].Rows[i]["ADVANCEDAMT"].ToString()),
                            dfdebtamt = Convert.ToInt64(ds.Tables[0].Rows[i]["DFDEBTAMT"].ToString()),
                            tddebtamt = Convert.ToInt64(ds.Tables[0].Rows[i]["TDDEBTAMT"].ToString()),
                            depofeeamt = Convert.ToInt64(ds.Tables[0].Rows[i]["DEPOFEEAMT"].ToString()),
                            netassetvalue = Convert.ToInt64(ds.Tables[0].Rows[i]["NETASSETVALUE"].ToString()),
                            requiredmarginamt = Convert.ToInt64(ds.Tables[0].Rows[i]["REQUIREDMARGINAMT"].ToString()),
                            sesecuredavl = Convert.ToInt64(ds.Tables[0].Rows[i]["SESECUREDAVL"].ToString()),
                            sesecured_buy = Convert.ToInt64(ds.Tables[0].Rows[i]["SESECURED_BUY"].ToString()),
                            accountvalue = Convert.ToInt64(ds.Tables[0].Rows[i]["ACCOUNTVALUE"].ToString()),
                            qttyamt = Convert.ToInt64(ds.Tables[0].Rows[i]["QTTYAMT"].ToString()),
                            mrcrlimit = Convert.ToInt64(ds.Tables[0].Rows[i]["MRCRLIMIT"].ToString()),
                            bankavlbal = Convert.ToInt64(ds.Tables[0].Rows[i]["BANKAVLBAL"].ToString()),
                            debtamt = Convert.ToInt64(ds.Tables[0].Rows[i]["DEBTAMT"].ToString()),
                            advancemaxamtfee = Convert.ToInt64(ds.Tables[0].Rows[i]["ADVANCEMAXAMTFEE"].ToString()),
                            receivingamt = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGAMT"].ToString()),
                            basicpurchasingpower = Convert.ToInt64(ds.Tables[0].Rows[i]["BASICPURCHASINGPOWER"].ToString()),
                            marginrate = Convert.ToDouble(ds.Tables[0].Rows[i]["MARGINRATE"].ToString()),
                            holdbalance = Convert.ToInt64(ds.Tables[0].Rows[i]["HOLDBALANCE"].ToString()),
                            bankinqirytime = ds.Tables[0].Rows[i]["BANKINQIRYTIME"].ToString()
                        };
                    }
                }

                return new list() { s = "ok", d = summary };
            }
            catch (Exception ex)
            {
                Log.Error("getsummaryAccount: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
        #endregion

        #region summaryAccount
        public static object getsecuritiesPortfolio(string strRequest, string accountNo)
        {
            try
            {
                string symbol = "";
                if (strRequest != null && strRequest.Length > 0)
                {
                    JObject request = JObject.Parse(strRequest);
                    JToken jToken;

                    if (request.TryGetValue("symbol", out jToken))
                        symbol = jToken.ToString();
                }
                
                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldAccountNo = new KeyField();
                fieldAccountNo.keyName = "p_accountid";
                fieldAccountNo.keyValue = accountNo;
                fieldAccountNo.keyType = "VARCHAR2";
                keyField.Add(fieldAccountNo);

                KeyField fieldSymbol = new KeyField();
                fieldSymbol.keyName = "p_symbol";
                fieldSymbol.keyValue = symbol;
                fieldSymbol.keyType = "VARCHAR2";
                keyField.Add(fieldSymbol);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_DO_SECURITIES, keyField);

                Models.securitiesPortfolio[] securities = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    securities = new securitiesPortfolio[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        securities[i] = new securitiesPortfolio()
                        {
                            custodycd = ds.Tables[0].Rows[i]["CUSTODYCD"].ToString(),
                            accountid = ds.Tables[0].Rows[i]["ACCOUNTID"].ToString(),
                            symbol = ds.Tables[0].Rows[i]["SYMBOL"].ToString(),
                            total = Convert.ToInt64(ds.Tables[0].Rows[i]["TOTAL"].ToString()),
                            trade = Convert.ToInt64(ds.Tables[0].Rows[i]["TRADE"].ToString()),
                            blocked = Convert.ToInt64(ds.Tables[0].Rows[i]["BLOCKED"].ToString()),
                            vsdmortgage = Convert.ToInt64(ds.Tables[0].Rows[i]["VSDMORTGAGE"].ToString()),
                            mortgage = Convert.ToInt64(ds.Tables[0].Rows[i]["MORTGAGE"].ToString()),
                            restrict = Convert.ToInt64(ds.Tables[0].Rows[i]["RESTRICT"].ToString()),
                            receivingright = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGRIGHT"].ToString()),
                            receivingt0 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT0"].ToString()),
                            receivingt1 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT1"].ToString()),
                            receivingt2 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT2"].ToString()),
                            costprice = Double.Parse(ds.Tables[0].Rows[i]["COSTPRICE"].ToString()),
                            costpriceamt = Convert.ToInt64(ds.Tables[0].Rows[i]["COSTPRICEAMT"].ToString()),
                            basicprice = Convert.ToInt64(ds.Tables[0].Rows[i]["BASICPRICE"].ToString()),
                            basicpriceamt = Convert.ToInt64(ds.Tables[0].Rows[i]["BASICPRICEAMT"].ToString()),
                            marginratio = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINRATIO"].ToString()),
                            requiredmarginamt = Convert.ToInt64(ds.Tables[0].Rows[i]["REQUIREDMARGINAMT"].ToString()),
                            marginamt = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINAMT"].ToString()),
                            pnlamt = Convert.ToInt64(ds.Tables[0].Rows[i]["PNLAMT"].ToString()),
                            pnlrate = ds.Tables[0].Rows[i]["PNLRATE"].ToString(),
                            issell = ds.Tables[0].Rows[i]["ISSELL"].ToString(),
                            withdraw = Convert.ToInt64(ds.Tables[0].Rows[i]["WITHDRAW"].ToString())
                        };
                    }
                }

                return new list() { s = "ok", d = securities };
            }
            catch (Exception ex)
            {
                Log.Error("getsecuritiesPortfolio: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
        #endregion

        
    }
}
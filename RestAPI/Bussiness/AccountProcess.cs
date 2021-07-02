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
        private static string COMMAND_DO_ADVANCEPAYMENT = "fopks_restapi.pr_advancePayment";
        private static string COMMAND_DO_RIGHT_REGISTER = "fopks_restapi.pr_rightRegister";
        private static string COMMAND_DO_CASH_TRANFER = "fopks_restapi.pr_post_internalCashTranfer";
        private static string COMMAND_DO_STOCK_TRANFER = "fopks_restapi.pr_post_inStockTranfer";
        private static string COMMAND_PO_INWARD_SE_TRANFER = "fopks_restapi.pr_post_Inward_SE_Transfer";
        private static string COMMAND_PO_OUTWARD_SE_TRANFER = "fopks_restapi.pr_post_outward_SE_Transfer";
        private static string COMMAND_PO_SENDSECURITIES_RIGHTTOCLOSE = "fopks_restapi.pr_sendSecurities_rightToClose";
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
                    maxCount = Int32.Parse(jToken.ToString());

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
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                if (request.TryGetValue("channel", out jToken))
                    via = jToken.ToString();
                else
                    via = modCommon.getConfigValue("DEFAULT_VIA", "B");
                if (request.TryGetValue("price", out jToken))
                    price = Int64.Parse(jToken.ToString());

                List<KeyField> keyField = new List<KeyField>();

                keyField.Add(new KeyField() { keyName = "p_accountId", keyType = "VARCHAR2", keyValue = accountNo });
                keyField.Add(new KeyField() { keyName = "p_symbol", keyType = "VARCHAR2", keyValue = symbol });
                keyField.Add(new KeyField() { keyName = "p_quoteprice", keyType = "VARCHAR2", keyValue = price.ToString() });
                keyField.Add(new KeyField() { keyName = "p_via", keyType = "VARCHAR2", keyValue = via });
                
                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_PPSE, keyField);

                Models.PPSE[] ppse = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    orders = new ordersInfo[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        orders[i] = new ordersInfo()
                        {
                            lastModified = ds.Tables[0].Rows[i]["TXDATE"].ToString(),
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
                string bankAccount = "", bankTxnum = "", desc = "", flag = "";
                long amt = 0;

                if (request.TryGetValue("bankAccount", out jToken))
                    bankAccount = jToken.ToString();
                if (request.TryGetValue("bankTxnum", out jToken))
                    bankTxnum = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    desc = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    Int64.TryParse(jToken.ToString(), out amt);
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();



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
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
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

                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_BANKDEPOSIT, ref v_arrParam, 6);
                string errparam = (string) v_arrParam[7].ParamValue;


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
                string custodycd = "", bankAccountNumber = "", feeType = "", accountId = "", feeCode = "", bankCode = "", bankBranchCode = "";
                long withdrawAmount = 0;
                string refId = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                custodycd = request.GetValue("custodyCode").ToString();
                accountId = request.GetValue("accountId").ToString();
                bankAccountNumber = request.GetValue("bankAccountNumber").ToString();
                feeType = request.GetValue("feeType").ToString();
                bankCode = request.GetValue("bankCode").ToString();
                bankBranchCode = request.GetValue("bankBranchCode").ToString();
                refId = request.GetValue("refId").ToString();

                jToken = request.GetValue("withdrawAmount");
                if (!Int64.TryParse(jToken.ToString(), out withdrawAmount))
                    return modCommon.getBoResponse(-10020);
                if (request.TryGetValue("feeCode", out jToken))
                    feeCode = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                flag = request.GetValue("flag").ToString();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[13];

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
                v_objParam.ParamName = "p_bankCode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankCode;
                v_objParam.ParamSize = bankCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_bankBranchCode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankBranchCode;
                v_objParam.ParamSize = bankBranchCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeType";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeType;
                v_objParam.ParamSize = feeType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeCode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeCode;
                v_objParam.ParamSize = feeCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_refId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = refId;
                v_objParam.ParamSize = refId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_ipAddress";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = ipAddress;
                v_objParam.ParamSize = ipAddress.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_TRF_MONEY_2_BANK, ref v_arrParam, 11);
                string v_strerrorMessage = (string)v_arrParam[12].ParamValue;

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
    
        public static object Inward_SE_Transfer(string strRequest, string p_ipAddress)
       {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestid = "", account = "", infee = "", infeesv = "", symbol = "", typetransfer = "", inward = "", description = "", flag = "",
                       quantitytransfer = "0", quantityblock = "0", quantitytransferbonus = "0", quantitytransferdividend = "0", price = "";
                                  

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestid", out jToken))
                    requestid = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    account = jToken.ToString();
                if (request.TryGetValue("infee", out jToken))
                    infee = jToken.ToString();
                if (request.TryGetValue("infeesv", out jToken))
                    infeesv = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("typetransfer", out jToken))
                    typetransfer = jToken.ToString();
                if (request.TryGetValue("inward", out jToken))
                    inward = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("quantitytransfer", out jToken))
                    quantitytransfer = jToken.ToString();
                if (request.TryGetValue("quantityblock", out jToken))
                    quantityblock = jToken.ToString();
                if (request.TryGetValue("quantitytransferbonus", out jToken))
                    quantitytransferbonus = jToken.ToString();
                if (request.TryGetValue("quantitytransferdividend", out jToken))
                    quantitytransferdividend = jToken.ToString();
                if (request.TryGetValue("price", out jToken))
                    price = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[16];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestid;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_account";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = account;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_infee";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = infee;
                v_objParam.ParamSize = infee.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_infeesv";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = infeesv;
                v_objParam.ParamSize = infeesv.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_symbol";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = symbol;
                v_objParam.ParamSize = symbol.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_typetransfer";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = typetransfer;
                v_objParam.ParamSize = typetransfer.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_inward";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = inward;
                v_objParam.ParamSize = inward.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantitytransfer";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantitytransfer;
                v_objParam.ParamSize = quantitytransfer.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantityblock";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityblock;
                v_objParam.ParamSize = quantityblock.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantitytransferbonus";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantitytransferbonus;
                v_objParam.ParamSize = quantitytransferbonus.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantitytransferdividend";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantitytransferdividend;
                v_objParam.ParamSize = quantitytransferdividend.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_price";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = price;
                v_objParam.ParamSize = price.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[14] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[15] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_INWARD_SE_TRANFER, ref v_arrParam, 14);
                string v_strerrorMessage = (string)v_arrParam[15].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("Inward_SE_Transfer:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object OutwardSETransfer(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestid = "", infee = "", infeesv = "", symbol = "", typetransfer = "", vsdmessage = "", acctno = "", flag = "",
                       transferto = "", custodycdto = "", acctnoto = "",description = "", quantitytransfer = "0", quantityblock = "0", price = "";


                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestid", out jToken))
                    requestid = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("infee", out jToken))
                    infee = jToken.ToString();
                if (request.TryGetValue("infeesv", out jToken))
                    infeesv = jToken.ToString();
                if (request.TryGetValue("typetransfer", out jToken))
                    typetransfer = jToken.ToString();
                if (request.TryGetValue("vsdmessage", out jToken))
                    vsdmessage = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    acctno = jToken.ToString();
                if (request.TryGetValue("transferto", out jToken))
                    transferto = jToken.ToString();
                    description = jToken.ToString();
                if (request.TryGetValue("quantitytransfer", out jToken))
                    quantitytransfer = jToken.ToString();
                if (request.TryGetValue("quantityblock", out jToken))
                    quantityblock = jToken.ToString();
                if (request.TryGetValue("custodycdto", out jToken))
                    custodycdto = jToken.ToString();
                if (request.TryGetValue("accountto", out jToken))
                    acctnoto = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("price", out jToken))
                    price = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[17];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestid;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_symbol";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = symbol;
                v_objParam.ParamSize = symbol.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_infee";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = infee;
                v_objParam.ParamSize = infee.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_infeesv";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = infeesv;
                v_objParam.ParamSize = infeesv.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_typetransfer";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = typetransfer;
                v_objParam.ParamSize = typetransfer.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_vsdmessage";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = vsdmessage;
                v_objParam.ParamSize = vsdmessage.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = acctno;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_transferto";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferto;
                v_objParam.ParamSize = transferto.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantitytransfer";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantitytransfer;
                v_objParam.ParamSize = quantitytransfer.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantityblock";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityblock;
                v_objParam.ParamSize = quantityblock.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycdto";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custodycdto;
                v_objParam.ParamSize = custodycdto.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctnoto";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = acctnoto;
                v_objParam.ParamSize = acctnoto.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_price";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = price;
                v_objParam.ParamSize = price.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[14] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[15] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[16] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_OUTWARD_SE_TRANFER, ref v_arrParam, 15);
                string v_strerrorMessage = (string)v_arrParam[16].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("OutwardSETransfer:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        public static object sendSecuritiesrightToClose(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestid = "", symbol = "", account = "", accountto = "", flag = "",
                      description = "";


                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestid", out jToken))
                    requestid = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    account = jToken.ToString();
                if (request.TryGetValue("accountto", out jToken))
                    accountto = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[8];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestid;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_symbol";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = symbol;
                v_objParam.ParamSize = symbol.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = account;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctnoto";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountto;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_SENDSECURITIES_RIGHTTOCLOSE, ref v_arrParam, 6);
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
                Log.Error("sendSecuritiesrightToClose:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object advancePayment(string strRequest, string p_ipAddress)
        {
           
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string accountId = "", custodyCode = "", matchedDate = "", feeCode = "", description = "";
                long advanceAmount = 0;
                string refId = "", flag = "";

                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("custodyCode", out jToken))
                    custodyCode = jToken.ToString();
                if (request.TryGetValue("matchedDate", out jToken))
                    matchedDate = jToken.ToString();
                if (request.TryGetValue("feeCode", out jToken))
                    feeCode = jToken.ToString();
                if (request.TryGetValue("Desc", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("refId", out jToken))
                    refId = jToken.ToString();
                if (request.TryGetValue("advanceAmount", out jToken))
                    Int64.TryParse(jToken.ToString(), out advanceAmount);
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[11];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = accountId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodyCode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custodyCode;
                v_objParam.ParamSize = custodyCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_matchedDate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = matchedDate;
                v_objParam.ParamSize = matchedDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeCode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeCode;
                v_objParam.ParamSize = feeCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_advanceAmount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = advanceAmount;
                v_objParam.ParamSize = advanceAmount.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_refId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = refId;
                v_objParam.ParamSize = refId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_desc";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
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
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_ADVANCEPAYMENT, ref v_arrParam, 9);
                string errparam = (string)v_arrParam[10].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("advancePayment: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        public static object internalStockTranfer(string strRequest, string p_ipAddress)
        {
            string preFixlogSession = "internalStockTranfer";
            Log.Info(preFixlogSession + "======================BEGIN");

            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;

                string requestid = "";
                string account = "";
                string symbol = "";
                string quantity = "";
                string blockedqtty = "";
                string price = "";
                string toaccount = "";
                string desc = "";
                string flag = "";

                if (request.TryGetValue("requestid", out jToken))
                    requestid = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    account = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("tradeQuantity", out jToken))
                    quantity = jToken.ToString();
                //if (request.TryGetValue("blockedqtty", out jToken))
                //    blockedqtty = jToken.ToString();
                //if (request.TryGetValue("price", out jToken))
                //    price = jToken.ToString();
                if (request.TryGetValue("toAccount", out jToken))
                    toaccount = jToken.ToString();
                if (request.TryGetValue("Description", out jToken))
                    desc = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[10];
                v_arrParam[0] = new StoreParameter() { ParamName = "p_requestid", ParamDirection = "1", ParamValue = requestid, ParamSize = requestid.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[1] = new StoreParameter() { ParamName = "p_account", ParamDirection = "1", ParamValue = account, ParamSize = account.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[2] = new StoreParameter() { ParamName = "p_symbol", ParamDirection = "1", ParamValue = symbol, ParamSize = symbol.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[3] = new StoreParameter() { ParamName = "p_quantity", ParamDirection = "1", ParamValue = quantity, ParamSize = quantity.Length, ParamType = Type.GetType("System.String").Name };
                //v_arrParam[4] = new StoreParameter() { ParamName = "p_blockedqtty", ParamDirection = "1", ParamValue = blockedqtty, ParamSize = blockedqtty.Length, ParamType = Type.GetType("System.String").Name };
                //v_arrParam[5] = new StoreParameter() { ParamName = "p_price", ParamDirection = "1", ParamValue = price, ParamSize = price.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[4] = new StoreParameter() { ParamName = "p_toaccount", ParamDirection = "1", ParamValue = toaccount, ParamSize = toaccount.ToString().Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[5] = new StoreParameter() { ParamName = "p_desc", ParamDirection = "1", ParamValue = desc, ParamSize = desc.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[6] = new StoreParameter() { ParamName = "p_ipAddress", ParamDirection = "1", ParamValue = p_ipAddress, ParamSize = p_ipAddress.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[7] = new StoreParameter() { ParamName = "p_flag", ParamDirection = "1", ParamValue = flag, ParamSize = flag.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[8] = new StoreParameter() { ParamName = "p_err_code", ParamDirection = "2", ParamSize = 4000, ParamType = Type.GetType("System.String").Name };
                v_arrParam[9] = new StoreParameter() { ParamName = "p_err_param", ParamDirection = "2", ParamSize = 4000, ParamType = Type.GetType("System.String").Name };

                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_STOCK_TRANFER, ref v_arrParam, 8);
                string errparam = (string)v_arrParam[9].ParamValue;
                return modCommon.getBoResponse(returnErr, errparam);
                Log.Info(preFixlogSession + "======================End");
            }
            catch (Exception ex)
            {
                Log.Error(preFixlogSession + "Loi: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        public  static object internalCashTranfer(string strRequest, string p_ipAddress)
        {
            string preFixlogSession = "internalCashTranfer";
            Log.Info(preFixlogSession + "======================BEGIN");
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;

                string requestid = "";
                string account = "";
                string amount = "";
                string toaccount = "";
                string desc = "";
                string flag = "";

                if (request.TryGetValue("requestid", out jToken))
                    requestid = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    account = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("toAccount", out jToken))
                    toaccount = jToken.ToString();
                if (request.TryGetValue("Description", out jToken))
                    desc = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[9];
                v_arrParam[0] = new StoreParameter() { ParamName = "p_requestid", ParamDirection = "1", ParamValue = requestid, ParamSize = requestid.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[1] = new StoreParameter() { ParamName = "p_account", ParamDirection = "1", ParamValue = account, ParamSize = account.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[2] = new StoreParameter() { ParamName = "p_amount", ParamDirection = "1", ParamValue = amount, ParamSize = amount.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[3] = new StoreParameter() { ParamName = "p_toaccount", ParamDirection = "1", ParamValue = toaccount, ParamSize = toaccount.ToString().Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[4] = new StoreParameter() { ParamName = "p_desc", ParamDirection = "1", ParamValue = desc, ParamSize = desc.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[5] = new StoreParameter() { ParamName = "p_ipAddress", ParamDirection = "1", ParamValue = p_ipAddress, ParamSize = p_ipAddress.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[6] = new StoreParameter() { ParamName = "p_flag", ParamDirection = "1", ParamValue = flag, ParamSize = flag.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[7] = new StoreParameter() { ParamName = "p_err_code", ParamDirection = "2", ParamSize = 4000, ParamType = Type.GetType("System.String").Name };
                v_arrParam[8] = new StoreParameter() { ParamName = "p_err_param", ParamDirection = "2", ParamSize = 4000, ParamType = Type.GetType("System.String").Name };

                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_CASH_TRANFER, ref v_arrParam, 7);
                string errparam = (string)v_arrParam[8].ParamValue;
                return modCommon.getBoResponse(returnErr, errparam);
                Log.Info(preFixlogSession + "======================End");
            }
            catch (Exception ex)
            {
                Log.Error(preFixlogSession + "Loi: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        public static object rightRegister(string strRequest, string p_ipAddress)
        {

            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string keyID = "", accountId = "", custodyCode = "", description = "";
                long exerciseQuantity = 0;//, exercisePrice = 0, exerciseValue= 0;
                string refId = "", flag ="";

                if (request.TryGetValue("keyID", out jToken))
                    keyID = jToken.ToString();
                if (request.TryGetValue("custodyCode", out jToken))
                    custodyCode = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("exerciseQuantity", out jToken))
                    Int64.TryParse(jToken.ToString(), out exerciseQuantity);
                //if (request.TryGetValue("exercisePrice", out jToken))
                //    Int64.TryParse(jToken.ToString(), out exercisePrice);
                //if (request.TryGetValue("exerciseValue", out jToken))
                //    Int64.TryParse(jToken.ToString(), out exerciseValue);
                //if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("refId", out jToken))
                    refId = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[10];

                v_arrParam[0] = new StoreParameter() { ParamName = "p_keyID", ParamDirection = "1", ParamValue = keyID, ParamSize=keyID.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[1] = new StoreParameter() { ParamName = "p_custodycd", ParamDirection = "1", ParamValue = custodyCode, ParamSize = custodyCode.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[2] = new StoreParameter() { ParamName = "p_accountId", ParamDirection = "1", ParamValue = accountId, ParamSize = accountId.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[3] = new StoreParameter() { ParamName = "p_exerciseQuantity", ParamDirection = "1", ParamValue = exerciseQuantity, ParamSize = exerciseQuantity.ToString().Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[4] = new StoreParameter() { ParamName = "p_description", ParamDirection = "1", ParamValue = description, ParamSize = description.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[5] = new StoreParameter() { ParamName = "p_refId", ParamDirection = "1", ParamValue = refId, ParamSize = refId.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[6] = new StoreParameter() { ParamName = "p_ipAddress", ParamDirection = "1", ParamValue = ipAddress, ParamSize = ipAddress.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[7] = new StoreParameter() { ParamName = "p_flag", ParamDirection = "1", ParamValue = flag, ParamSize = flag.Length, ParamType = Type.GetType("System.String").Name };
                v_arrParam[8] = new StoreParameter() { ParamName = "p_err_code", ParamDirection = "2", ParamSize = 4000, ParamType = Type.GetType("System.String").Name };
                v_arrParam[9] = new StoreParameter() { ParamName = "p_err_param", ParamDirection = "2", ParamSize = 4000, ParamType = Type.GetType("System.String").Name };
                
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_RIGHT_REGISTER, ref v_arrParam, 8);
                string errparam = (string)v_arrParam[9].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("rightRegister: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
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
                if(ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    summary = new summaryAccount[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        summary[i] = new summaryAccount()
                        {
                            totalCash = Convert.ToInt64(ds.Tables[0].Rows[i]["BALANCE"].ToString()),
                            availableCash = Convert.ToInt64(ds.Tables[0].Rows[i]["CIBALANCE"].ToString()),
                            termDeposit = Convert.ToInt64(ds.Tables[0].Rows[i]["TDBALANCE"].ToString()),
                            depositInterest = Convert.ToInt64(ds.Tables[0].Rows[i]["INTERESTBALANCE"].ToString()),
                            receivingt0 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT0"].ToString()),
                            receivingt1 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT1"].ToString()),
                            receivingt2 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT2"].ToString()),
                            stockValue = Convert.ToInt64(ds.Tables[0].Rows[i]["SECURITIESAMT"].ToString()),
                            marginStockValue = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINQTTYAMT"].ToString()),
                            nonMarginStockValue = Convert.ToInt64(ds.Tables[0].Rows[i]["NONMARGINQTTYAMT"].ToString()),
                            dealFinanceStockValue = Convert.ToInt64(ds.Tables[0].Rows[i]["DFQTTYAMT"].ToString()),
                            totalDebt = Convert.ToInt64(ds.Tables[0].Rows[i]["TOTALDEBTAMT"].ToString()),
                            securedAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["SECUREAMT"].ToString()),
                            //trfbuyamt = Convert.ToInt64(ds.Tables[0].Rows[i]["TRFBUYAMT"].ToString()),
                            marginDebt = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINAMT"].ToString()),
                            t0Debt = Convert.ToInt64(ds.Tables[0].Rows[i]["T0DEBTAMT"].ToString()),
                            advancedAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["ADVANCEDAMT"].ToString()),
                            dealFinancingDebt = Convert.ToInt64(ds.Tables[0].Rows[i]["DFDEBTAMT"].ToString()),
                            //tddebtamt = Convert.ToInt64(ds.Tables[0].Rows[i]["TDDEBTAMT"].ToString()),
                            stockDepositFee = Convert.ToInt64(ds.Tables[0].Rows[i]["DEPOFEEAMT"].ToString()),
                            netassetvalue = Convert.ToInt64(ds.Tables[0].Rows[i]["NETASSETVALUE"].ToString()),
                            //requiredmarginamt = Convert.ToInt64(ds.Tables[0].Rows[i]["REQUIREDMARGINAMT"].ToString()),
                            marginableValue = Convert.ToInt64(ds.Tables[0].Rows[i]["SESECUREDAVL"].ToString()),
                            //sesecured_buy = Convert.ToInt64(ds.Tables[0].Rows[i]["SESECURED_BUY"].ToString()),
                            //accountvalue = Convert.ToInt64(ds.Tables[0].Rows[i]["ACCOUNTVALUE"].ToString()),
                            //qttyamt = Convert.ToInt64(ds.Tables[0].Rows[i]["QTTYAMT"].ToString()),
                            //mrcrlimit = Convert.ToInt64(ds.Tables[0].Rows[i]["MRCRLIMIT"].ToString()),
                            //bankavlbal = Convert.ToInt64(ds.Tables[0].Rows[i]["BANKAVLBAL"].ToString()),
                            //debtamt = Convert.ToInt64(ds.Tables[0].Rows[i]["DEBTAMT"].ToString()),
                            //advancemaxamtfee = Convert.ToInt64(ds.Tables[0].Rows[i]["ADVANCEMAXAMTFEE"].ToString()),
                            receivingamt = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGAMT"].ToString()),
                            purchasingPower = Convert.ToInt64(ds.Tables[0].Rows[i]["PURCHASINGPOWER"].ToString()),
                            marginrate = Convert.ToDouble(ds.Tables[0].Rows[i]["MARGINRATE"].ToString()),
                            //holdbalance = Convert.ToInt64(ds.Tables[0].Rows[i]["HOLDBALANCE"].ToString()),
                            //bankinqirytime = ds.Tables[0].Rows[i]["BANKINQIRYTIME"].ToString()
                            caReceiving = Convert.ToInt64(ds.Tables[0].Rows[i]["CARECEIVING"].ToString()),
                            blockedAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["BLOCKEDAMOUNT"].ToString()),
                            smsFee = Convert.ToInt64(ds.Tables[0].Rows[i]["SMSFEE"].ToString()),
                            maxLoanLimit = Convert.ToInt64(ds.Tables[0].Rows[i]["MAXLOANLIMIT"].ToString()),
                            withdrawableCash = Convert.ToInt64(ds.Tables[0].Rows[i]["WITHDRAWABLECASH"].ToString()),
                            collateralValue = Convert.ToInt64(ds.Tables[0].Rows[i]["COLLATERALVALUE"].ToString()),
                            orderSecured = Convert.ToInt64(ds.Tables[0].Rows[i]["ORDERSECURED"].ToString()),
                            marginCallAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINCALLAMOUNT"].ToString())
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

        #region Portfolio
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
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    securities = new securitiesPortfolio[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        securities[i] = new securitiesPortfolio()
                        {
                            custodycd = ds.Tables[0].Rows[i]["CUSTODYCD"].ToString(),
                            accountid = ds.Tables[0].Rows[i]["ACCOUNTID"].ToString(),
                            symbol = ds.Tables[0].Rows[i]["SYMBOL"].ToString(),
                            totalQuantity = Convert.ToInt64(ds.Tables[0].Rows[i]["TOTAL"].ToString()),
                            tradeQuantity = Convert.ToInt64(ds.Tables[0].Rows[i]["TRADE"].ToString()),
                            blockedQuantity = Convert.ToInt64(ds.Tables[0].Rows[i]["BLOCKED"].ToString()),
                            //vsdmortgage = Convert.ToInt64(ds.Tables[0].Rows[i]["VSDMORTGAGE"].ToString()),
                            mortgageQuantity = Convert.ToInt64(ds.Tables[0].Rows[i]["MORTGAGE"].ToString()),
                            restrictedQuantity = Convert.ToInt64(ds.Tables[0].Rows[i]["RESTRICT"].ToString()),
                            receivingRightQuantity = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGRIGHT"].ToString()),
                            receivingt0 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT0"].ToString()),
                            receivingt1 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT1"].ToString()),
                            receivingt2 = Convert.ToInt64(ds.Tables[0].Rows[i]["RECEIVINGT2"].ToString()),
                            costprice = Double.Parse(ds.Tables[0].Rows[i]["COSTPRICE"].ToString()),
                            initialValue = Convert.ToInt64(ds.Tables[0].Rows[i]["COSTPRICEAMT"].ToString()),
                            marketPrice = Convert.ToInt64(ds.Tables[0].Rows[i]["BASICPRICE"].ToString()),
                            stockValue = Convert.ToInt64(ds.Tables[0].Rows[i]["BASICPRICEAMT"].ToString()),
                            //marginratio = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINRATIO"].ToString()),
                            //requiredmarginamt = Convert.ToInt64(ds.Tables[0].Rows[i]["REQUIREDMARGINAMT"].ToString()),
                            //marginamt = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINAMT"].ToString()),
                            unrealizedProfit = Convert.ToInt64(ds.Tables[0].Rows[i]["PNLAMT"].ToString()),
                            unrealizedProfitRate = ds.Tables[0].Rows[i]["PNLRATE"].ToString(),
                            //issell = ds.Tables[0].Rows[i]["ISSELL"].ToString(),
                            pendingToWithdraw = Convert.ToInt64(ds.Tables[0].Rows[i]["WITHDRAW"].ToString())
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
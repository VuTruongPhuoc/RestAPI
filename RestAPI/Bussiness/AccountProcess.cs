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
        private static string COMMAND_PO_SECURITIES_BLOCK = "fopks_restapi.pr_post_Securities_Block";
        private static string COMMAND_PO_UPDATE_CAREBY = "fopks_restapi.pr_updateCareby";
        private static string COMMAND_PO_CashWithdrawPPse = "fopks_restapi.pr_CashWithdrawPPse";
        private static string COMMAND_DO_ACCOUNTSAVING = "fopks_restapi.pr_account_saving";
        private static string COMMAND_DO_ACCOUNTSFEECOLLECT = "fopks_restapi.pr_accounts_feecollect";
        private static string COMMAND_DO_ACCOUNTSMARGINLIMIT = "fopks_restapi.pr_accounts_marginlimit";
        private static string COMMAND_DO_RIGHTDIVIDENDTRANSFER = "fopks_restapi.pr_right_dividend_transfer";
        private static string COMMAND_DO_DEPOSITFEETRANSFER = "fopks_restapi.pr_accounts_fee_transfer";
        private static string COMMAND_DO_SMSFEETRANSFER = "fopks_restapi.pr_sms_fee_transfer";
        private static string COMMAND_DO_CHANGEAFTYPE = "fopks_restapi.pr_change_aftype";
        private static string COMMAND_DO_ACCOUNTSAVINGSSETTLEMENT = "fopks_restapi.pr_saving_settlement";
        private static string COMMAND_DO_CASHWITHDRAW = "fopks_restapi.pr_cash_withdraw";
        private static string COMMAND_DO_CANCELCASHWITHDRAW = "fopks_restapi.pr_cancel_cash_withdraw";
        private static string COMMAND_DO_SAVINGSOPEN = "fopks_restapi.pr_savings_open";
        private static string COMMAND_DO_CASHWITHDRAWTYPE = "fopks_restapi.pr_cash_withdraw_type";
        private static string COMMAND_DO_VSD_MESSAGE_0047 = "fopks_restapi.pr_vsd_message_0047";
        private static string COMMAND_DO_UPDATECOSTPRICE = "fopks_restapi.pr_update_costprice";
        private static string COMMAND_DO_CASH_IN_ADVANCE = "fopks_api.pr_GetInfor4AdvancePayment";

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
                            txdate = ds.Tables[0].Rows[i]["TXDATE"].ToString(),
                            channel = ds.Tables[0].Rows[i]["CHANNEL"].ToString(),
                            maker = ds.Tables[0].Rows[i]["MAKER"].ToString()
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
                            txdate = ds.Tables[0].Rows[i]["TXDATE"].ToString(),
                            channel = ds.Tables[0].Rows[i]["CHANNEL"].ToString(),
                            maker = ds.Tables[0].Rows[i]["MAKER"].ToString()
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
                            quotePrice = Convert.ToInt64(ds.Tables[0].Rows[i]["QUOTEPRICE"].ToString()),
                            channel = ds.Tables[0].Rows[i]["CHANNEL"].ToString(),
                            maker = ds.Tables[0].Rows[i]["MAKER"].ToString()
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

        public static object accountSaving(string strRequest)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", custodyCode = "", accountNo = "", amount = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("custodyCode", out jToken))
                    custodyCode = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();


                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[8];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
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
                v_objParam.ParamName = "p_accountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
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
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_ACCOUNTSAVING, ref v_arrParam, 6);
                string errparam = (string)v_arrParam[7].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("accountSaving: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        //DNS.2022.12.1.56.APIChuyentien: DNSE-1908
        public static object cashWithdrawType(string strRequest)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", custodyCode = "", accountId = "", feeType = "", feeCode = "", amount = "", refId = "", withdrawType = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("custodyCode", out jToken))
                    custodyCode = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("feeType", out jToken))
                    feeType = jToken.ToString();
                if (request.TryGetValue("feeCode", out jToken))
                    feeCode = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("refId", out jToken))
                    refId = jToken.ToString();
                if (request.TryGetValue("withdrawType", out jToken))
                    withdrawType = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();


                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[12];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custodyCode;
                v_objParam.ParamSize = custodyCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = accountId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feetype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeType;
                v_objParam.ParamSize = feeType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feecode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeCode;
                v_objParam.ParamSize = feeCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
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
                v_objParam.ParamName = "p_withdrawtype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = withdrawType;
                v_objParam.ParamSize = withdrawType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_CASHWITHDRAWTYPE, ref v_arrParam, 10);
                string errparam = (string)v_arrParam[11].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("cashWithdrawType: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        // API thu phi: DNSE-1670 DNS.2022.04.1.15
        public static object accountsFeecollect(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountId = "", amount = "", feeCode = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("feeCode", out jToken))
                    feeCode = jToken.ToString();
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
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = accountId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
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
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_ACCOUNTSFEECOLLECT, ref v_arrParam, 6);
                string errparam = (string)v_arrParam[7].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("AccountsFeecollect:.strRequest: " + strRequest, ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
        // API 1813: DNS.2022.07.1.15
        public static object AccountsMarginLimit(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountNo = "", maxLoanLimit = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("maxLoanLimit", out jToken))
                    maxLoanLimit = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[7];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_maxLoanLimit";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = maxLoanLimit;
                v_objParam.ParamSize = maxLoanLimit.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
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
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_ACCOUNTSMARGINLIMIT, ref v_arrParam, 5);
                string errparam = (string)v_arrParam[6].ParamValue;


                return modCommon.getBoResponse(returnErr, errparam);

            }
            catch (Exception ex)
            {
                Log.Error("AccountsMarginLimit:.strRequest: " + strRequest, ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        // DNS.2022.12.1.53.APITietkiem DNSE-1888
        public static object cashWithdraw(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountNo = "", amount = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
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
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_autoid";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
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
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_CASHWITHDRAW, ref v_arrParam, 6);
                string errparam = (string)v_arrParam[7].ParamValue;


                if (returnErr == 0)
                {
                    cashWithdraw s1 = new cashWithdraw()
                    {
                        Autoid = v_arrParam[5].ParamValue.ToString()
                    };
                    return modCommon.getBoResponseWithData(returnErr, s1, errparam);
                }

                return modCommon.getBoResponse(returnErr, errparam);
            }
            catch (Exception ex)
            {
                Log.Error("cashWithdraw:.strRequest: " + strRequest, ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        // DNS.2022.12.1.53.APITietkiem DNSE-1889
        public static object cancelCashWithdraw(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", refId = "", accountNo = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("refId", out jToken))
                    refId = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[7];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_id1214";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = refId;
                v_objParam.ParamSize = refId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
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
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_CANCELCASHWITHDRAW, ref v_arrParam, 5);
                string errparam = (string)v_arrParam[6].ParamValue;

                return modCommon.getBoResponse(returnErr, errparam);
            }
            catch (Exception ex)
            {
                Log.Error("cancelCashWithdraw:.strRequest: " + strRequest, ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        // DNS.2022.10.1.43.APITietkiem: DNSE-1819
        public static object accountSavingsSettlement(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", custodyCode = "", accountNo = "", principal = "", interest = "", taxAmount = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("custodyCode", out jToken))
                    custodyCode = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("principal", out jToken))
                    principal = jToken.ToString();
                if (request.TryGetValue("interest", out jToken))
                    interest = jToken.ToString();
                if (request.TryGetValue("taxAmount", out jToken))
                    taxAmount = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[14];


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
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
                v_objParam.ParamName = "p_accountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_principal";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = principal;
                v_objParam.ParamSize = principal.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_interest";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = interest;
                v_objParam.ParamSize = interest.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_taxAmount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = taxAmount;
                v_objParam.ParamSize = taxAmount.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
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

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_type";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_principal_txnum";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_interest_txnum";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_taxAmount_txnum";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                long returnErr = TransactionProcess.doTransaction(COMMAND_DO_ACCOUNTSAVINGSSETTLEMENT, ref v_arrParam, 8);
                string v_strerrorMessage = (string)v_arrParam[9].ParamValue;
                string v_errtype = (string)v_arrParam[10].ParamValue;

                if (returnErr == 0)
                {
                    accountSavingSettlement s1 = new accountSavingSettlement()
                    {

                        principal_txnum = v_arrParam[11].ParamValue.ToString(),
                        interest_txnum = v_arrParam[12].ParamValue.ToString(),
                        taxAmount_txnum = v_arrParam[13].ParamValue.ToString()
                    };
                    return modCommon.getBoResponseWithData(returnErr, s1, v_strerrorMessage);

                }
                else
                {
                    if (v_errtype == "err_1211")
                    {
                        accountSavingSettlementErr1211 s1211 = new accountSavingSettlementErr1211()
                        {

                            principal_errmsg = v_arrParam[9].ParamValue.ToString()

                        };
                        return modCommon.getBoResponseWithData(returnErr, s1211);
                    }
                    else if(v_errtype == "err_1212")
                    {
                        accountSavingSettlementErr1212 s1212 = new accountSavingSettlementErr1212()
                        {

                            interest_errmsg = v_arrParam[9].ParamValue.ToString()

                        };
                        return modCommon.getBoResponseWithData(returnErr, s1212);
                    }
                    else if (v_errtype == "err_1213")
                    {
                        accountSavingSettlementErr1213 s1213 = new accountSavingSettlementErr1213()
                        {

                            taxAmount_errmsg = v_arrParam[9].ParamValue.ToString()

                        };
                        return modCommon.getBoResponseWithData(returnErr, s1213);
                    }
                    else if (v_errtype == "err")
                    {
                        return modCommon.getBoResponse(returnErr, v_strerrorMessage);
                    }
                }
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error("accountSavingsSettlement:.strRequest: " + strRequest, ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        // DNS.2022.10.1.43.APITietkiem: DNSE-1890
        public static object savingsOpen(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", refId = "", accountNo = "", toAccountNo = "", amount = "", description = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("refId", out jToken))
                    refId = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("toAccountNo", out jToken))
                    toAccountNo = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();


                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[12];


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_id1214";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = refId;
                v_objParam.ParamSize = refId.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_toaccountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = toAccountNo;
                v_objParam.ParamSize = toAccountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_type";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_1216_txnum";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_1217_txnum";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_DO_SAVINGSOPEN, ref v_arrParam, 7);
                string v_strerrorMessage = (string)v_arrParam[8].ParamValue;
                string v_errtype = (string)v_arrParam[9].ParamValue;

                if (returnErr == 0)
                {
                    savingsOpen s1 = new savingsOpen()
                    {

                        txnum_1216 = v_arrParam[10].ParamValue.ToString(),
                        txnum_1217 = v_arrParam[11].ParamValue.ToString()
                    };
                    return modCommon.getBoResponseWithData(returnErr, s1, v_strerrorMessage);
                }
                else
                {
                    if (v_errtype == "err_1216")
                    {
                        savingsOpenErr1216 s1216 = new savingsOpenErr1216()
                        {

                            errmsg_1216 = v_arrParam[8].ParamValue.ToString()

                        };
                        return modCommon.getBoResponseWithData(returnErr, s1216);
                    }
                    else if (v_errtype == "err_1217")
                    {
                        savingsOpenErr1217 s1217 = new savingsOpenErr1217()
                        {

                            errmsg_1217 = v_arrParam[8].ParamValue.ToString()

                        };
                        return modCommon.getBoResponseWithData(returnErr, s1217);
                    }
                    else if (v_errtype == "err")
                    {
                        return modCommon.getBoResponse(returnErr, v_strerrorMessage);
                    }
                }
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error("savingsOpen:.strRequest: " + strRequest, ex);
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
                string refId = "", flag = "", description = "";

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
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                flag = request.GetValue("flag").ToString();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[14];

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
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_TRF_MONEY_2_BANK, ref v_arrParam, 12);
                string v_strerrorMessage = (string)v_arrParam[13].ParamValue;

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

        // DNS.2023.01.1.03.API0047: DNSE-1929
        public static object vsdMessage0047(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", custodyCode = "", flag = "";


                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("custodyCode", out jToken))
                    custodyCode = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[5];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestId";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = requestId.Length;
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
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_VSD_MESSAGE_0047, ref v_arrParam, 3);
                string errparam = (string)v_arrParam[4].ParamValue;

                return modCommon.getBoResponse(returnErr, errparam);
            }
            catch (Exception ex)
            {
                Log.Error("vsdMessage0047:.strRequest: " + strRequest, ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        // API Chuyen tien tren suc mua thang du: DNSE-1584 DNS.2021.10.1.44
        public static object CashWithdrawPPse(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", custodyCode = "", accountId = "", bankAccountNumber = "", bankCode = "", bankBranchCode = "", transferFeeType = "",
                       withdrawFeeCode = "", transferFeeCode = "", flag = "", amount = "0";


                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("custodyCode", out jToken))
                    custodyCode = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("bankAccountNumber", out jToken))
                    bankAccountNumber = jToken.ToString();
                if (request.TryGetValue("bankCode", out jToken))
                    bankCode = jToken.ToString();
                if (request.TryGetValue("bankBranchCode", out jToken))
                    bankBranchCode = jToken.ToString();
                if (request.TryGetValue("transferFeeType", out jToken))
                    transferFeeType = jToken.ToString();
                if (request.TryGetValue("withdrawFeeCode", out jToken))
                    withdrawFeeCode = jToken.ToString();
                if (request.TryGetValue("transferFeeCode", out jToken))
                    transferFeeCode = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[13];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custodyCode;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_bankacc";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankAccountNumber;
                v_objParam.ParamSize = bankAccountNumber.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_bankcode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankCode;
                v_objParam.ParamSize = bankCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_bankbranchcode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bankBranchCode;
                v_objParam.ParamSize = bankBranchCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feetype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferFeeType;
                v_objParam.ParamSize = transferFeeType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feewithdraw";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = withdrawFeeCode;
                v_objParam.ParamSize = withdrawFeeCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feetransfer";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferFeeCode;
                v_objParam.ParamSize = transferFeeCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_CashWithdrawPPse, ref v_arrParam, 11);
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
                Log.Error("CashWithdrawPPse:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object Inward_SE_Transfer(string strRequest, string p_ipAddress)
       {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountId = "", transferFeeCode = "", transactionFeeCode = "", symbol = "", transferType = "", transferorCompany = "", description = "", flag = "",
                       tradingQuantity = "0", blockQuantity = "0", bonusQuantity = "0", dividendQuantity = "0", transferPrice = "";
                                  

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("transferFeeCode", out jToken))
                    transferFeeCode = jToken.ToString();
                if (request.TryGetValue("transactionFeeCode", out jToken))
                    transactionFeeCode = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("transferType", out jToken))
                    transferType = jToken.ToString();
                if (request.TryGetValue("transferorCompany", out jToken))
                    transferorCompany = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("tradingQuantity", out jToken))
                    tradingQuantity = jToken.ToString();
                if (request.TryGetValue("blockQuantity", out jToken))
                    blockQuantity = jToken.ToString();
                if (request.TryGetValue("bonusQuantity", out jToken))
                    bonusQuantity = jToken.ToString();
                if (request.TryGetValue("dividendQuantity", out jToken))
                    dividendQuantity = jToken.ToString();
                if (request.TryGetValue("transferPrice", out jToken))
                    transferPrice = jToken.ToString();
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
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_account";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_infee";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferFeeCode;
                v_objParam.ParamSize = transferFeeCode.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_infeesv";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transactionFeeCode;
                v_objParam.ParamSize = transactionFeeCode.Length;
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
                v_objParam.ParamValue = transferType;
                v_objParam.ParamSize = transferType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_inward";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferorCompany;
                v_objParam.ParamSize = transferorCompany.Length;
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
                v_objParam.ParamValue = tradingQuantity;
                v_objParam.ParamSize = tradingQuantity.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantityblock";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = blockQuantity;
                v_objParam.ParamSize = blockQuantity.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantitytransferbonus";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = bonusQuantity;
                v_objParam.ParamSize = bonusQuantity.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantitytransferdividend";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dividendQuantity;
                v_objParam.ParamSize = dividendQuantity.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_price";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferPrice;
                v_objParam.ParamSize = transferPrice.ToString().Length;
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
                string requestId = "", transferFeeCode = "", transactionFeeCode = "", symbol = "", transferType = "", vsdMessageType = "", transferorAccountId = "", flag = "",
                       transfereeCompany = "", transfereeCustodyCode = "", accountIdTo = "", description = "", tradingQuantity = "0", blockQuantity = "0", transferPrice = "", vsdBiccode = "";


                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("transferFeeCode", out jToken))
                    transferFeeCode = jToken.ToString();
                if (request.TryGetValue("transactionFeeCode", out jToken))
                    transactionFeeCode = jToken.ToString();
                if (request.TryGetValue("transferType", out jToken))
                    transferType = jToken.ToString();
                if (request.TryGetValue("vsdMessageType", out jToken))
                    vsdMessageType = jToken.ToString();
                if (request.TryGetValue("vsdBiccode", out jToken))
                    vsdBiccode = jToken.ToString();
                if (request.TryGetValue("transferorAccountId", out jToken))
                    transferorAccountId = jToken.ToString();
                if (request.TryGetValue("transfereeCompany", out jToken))
                    transfereeCompany = jToken.ToString();
                    description = jToken.ToString();
                if (request.TryGetValue("tradingQuantity", out jToken))
                    tradingQuantity = jToken.ToString();
                if (request.TryGetValue("blockQuantity", out jToken))
                    blockQuantity = jToken.ToString();
                if (request.TryGetValue("transfereeCustodyCode", out jToken))
                    transfereeCustodyCode = jToken.ToString();
                if (request.TryGetValue("accountIdTo", out jToken))
                    accountIdTo = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("transferPrice", out jToken))
                    transferPrice = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[18];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
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
                v_objParam.ParamValue = transferFeeCode;
                v_objParam.ParamSize = transferFeeCode.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_infeesv";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transactionFeeCode;
                v_objParam.ParamSize = transactionFeeCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_typetransfer";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferType;
                v_objParam.ParamSize = transferType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_vsdmessage";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = vsdMessageType;
                v_objParam.ParamSize = vsdMessageType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_vsdbiccode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = vsdBiccode;
                v_objParam.ParamSize = vsdBiccode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferorAccountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_transferto";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transfereeCompany;
                v_objParam.ParamSize = transfereeCompany.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantitytransfer";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = tradingQuantity;
                v_objParam.ParamSize = tradingQuantity.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantityblock";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = blockQuantity;
                v_objParam.ParamSize = blockQuantity.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycdto";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transfereeCustodyCode;
                v_objParam.ParamSize = transfereeCustodyCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctnoto";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountIdTo;
                v_objParam.ParamSize = accountIdTo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_price";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferPrice;
                v_objParam.ParamSize = transferPrice.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[14] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[15] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[16] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[17] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_OUTWARD_SE_TRANFER, ref v_arrParam, 16);
                string v_strerrorMessage = (string)v_arrParam[17].ParamValue;

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
                string requestId = "", symbol = "", accountId = "", flag = "",
                      description = "";


                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[7];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
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
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_SENDSECURITIES_RIGHTTOCLOSE, ref v_arrParam, 5);
                string v_strerrorMessage = (string)v_arrParam[6].ParamValue;

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
        public static object SecuritiesBlock(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountId = "", transactionFeeCode = "", symbol = "", blockType = "", quantity = "", description = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("transactionFeeCode", out jToken))
                    transactionFeeCode = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("blockType", out jToken))
                    blockType = jToken.ToString();
                if (request.TryGetValue("quantity", out jToken))
                    quantity = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[10];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_account";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feecd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transactionFeeCode;
                v_objParam.ParamSize = transactionFeeCode.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_symbol";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = symbol;
                v_objParam.ParamSize = symbol.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qttytype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = blockType;
                v_objParam.ParamSize = blockType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantity;
                v_objParam.ParamSize = quantity.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_SECURITIES_BLOCK, ref v_arrParam, 8);
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
                Log.Error("SecuritiesBlock:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object updateCareby(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountId = "", careBy = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("careBy", out jToken))
                    careBy = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[5];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_careby";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = careBy;
                v_objParam.ParamSize = careBy.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_UPDATE_CAREBY, ref v_arrParam, 3);
                string v_strerrorMessage = (string)v_arrParam[4].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("updateCareby " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object rightDividendTransfer(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", keyId = "", accountNo = "", toAccountNo = "", description = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("keyId", out jToken))
                    keyId = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("toAccountNo", out jToken))
                    toAccountNo = jToken.ToString();
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
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_keyid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = keyId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_toaccountno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = toAccountNo;
                v_objParam.ParamSize = toAccountNo.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.ToString().Length;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_DO_RIGHTDIVIDENDTRANSFER, ref v_arrParam, 6);
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
                Log.Error("rightDividendTransfer " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object depositfeetransfer(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountNo = "", toAccountNo = "", transferType = "", amount = "", description = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("toAccountNo", out jToken))
                    toAccountNo = jToken.ToString();
                if (request.TryGetValue("transferType", out jToken))
                    transferType = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[9];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_toAccountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = toAccountNo;
                v_objParam.ParamSize = toAccountNo.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_transferType";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferType;
                v_objParam.ParamSize = transferType.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_DO_DEPOSITFEETRANSFER, ref v_arrParam, 7);
                string v_strerrorMessage = (string)v_arrParam[8].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("depositfeetransfer " + strRequest, ex);
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
                StoreParameter[] v_arrParam = new StoreParameter[12];

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

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_status";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Int64").Name;
                v_arrParam[11] = v_objParam;
                long returnErr = 0;
                returnErr = TransactionProcess.doTransaction(COMMAND_DO_ADVANCEPAYMENT, ref v_arrParam, 9);
                string errparam = (string)v_arrParam[10].ParamValue;

                if (returnErr == 0)
                {
                    advancePayment1153 status = new advancePayment1153() { status = v_arrParam[11].ParamValue.ToString() };
                    return modCommon.getBoResponseWithData(returnErr, status, errparam);
                }
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
                            marginCallAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["MARGINCALLAMOUNT"].ToString()),
                            ppwithdraw = Convert.ToInt64(ds.Tables[0].Rows[i]["PPWITHDRAW"].ToString())
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
                            pendingToWithdraw = Convert.ToInt64(ds.Tables[0].Rows[i]["WITHDRAW"].ToString()),
                            rightReceiving = Convert.ToInt64(ds.Tables[0].Rows[i]["RIGHTRECEIVING"].ToString()),
                            dividendReceiving = Convert.ToInt64(ds.Tables[0].Rows[i]["DIVIDENDRECEIVING"].ToString()),
                            loanRate = Double.Parse(ds.Tables[0].Rows[i]["LOANRATE"].ToString()),
                            collateralRate = Double.Parse(ds.Tables[0].Rows[i]["COLLATERALRATE"].ToString()),
                            loanPrice = Convert.ToInt64(ds.Tables[0].Rows[i]["LOANPRICE"].ToString()),
                            collateralPrice = Convert.ToInt64(ds.Tables[0].Rows[i]["COLLATERALPRICE"].ToString())
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

        public static object smsfeetransfer(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountNo = "", toAccountNo = "", transferType = "", amount = "", description = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("toAccountNo", out jToken))
                    toAccountNo = jToken.ToString();
                if (request.TryGetValue("transferType", out jToken))
                    transferType = jToken.ToString();
                if (request.TryGetValue("amount", out jToken))
                    amount = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[9];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_toaccountno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = toAccountNo;
                v_objParam.ParamSize = toAccountNo.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_transfertype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferType;
                v_objParam.ParamSize = transferType.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amount";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_DO_SMSFEETRANSFER, ref v_arrParam, 7);
                string v_strerrorMessage = (string)v_arrParam[8].ParamValue;

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("smsfeetransfer " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        //DNS.2022.09.0.36 thay doi aftype
        public static object changeactypeaccount(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", actype = "", accountNo = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("actype", out jToken))
                    actype = jToken.ToString();
                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[5];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_actype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = actype;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountNo";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_DO_CHANGEAFTYPE, ref v_arrParam, 3);
                string v_strerrorMessage = (string)v_arrParam[4].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("changeactypeaccount " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        public static object updatecostprice(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", accountId = "", symbol = "", newCostPrice = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("accountId", out jToken))
                    accountId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("newCostPrice", out jToken))
                    newCostPrice = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[7];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_account";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountId;
                v_objParam.ParamSize = accountId.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_symbol";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = symbol;
                v_objParam.ParamSize = symbol.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newcostprice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = newCostPrice;
                v_objParam.ParamSize = newCostPrice.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = flag.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_DO_UPDATECOSTPRICE, ref v_arrParam, 5);
                string v_strerrorMessage = (string)v_arrParam[6].ParamValue;

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("updatecostprice " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object getCashInAdvance(string strRequest, string accountNo)
        {
            try
            {

                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldAccountNo = new KeyField();
                fieldAccountNo.keyName = "AFACCTNO";
                fieldAccountNo.keyValue = accountNo;
                fieldAccountNo.keyType = "VARCHAR2";
                keyField.Add(fieldAccountNo);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_DO_CASH_IN_ADVANCE, keyField);

                Models.getCashInAdvance[] summary = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    summary = new getCashInAdvance[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        summary[i] = new getCashInAdvance()
                        {
                            accountNo = ds.Tables[0].Rows[i]["AFACCTNO"].ToString(),
                            txDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TXDATE"]).ToString("yyyy-MM-dd"),
                            clearingDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["DUEDATE"]).ToString("yyyy-MM-dd"),
                            execAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["AMT"].ToString()),
                            advancedAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["AAMT"].ToString()),
                            pendingAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["PDAAMT"].ToString()),
                            availableAdvanceAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["MAXAVLAMT"].ToString()),
                            advanceDays = Convert.ToInt32(ds.Tables[0].Rows[i]["DAYS"].ToString()),
                            minAdvanceAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["ADVMINAMT"].ToString()),
                            feeRate = Convert.ToDouble(ds.Tables[0].Rows[i]["FEERATE"].ToString()),
                            autoAdvance = ds.Tables[0].Rows[i]["AUTOADV"].ToString(),
                            minFeeAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["MINFEEAMT"].ToString()),
                            basis = Convert.ToInt32(ds.Tables[0].Rows[i]["DRATE"].ToString()),
                            maxAdvanceAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["ADVMAXAMT"].ToString()),
                            maxFeeAmount = Convert.ToInt64(ds.Tables[0].Rows[i]["MAXFEEAMT"].ToString()),
                            pendingApproval = Convert.ToInt64(ds.Tables[0].Rows[i]["PENDINGAPPROVAL"].ToString()),
                        };
                    }
                }

                return new list() { s = "ok", d = summary };
            }
            catch (Exception ex)
            {
                Log.Error("getCashInAdvance: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
    }
}
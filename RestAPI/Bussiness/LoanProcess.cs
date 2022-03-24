using System;
using RestAPI.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using CommonLibrary;

namespace RestAPI.Bussiness
{
    public static class LoanProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string COMMAND_PO_LOANINFO = "fopks_restapi.pr_get_loan";
        private static string COMMAND_PO_LOANDRAWNDOWN = "fopks_restapi.pr_loan_drawndown";
        private static string COMMAND_PO_LOANEXTEND = "fopks_restapi.pr_loan_extend";
        private static string COMMAND_PO_LOANPAYMENT = "fopks_restapi.pr_loan_payment";
        private static string COMMAND_PO_LOANRATE = "fopks_restapi.pr_loan_rate";

        //Thong tin mon vay DNS.2022.01.1.02
        public static object getloaninfo(string strRequest, string id)
        {
            try
            {

                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldId = new KeyField();
                fieldId.keyName = "p_id";
                fieldId.keyValue = id;
                fieldId.keyType = "VARCHAR2";
                keyField.Add(fieldId);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_PO_LOANINFO, keyField);

                Models.loanInfo[] summary = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    summary = new loanInfo[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        summary[i] = new loanInfo()
                        {
                            custodyCode = ds.Tables[0].Rows[i]["CUSTODYCD"].ToString(),
                            subAccountId = ds.Tables[0].Rows[i]["AFACCTNO"].ToString(),
                            lnTypeId = ds.Tables[0].Rows[i]["ACTYPE"].ToString(),
                            lnAccountnNo = ds.Tables[0].Rows[i]["ACCTNO"].ToString(),
                            id = Convert.ToInt64(ds.Tables[0].Rows[i]["AUTOID"].ToString()),
                            releaseDate = ds.Tables[0].Rows[i]["RLSDATE"].ToString(),
                            t1 = ds.Tables[0].Rows[i]["DUEDATE"].ToString(),
                            dueDate = ds.Tables[0].Rows[i]["OVERDUEDATE"].ToString(),
                            unduePrincipal = Convert.ToDouble(ds.Tables[0].Rows[i]["PRINNML"].ToString()),
                            duePrincipal = Convert.ToDouble(ds.Tables[0].Rows[i]["PRINDUE"].ToString()),
                            overduePrincipal = Convert.ToDouble(ds.Tables[0].Rows[i]["PRINOVD"].ToString()),
                            undueInterest = Convert.ToDouble(ds.Tables[0].Rows[i]["INTNMLACR"].ToString()),
                            dueInterest = Convert.ToDouble(ds.Tables[0].Rows[i]["INTDUE"].ToString()),
                            overdueInterest = Convert.ToDouble(ds.Tables[0].Rows[i]["INTOVD"].ToString()),
                            interestOnOverduePrincipal = Convert.ToDouble(ds.Tables[0].Rows[i]["INTOVDPRIN"].ToString()),
                            undueFee = Convert.ToDouble(ds.Tables[0].Rows[i]["FEEINTNMLACR"].ToString()),
                            dueFee = Convert.ToDouble(ds.Tables[0].Rows[i]["FEEINTDUE"].ToString()),
                            overdueFee = Convert.ToInt64(ds.Tables[0].Rows[i]["FEEINTNMLOVD"].ToString()),
                            feeOnOverduePrincipal = Convert.ToDouble(ds.Tables[0].Rows[i]["FEEINTOVDACR"].ToString()),
                            interestRate1 = Convert.ToDouble(ds.Tables[0].Rows[i]["RATE1"].ToString()),
                            interestRate2 = Convert.ToDouble(ds.Tables[0].Rows[i]["RATE2"].ToString()),
                            interestRate3 = Convert.ToDouble(ds.Tables[0].Rows[i]["RATE3"].ToString()),
                            feeRate1 = Convert.ToDouble(ds.Tables[0].Rows[i]["CFRATE1"].ToString()),
                            feeRate2 = Convert.ToDouble(ds.Tables[0].Rows[i]["CFRATE2"].ToString()),
                            feeRate3 = Convert.ToDouble(ds.Tables[0].Rows[i]["CFRATE3"].ToString()),
                            extendTimes = Convert.ToInt64(ds.Tables[0].Rows[i]["EXTENDTIMES"].ToString()),
                            isSettled = ds.Tables[0].Rows[i]["ISSETTLED"].ToString(),
                        };
                    }
                }

                return new list() { s = "ok", d = summary };
            }
            catch (Exception ex)
            {
                Log.Error("getloaninfo: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        // API giai ngan: DNS.2022.01.1.02
        public static object LoanDrawndown(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", account = "", lnTypeId = "", amount = "", description = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    account = jToken.ToString();
                if (request.TryGetValue("lnTypeId", out jToken))
                    lnTypeId = jToken.ToString();
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
                v_objParam.ParamName = "p_account";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = account;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_lntype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = lnTypeId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = amount;
                v_objParam.ParamSize = amount.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_desc";
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
                v_objParam.ParamName = "p_autoid";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_LOANDRAWNDOWN, ref v_arrParam, 7);
                string v_strerrorMessage = (string)v_arrParam[8].ParamValue;

                if (returnErr == 0)
                {
                    idResponse id = new idResponse() { id = (string)v_arrParam[6].ParamValue };
                    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                }

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("LoanDrawndown:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        // API UPDATE LAI VAY: DNS.2022.03.1.12
        public static object LoanRate(string strRequest, string id, string p_ipAddress) 
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "",flag = "";
                string undueInterest = "", dueInterest = "", overdueInterest = "", interestOnOverduePrincipal = "", undueFee = "", dueFee = "", overdueFee = "", feeOnOverduePrincipal = "";
               
                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("id", out jToken))
                    id = jToken.ToString();
                if (request.TryGetValue("undueInterest", out jToken))
                    undueInterest = jToken.ToString();
                if (request.TryGetValue("dueInterest", out jToken))
                    dueInterest = jToken.ToString();
                if (request.TryGetValue("overdueInterest", out jToken))
                    overdueInterest = jToken.ToString();
                if (request.TryGetValue("interestOnOverduePrincipal", out jToken))
                    interestOnOverduePrincipal = jToken.ToString();
                if (request.TryGetValue("undueFee", out jToken))
                    undueFee = jToken.ToString();
                if (request.TryGetValue("dueFee", out jToken))
                    dueFee = jToken.ToString();
                if (request.TryGetValue("overdueFee", out jToken))
                    overdueFee = jToken.ToString();
                if (request.TryGetValue("feeOnOverduePrincipal", out jToken))
                feeOnOverduePrincipal = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[21];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_requestid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = requestId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_autoid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = id;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_undueInterest";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = undueInterest;
                v_objParam.ParamSize = undueInterest.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dueInterest";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dueInterest;
                v_objParam.ParamSize = dueInterest.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_overdueInterest";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = overdueInterest;
                v_objParam.ParamSize = overdueInterest.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_interestOnOverduePrincipal";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = interestOnOverduePrincipal;
                v_objParam.ParamSize = interestOnOverduePrincipal.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_undueFee";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = undueFee;
                v_objParam.ParamSize = undueFee.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dueFee";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dueFee;
                v_objParam.ParamSize = dueFee.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;



                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_overdueFee";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = overdueFee;
                v_objParam.ParamSize = overdueFee.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeOnOverduePrincipal";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeOnOverduePrincipal;
                v_objParam.ParamSize = feeOnOverduePrincipal.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;



                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newundueInterest";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[10] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newdueInterest";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newoverdueInterest";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[12] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newinterestOnOverduePrincipal";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[13] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newundueFee";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[14] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newdueFee";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[15] = v_objParam;



                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newoverdueFee";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[16] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newfeeOnOverduePrincipal";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[17] = v_objParam;



                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_flag";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = flag;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[18] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[19] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[20] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_LOANRATE, ref v_arrParam, 19);
                string v_strerrorMessage = (string)v_arrParam[20].ParamValue;

                if (returnErr == 0)
                {
                    loanrate loanrate = new loanrate()
                    {

                        newundueInterest = Convert.ToDouble(v_arrParam[10].ParamValue.ToString()),
                        newdueInterest = Convert.ToDouble(v_arrParam[11].ParamValue.ToString()),
                        newoverdueInterest = Convert.ToDouble(v_arrParam[12].ParamValue.ToString()),
                        newinterestOnOverduePrincipal = Convert.ToDouble(v_arrParam[13].ParamValue.ToString()),
                        newundueFee = Convert.ToDouble(v_arrParam[14].ParamValue.ToString()),
                        newdueFee = Convert.ToDouble(v_arrParam[15].ParamValue.ToString()),
                        newoverdueFee = Convert.ToDouble(v_arrParam[16].ParamValue.ToString()),
                        newfeeOnOverduePrincipal = Convert.ToDouble(v_arrParam[17].ParamValue.ToString()),

                    };
                    return modCommon.getBoResponseWithData(returnErr, loanrate, v_strerrorMessage);
                }


                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("LoanRate:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        // API gia han no: DNS.2022.01.1.02
        public static object LoanExtend(string strRequest, string id, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", subAccountId = "", extendDate = "", feeId = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("subAccountId", out jToken))
                    subAccountId = jToken.ToString();
                if (request.TryGetValue("extendDate", out jToken))
                    extendDate = jToken.ToString();
                if (request.TryGetValue("feeId", out jToken))
                    feeId = jToken.ToString();
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
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = subAccountId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_autoid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = id;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_newduedate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = extendDate;
                v_objParam.ParamSize = extendDate.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = feeId;
                v_objParam.ParamSize = feeId.Length;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_LOANEXTEND, ref v_arrParam, 6);
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
                Log.Error("LoanExtend:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        // API tra no: DNS.2022.01.1.02
        public static object LoanPayment(string strRequest, string id, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", account = "", payAmount = "", description = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    account = jToken.ToString();
                if (request.TryGetValue("payAmount", out jToken))
                    payAmount = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("flag", out jToken))
                    flag = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[20];

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
                v_objParam.ParamValue = account;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_autoid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = id;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_amt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = payAmount;
                v_objParam.ParamSize = payAmount.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_desc";
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
                v_objParam.ParamName = "p_overduefee";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_duefee";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_unduefee";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_intoverdueprint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_overdueint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dueint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_undueint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_overdueprint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dueprint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[14] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_undueprint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[15] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_feeoverdueprint";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = orderid;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[16] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_issettled";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[17] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[18] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[19] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_LOANPAYMENT, ref v_arrParam, 18);
                string v_strerrorMessage = (string)v_arrParam[19].ParamValue;

                if (returnErr == 0)
                {
                    loanpaymentResponse loanpayment = new loanpaymentResponse()
                    {
                        overdueFee = Convert.ToDouble(v_arrParam[6].ParamValue.ToString()),
                        dueFee = Convert.ToDouble(v_arrParam[7].ParamValue.ToString()),
                        undueFee = Convert.ToDouble(v_arrParam[8].ParamValue.ToString()),
                        interestOnOverduePrincipal = Convert.ToDouble(v_arrParam[9].ParamValue.ToString()),
                        overdueInterest = Convert.ToDouble(v_arrParam[10].ParamValue.ToString()),
                        dueInterest = Convert.ToDouble(v_arrParam[11].ParamValue.ToString()),
                        undueInterest = Convert.ToDouble(v_arrParam[12].ParamValue.ToString()),
                        overduePrincipal = Convert.ToDouble(v_arrParam[13].ParamValue.ToString()),
                        duePrincipal = Convert.ToDouble(v_arrParam[14].ParamValue.ToString()),
                        unduePrincipal = Convert.ToDouble(v_arrParam[15].ParamValue.ToString()),
                        feeOnOverduePrincipal = Convert.ToDouble(v_arrParam[16].ParamValue.ToString()),
                        isSettled = (string)v_arrParam[17].ParamValue,
                    };
                    return modCommon.getBoResponseWithData(returnErr, loanpayment, v_strerrorMessage);
                }

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("LoanPayment:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
    }
}
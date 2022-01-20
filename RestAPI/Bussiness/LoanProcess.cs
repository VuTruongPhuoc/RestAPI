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
                StoreParameter[] v_arrParam = new StoreParameter[8];

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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_LOANDRAWNDOWN, ref v_arrParam, 6);
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
                Log.Error("LoanDrawndown:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        // API gia han no: DNS.2022.01.1.02
        public static object LoanExtend(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", subAccountId = "", id = "", extendDate = "", feeId = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("subAccountId", out jToken))
                    subAccountId = jToken.ToString();
                if (request.TryGetValue("id", out jToken))
                    id = jToken.ToString();
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
        public static object LoanPayment(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", account = "", id = "", payAmount = "", description = "", flag = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("account", out jToken))
                    account = jToken.ToString();
                if (request.TryGetValue("id", out jToken))
                    id = jToken.ToString();
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
                StoreParameter[] v_arrParam = new StoreParameter[8];

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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_LOANPAYMENT, ref v_arrParam, 6);
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
                Log.Error("LoanPayment:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
    }
}
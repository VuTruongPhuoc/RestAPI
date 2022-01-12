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
        // Su kien quyen chia co tuc bang co phieu DNSE-1580: DNSE-1580: DNS.2021.10.1.44

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
                            custodCode = ds.Tables[0].Rows[i]["CUSTODYCD"].ToString(),
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
                            feeintovdacr = Convert.ToDouble(ds.Tables[0].Rows[i]["FEEINTOVDACR"].ToString()),
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


    }
}
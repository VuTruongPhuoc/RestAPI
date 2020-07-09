using System;
using RestAPI.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
namespace RestAPI.Bussiness
{
    public static class AccountProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string COMMAND_GET_EXECUTIONS = "fopks_restapi.pr_get_executions";

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

    }
}
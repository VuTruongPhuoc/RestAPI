
using CommonLibrary;
using log4net;
using Newtonsoft.Json.Linq;
using RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RestAPI.Bussiness
{
    public class CustomersProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string COMMAND_GET_ACCOUNT = "fopks_restapi.pr_get_accounts";
        private const string COMMAND_POST_OPEN_ACCOUNT = "fopks_restapi.pr_post_openAccount";

        public static object getAccounts(string custodycd)
        {
            try
            {

                List<KeyField> keyField = new List<KeyField>();

                KeyField field = new KeyField();
                field.keyName = "p_custodycd";
                field.keyValue = custodycd;
                field.keyType = "VARCHAR2";
                keyField.Add(field);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_ACCOUNT, keyField);

                Account[] accounts = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    accounts = new Account[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        accounts[i] = new Account()
                        {
                            id = ds.Tables[0].Rows[i]["ID"].ToString(),
                            name = ds.Tables[0].Rows[i]["NAME"].ToString(),
                            currency = ds.Tables[0].Rows[i]["CURRENCY"].ToString()
                        };
                    }
                }

                return new list() { s = "ok", d = accounts };
            }
            catch (Exception ex)
            {
                Log.Error("getAccounts:.custodycd=" + custodycd + ":.", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        public static object openAccount(string strRequest, string custodycd)
        {
            try
            {

                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string accountNo = "", actype = "";
                

                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("actype", out jToken))
                    actype = jToken.ToString();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[5];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custodycd;
                v_objParam.ParamSize = custodycd.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = accountNo.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_actype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = actype;
                v_objParam.ParamSize = actype.Length;
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

                
                long returnErr = TransactionProcess.doTransaction(COMMAND_POST_OPEN_ACCOUNT, ref v_arrParam, 3); ;
                string v_strerrorMessage = (string)v_arrParam[4].ParamValue;


                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("openAccount:.strRequest: " + strRequest + " :.custodycd:." + custodycd, ex);
                return 1;
            }
        }
    }
}
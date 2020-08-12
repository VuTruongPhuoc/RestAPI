
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
        private const string COMMAND_POST_OPEN_CFMAST = "fopks_restapi.pr_post_openCfmast";

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
                            currency = ds.Tables[0].Rows[i]["CURRENCY"].ToString(),
                            actype = ds.Tables[0].Rows[i]["ACTYPE"].ToString()
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
                long amt = 0;
                

                if (request.TryGetValue("accountNo", out jToken))
                    accountNo = jToken.ToString();
                if (request.TryGetValue("actype", out jToken))
                    actype = jToken.ToString();
                if (request.TryGetValue("amt", out jToken))
                    Int64.TryParse(jToken.ToString(), out amt);

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[6];

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
                v_objParam.ParamName = "p_amt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Convert.ToString(amt);
                v_objParam.ParamSize = amt.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                
                long returnErr = TransactionProcess.doTransaction(COMMAND_POST_OPEN_ACCOUNT, ref v_arrParam, 4); ;
                string v_strerrorMessage = (string)v_arrParam[5].ParamValue;


                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("openAccount:.strRequest: " + strRequest + " :.custodycd:." + custodycd, ex);
                return 1;
            }
        }

        public static object openCfmast(string strRequest, string custodycd)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string fullname = "", idtype ="", idcode = "", idplace ="", country="", address ="";
                string mobile = "", email = "", description = "", careby = "", sex = "";
                string iddate = "", birthday ="", opndate ="";
                if (request.TryGetValue("fullname", out jToken))
                    fullname = jToken.ToString();
                if (request.TryGetValue("idtype", out jToken))
                    idtype = jToken.ToString();
                if (request.TryGetValue("idcode", out jToken))
                    idcode = jToken.ToString();
                if (request.TryGetValue("iddate", out jToken))
                    iddate = jToken.ToString();
                if (request.TryGetValue("idplace", out jToken))
                    idplace = jToken.ToString();
                if (request.TryGetValue("birthday", out jToken))
                    birthday = jToken.ToString();
                if (request.TryGetValue("country", out jToken))
                    country = jToken.ToString();
                if (request.TryGetValue("address", out jToken))
                    address = jToken.ToString();
                if (request.TryGetValue("mobile", out jToken))
                    mobile = jToken.ToString();
                if (request.TryGetValue("email", out jToken))
                    email = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("opndate", out jToken))
                    opndate = jToken.ToString();
                if (request.TryGetValue("careby", out jToken))
                    careby = jToken.ToString();
                if (request.TryGetValue("sex", out jToken))
                    sex = jToken.ToString();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[17];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custodycd;
                v_objParam.ParamSize = custodycd.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_fullname";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = fullname;
                v_objParam.ParamSize = fullname.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idtype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = idtype;
                v_objParam.ParamSize = idtype.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idcode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = idcode;
                v_objParam.ParamSize = idcode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_iddate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = iddate;
                v_objParam.ParamSize = iddate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idplace";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = idplace;
                v_objParam.ParamSize = idplace.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_birthday";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = birthday;
                v_objParam.ParamSize = birthday.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_country";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = country;
                v_objParam.ParamSize = country.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_address";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = address;
                v_objParam.ParamSize = address.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_mobile";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = mobile;
                v_objParam.ParamSize = mobile.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_email";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = email;
                v_objParam.ParamSize = email.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_opndate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = opndate;
                v_objParam.ParamSize = opndate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_careby";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = careby;
                v_objParam.ParamSize = careby.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_sex";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = sex;
                v_objParam.ParamSize = sex.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[14] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errcode;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[15] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                //v_objParam.ParamValue = errparam;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[16] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_POST_OPEN_CFMAST, ref v_arrParam, 15); ;
                string v_strerrorMessage = (string)v_arrParam[16].ParamValue;


                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("openAccount:.strRequest: " + strRequest, ex);
                return 1;
            }
        }
    }
}
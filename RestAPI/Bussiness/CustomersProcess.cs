
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
        private const string COMMAND_POST_OPEN_CFMAST = "fopks_restapi.pr_post_openCustomer";

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
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    accounts = new Account[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        accounts[i] = new Account()
                        {
                            id = ds.Tables[0].Rows[i]["ID"].ToString(),
                            name = ds.Tables[0].Rows[i]["NAME"].ToString(),
                            currency = ds.Tables[0].Rows[i]["CURRENCY"].ToString(),
                            accountType = ds.Tables[0].Rows[i]["ACTYPE"].ToString(),
                            accountTypeName = ds.Tables[0].Rows[i]["TYPENAME"].ToString(),
                            accountTypeBriefName = ds.Tables[0].Rows[i]["SHORTNAME"].ToString(),
                            openDate = ds.Tables[0].Rows[i]["OPNDATE"].ToString()
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
                string accountNo = "", actype = "";
                long loanLimit = 0;

                if (strRequest != null && strRequest.Length > 0)
                {
                    JObject request = JObject.Parse(strRequest);
                    JToken jToken;

                    if (request.TryGetValue("accountNo", out jToken))
                        accountNo = jToken.ToString();
                    if (request.TryGetValue("accountType", out jToken))
                        actype = jToken.ToString();
                    if (request.TryGetValue("loanLimit", out jToken))
                        Int64.TryParse(jToken.ToString(), out loanLimit);
                }
                

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
                v_objParam.ParamDirection = "3";
                v_objParam.ParamValue = accountNo;
                v_objParam.ParamSize = 100;
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
                v_objParam.ParamName = "p_loanLimit";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Convert.ToString(loanLimit);
                v_objParam.ParamSize = loanLimit.ToString().Length;
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
                string v_strerrorMessage = (string) v_arrParam[5].ParamValue;
                if (returnErr == 0)
                {
                    idResponse id = new idResponse() { id = (string)v_arrParam[1].ParamValue };
                    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                }

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("openAccount:.strRequest: " + strRequest + " :.custodycd:." + custodycd, ex);
                return modCommon.getBoResponse(-1);
            }
        }

        public static object openCustomer(string strRequest)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;

                string custodycd = "", fullName = "", customerType = "";
                string identityNumber = "", issueDate = "", issuePlace = "";
                string dateOfBirth = "", email = "", phone1 = "", address = "", pin = "";
                string floorTrading = "Y", teleTrading = "N";
                string managementGroup = "", accountManagementType = "", marginAllowance = "", description = "";
                string nationality = "", gender = "", expiryDate = "", identificationType = "";
                string province = "", custAtCom = "", vat = "", onlineTradingRegister = "Y";

                long onlineTransaction = 0;

                if (request.TryGetValue("custodycd", out jToken))
                    custodycd = jToken.ToString();

                fullName = request.GetValue("fullName").ToString();
                customerType = request.GetValue("customerType").ToString();
                identityNumber = request.GetValue("identityNumber").ToString();
                issueDate = request.GetValue("issueDate").ToString();
                issuePlace = request.GetValue("issuePlace").ToString();
                dateOfBirth = request.GetValue("dateOfBirth").ToString();
                email = request.GetValue("email").ToString();
                phone1 = request.GetValue("phone1").ToString();
                address = request.GetValue("address").ToString();
                custAtCom = request.GetValue("custAtCom").ToString();
                vat = request.GetValue("vat").ToString();
                //onlineTradingRegister = request.GetValue("onlineTradingRegister").ToString();

                if (request.TryGetValue("province", out jToken))
                    province = jToken.ToString();
                if (request.TryGetValue("pin", out jToken))
                    pin = jToken.ToString();
                if (request.TryGetValue("floorTrading", out jToken))
                    floorTrading = jToken.ToString();
                if (request.TryGetValue("teleTrading", out jToken))
                    teleTrading = jToken.ToString();
                if (request.TryGetValue("onlineTransaction", out jToken))
                    onlineTransaction = Int64.Parse(jToken.ToString());
                if (request.TryGetValue("managementGroup", out jToken))
                    managementGroup = jToken.ToString();
                if (request.TryGetValue("accountManagementType", out jToken))
                    accountManagementType = jToken.ToString();
                if (request.TryGetValue("marginAllowance", out jToken))
                    marginAllowance = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("nationality", out jToken))
                    nationality = jToken.ToString();
                if (request.TryGetValue("gender", out jToken))
                    gender = jToken.ToString();
                if (request.TryGetValue("expiryDate", out jToken))
                    expiryDate = jToken.ToString();
                if (request.TryGetValue("identificationType", out jToken))
                    identificationType = jToken.ToString();
                if (request.TryGetValue("onlineTradingRegister", out jToken))
                    onlineTradingRegister = jToken.ToString();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[28];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "3";
                v_objParam.ParamValue = custodycd;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_fullname";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = fullName;
                v_objParam.ParamSize = fullName.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_customerType";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = customerType;
                v_objParam.ParamSize = customerType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idcode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = identityNumber;
                v_objParam.ParamSize = identityNumber.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_iddate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = issueDate;
                v_objParam.ParamSize = issueDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idplace";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = issuePlace;
                v_objParam.ParamSize = issuePlace.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_birthday";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dateOfBirth;
                v_objParam.ParamSize = dateOfBirth.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_email";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = email;
                v_objParam.ParamSize = email.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_mobile";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = phone1;
                v_objParam.ParamSize = phone1.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_address";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = address;
                v_objParam.ParamSize = address.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_pin";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = pin;
                v_objParam.ParamSize = pin.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_floorTrading";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = floorTrading;
                v_objParam.ParamSize = floorTrading.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_teleTrading";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = teleTrading;
                v_objParam.ParamSize = teleTrading.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_onlineTransaction";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = onlineTransaction;
                v_objParam.ParamSize = onlineTransaction.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_managementGroup";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = managementGroup;
                v_objParam.ParamSize = managementGroup.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[14] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_accountManagementType";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = accountManagementType;
                v_objParam.ParamSize = accountManagementType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[15] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_marginAllowance";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = marginAllowance;
                v_objParam.ParamSize = marginAllowance.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[16] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_description";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = description;
                v_objParam.ParamSize = description.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[17] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_nationality";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = nationality;
                v_objParam.ParamSize = nationality.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[18] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_gender";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = gender;
                v_objParam.ParamSize = gender.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[19] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_expiryDate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = expiryDate;
                v_objParam.ParamSize = expiryDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[20] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idtype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = identificationType;
                v_objParam.ParamSize = identificationType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[21] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_province";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = province;
                v_objParam.ParamSize = province.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[22] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custAtCom";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custAtCom;
                v_objParam.ParamSize = custAtCom.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[23] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_vat";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = vat;
                v_objParam.ParamSize = vat.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[24] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[25] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[26] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_onlineTrading";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = onlineTradingRegister;
                v_objParam.ParamSize = onlineTradingRegister.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[27] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_POST_OPEN_CFMAST, ref v_arrParam, 25);
                string v_strerrorMessage = (string)v_arrParam[26].ParamValue;

                if (returnErr == 0)
                {
                    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                    return modCommon.getBoResponseWithData(returnErr,id,v_strerrorMessage);
                }
                
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("openCustomer:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
    }
}
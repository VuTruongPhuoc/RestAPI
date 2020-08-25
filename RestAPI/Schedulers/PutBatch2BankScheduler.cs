using System;
using System.Text;
using System.Collections.Generic;
using CommonLibrary;
using log4net;
using System.Data;
using RestAPI.Bussiness;
using RestAPI.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace RestAPI.Schedulers
{
    public class PutBatch2BankScheduler : AbstractScheduler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static PutBatch2BankScheduler _instance;
        public static PutBatch2BankScheduler Instance => _instance ?? (_instance = new PutBatch2BankScheduler());

        private const string KEY_PUT_BATCH_SERVICE_URL = "PUT_BATCH_SERVICE_URL";
        private const string KEY_PUT_BATCH_AUTH_API_KEY = "PUT_BATCH_AUTH_API_KEY";
        private const string KEY_PUT_BATCH_INTERVAL = "PUT_BATCH_INTERVAL";
        private const string DEF_PUT_BATCH_INTERVAL = "3000";
        private const string COMMAND_GET_TRANSACTION = "pck_gwtransfer.pr_Get_PutbatchTrans";
        private const string COMMAND_COMPLETE_TRANSACTION = "pck_gwtransfer.pr_Complete_PutbatchTrans";

        private string _url;
        private string _xAuthApiKey;

        private PutBatch2BankScheduler () : base (modCommond.GetConfigValue(KEY_PUT_BATCH_INTERVAL, DEF_PUT_BATCH_INTERVAL))
        {
            _url = modCommond.GetConfigValue(KEY_PUT_BATCH_SERVICE_URL, "");
            _xAuthApiKey = modCommond.GetConfigValue(KEY_PUT_BATCH_AUTH_API_KEY, "");
        }

        public override void Execute()
        {
            try
            {
                DataSet ds = GetPutBatchTransaction();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return;

                Log.Info(String.Format("Execute.:GetPutBatchExecutions: batchid={0} cnt={1}",
                    ds.Tables[0].Rows[0]["BATCHID"].ToString(), ds.Tables[0].Rows.Count));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    WithdrawalTransactionInfo reponse = CreateWithdrawalTransaction(new WithdrawalTransaction
                    {
                        benAccNo = ds.Tables[0].Rows[i]["ACCOUNT"].ToString(),
                        benAccType = ds.Tables[0].Rows[i]["ACCOUNTTYPE"].ToString(),
                        benName = ds.Tables[0].Rows[i]["BENEFCUSTNAME"].ToString(),
                        desc = ds.Tables[0].Rows[i]["REMARK"].ToString(),
                        amt = Convert.ToDouble(ds.Tables[0].Rows[i]["AMOUNT"].ToString()),
                        benBankName = ds.Tables[0].Rows[i]["BANKNAME"].ToString(),
                        benBankCode = ds.Tables[0].Rows[i]["BANKCODE"].ToString(),
                        benBankBranchName = ds.Tables[0].Rows[i]["BANKBRANCHNAME"].ToString(),
                        benBankBranchCode = ds.Tables[0].Rows[i]["BANKBRANCHCODE"].ToString(),
                        benBankNapasCode = ds.Tables[0].Rows[i]["BENBANKNAPASCODE"].ToString(),
                        srcTransId = ds.Tables[0].Rows[i]["TRANSACTIONID"].ToString()
                    });

                    CompletePutbatchTransaction(reponse);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Execute.:", ex);
            }
        }

        private WithdrawalTransactionInfo CreateWithdrawalTransaction(WithdrawalTransaction tran)
        {
            WithdrawalTransactionInfo result = null;
            HttpClient client = new HttpClient();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _url);
                request.Headers.Add("X-AUTH-APIKEY", _xAuthApiKey);
                request.Content = new StringContent(JsonConvert.SerializeObject(tran), Encoding.UTF8, "application/json");
                modCommon.LogFullRequest(request);

                HttpResponseMessage response = client.SendAsync(request).Result;
                modCommon.LogFullResponses(response, response.StatusCode);
                result = response.Content.ReadAsAsync<WithdrawalTransactionInfo>().Result;

                if (result == null)
                {
                    result = new WithdrawalTransactionInfo()
                    {
                        srcTransId = tran.srcTransId,
                        code = response.StatusCode.ToString().ToUpper(),
                        message = "No response results",
                    };
                } 
                else if (response.IsSuccessStatusCode)
                {
                    result.code = "OK";
                }

                result.srcTransId = tran.srcTransId;
            }
            catch (Exception ex)
            {
                Log.Error("CreateWithdrawalTransaction.:", ex);
                result = new WithdrawalTransactionInfo()
                {
                    srcTransId = tran.srcTransId,
                    code = "NOTOK",
                    message = ex.Message,
                };
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }

        private void CompletePutbatchTransaction(WithdrawalTransactionInfo tran)
        {
            try
            {
                Log.Info(String.Format("CompletePutbatchTransaction: srcTransId={0} status={1} code={2} message={3}",
                    tran.srcTransId, tran.status, tran.code, tran.message));

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[6];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_transactionid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = tran.srcTransId;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_res_status";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = tran.status;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_res_code";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = tran.code;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_res_message";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = tran.message;
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                TransactionProcess.doTransaction(COMMAND_COMPLETE_TRANSACTION, ref v_arrParam, 0);
            }
            catch (Exception ex)
            {
                Log.Error("CompletePutbatchTransaction.:", ex);
            }
        }

        private DataSet GetPutBatchTransaction()
        {
            try
            {
                List<KeyField> keyField = new List<KeyField>();

                KeyField fieldAccountNo = new KeyField();
                fieldAccountNo.keyName = "p_maxcount";
                fieldAccountNo.keyValue = "10";
                fieldAccountNo.keyType = "NUMBER";
                keyField.Add(fieldAccountNo);

                return GetDataProcess.executeGetData(COMMAND_GET_TRANSACTION, keyField);
            }
            catch (Exception ex)
            {
                Log.Error("GetPutBatchExecutions.:", ex);
            }
            return null;
        }

    }
}

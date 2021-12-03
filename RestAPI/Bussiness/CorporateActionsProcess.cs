using System;
using RestAPI.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using CommonLibrary;

namespace RestAPI.Bussiness
{
    public static class CorporateActionsProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string COMMAND_PO_STOCK_DIVIDEND = "fopks_restapi.pr_stock_dividend";

        // Su kien quyen chia co tuc bang co phieu DNSE-1580: DNSE-1580: DNS.2021.10.1.44
        public static object stockdividend(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", symbol = "", dividendRate = "", description = "", exercisePriceForOddShare = "", quantityUnit = "", quantityDecimals = "",
                       exerciseDate = "", isinCode = "", estimateReceivingDate = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("dividendRate", out jToken))
                    dividendRate = jToken.ToString();
                if (request.TryGetValue("description", out jToken))
                    description = jToken.ToString();
                if (request.TryGetValue("exercisePriceForOddShare", out jToken))
                    exercisePriceForOddShare = jToken.ToString();
                if (request.TryGetValue("quantityUnit", out jToken))
                    quantityUnit = jToken.ToString();
                if (request.TryGetValue("quantityDecimals", out jToken))
                    quantityDecimals = jToken.ToString();
                if (request.TryGetValue("exerciseDate", out jToken))
                    exerciseDate = jToken.ToString();
                if (request.TryGetValue("isinCode", out jToken))
                    isinCode = jToken.ToString();
                if (request.TryGetValue("estimateReceivingDate", out jToken))
                    estimateReceivingDate = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[12];

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
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_dividendRate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dividendRate;
                v_objParam.ParamSize = dividendRate.ToString().Length;
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
                v_objParam.ParamName = "p_exprice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exercisePriceForOddShare;
                v_objParam.ParamSize = exercisePriceForOddShare.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qttyunit";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityUnit;
                v_objParam.ParamSize = quantityUnit.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qttydecimals";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityDecimals;
                v_objParam.ParamSize = quantityDecimals.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_exercisedate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exerciseDate;
                v_objParam.ParamSize = exerciseDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_isincode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = isinCode;
                v_objParam.ParamSize = isinCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_actiondate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = estimateReceivingDate;
                v_objParam.ParamSize = estimateReceivingDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_STOCK_DIVIDEND, ref v_arrParam, 10);
                string v_strerrorMessage = (string)v_arrParam[11].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("stockdividend:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }


    }
}
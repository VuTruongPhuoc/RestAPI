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
        private static string COMMAND_PO_CASH_DIVIDEND = "fopks_restapi.pr_cash_dividend";
        private static string COMMAND_PO_STOCK_BONUS = "fopks_restapi.pr_stock_bonus";
        private static string COMMAND_PO_RIGHT = "fopks_restapi.pr_right";
        // Su kien quyen chia co tuc bang co phieu DNSE-1580: DNSE-1580: DNS.2021.10.1.44
        public static object stockdividend(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", symbol = "", dividendRate = "", exercisePriceForOddShare = "", quantityUnit = "", quantityDecimals = "",
                       exerciseDate = "", isinCode = "", estimateReceivingDate = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("dividendRate", out jToken))
                    dividendRate = jToken.ToString();
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
                StoreParameter[] v_arrParam = new StoreParameter[11];

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
                v_objParam.ParamName = "p_exprice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exercisePriceForOddShare;
                v_objParam.ParamSize = exercisePriceForOddShare.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qttyunit";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityUnit;
                v_objParam.ParamSize = quantityUnit.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_qttydecimals";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityDecimals;
                v_objParam.ParamSize = quantityDecimals.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_exercisedate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exerciseDate;
                v_objParam.ParamSize = exerciseDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_isincode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = isinCode;
                v_objParam.ParamSize = isinCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_actiondate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = estimateReceivingDate;
                v_objParam.ParamSize = estimateReceivingDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_STOCK_DIVIDEND, ref v_arrParam, 9);
                string v_strerrorMessage = (string)v_arrParam[10].ParamValue;

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

        public static object cashdividend(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", symbol = "", dividendType = "", cashDividendRate = "", cashDividendPerShare = "",
                       exerciseDate = "", isinCode = "", estimateReceivingDate = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("dividendType", out jToken))
                    dividendType = jToken.ToString();
                if (request.TryGetValue("cashDividendRate", out jToken))
                    cashDividendRate = jToken.ToString();
                if (request.TryGetValue("cashDividendPerShare", out jToken))
                    cashDividendPerShare = jToken.ToString();
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
                StoreParameter[] v_arrParam = new StoreParameter[10];

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
                v_objParam.ParamName = "p_dividendType";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = dividendType;
                v_objParam.ParamSize = dividendType.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_cashDividendRate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = cashDividendRate;
                v_objParam.ParamSize = cashDividendRate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_cashDividendPerShare";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = cashDividendPerShare;
                v_objParam.ParamSize = cashDividendPerShare.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_exercisedate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exerciseDate;
                v_objParam.ParamSize = exerciseDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_isincode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = isinCode;
                v_objParam.ParamSize = isinCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_actiondate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = estimateReceivingDate;
                v_objParam.ParamSize = estimateReceivingDate.Length;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_CASH_DIVIDEND, ref v_arrParam, 8);
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
                Log.Error("cashdividend:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object stockbonus(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", symbol = "", dividendRate = "", exercisePriceForOddShare = "", quantityUnit = "", 
                    quantityDecimals = "",exerciseDate = "", isinCode = "", estimateReceivingDate = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("dividendRate", out jToken))
                    dividendRate = jToken.ToString();
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
                StoreParameter[] v_arrParam = new StoreParameter[11];

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
                v_objParam.ParamName = "p_exercisePriceForOddShare";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exercisePriceForOddShare;
                v_objParam.ParamSize = exercisePriceForOddShare.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantityUnit";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityUnit;
                v_objParam.ParamSize = quantityUnit.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantityDecimals";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityDecimals;
                v_objParam.ParamSize =  quantityDecimals.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_exercisedate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exerciseDate;
                v_objParam.ParamSize = exerciseDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_isincode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = isinCode;
                v_objParam.ParamSize = isinCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_actiondate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = estimateReceivingDate;
                v_objParam.ParamSize = estimateReceivingDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_STOCK_BONUS, ref v_arrParam, 9);
                string v_strerrorMessage = (string)v_arrParam[10].ParamValue;

                //if (returnErr == 0)
                //{
                //    idResponse id = new idResponse() { id = (string)v_arrParam[0].ParamValue };
                //    return modCommon.getBoResponseWithData(returnErr, id, v_strerrorMessage);
                //}

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("cashdividend:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object right(string strRequest, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string requestId = "", symbol = "", transferStartDate = "", transferEndDate = "", subscribeStartDate = "", subscribeEndDate = "",
                       isTransferAllowed = "", exercisePrice = "", rightOfferingRatio = "", rightToPurchaseSharesRatio = "",
                       quantityRoundType = "", exerciseDate = "", isinCode = "", estimateReceivingDate = "";

                //if (request.TryGetValue("custodycd", out jToken))
                //    custodycd = jToken.ToString();

                if (request.TryGetValue("requestId", out jToken))
                    requestId = jToken.ToString();
                if (request.TryGetValue("symbol", out jToken))
                    symbol = jToken.ToString();
                if (request.TryGetValue("transferStartDate", out jToken))
                    transferStartDate = jToken.ToString();
                if (request.TryGetValue("transferEndDate", out jToken))
                    transferEndDate = jToken.ToString();
                if (request.TryGetValue("subscribeStartDate", out jToken))
                    subscribeStartDate = jToken.ToString();
                if (request.TryGetValue("subscribeEndDate", out jToken))
                    subscribeEndDate = jToken.ToString();
                if (request.TryGetValue("isTransferAllowed", out jToken))
                    isTransferAllowed = jToken.ToString();
                if (request.TryGetValue("exercisePrice", out jToken))
                    exercisePrice = jToken.ToString();
                if (request.TryGetValue("rightOfferingRatio", out jToken))
                    rightOfferingRatio = jToken.ToString();
                if (request.TryGetValue("rightToPurchaseSharesRatio", out jToken))
                    rightToPurchaseSharesRatio = jToken.ToString();
                if (request.TryGetValue("quantityRoundType", out jToken))
                    quantityRoundType = jToken.ToString();
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
                StoreParameter[] v_arrParam = new StoreParameter[16];

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
                v_objParam.ParamName = "p_transferStartDate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferStartDate;
                v_objParam.ParamSize = transferStartDate.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_transferEndDate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = transferEndDate;
                v_objParam.ParamSize = transferEndDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_subscribeStartDate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = subscribeStartDate;
                v_objParam.ParamSize = subscribeStartDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_subscribeEndDate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = subscribeEndDate;
                v_objParam.ParamSize = subscribeEndDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_isTransferAllowed";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = isTransferAllowed;
                v_objParam.ParamSize = isTransferAllowed.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_exercisePrice";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exercisePrice;
                v_objParam.ParamSize = exercisePrice.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_rightOfferingRatio";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = rightOfferingRatio;
                v_objParam.ParamSize = rightOfferingRatio.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_rightToPurchaseSharesRatio";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = rightToPurchaseSharesRatio;
                v_objParam.ParamSize = rightToPurchaseSharesRatio.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_quantityRoundType";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = quantityRoundType;
                v_objParam.ParamSize = quantityRoundType.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_exercisedate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = exerciseDate;
                v_objParam.ParamSize = exerciseDate.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_isincode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = isinCode;
                v_objParam.ParamSize = isinCode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_actiondate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = estimateReceivingDate;
                v_objParam.ParamSize = estimateReceivingDate.Length;
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


                long returnErr = TransactionProcess.doTransaction(COMMAND_PO_RIGHT, ref v_arrParam, 14);
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
                Log.Error("right:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
    }
}
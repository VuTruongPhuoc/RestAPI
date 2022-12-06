using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Bussiness
{
    public class BussinessObject
    {
    }

    public class list
    {
        public string s = "";
        public object[] d;
    }

    public class BoResponse
    {
        public string s = "";
        public string errmsg = "";
    }
    public class BoResponseWithData
    {
        public string s = "";
        public string errmsg = "";
        public object d = null;
    }
    public class orderResponse
    {
        public string orderid = "";
    }
    public class idResponse
    {
        public string id = "";
    }

    public class KeyField
    {
        public string keyName = "";
        public string keyValue = "";
        public string keyType = "";   //VARCHAR2 OR NUMBER
    }

    public class HealthCheck
    {
        public string errorCode = "200";
        public string dbHostStatus = "ok";
        public string dbReportStatus = "ok";
        //public string enPayStatus = "ok";
    }

    // DNS.2022.11.0.49 DNSE-1862
    public class checkHOStatus
    {
        public string errorCode = "ok";
        public string hoStatus = "";
    }

    public class loanpaymentResponse
    {
        public double overdueFee = 0;
        public double dueFee = 0;
        public double undueFee = 0;
        public double interestOnOverduePrincipal = 0;
        public double overdueInterest = 0;
        public double dueInterest = 0;
        public double undueInterest = 0;
        public double overduePrincipal = 0;
        public double duePrincipal = 0;
        public double unduePrincipal = 0;
        public double feeOnOverduePrincipal = 0;
        public string isSettled;

    }
    public class loanrate
    {
        public double newundueInterest = 0;
        public double newdueInterest = 0;
        public double newoverdueInterest = 0;
        public double newinterestOnOverduePrincipal = 0;
        public double newundueFee = 0;
        public double newdueFee = 0;
        public double newoverdueFee = 0;
        public double newfeeOnOverduePrincipal = 0;
    }

    public class loandrawndown
    {
        public Int64 id;
    }

    public class LoanTypeId
    {
        public string lnTypeId = ""; 
    }

    public class EditLoanType
    {
        public string lnTypeId = "";
        public string autoApply = "";
        public string basis = "";
        public string loanCalendar = "";
        public Int64 preferentialDays = 0;
        public Int64 term = 0;
        public double rate1 = 0;
        public double rate2 = 0;
        public double rate3 = 0;
        public double cfRate1 = 0;
        public double cfRate2 = 0;
        public double cfRate3 = 0;
        public string autoPrepay = "";
        public string autoRenew = "";
        public double prepayFee = 0;
        public Int64 warningDays = 0;
        public string notes = "";
    }

    public class keyIdCamastid
    {
        public string keyId = "";
    }

    public class accountSavingSettlement
    {
        public string principal_txnum = "";
        public string interest_txnum = "";
        public string taxAmount_txnum = "";
    }

    public class accountSavingSettlementErr1211
    {
        public string principal_errmsg = "";
    }

    public class accountSavingSettlementErr1212
    {
        public string interest_errmsg = "";
    }

    public class accountSavingSettlementErr1213
    {
        public string taxAmount_errmsg = "";
    }
}
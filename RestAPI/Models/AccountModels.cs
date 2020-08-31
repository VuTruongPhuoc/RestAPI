using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{

    #region execution
    public class Execution
    {
        public string id = "";
        public string instrument = "";
        public long price = 0;
        public string time = "";
        public long qty = 0;
        public string side = "";
    }
    #endregion

    #region orders
    public class orders
    {
        public string id = "";
        public string instrument = "";
        public long qty = 0;
        public string side = "";
        public string type = "";
        public string timeInForce = "";
        public long filledqty = 0;
        public double avgprice = 0;
        public long limitprice = 0;
        public long stopprice = 0;
        public string parentid = "";
        public string parenttype = "";
        public string duration = "";
        public string status = "";
        public string txdate = "";
        public string lastModified = "";
        public string createdDate = "";
    }
    public class ordersInfo
    {
        public string txdate = "";
        public string txtime = "";
        public string action = "";
        public string orderId = "";
        public long matchQtty = 0;
        public long matchPrice = 0;
        public long quoteQtty = 0;
        public long quotePrice = 0;
    }
    #endregion

    #region ordersHistory
    public class ordersHistory
        {
        public string id = "";
        public string instrument = "";
        public long qty = 0;
        public string side = "";
        public string type = "";
        public long filledqty = 0;
        public long avgprice = 0;
        public long limitprice = 0;
        public long stopprice = 0;
        public string parentid = "";
        public string parenttype = "";
        public string duration = "";
        public string status = "";
        public long lastModified = 0;
    }
    #endregion

    #region "Account"
    public class Account
    {
        public string id = "";
        public string name = "";
        public string currency = "";
        public string accountType = "";
        public string accountTypeName = "";
        public string openDate = "";
    }
    #endregion

    public class PPSE
    {
        public long ppse = 0;
        public long maxQtty = 0;
        public long trade = 0;
    }

    public class summaryAccount
    {
        public long balance = 0;
        public long cibalance = 0;
        public long tdbalance = 0;
        public long interestbalance = 0; 
        public long receivingt1 = 0;
        public long receivingt2 = 0;
        public long receivingt3 = 0;
        public long securitiesamt = 0;
        public long marginqttyamt = 0;
        public long nonmarginqttyamt = 0;
        public long dfqttyamt = 0;
        public long totaldebtamt = 0;
        public long secureamt = 0;
        public long trfbuyamt = 0;
        public long marginamt = 0;
        public long t0debtamt = 0;
        public long advancedamt = 0;
        public long dfdebtamt = 0;
        public long tddebtamt = 0;
        public long depofeeamt = 0;
        public long netassetvalue = 0;
        public long requiredmarginamt = 0;
        public long sesecuredavl = 0;
        public long sesecured_buy = 0;
        public long accountvalue = 0;
        public long qttyamt = 0;
        public long mrcrlimit = 0;
        public long bankavlbal = 0;
        public long debtamt = 0;
        public long advancemaxamtfee = 0;
        public long receivingamt = 0;
        public long basicpurchasingpower = 0;
        public long marginrate = 0;
        public long holdbalance = 0;
        public string bankinqirytime = "";
    }

    public class securitiesPortfolio
    {
        public string custodycd = "";
        public string accountid = "";
        public string symbol = "";
        public long total = 0;
        public long trade = 0;
        public long blocked = 0;
        public long vsdmortgage = 0;
        public long mortgage = 0;
        public long restrict = 0;
        public long receivingright = 0;
        public long receivingt0 = 0;
        public long receivingt1 = 0;
        public long receivingt2 = 0;
        public double costprice = 0;
        public long costpriceamt = 0;
        public long basicprice = 0;
        public long basicpriceamt = 0;
        public long marginratio = 0;
        public long requiredmarginamt = 0;
        public long marginamt = 0;
        public long pnlamt = 0;
        public string pnlrate = "";
        public string issell = "";
        public long withdraw = 0;
    }

    public class WithdrawalTransactionInfo : WithdrawalTransaction
    {
        public string enpayTransId;
        public string status;
        public string code;
        public string message;
    }

    public class WithdrawalTransaction
    {
        public string benAccNo;
        public string benAccType;
        public string benName;
        public string desc;
        public int amt;
        public string benBankName;
        public string benBankCode;
        public string benBankBranchName;
        public string benBankBranchCode;
        public string benBankNapasCode;
        public string srcTransId;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using CommonLibrary;
using RestAPI.Schedulers;

[assembly: OwinStartup(typeof(RestAPI.Startup))]

namespace RestAPI
{
    public partial class Startup
    {
        private const string KEY_PUT_BATCH_PROCESS = "PUT_BATCH_PROCESS";

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            if (modCommond.GetConfigValue(KEY_PUT_BATCH_PROCESS, "N").Equals("Y"))
                PutBatch2BankScheduler.Instance.Start(); // Call enpay
        }
    }
}
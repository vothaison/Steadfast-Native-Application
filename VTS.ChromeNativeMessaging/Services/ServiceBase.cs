using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VTS.ChromeNativeMessaging.Services
{
    class ServiceBase<RQ, RS>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string LogMessagePrefix = "ServiceLog: ";

        public RS Run(RQ request)
        {
            string className = this.GetType().FullName;
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                log.DebugFormat("{0} Begin {1} Service Run. Request: {2}", this.LogMessagePrefix, className, JsonConvert.SerializeObject(request));
                
                RS result = DoRun(request);

                stopwatch.Stop();

                log.DebugFormat("{0} Finish {1} Service Run in {2}ms. Response: {3}", this.LogMessagePrefix, className, stopwatch.Elapsed.TotalMilliseconds, JsonConvert.SerializeObject(result));

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                log.Error(this.LogMessagePrefix + ex.Message, ex);
                throw;
            }

        }

        protected virtual RS DoRun(RQ request)
        {
            throw new NotImplementedException();
        }
    }
}

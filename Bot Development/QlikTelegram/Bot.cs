using QlikSenseEasy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace QlikTelegram {
    public class Bot {
        private Timer AlertTimer = new Timer(AppSettings.AlertSeconds);

        public Bot() {
            //alert settup
            //AlertTimer.AutoReset = true;
            //AlertTimer.Elapsed += CheckAlerts;
            //AlertTimer.Start();
        }
        
        protected void InitializeQSServer() {
            QSApp QS = new QSApp();
            QS.qsAppName = AppSettings.DemoQsAppName;
            QS.qsAppId = AppSettings.DemoQsAppId;
            QS.qsServer = AppSettings.DemoQsServer;
            if (ConfigurationManager.AppSettings["DemoqsServerSSL"] != null
                && ConfigurationManager.AppSettings["DemoqsServerSSL"].ToLower().StartsWith("y"))
                cntqsServerSSL = true;
            cntqsServerVirtualProxy = ConfigurationManager.AppSettings["DemoqsServerVirtualProxy"];
            if (cntqsServerVirtualProxy == null) cntqsServerVirtualProxy = "";
            QS.qsSingleServer = AppSettings.DemoQsSingleServer;
            QS.qsSingleApp = AppSettings.DemoQsSingleApp;
            QS.qsAlternativeStreams = AppSettings.QsAlternativeStreams;
           
        }
    }
}

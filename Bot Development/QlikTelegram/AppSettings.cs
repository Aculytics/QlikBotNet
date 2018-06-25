using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace QlikTelegram {
    public static class AppSettings {
        public static readonly long AlertSeconds = Convert.ToInt64(ConfigurationManager.AppSettings["AlertSeconds"]) * 1000;
        public static readonly string BotType = ConfigurationManager.AppSettings["BotType"];
        // Telegram Bot Settings
        //talk to userinfobot to find out your user id
        public static readonly long TelegramAdministratorId = Convert.ToInt64(ConfigurationManager.AppSettings["cntTelegramAdministratorID"]);
        public static readonly string TelegramBotToken = ConfigurationManager.AppSettings["cntBotToken"];
        public static readonly long TelegramBotLocalAdministrator;

        // Jabber Bot Settings
        public static readonly string JabberUser = ConfigurationManager.AppSettings["JabberUser"];
        public static readonly string JabberPassword = ConfigurationManager.AppSettings["JabberPassword"];
        public static readonly string JabberServer = ConfigurationManager.AppSettings["JabberServer"];
        public static readonly string JabberConnectServer = ConfigurationManager.AppSettings["JabberConnectServer"];

        public static readonly string DemoQsAppName = ConfigurationManager.AppSettings["DemoqsAppName"];
        public static readonly string DemoQsAppId = ConfigurationManager.AppSettings["DemoqsAppId"];
        public static readonly string DemoQsServer = ConfigurationManager.AppSettings["DemoqsServer"];
        public static readonly string DemoQsServerHeaderAuth = ConfigurationManager.AppSettings["DemoqsServerHeaderAuth"];

        public static readonly string DemoQsSingleServer = ConfigurationManager.AppSettings["DemoqsSingleServer"];
        public static readonly string DemoQsSingleApp = ConfigurationManager.AppSettings["DemoqsSingleApp"];
        public static readonly string QsAlternativeStreams = ConfigurationManager.AppSettings["cntAlternativeStreams"];
        public static readonly string QsStreamIdPublishNewApps = ConfigurationManager.AppSettings["cntStreamIdPublishNewApps"];

        public static readonly string QsSheetForAnalysis = ConfigurationManager.AppSettings["cntQSSheetForAnalysis"];
        public static readonly string NPrintingImgsPath = ConfigurationManager.AppSettings["NPrintingImgsPath"];
        public static readonly string NPrintingDefaultReport = ConfigurationManager.AppSettings["NPrintingDefaultReport"];

        public static readonly string QlikUsersCSV = ConfigurationManager.AppSettings["cntQlikUsersCSV"];

        public static readonly string LuisURL = ConfigurationManager.AppSettings["cntLuisURL"];
        public static readonly string LuisAppID = ConfigurationManager.AppSettings["cntLuisAppID"];
        public static readonly string LuisKey = ConfigurationManager.AppSettings["cntLuisKey"];

        public static readonly string ApiAiKey = ConfigurationManager.AppSettings["cntApiAiKey"];
        public static readonly string ApiAiLanguage = ConfigurationManager.AppSettings["cntApiAiLanguage"];

        public static readonly string RasaUrl = ConfigurationManager.AppSettings["cntRasaUrl"];

        public static readonly string BingSearchKey = ConfigurationManager.AppSettings["cntBingSearchKey"];

        public static readonly string CaptureImageApp = ConfigurationManager.AppSettings["cntCaptureImageApp"];
        public static readonly string CaptureWeb = ConfigurationManager.AppSettings["cntCaptureWeb"];
        public static readonly string CaptureTimeout = ConfigurationManager.AppSettings["cntCaptureTimeout"];

        public static readonly string FolderConnection = ConfigurationManager.AppSettings["cntFolderConnection"];

        

        static AppSettings() {
            if (ConfigurationManager.AppSettings["cntBotLocalAdministrator"] != null && ConfigurationManager.AppSettings["cntBotLocalAdministrator"].Trim().Length > 0) {
                TelegramBotLocalAdministrator = Convert.ToInt32(ConfigurationManager.AppSettings["cntBotLocalAdministrator"]);
            }
            else {
                TelegramBotLocalAdministrator = -1;
            }
            
            if (NPrintingDefaultReport == null || NPrintingDefaultReport == "") {
                NPrintingDefaultReport = "ReportSales.pdf";
            }

            
        }
    }
}

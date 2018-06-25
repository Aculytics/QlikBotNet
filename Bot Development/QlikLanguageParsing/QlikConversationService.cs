using QlikNLP;
using QlikSenseEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlikConversationService {
    public enum ResponseAction { None = 0, OpenApp = 1, ShowKPI = 2, ShowDimension = 3, ShowMeasure = 4, ShowSheet = 5, ShowStory = 6 };

    public class ResponseOptions {
        public string Title;
        public string ID;
        public ResponseAction Action;
    }
    public class Response {
        public string TextMessage = "";
        public string VoiceMessage = "";
        public string NewsSearch = "";
        public NLP NLPrediction;
        public string WarningText = "";
        public string ErrorText = "";
        public string OtherAction = "";
        public QSFoundObject ChartFound;
        public List<ResponseOptions> Options = new List<ResponseOptions>();
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace QlikNLP {
    public class RasaNLU {
        private string _ServerUrl;

        public RasaNLU(string ServerUrl)
        {
            _ServerUrl = ServerUrl;
        }

        private PredictedIntent Predicted = new PredictedIntent();
        private readonly HttpClient client = new HttpClient();

        public bool HasPrediction { get { return Predicted.HasPrediction; } }
        public string OriginalQuery { get { return Predicted.OriginalQuery; } }
        public IntentType Intent { get { return Predicted.Intent; } }
        public string Measure { get { return Predicted.Measure; } }
        public string Measure2 { get { return Predicted.Measure2; } }
        public string Element { get { return Predicted.Element; } }
        public string Dimension { get { return Predicted.Dimension; } }
        public string Dimension2 { get { return Predicted.Dimension2; } }
        public string Percentage { get { return Predicted.Percentage; } }
        public double? Number { get { return Predicted.Number; } }
        public string ChartType { get { return Predicted.ChartType; } }
        public double DistanceKm { get { return Predicted.DistanceKm; } }
        public string Language { get { return Predicted.Language; } }
        public string Response { get { return Predicted.Response; } }

        public bool Predict(string TextToPredict) {
            Predicted.HasPrediction = false;
            Predicted.OriginalQuery = null;
            Predicted.Intent = IntentType.None;
            Predicted.Response = null;

            Predicted.Measure = null;
            Predicted.Measure2 = null;
            Predicted.Element = null;
            Predicted.Dimension = null;
            Predicted.Dimension2 = null;
            Predicted.Percentage = null;
            Predicted.Number = 0;
            Predicted.ChartType = null;
            Predicted.DistanceKm = 0;
            Predicted.Language = null;
            string DistanceUnit = "km";

            try {
                var json = QueryServer(TextToPredict).Result;
                Console.WriteLine(json);
                dynamic result = JsonConvert.DeserializeObject(json);
                dynamic entities = result.entities;
                dynamic intentRankings = result.intent_ranking;
                string intent = result.intent.name;

                //if (!response.IsError && response.Status.ErrorType == "success") {
                if (true) {
                    foreach (var entity in entities) {
                        string strParam = entity.value;
                        string strName = entity.entity;

                        if (strParam.StartsWith("[\r\n")) 
                        {
                            var newStrParam = strParam.Replace("[\r\n  \"", "").Replace("\"\r\n]", "");
                            strParam = newStrParam;
                        }
                            

                        if (strName == "Measure" && strParam != "") Predicted.Measure = strParam;
                        if (strName == "Measure2" && strParam != "") Predicted.Measure2 = strParam;
                        if (strName == "Element" && strParam != "") Predicted.Element = strParam;
                        if (strName == "Dimension" && strParam != "") Predicted.Dimension = strParam;
                        if (strName == "Dimension1" && strParam != "") Predicted.Dimension2 = strParam;
                        if (strName == "Percentage" && strParam != "") {
                            Predicted.Percentage = strParam;
                            Predicted.Number = PercentageToDouble(Predicted.Percentage);
                        }
                        if (strName == "Number" && strParam != "") Predicted.Number = Convert.ToDouble(strParam);
                        if (strName == "ChartType" && strParam != "") Predicted.ChartType = strParam;
                        if (strName == "Distance" && strParam != "") Predicted.DistanceKm = Convert.ToDouble(strParam);
                        if (strName == "Language" && strParam != "") Predicted.Language = strParam;
                        if (strName == "DistanceUnit" && strParam != "") DistanceUnit = strParam.ToLower();
                    }
                    if (DistanceKm > 0 && DistanceUnit != "km") {
                        switch (DistanceUnit) {
                            case "mi": case "miles": Predicted.DistanceKm *= 1.61; break;
                            case "m": case "meters": Predicted.DistanceKm *= 0.001; break;
                        }
                    }


                    if (intent == "input.unknown") {
                        this.Predicted.HasPrediction = false;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.None;
                        this.Predicted.Number = 0;
                        this.Predicted.Response = "I do not understand you, could you try again with other question?";
                    }
                    else if (intent == "Default Welcome Intent") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.GoodAnswer;
                        this.Predicted.Number = 0;
                        this.Predicted.Response = "Howdy!";
                    }
                    /*else if (response.Result.Source == "domains") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.DirectResponse;
                        this.Predicted.Response = response.Result.Fulfillment.Speech;
                    }*/
                    else if (intent == "qs-ShowMeasure") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.KPI;
                        this.Predicted.Response = "You want to know the value of " + Predicted.Measure;
                    }
                    else if (intent == "qs-ShowChart") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Chart;
                        this.Predicted.Response = "You want to know the value of " + Predicted.Measure;
                    }
                    else if (intent == "qs-ShowMeasureForElement") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Measure4Element;
                        this.Predicted.Response = "You want to know the value of " + Predicted.Measure;
                    }
                    else if (intent == "qs-Alert") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Alert;
                        this.Predicted.Response = "You want to be alerted when " + Predicted.Measure + " changes by " + Predicted.Percentage;
                    }
                    else if (intent == "qs-GoodAnswer") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.GoodAnswer;
                        this.Predicted.Response = ":-)";
                    }
                    else if (intent == "qs-RankingTop") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.RankingTop;
                        this.Predicted.Response = "You want to know the top elements by " + Predicted.Dimension;
                    }
                    else if (intent == "qs-RankingBottom") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.RankingBottom;
                        this.Predicted.Response = "You want to know the bottom elements by " + Predicted.Dimension;
                    }
                    else if (intent == "qs-Filter") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Filter;
                        this.Predicted.Response = "You want to filter " + Predicted.Dimension + " by " + Predicted.Element;
                    }
                    else if (intent == "Hello" || intent == "input.welcome") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Hello;
                        this.Predicted.Response = "Hello";
                    }
                    else if (intent == "Bye") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Bye;
                        this.Predicted.Response = "Bye";
                    }
                    else if (intent == "BadWords") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.BadWords;
                        this.Predicted.Response = ":-(\nI prefer not to answer this type of questions, I am a robot but I am very polite.";
                    }
                    else if (intent == "qs-Reports") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Reports;
                        this.Predicted.Response = "I will show you the available reports";
                    }
                    else if (intent == "qs-CreateChart") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.CreateChart;
                        this.Predicted.Response = "I will create a chart for you";
                    }
                    else if (intent == "qs-ShowAnalysis") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowAnalysis;
                        this.Predicted.Response = "I will create an analysis for you";
                    }
                    else if (intent == "qs-GeoFilter") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.GeoFilter;
                        this.Predicted.Response = "I will filter the information based on your location";
                    }
                    else if (intent == "qs-ContactQlik") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ContactQlik;
                        this.Predicted.Response = "You can go to www.qlik.com";
                    }
                    else if (intent == "Help") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Help;
                        this.Predicted.Response = "You can ask for any information in the current Qlik Sense app";
                    }
                    else if (intent == "PersonalInformation") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.PersonalInformation;
                        this.Predicted.Response = "I think you better go to https://www.facebook.com/profile.php?id=100016396000544 and know everything about me";    // StringResources
                    }
                    else if (intent == "qs-ClearAllFilters") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ClearAllFilters;
                        this.Predicted.Response = "I will clear all filters";
                    }
                    else if (intent == "qs-ClearDimensionFilter") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ClearDimensionFilter;
                        this.Predicted.Response = "I will clear this dimension filter";
                    }
                    else if (intent == "qs-CurrentSelections") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.CurrentSelections;
                        this.Predicted.Response = "I will show you the current selections in the app";
                    }
                    else if (intent == "qs-ShowAllApps") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowAllApps;
                        this.Predicted.Response = "I will show all the available apps you can connect";
                    }
                    else if (intent == "qs-ShowAllDimensions") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowAllDimensions;
                        this.Predicted.Response = "I will show you all the master dimensions in the current app";
                    }
                    else if (intent == "qs-ShowAllMeasures") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowAllMeasures;
                        this.Predicted.Response = "I will show you all the master measures in the current app";
                    }
                    else if (intent == "qs-ShowAllSheets") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowAllSheets;
                        this.Predicted.Response = "I will show you all the sheets in the current app";
                    }
                    else if (intent == "qs-ShowAllStories") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowAllStories;
                        this.Predicted.Response = "I will show you all the stories in the current app";
                    }
                    else if (intent == "qs-ShowKPIs") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowKPIs;
                        this.Predicted.Response = "I will show you the most used metrics in the current app";
                    }
                    else if (intent == "qs-ShowMeasureByMeasure") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowMeasureByMeasure;
                        this.Predicted.Response = "I will show you the result of analyzing these two measures";
                    }
                    else if (intent == "qs-ShowListOfElements") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowListOfElements;
                        this.Predicted.Response = "I will show you a list of elements for this dimension";
                    }
                    else if (intent == "ChangeLanguage") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ChangeLanguage;
                        this.Predicted.Response = "I will change the language";
                    }
                    else if (intent == "Apologize") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.Apologize;
                        this.Predicted.Response = "OK";
                    }
                    else if (intent == "qs-ShowElementsAboveValue") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowElementsAboveValue;
                        this.Predicted.Response = "You want to know the elements that meet a measure is above this value";
                    }
                    else if (intent == "qs-ShowElementsBelowValue") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.ShowElementsBelowValue;
                        this.Predicted.Response = "You want to know the elements that meet a measure is below this value";
                    }
                    else if (intent == "qs-CreateCollaborationGroup") {
                        this.Predicted.HasPrediction = true;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.CreateCollaborationGroup;
                        this.Predicted.Response = "You want to create a group to collaborate with other people";
                    }
                    else {
                        this.Predicted.HasPrediction = false;
                        this.Predicted.OriginalQuery = TextToPredict;
                        this.Predicted.Intent = IntentType.None;
                        this.Predicted.Response = "I do not have the logic to answer " + TextToPredict + " yet";
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                Predicted.HasPrediction = false;
            }

            return Predicted.HasPrediction;
        }

        private double PercentageToDouble(string Percentage) {
            double d = 0;
            string strPct = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentSymbol;

            if (!Percentage.Contains(strPct)) return 0;

            try {
                d = double.Parse(Percentage.Replace(strPct, "")) / 100;
            }
            catch (Exception e) {
                d = 0;
            }

            return d;
        }

        private async Task<string> QueryServer(string TextToPredict) {
            var query = JsonConvert.SerializeObject(new Dictionary <string, string> {
                    { "q",TextToPredict }
                });
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_ServerUrl + @"/parse", content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}

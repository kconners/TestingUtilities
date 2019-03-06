using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using Newtonsoft.Json.Linq;

using TestingUtilities.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TestingUtilities
{
    public class RestHelper
    {
        string Endpoint { get; set; }
        string Client_ID { get; set; }
        string Client_Secret { get; set; }
        string Token { get; set; }

        private static string dataProperty = "data";
        private static string statusProperty = "status";
        private static string messageProperty = "message";
        private static string responseCodeProperty = "responseCode";

        public void SetDataProperty(string newPropertyName)
        {
            dataProperty = newPropertyName;
        }
        public void SetStatusProperty(string newPropertyName)
        {
            statusProperty = newPropertyName;
        }
        public void SetMessageProperty(string newPropertyName)
        {
            messageProperty = newPropertyName;
        }
        public void SetResponceCodeProperty(string newPropertyName)
        {
            responseCodeProperty = newPropertyName;
        }



        WebElem webElem = new WebElem();

        static HttpClient CLIENT;
        public RestHelper(string URL)
        {
            Endpoint = URL;
        }
        
        public string GetAuth()
        {
            string _token = string.Empty;

            Rest.ApplicationPermissions applicationPermissions = new Rest.ApplicationPermissions { clientId = Client_ID, clientSecret = Client_Secret };


            ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            var client = new RestClient(Endpoint + "/auth/token");
            var request = new RestRequest(Method.POST);

            request.AddJsonBody(applicationPermissions);

            IRestResponse response = client.Execute(request);

            Rest.Auth _auth = new Rest.Auth();
            _auth = Newtonsoft.Json.JsonConvert.DeserializeObject<Rest.Auth>(response.Content);
            _token = _auth.data.accessToken;
            Token = _token;
            return _token;
        }
        public IRestResponse GetAuth(string _Endpoint, string _Client_ID, string _Client_Secret)
        {
            string _token = string.Empty;

            Rest.ApplicationPermissions applicationPermissions = new Rest.ApplicationPermissions { clientId = _Client_ID, clientSecret = _Client_Secret };


            ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            var client = new RestClient(_Endpoint + "/auth/token");
            var request = new RestRequest(Method.POST);

            request.AddJsonBody(applicationPermissions);

            IRestResponse response = client.Execute(request);
            return response;
        }

        public Guid GetAuthTokenFromAuthResponce(IRestResponse restResponse)
        {
            string _token = string.Empty;
            Rest.Auth _auth = new Rest.Auth();
            _auth = Newtonsoft.Json.JsonConvert.DeserializeObject<Rest.Auth>(restResponse.Content);
            _token = _auth.data.accessToken;
            Token = _token;
            return new Guid(_token);
        }

        public enum verb
        {
            post,
            put,
            get,
            patch
        }


        public Models.Rest.RestResponce PUT(string _route, Dictionary<string, string> QueryParams, object body)
        {
            return MakeCall(verb.put, _route, QueryParams, body);
        }
        public Models.Rest.RestResponce PUT(string _route, object body)
        {
            return MakeCall(verb.put, _route, new Dictionary<string, string> { }, body);
        }
        
        public Models.Rest.RestResponce POST(string _route, object body)
        {
            return MakeCall(verb.post, _route, new Dictionary<string, string> { }, body);
        }
        public Models.Rest.RestResponce POST(string _route, Dictionary<string, string> QueryParams, object body)
        {
            return MakeHTTPClientCall(verb.post, _route, QueryParams, body);
        }
        
        public Models.Rest.RestResponce PATCH(string _route, Dictionary<string, string> QueryParams, object body)
        {
            return MakeCall(verb.patch, _route, QueryParams, body);
        }
        public Models.Rest.RestResponce PATCH(string _route, object body)
        {
            return MakeCall(verb.patch, _route, new Dictionary<string, string> { }, body);
        }
        
        public Models.Rest.RestResponce GET(string _route, Dictionary<string, string> QueryParams, bool GetAuth = false)
        {
            return MakeCall(verb.get, _route, QueryParams);
        }
        public Models.Rest.RestResponce GET(string _route, bool GetAuth = false)
        {
            if (GetAuth == true)
            {
                return MakeCall(verb.get, _route, new Dictionary<string, string> { });
            }
            else
            {
                return MakeCallNoAuth(verb.get, _route, new Dictionary<string, string> { });
            }
        }

        string PostAsync(string _route, Dictionary<string, string> QueryParams, object _body)
        {
            string QueryString = string.Empty;
            int countr = 1;
            if (QueryParams.Count >= 1)
            {
                QueryString = "?";
                foreach (var param in QueryParams)
                {
                    if (countr == 1)
                    { QueryString = QueryString + param.Key + "=" + param.Value; }

                    else
                    { QueryString = QueryString + "&" + param.Key + "=" + param.Value; }
                    countr = countr++;
                }
            }

            string DATA = Newtonsoft.Json.JsonConvert.SerializeObject(_body);
            System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");

            HttpResponseMessage response = CLIENT.PostAsync(CLIENT.BaseAddress + _route + QueryString, content).Result;
            string description = string.Empty;
            try
            {
                string result = response.Content.ReadAsStringAsync().Result;
                description = result;
            }
            catch
            {
                webElem.Log(string.Format("There was an issue with the responce from a Post call to {0}", _route));
            }

            return description;
        }

        public void ChangePropertyName(JObject BeingFixed, JObject Original, string FixedPropertyName, string OriginalPropertyName)
        {
            IDictionary<string, JToken> dictionary = BeingFixed;
            if (dictionary.ContainsKey(OriginalPropertyName))
            {
                BeingFixed.Property(OriginalPropertyName).Replace(new JProperty(FixedPropertyName, Original.Property(OriginalPropertyName).Value));
            }

        }
        private Models.Rest.RestResponce MakeHTTPClientCall(verb _verb, string _route, Dictionary<string, string> QueryParams, object body = null)
        {

            Models.Rest.RestResponce restResponce = new Models.Rest.RestResponce();

            string Output = string.Empty;
            switch (_verb)
            {
                case verb.get:

                    break;
                case verb.patch:

                    break;
                case verb.post:
                    Output = Newtonsoft.Json.JsonConvert.SerializeObject(PostAsync(_route, QueryParams, body));
                    break;
                case verb.put:

                    break;
            }

            JValue jValue = (JValue)Output;
            string LLLL = Output.ToString();

            dynamic respo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(LLLL);
            JObject RET = JObject.Parse(respo);

            restResponce.data = RET[dataProperty];
            restResponce.status = RET.Value<string>(statusProperty);
            restResponce.userMessage = RET.Value<string>(messageProperty);
            restResponce.responseCode = RET.Value<int>(responseCodeProperty);

            restResponce.isError = Convert.ToBoolean(RET.Value<string>("isError"));
            restResponce.developerMessage = RET["developerMessage"];
            restResponce.moreInfo = RET["moreInfo"];
            restResponce.httpStatusCode = RET.Value<int>("httpStatusCode");





            if (restResponce.isError == true)
            {
                restResponce.errors = new List<Rest.Error>();
                try
                {
                    JArray errors = (JArray)RET["errors"];
                    foreach (var i in errors)
                    {
                        Rest.Error error = new Rest.Error();
                        error._object = i.Value<string>("object");
                        error._key = i.Value<string>("key");
                        error._value = i.Value<string>("value");
                        error._isVisible = i.Value<bool>("isVisible");
                        error._errorMessage = i.Value<string>("errorMessage");

                        restResponce.errors.Add(error);
                    }
                }
                catch
                {
                    JObject error = (JObject)RET["errors"];
                    Rest.Error er = new Rest.Error();
                    er._object = error.Value<string>("object");
                    er._key = error.Value<string>("key");
                    er._value = error.Value<string>("value");
                    er._isVisible = error.Value<bool>("isVisible");
                    er._errorMessage = error.Value<string>("errorMessage");

                    restResponce.errors.Add(er);
                }
            }

            return restResponce;
        }

        private Models.Rest.RestResponce MakeCallNoAuth(verb _verb, string _route, Dictionary<string, string> QueryParams, object body = null)
        {
            Models.Rest.RestResponce restResponce = new Models.Rest.RestResponce();
            
            var client = new RestClient(Endpoint + _route);
            var request = new RestRequest();
            switch (_verb)
            {
                case verb.get:
                    request = new RestRequest(Method.GET);
                    break;
                case verb.patch:
                    request = new RestRequest(Method.PATCH);
                    break;
                case verb.post:
                    request = new RestRequest(Method.POST);
                    break;
                case verb.put:
                    request = new RestRequest(Method.PUT);
                    break;
            }
            foreach (var i in QueryParams)
            {
                request.AddQueryParameter(i.Key, i.Value);
            }
        
            request.AddHeader("Accept", "application/json");
            
            if (body != null)
            {
                request.AddParameter("undefined", body, ParameterType.RequestBody);
            }
            
            var response = client.Execute(request);
            string L = response.Content;

            JObject respo = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(L);

            restResponce.data = respo[dataProperty];
            restResponce.status = respo.Value<string>(statusProperty);
            restResponce.userMessage = respo.Value<string>(messageProperty);
            restResponce.responseCode = respo.Value<int>(responseCodeProperty);

            restResponce.isError = Convert.ToBoolean(respo.Value<string>("isError"));
            restResponce.developerMessage = respo["developerMessage"];
            restResponce.moreInfo = respo["moreInfo"];
            restResponce.httpStatusCode = respo.Value<int>("httpStatusCode");


            if (restResponce.isError == true)
            {
                restResponce.errors = new List<Rest.Error>();
                try
                {
                    JArray errors = (JArray)respo["errors"];
                    foreach (var i in errors)
                    {
                        Rest.Error error = new Rest.Error();
                        error._object = i.Value<string>("object");
                        error._key = i.Value<string>("key");
                        error._value = i.Value<string>("value");
                        error._isVisible = i.Value<bool>("isVisible");
                        error._errorMessage = i.Value<string>("errorMessage");

                        restResponce.errors.Add(error);
                    }
                }
                catch
                {
                    JObject error = (JObject)respo["errors"];
                    Rest.Error er = new Rest.Error();
                    er._object = error.Value<string>("object");
                    er._key = error.Value<string>("key");
                    er._value = error.Value<string>("value");
                    er._isVisible = error.Value<bool>("isVisible");
                    er._errorMessage = error.Value<string>("errorMessage");

                    restResponce.errors.Add(er);
                }
            }

            return restResponce;
        }
        private Models.Rest.RestResponce MakeCall(verb _verb, string _route, Dictionary<string, string> QueryParams, object body = null)
        {
            Models.Rest.RestResponce restResponce = new Models.Rest.RestResponce();
            GetAuth();

            var client = new RestClient(Endpoint + _route);
            var request = new RestRequest();
            switch (_verb)
            {
                case verb.get:
                    request = new RestRequest(Method.GET);
                    break;
                case verb.patch:
                    request = new RestRequest(Method.PATCH);
                    break;
                case verb.post:
                    request = new RestRequest(Method.POST);
                    break;
                case verb.put:
                    request = new RestRequest(Method.PUT);
                    break;
            }
            foreach (var i in QueryParams)
            {
                request.AddQueryParameter(i.Key, i.Value);
            }
            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddHeader("Accept", "application/json");
            
            if (body != null)
            {
                request.AddParameter("undefined", body, ParameterType.RequestBody);
            }
            
            var response = client.Execute(request);
            string L = response.Content;

            JObject respo = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(L);
            
            restResponce.data = respo[dataProperty];
            restResponce.status = respo.Value<string>(statusProperty);
            restResponce.userMessage = respo.Value<string>(messageProperty);
            restResponce.responseCode = respo.Value<int>(responseCodeProperty);

            restResponce.isError = Convert.ToBoolean(respo.Value<string>("isError"));
            restResponce.developerMessage = respo["developerMessage"];
            restResponce.moreInfo = respo["moreInfo"];
            restResponce.httpStatusCode = respo.Value<int>("httpStatusCode");


            if (restResponce.isError == true)
            {
                restResponce.errors = new List<Rest.Error>();
                try
                {
                    JArray errors = (JArray)respo["errors"];
                    foreach (var i in errors)
                    {
                        Rest.Error error = new Rest.Error();
                        error._object = i.Value<string>("object");
                        error._key = i.Value<string>("key");
                        error._value = i.Value<string>("value");
                        error._isVisible = i.Value<bool>("isVisible");
                        error._errorMessage = i.Value<string>("errorMessage");

                        restResponce.errors.Add(error);
                    }
                }
                catch
                {
                    JObject error = (JObject)respo["errors"];
                    Rest.Error er = new Rest.Error();
                    er._object = error.Value<string>("object");
                    er._key = error.Value<string>("key");
                    er._value = error.Value<string>("value");
                    er._isVisible = error.Value<bool>("isVisible");
                    er._errorMessage = error.Value<string>("errorMessage");

                    restResponce.errors.Add(er);
                }
            }

            return restResponce;
        }
    }
}

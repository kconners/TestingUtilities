using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingUtilities.Models
{
    public class Rest
    {
        public class RestResponce
        {
            public bool isError { get; set; }
            public dynamic data { get; set; }
            public object developerMessage { get; set; }
            public string userMessage { get; set; }
            public string status { get; set; }
            public object moreInfo { get; set; }
            public int responseCode { get; set; }
            public int httpStatusCode { get; set; }
            public List<Error> errors { get; set; }
        }
        public class Error
        {
            public string _object { get; set; }
            public string _key { get; set; }
            public string _value { get; set; }
            public bool _isVisible { get; set; }
            public string _errorMessage { get; set; }
        }
        public enum verb
        {
            post,
            put,
            get,
            patch
        }

        public class Data
        {
            public string tokenType { get; set; }
            public string accessToken { get; set; }
            public int expiresIn { get; set; }
        }
        public class GetAuth
        {
            public ApplicationPermissions secrets { get; set; }
        }
        public class ApplicationPermissions
        {
            public string clientId { get; set; }
            public string clientSecret { get; set; }
        }
        public class Auth
        {
            public bool isError { get; set; }
            public Data data { get; set; }
            public object developerMessage { get; set; }
            public string userMessage { get; set; }
            public object moreInfo { get; set; }
            public int responseCode { get; set; }
            public int httpStatusCode { get; set; }
            public object errors { get; set; }
            public string requestId { get; set; }
        }
    }
}

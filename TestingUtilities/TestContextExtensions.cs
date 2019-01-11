using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestingUtilities
{
    public static class TestContextExtensions
    {
        private static string FilePath { get; set; }
        private static JObject R;

        private static string _scriptname;
        private static string _application;
        private static string _client;
        
        public static void Client(this TestContext context, string Client)
        {
            _client = Client;
        }
        public static void ScriptName(this TestContext context, string ScriptName)
        {
            _scriptname = ScriptName;
        }
        public static void Application(this TestContext context, string Application)
        {
            _application = Application;
        }


        public static string GetEnvironment(this TestContext context)
        {
            return getValue("env", "dev");

        }
        public static string GetEnvironmentUrl(this TestContext context)
        {
            string url = "https://www.google.com/";


            return url;
        }
        public static string GetRunMode(this TestContext context)
        {


            return getValue( "runmode", "local");
        }
        public static string GetBrowserType(this TestContext context)
        {

            return getValue( "browser", "chrome");
        }
        public static string GetBackingData(this TestContext context)
        {
            return getValue( "backingdata", "db");
        }
        public static string GetJSONLocation(this TestContext context)
        {
            return getValue( "fullfilenameforjson", "");
        }
       
        public static string GetUserName(this TestContext context)
        {
            return getValue( "UserName", "Automation");
        }
        public static string getClient(this TestContext context)
        {
            return getValue( "Client_Name", "LoyaltyWare");
        }
        public static string getApplication(this TestContext context)
        {
            return getValue( "Application", "");
        }
        
        public static string GetKillBrowser(this TestContext context)
        {
            return getValue("killbrowser", "Yes");
        }
        
        private static string getValue(string name, string Default)
        {
            var value = Default;
            if (TestContext.Parameters[name] != null)
            {
                value = TestContext.Parameters[name].ToString();
            }

            return value;
        }
    }
}

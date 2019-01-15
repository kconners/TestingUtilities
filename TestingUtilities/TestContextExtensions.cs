﻿using System;
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

        public static void ScriptName(this TestContext context, string ScriptName)
        {
            _scriptname = ScriptName;
        }
        public static string GetBrowserType(this TestContext context)
        {

            return getValue("browser", "chrome");
        }

        public static string GetHeadless(this TestContext context)
        {
            return getValue("headless", "no");
        }
        public static string GetRunMode(this TestContext context)
        {

            return getValue("runmode", "local");
        }
        public static string GetKillBrowser(this TestContext context)
        {

            return getValue("killbrowser", "Yes");
        }
        public static string GetEnvironment(this TestContext context)
        {

            return getValue("environment", "qa");
        }
        public static string GetCycleID(this TestContext context)
        {

            return getValue("runid", Convert.ToString(new Guid()));
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

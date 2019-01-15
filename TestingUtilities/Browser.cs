using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using NUnit.Framework;
using System.IO;
using Newtonsoft;


namespace TestingUtilities
{

    public class Browser
    {
        public WebElem we { get; set; }
        public IWebDriver driv { get; set; }
        public string runMode { get; set; }
        public string env { get; set; }
        public string headless { get; set; }
        public string browserType { get; set; }
        public string KillTheBrowserAtTheEnd { get; set; }
        public string PathToDrivers { get; set; }
        public Browser(TestContext context)
        {
            runMode = context.GetRunMode();
            browserType = context.GetBrowserType();
            KillTheBrowserAtTheEnd = context.GetKillBrowser();
            env = context.GetEnvironment();
            headless = context.GetHeadless();
        }
        private BrowserType GetBrowserType()
        {
            if (browserType.ToLower() == "chrome" || browserType.ToLower() == "c")
                return BrowserType.Chrome;
            else if (browserType.ToLower() == "firefox" || browserType.ToLower() == "ff")
                return BrowserType.Firefox;
            else if (browserType.ToLower() == "internetexplorer" || browserType.ToLower() == "ie")
                return BrowserType.InternetExplorer;
            else if (browserType.ToLower() == "phantomjs" || browserType.ToLower() == "pj")
                return BrowserType.PhantomJS;
            else return BrowserType.Chrome;
        }
        public IWebDriver OpenBrowser()
        {

            if (GetBrowserType() == BrowserType.Chrome)
            {
                OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();

                options.AddArgument("--start-maximized");
                options.AddArgument("--ignore-certificate-errors");
                options.AddArgument("--disable-popup-blocking");
                options.AddArgument("--safebrowsing-disable-download-protection");
                options.AddArgument("--incognito");

                if (headless.ToLower() == "yes")
                {
                    options.AddArgument("--headless");
                }
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("disable-popup-blocking", true);
                //options.AddUserProfilePreference("safebrowsing.enabled", true);
                options.AddUserProfilePreference("safebrowsing", "enabled");
                string curentDir = System.IO.Directory.GetCurrentDirectory();
                Console.WriteLine(curentDir);
                driv = new OpenQA.Selenium.Chrome.ChromeDriver(@"C:\Drivers", options);
            }

            return driv;
        }
        public IWebDriver OpenBrowser(string PathToDrivers)
        {

            if (GetBrowserType() == BrowserType.Chrome)
            {
                OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();

                options.AddArgument("--start-maximized");
                options.AddArgument("--ignore-certificate-errors");
                options.AddArgument("--disable-popup-blocking");
                options.AddArgument("--safebrowsing-disable-download-protection");
                options.AddArgument("--incognito");
                if (headless.ToLower() == "yes")
                {
                    options.AddArgument("--headless");
                }
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("disable-popup-blocking", true);
                //options.AddUserProfilePreference("safebrowsing.enabled", true);
                options.AddUserProfilePreference("safebrowsing", "enabled");

                driv = new OpenQA.Selenium.Chrome.ChromeDriver(PathToDrivers, options);
            }

            return driv;
        }
        public IWebDriver StartBrowser(string URL)
        {
            driv = OpenBrowser();
            driv.Navigate().GoToUrl(URL);
            we = new WebElem(this);
            return driv;
        }
        public IWebDriver StartBrowser(int Zoom = 100)
        {
            string URL = string.Empty;



            if (env.ToLower() == "qa")
            { URL = "https://scqa01.greyhound.com"; }
            else if (env.ToLower() == "uat" || env.ToLower() == "stage" || env.ToLower() == "stg")
            { URL = "https://scstg01.greyhound.com"; }
            driv = OpenBrowser();



            driv.Navigate().GoToUrl(URL);
            ((IJavaScriptExecutor)driv).ExecuteScript("document.body.style.zoom='" + Convert.ToString(Zoom) + "%';");
            we = new WebElem(this);
            return driv;
        }


        
        public void Finish()
        {
            if (KillTheBrowserAtTheEnd.ToLower() == "yes" && driv != null)
            {
                driv.Quit();
            }

        }
        public enum BrowserType
        {
            Firefox,
            Chrome,
            InternetExplorer,
            PhantomJS
        }

    }
}

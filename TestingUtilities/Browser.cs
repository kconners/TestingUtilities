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

        private ClickSetting _clickSetting { get; set; }
        private TypeSetting _typeSetting { get; set; }

        public string runMode { get; set; }
        public string env { get; set; }
        public string headless { get; set; }
        public string browserType { get; set; }
        public string KillTheBrowserAtTheEnd { get; set; }
        public string PathToDrivers { get; set; }
        public string application { get; set; }
        public string runID { get; set; }
        public string logLevel { get; set; }

        private int _PageTimeout { get; set; }
        private int _PollInterval { get; set; }
        public Browser(TestContext context)
        {
            runMode = context.GetRunMode();
            browserType = context.GetBrowserType();
            KillTheBrowserAtTheEnd = context.GetKillBrowser();
            env = context.GetEnvironment();
            headless = context.GetHeadless();
            application = context.GetApplication();
            runID = context.GetCycleID();
            logLevel = context.GetLogLevel();
            _PageTimeout = Convert.ToInt32(context.GetTimeOut());
            _PollInterval = Convert.ToInt32(context.GetPollIntervalInMilliseconds());

            _clickSetting = new ClickSetting() { PageTimeout = TimeSpan.FromSeconds(_PageTimeout), PollInterval = TimeSpan.FromMilliseconds(_PollInterval) };
            _typeSetting = new TypeSetting() { PageTimeout = TimeSpan.FromSeconds(_PageTimeout), PollInterval = TimeSpan.FromMilliseconds(_PollInterval) };

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
                driv = new OpenQA.Selenium.Chrome.ChromeDriver(options);
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
        public IWebDriver StartBrowser(string URL )
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
        public IWebDriver StartBrowser(string DirPath = "", int Zoom = 100)
        {
            string URL = string.Empty;
            List<Models.URL> uRLs = new List<Models.URL>();
            we = new WebElem(this);

            if (DirPath == "")
            {
                DirPath = Path.GetDirectoryName(typeof(Browser).Assembly.Location) + @"..\..\..\TestConfig\urls.json";
            }
            using (StreamReader r = new StreamReader(DirPath))
            {
                string json = r.ReadToEnd();
            
                uRLs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.URL>>(json);
            }

            URL = uRLs.Where(i => i.environment.ToLower() == env.ToLower() && i.application.ToLower() == application.ToLower()).FirstOrDefault().url;
            we.Log(URL);
            driv = OpenBrowser();
            driv.Navigate().GoToUrl(URL);
       //     ((IJavaScriptExecutor)driv).ExecuteScript("document.body.style.zoom='" + Convert.ToString(Zoom) + "%';");
            we = new WebElem(this);
            return driv;
        }
       
        public bool SwitchToTab(string title)
        {
            try
            {
                IList<string> tabs = new List<string>(driv.WindowHandles);
                int countr = 0;
                while (countr < 5)
                {
                    driv.SwitchTo().Window(tabs[countr]);
                    Console.WriteLine(string.Format("Logging message: '{0}'", driv.Title.Trim().ToLower()));
                    if (driv.Title.Trim().ToLower() == title.Trim().ToLower())
                    {
                        break;
                    }
                    countr = countr + 1;
                }
                return true;
            }
            catch
            {
                Assert.Fail("This was broken");
                return false;
            }
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

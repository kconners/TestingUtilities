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
        public IWebDriver driv { get; set; }
        public string runMode { get; set; }
        public string browserType { get; set; }
        public string KillTheBrowserAtTheEnd { get; set; }
        public Browser(TestContext context)
        {
            runMode = context.GetRunMode();
            browserType = context.GetBrowserType();
            KillTheBrowserAtTheEnd = context.GetKillBrowser();
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
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("disable-popup-blocking", true);
                //options.AddUserProfilePreference("safebrowsing.enabled", true);
                options.AddUserProfilePreference("safebrowsing", "enabled");

                driv = new OpenQA.Selenium.Chrome.ChromeDriver(@"\\tyFPS01\DA-Common\QA\Tools\LocallyGrownApps\Automation\Webdrivers", options);
            }
            if (GetBrowserType() == BrowserType.Firefox)
            {

                OpenQA.Selenium.Firefox.FirefoxOptions options = new OpenQA.Selenium.Firefox.FirefoxOptions();

                //options.AddArgument("--start-maximized");
                //options.AddArgument("--ignore-certificate-errors");
                //options.AddArgument("--disable-popup-blocking");

                if (File.Exists("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe"))
                {
                    options.BrowserExecutableLocation = "C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe";
                }
                else if (File.Exists("C:\\Program Files\\Mozilla Firefox\\firefox.exe"))
                {
                    options.BrowserExecutableLocation = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                }

                driv = new OpenQA.Selenium.Firefox.FirefoxDriver(@"\\tyFPS01\DA-Common\QA\Tools\LocallyGrownApps\Automation\Webdrivers", options);

            }
            else if (GetBrowserType() == BrowserType.PhantomJS)
            {
            //    driv = new OpenQA.Selenium.PhantomJS.PhantomJSDriver(@"\\tyFPS01\DA-Common\QA\Tools\LocallyGrownApps\Automation\Webdrivers");
            }

            return driv;
        }
        public IWebDriver OpenBrowser(string DownLoadPath)
        {

            if (GetBrowserType() == BrowserType.Chrome)
            {
                OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();

                options.AddArgument("--start-maximized");
                options.AddArgument("--ignore-certificate-errors");
                options.AddArgument("--disable-popup-blocking");
                options.AddArgument("--incognito");
                options.AddArgument("--safebrowsing-disable-download-protection");

                options.AddUserProfilePreference("download.default_directory", DownLoadPath);
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("disable-popup-blocking", true);
                options.AddUserProfilePreference("safebrowsing", "enabled");
                //options.AddUserProfilePreference("safebrowsing.enabled", true);

                driv = new OpenQA.Selenium.Chrome.ChromeDriver(@"\\tyFPS01\DA-Common\QA\Tools\LocallyGrownApps\Automation\Webdrivers", options);
            }
            if (GetBrowserType() == BrowserType.Firefox)
            {

                OpenQA.Selenium.Firefox.FirefoxOptions options = new OpenQA.Selenium.Firefox.FirefoxOptions();

                options.SetPreference("browser.download.folderList", 2);
                options.SetPreference("browser.download.dir", DownLoadPath);
                options.SetPreference("browser.download.useDownloadDir", true);
                //options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
                options.SetPreference("pdfjs.disabled", true);

                //options.AddArgument("--start-maximized");
                //options.AddArgument("--ignore-certificate-errors");
                //options.AddArgument("--disable-popup-blocking");

                if (File.Exists("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe"))
                {
                    options.BrowserExecutableLocation = "C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe";
                }
                else if (File.Exists("C:\\Program Files\\Mozilla Firefox\\firefox.exe"))
                {
                    options.BrowserExecutableLocation = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                }

                driv = new OpenQA.Selenium.Firefox.FirefoxDriver(@"\\tyFPS01\DA-Common\QA\Tools\LocallyGrownApps\Automation\Webdrivers", options);

            }
           // else if (GetBrowserType() == BrowserType.PhantomJS)
           // {
           //     driv = new OpenQA.Selenium.PhantomJS.PhantomJSDriver(@"\\tyFPS01\DA-Common\QA\Tools\LocallyGrownApps\Automation\Webdrivers");
           // }

            return driv;
        }
        public IWebDriver StartBrowser(string URL)
        {
            driv = OpenBrowser();
            driv.Navigate().GoToUrl(URL);

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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using NUnit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FluentAssertions;
using System.Threading;
using SeleniumExtras.WaitHelpers;
using System.IO;
using NUnit.Framework;


namespace TestingUtilities
{
   

    public class WebElem
    {
        private ClickSetting clickSetting { get; set; }
        private TypeSetting typeSetting { get; set; }

        private string RunID = string.Empty;
        private List<string> LogLevels = new List<string>();

        public WebElem(Browser b)
        {
            
            clickSetting = new ClickSetting() { PageTimeout = TimeSpan.FromSeconds(10), PollInterval = TimeSpan.FromMilliseconds(500) };
            typeSetting = new TypeSetting() { PageTimeout = TimeSpan.FromSeconds(10), PollInterval = TimeSpan.FromMilliseconds(500) };

            this.driv = b.driv;
            lines = new List<LogItem>();
            lines.Add(new LogItem { LogLevel = loglevel.always, LogMessage = string.Format("Start Logging for test {0}", NUnit.Framework.TestContext.CurrentContext.Test.Name) });
            RunID = b.runID;

            foreach (var ll in b.logLevel.Split(','))
                {
                LogLevels.Add(ll.Trim().ToLower());
            }
        }
        public WebElem(IWebDriver Driv, ClickSetting defaultClickSettings, TypeSetting defaultTypeSetting)
        {
            
            clickSetting = defaultClickSettings;
            typeSetting = defaultTypeSetting;
            driv = Driv;
        }
        public WebElem(IWebDriver Driv, ClickSetting defaultClickSettings)
        {

            clickSetting = defaultClickSettings;
            driv = Driv;
        }
        public WebElem()
        {
        }
        public WebElem(TestContext context)
        {
         

            int _PageTimeout;
            int _PollInterval;

            _PageTimeout = Convert.ToInt32(context.GetTimeOut());
            _PollInterval = Convert.ToInt32(context.GetPollIntervalInMilliseconds());

            clickSetting = new ClickSetting() { PageTimeout = TimeSpan.FromSeconds(_PageTimeout), PollInterval = TimeSpan.FromMilliseconds(_PollInterval) };
            typeSetting = new TypeSetting() { PageTimeout = TimeSpan.FromSeconds(_PageTimeout), PollInterval = TimeSpan.FromMilliseconds(_PollInterval) };

            lines = new List<LogItem>();
            lines.Add(new LogItem { LogLevel = loglevel.always, LogMessage = string.Format("Start Logging for test {0}", NUnit.Framework.TestContext.CurrentContext.Test.Name) });
            RunID = Convert.ToString(Guid.NewGuid());

            foreach (var ll in context.GetLogLevel().Split(','))
            {
                LogLevels.Add(ll.Trim().ToLower());
            }
        }

        public IWebDriver driv;
        private List<LogItem> lines;
        // WriteAllLines creates a file, writes a collection of strings to the file,
        // and then closes the file.  You do NOT need to call Flush() or Close().
        bool WasSuccessful;
        private IWebElement HH;
        private IWebElement ClickOn, UntilThisIsClickable;
        //private int countr = 0;
        //private static int attempt = 0;
        //private int TimeOutSeconds = 0;
        //private int TimeOutSecondsForClickUntilClickable = 0;
        //private static System.Timers.Timer aTimer, bTimer;
        string OptionToClick { get; set; }

        

        public void AcceptAlert()
        {
            Wait(1.5);
            IAlert alert = driv.SwitchTo().Alert();
            alert.Accept();
        }
        public void Clear(by ByString)
        {
            var wait = new WebDriverWait(driv, typeSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(ByString));
            wait.Until(driver => driv.FindElement(ByString).Enabled == true);   
            driv.FindElement(ByString).Clear(); 
        }
        
        public void Click(by ByString)
        {
            var wait = new WebDriverWait(driv, clickSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(ByString));
            scrollintoView(driv.FindElement(ByString));
            IWebElement item = driv.FindElement(ByString);
            Click(item, ByString.Description);
        }
        public void Click(by ByString, Direction direction)
        {
            var wait = new WebDriverWait(driv, clickSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(ByString));
            scrollintoView(driv.FindElement(ByString));
            IWebElement item = driv.FindElement(ByString);
            Click(item, ByString.Description, direction);
        }
        public void Click(IWebElement item,string elementDescription, Direction scrollFirst = Direction.Up)
        {
            int countr = 0;
            Clicker clicker = new Clicker(driv,clickSetting);
            clicker.Click(item, elementDescription, scrollFirst);
        }
        public void Click(IWebElement item, string elementDescription, ClickSetting _clickSetting,Direction scrollFirst = Direction.Up)
        {
            int countr = 0;
            Clicker clicker = new Clicker(driv, _clickSetting);
            clicker.Click(item, elementDescription, scrollFirst);
        }

        public void Click(by ByString, ClickSetting click)
        {
            var wait = new WebDriverWait(driv, click.PageTimeout);
            wait.Until(driver => driv.FindElement(ByString));
            scrollintoView(driv.FindElement(ByString));
            IWebElement item = driv.FindElement(ByString);
            Click(item, ByString.Description);
        }
        

        public void Click(by Parent, string XpathToClick)
        {
            var wait = new WebDriverWait(driv, clickSetting.PageTimeout);
            var wait2 = new WebDriverWait(driv, clickSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(Parent));
            IWebElement Par = driv.FindElement(Parent);

            wait2.Until(driver => Par.FindElement(By.XPath(XpathToClick)));
            scrollintoView(Par.FindElement(By.XPath(XpathToClick)));
            IWebElement item = Par.FindElement(By.XPath(XpathToClick));
            Click(item, string.Format("The child at xpath='{0}' of Parent = '{1}'", XpathToClick, Parent.Description));
        }
        public void Click(by Parent, string XpathToClick, ClickSetting click)
        {
            var wait = new WebDriverWait(driv, click.PageTimeout);
            var wait2 = new WebDriverWait(driv, click.PageTimeout);
            wait.Until(driver => driv.FindElement(Parent));
            IWebElement Par = driv.FindElement(Parent);

            wait2.Until(driver => Par.FindElement(By.XPath(XpathToClick)));
            scrollintoView(Par.FindElement(By.XPath(XpathToClick)));
            IWebElement item = Par.FindElement(By.XPath(XpathToClick));
            Click(item, string.Format("The child at xpath='{0}' of Parent = '{1}'", XpathToClick, Parent.Description));
        }
        
        public bool DidPageLoad(by THeElement, bool TrueToHighLight = false, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(timeout));
                wait.Until(driver => driv.FindElement(THeElement));
                if (TrueToHighLight == true)
                { HighLight(THeElement, 5); }
                return true;
            }
            catch
            {
                Log(string.Format("There was an issue finding {0}", THeElement.Description));
                return false;
            }
        }
        public string GetAttribute(by ElementIDentity, string AttributeName)
        {
            return GetAttribute(driv.FindElement(ElementIDentity), AttributeName);
        }
        public string GetAttribute(IWebElement ElementIDentity, string AttributeName)
        {
            string Value = string.Empty;

            IJavaScriptExecutor executor = (IJavaScriptExecutor)driv;
            Object aa = executor.ExecuteScript("var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;", ElementIDentity);
            try
            {
                Dictionary<string, object> LL = (Dictionary<string, object>)aa;
                Value = LL[AttributeName].ToString();
            }
            catch
            {
                Value = string.Format("Could not find the desired key, this is what was there: '{0}'", Newtonsoft.Json.JsonConvert.SerializeObject(aa));
            }

            return Value;
        }
        public string GetValue(by ElementIDentity, int TimeoutSeconds = 10)
        {
            string Value = string.Empty;


            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(ElementIDentity));

            IWebElement webElement = driv.FindElement(ElementIDentity);

            return GetValueOfElement(webElement);
        }
        public string GetValue(IWebElement ElementIDentity, int TimeoutSeconds = 10)
        {
            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(ElementIDentity));
            return GetValueOfElement(ElementIDentity);
        }
        public void Log(string Message)
        {
            Console.WriteLine(string.Format("Logging message: '{0}'", Message));
            lines.Add(new LogItem { LogLevel = loglevel.always, LogMessage = string.Format("Logging message: '{0}'", Message) });
        }
        public void Log(loglevel logLevel,string Message)
        {
            Console.WriteLine(string.Format("Logging message: '{0}'", Message));
            lines.Add(new LogItem { LogLevel = logLevel, LogMessage = string.Format("Logging message: '{0}'", Message) });
        }
        public void SelectFromDropDown(by ElementIDentity, string ValueToSelect, double TimeoutSeconds = 10)
        {
            Int32 TimeOUT = Convert.ToInt32(TimeoutSeconds * 1000);

            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(ElementIDentity));

            var picker = driv.FindElement(ElementIDentity);

            var selectElement = new SelectElement(picker);

            selectElement.SelectByText(ValueToSelect);

        }
        public void SetCheckBox(by CheckBox, bool ShouldBeChecked)
        {

            var wait = new WebDriverWait(driv, clickSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(CheckBox));

            bool IsCurrentlyChecked = Convert.ToBoolean(GetValue(CheckBox));

            if (IsCurrentlyChecked != ShouldBeChecked)
            {
                Click(CheckBox);
            }

        }
        public void SetCheckBox(by CheckBox, bool ShouldBeChecked, ClickSetting clickSetting)
        {

            var wait = new WebDriverWait(driv, clickSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(CheckBox));

            bool IsCurrentlyChecked = Convert.ToBoolean(GetValue(CheckBox));

            if (IsCurrentlyChecked != ShouldBeChecked)
            {
                Click(CheckBox, clickSetting);
            }

        }
        public void SetCheckBox(by CheckBox, by ClickForCheck, bool ShouldBeChecked, ClickSetting _clickSetting)
        {

            var wait = new WebDriverWait(driv, _clickSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(CheckBox));
            scrollintoView(driv.FindElement(CheckBox));


            bool IsCurrentlyChecked = Convert.ToBoolean(GetValue(CheckBox));

            if (IsCurrentlyChecked != ShouldBeChecked)
            {
                Click(ClickForCheck, _clickSetting);
            }

        }
        public void SetCheckBox(by CheckBox, by ClickForCheck, bool ShouldBeChecked)
        {

            var wait = new WebDriverWait(driv, clickSetting.PageTimeout);
            wait.Until(driver => driv.FindElement(CheckBox));
            scrollintoView(driv.FindElement(CheckBox));


            bool IsCurrentlyChecked = Convert.ToBoolean(GetValue(CheckBox));

            if (IsCurrentlyChecked != ShouldBeChecked)
            {
                Click(ClickForCheck, clickSetting);
            }

        }
        public void scrollintoView(IWebElement test)
        {
            ((IJavaScriptExecutor)driv).ExecuteScript("arguments[0].scrollIntoView(true);", test);
        }
        public void WriteLog()
        {
            string Year, Month, Day, Hour, Min, Second;
            DateTime dateTime = DateTime.Now;
            Year = Convert.ToString(dateTime.Year);
            Month = Convert.ToString(dateTime.Month);
            Day = Convert.ToString(dateTime.Day);
            Hour = Convert.ToString(dateTime.Hour);
            Min = Convert.ToString(dateTime.Minute);
            Second = Convert.ToString(dateTime.Second);

            string DirPath = Path.GetDirectoryName(typeof(Browser).Assembly.Location) + @"..\..\..\..\Logs\"+ NUnit.Framework.TestContext.CurrentContext.Test.ClassName + Year+"_"+ Month + "_" + Day + "_" + Hour + "_" + Min + "_" + Second + ".txt";
            Log(DirPath);
            if (!LogLevels.Contains(loglevel.all.ToString()))
            {
                lines = lines.Where(i => LogLevels.Contains(i.LogLevel.ToString()) || i.LogLevel == loglevel.always).ToList();
            }
            
            List<string> mssage = new List<string>();


            foreach (var LogMessage in lines.OrderBy(i => i.TestCase).ThenBy(c => c.now))
            {
                mssage.Add(LogMessage.TestCase.ToString() + " " + LogMessage.now.ToString() + " " + LogMessage.LogLevel.ToString() + " " + LogMessage.LogMessage);
            }

            System.IO.File.WriteAllLines(DirPath, mssage.ToArray());
        }
        public void scroll(IWebElement test, Direction direction)
        {
            int height = test.Size.Height;
            int width = test.Size.Width;
            string script = "";
            if (direction == Direction.Down)
            { script = "window.scrollBy(0," + Convert.ToString(height * 2) + ");"; }
            else if (direction == Direction.Up)
            { script = "window.scrollBy(0,-" + Convert.ToString(height * 2) + ");"; }

            ((IJavaScriptExecutor)driv).ExecuteScript(script);
        }
        public void Type(by txtBox, string whattotype, int OneToClearTheOriginalValue)
        {
            int TimeoutSeconds;
            TimeoutSeconds = 10;
            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(txtBox));
            wait.Until(driver => driv.FindElement(txtBox).Enabled == true);
            if (OneToClearTheOriginalValue == 1)
            { driv.FindElement(txtBox).Clear(); }
            if (whattotype.Length >= 1)
            {
                driv.FindElement(txtBox).SendKeys(whattotype);
            }
            Wait(.25);
        }
        public void Wait(double NUmberOfSeconds)
        {
            Int32 t = Convert.ToInt32(NUmberOfSeconds * 1000);
            Thread.Sleep(t);
        }
        public void HighLight(IWebElement Element, double TimeoutSeconds = 10)
        {
            Int32 TimeOUT = Convert.ToInt32(TimeoutSeconds * 1000);
            scrollintoView(Element);
            string OColor = Element.GetCssValue("border-color");
            var jsDriver = (IJavaScriptExecutor)driv;
            var element = Element;
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
            Thread.Sleep(TimeOUT);
            string tEst = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: " + OColor + @"""";
            highlightJavascript = tEst + ";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
        }
        public void HighLight(by Locator, double TimeoutSeconds = 10)
        {
            IWebElement Element = driv.FindElement(Locator);

            Int32 TimeOUT = Convert.ToInt32(TimeoutSeconds * 1000);
            scrollintoView(Element);
            string OColor = Element.GetCssValue("border-color");
            var jsDriver = (IJavaScriptExecutor)driv;
            var element = Element;
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
            Thread.Sleep(TimeOUT);
            string tEst = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: " + OColor + @"""";
            highlightJavascript = tEst + ";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
        }
        public void ClickAnchorInTable(by Table, string Find, int ColumNumber, string ButtonToClick, int NumberOfPagesToCheck = 10)
        {

            int startr = 2;
            try
            {
                Click(Table, ".//td[contains(.,'" + Find + "') and  position() = " + ColumNumber + "]/..//a[contains(.,'" + ButtonToClick + "')]");
            }
            catch
            {
                while (startr <= NumberOfPagesToCheck)
                {
                    try
                    {
                        Click(new by(By.XPath(".//a[contains(.,'" + Convert.ToString(startr) + "')]"), string.Format("The anchor representing the {0} page of the table {1}", Convert.ToString(startr), Table.Description)));

                        Click(Table, ".//td[contains(.,'" + Find + "') and  position() = " + ColumNumber + "]/..//a[contains(.,'" + ButtonToClick + "')]");
                        break;
                    }
                    catch { }
                    startr++;
                }
            }

        }
        public void ClickAnchorInTable(by Table, string Find, int ColumNumber, string ButtonToClick, ClickSetting clickSetting,int NumberOfPagesToCheck = 10)
        {

            int startr = 2;
            try
            {
                Click(Table, ".//td[contains(.,'" + Find + "') and  position() = " + ColumNumber + "]/..//a[contains(.,'" + ButtonToClick + "')]", clickSetting);
            }
            catch
            {
                while (startr <= NumberOfPagesToCheck)
                {
                    try
                    {
                        Click(new by(By.XPath(".//a[contains(.,'" + Convert.ToString(startr) + "')]"), string.Format("The anchor representing the {0} page of the table {1}", Convert.ToString(startr), Table.Description)), clickSetting);

                        Click(Table, ".//td[contains(.,'" + Find + "') and  position() = " + ColumNumber + "]/..//a[contains(.,'" + ButtonToClick + "')]", clickSetting);
                        break;
                    }
                    catch { }
                    startr++;
                }
            }

        }
     
        public string GetStringFromATable(By Table, string MatchOnValue, int FindInColumn, int GetValueFrom, int NumberOfPagesToCheck = 10, int TimeToWaitPerPage = 10)
        {
            string RetrivedValue = "";

            try
            {
                IWebElement parent = driv.FindElement(Table);
                IWebElement row = parent.FindElement(By.XPath(".//td[contains(.,'" + MatchOnValue + "') and  position() = " + FindInColumn + "]/.."));
                RetrivedValue = GetValue(row.FindElement(By.XPath(".//td[position() = " + GetValueFrom + "]")));
            }
            catch
            {
                Log(string.Format("There was an issue finding the value '{0}' in the {1} column", MatchOnValue, FindInColumn));
            }


            return RetrivedValue;
        }
        public bool IsStringInATable(by Table, string LookingFor, int LookInColuumn, int numberOfPagesToTry = 10)
        {
            bool foundIt;
            foundIt = false;
            IWebElement Element = driv.FindElement(Table);

            int startr = 2;
            try
            {
                IWebElement Cell = Element.FindElement(By.XPath("//tr[contains(td[" + LookInColuumn + "], '" + LookingFor + "')]"));
                HighLight(Cell);
                foundIt = true;
            }
            catch
            {

                while (startr <= numberOfPagesToTry)
                {
                    try
                    {
                        try
                        {
                            Click(new by(By.XPath(".//a[contains(.,'" + Convert.ToString(startr) + "')]"),string.Format("The anchor representing the {0} page of the table {1}", Convert.ToString(startr), Table.Description)));
                        }
                        catch { break; }
                        IWebElement Cell = Element.FindElement(By.XPath("//tr[contains(td[" + LookInColuumn + "], '" + LookingFor + "')]"));
                        foundIt = true;
                        break;
                    }
                    catch
                    {
                        Log(string.Format("Did not find '{0}' on page {1}", LookingFor, Convert.ToString(startr)));
                    }
                    startr++;
                }

            }

            return foundIt;

        }
     
        public void RightClickAndSelect(By ByString, string SelectOption, int TimeoutSeconds = 10)
        {
            OptionToClick = SelectOption;
            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(ByString));
            scrollintoView(driv.FindElement(ByString));
            IWebElement item = driv.FindElement(ByString);
            RightClick(item, TimeoutSeconds);
        }
        public void RightClick(IWebElement item, int TimeoutSeconds = 10)
        {
            //int countr = 0;
            //bool didItWork = false;
            //HH = item;
            //TimeOutSeconds = TimeoutSeconds * 2;
            //System.Timers.Timer aTimer = new System.Timers.Timer();
            //aTimer.Interval = 500;
            //aTimer.Elapsed += TimedRightClickOnWebElement;
            //aTimer.Enabled = true;
            //while (aTimer.Enabled == true)
            //{ }
            //if (countr > TimeoutSeconds && WasSuccessful == false) { didItWork = false; }
            //else if (countr <= TimeoutSeconds || WasSuccessful == true) { didItWork = true; }
            //aTimer.Dispose();
            //didItWork.Should().BeTrue();
        }
        public void SelectFromDropDown(By ElementIDentity, string ValueToSelect, double TimeoutSeconds = 10)
        {
            Int32 TimeOUT = Convert.ToInt32(TimeoutSeconds * 1000);

            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(ElementIDentity));

            var picker = driv.FindElement(ElementIDentity);

            var selectElement = new SelectElement(picker);

            selectElement.SelectByText(ValueToSelect);

        }
        public void Type(List<By> txtBox, string whattotype, int OneToClearTheOriginalValue)
        {
            By textBox;
            foreach (var I in txtBox)
            {
                try
                {
                    int TimeoutSeconds;
                    TimeoutSeconds = 2;
                    var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
                    wait.Until(driver => driv.FindElement(I));
                    wait.Until(driver => driv.FindElement(I).Enabled == true);
                    textBox = I;

                    if (OneToClearTheOriginalValue == 1)
                    { driv.FindElement(textBox).Clear(); }
                    driv.FindElement(textBox).SendKeys(whattotype);
                    Wait(.25);

                }
                catch
                {
                    Log("Unable to find a textbox");
                }
            }

        }

        private string GetValueOfElement(IWebElement web)
        {

            string Value = string.Empty;
            IWebElement webElement = web;

            //  Log(string.Format("{0}", GetAttribute(webElement, "type")));
            //   Log(string.Format("This? {0}", webElement.TagName));
            if (GetAttribute(webElement, "type") == "text")
            {
                if (webElement.Text.Length >= 1) Value = webElement.Text;
                else if (webElement.GetAttribute("value").Length >= 1) Value = webElement.GetAttribute("value");
            }
            else if (GetAttribute(webElement, "class") == "value")
            {
                if (webElement.Text.Length >= 1) Value = webElement.Text;
                else if (webElement.GetAttribute("value").Length >= 1) Value = webElement.GetAttribute("value");
            }
            else if (GetAttribute(webElement, "type") == "checkbox")
            {
                Value = "false";

                if (webElement.GetAttribute("checked") == "checked" || webElement.GetAttribute("checked") == "true")
                { Value = "true"; }
            }
            else if (GetAttribute(webElement, "type") == "radio")
            {
                Value = "false";
                if (webElement.GetAttribute("checked") == "checked")
                { Value = "true"; }
            }
            else if (GetAttribute(webElement, "class") == "custom checkbox checked")
            {
                Value = "true";
            }
            else if (GetAttribute(webElement, "class") == "custom checkbox")
            {
                Value = "false";
            }
            else if (GetAttribute(webElement, "class") == "current")
            {
                if (webElement.Text.Length >= 1) Value = webElement.Text;
                else if (webElement.GetAttribute("value").Length >= 1) Value = webElement.GetAttribute("value");
            }
            else if (GetAttribute(webElement, "tagname").ToLower() == "li" || GetAttribute(webElement, "tagname").ToLower().Contains("li") || webElement.TagName.ToLower().Contains("li"))
            {

                if (webElement.Text.Length >= 1)
                {
                    Value = webElement.Text;
                }
                else if (webElement.GetAttribute("textContent").Length >= 1)
                {
                    Value = webElement.GetAttribute("textContent");
                }
            }
            else
            {
                try
                {
                    if (webElement.Text.Length >= 1) Value = webElement.Text;
                    else if (webElement.GetAttribute("value").Length >= 1) Value = webElement.GetAttribute("value");
                }
                catch
                {
                    Value = "";
                }
            }
            return Value;
        }
      
        

        private class LogItem
        {
            public loglevel LogLevel { get; set; }
            public Guid TestRunID { get; set; }
            public string LogMessage { get; set; }
            public string TestCase = NUnit.Framework.TestContext.CurrentContext.Test.FullName;
            public DateTime now = DateTime.Now;
        }
        public enum Direction
        {
            Down,
            Up
        }
        public enum loglevel
        {
            all,
            always,
            debug,
            info,
            off
        }
        private class URLRoot
        {
            public string Items { get; set; }
        }
    }
}

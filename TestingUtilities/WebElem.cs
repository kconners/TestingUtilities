using System;
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


namespace TestingUtilities
{
    public class WebElem
    {
        public WebElem(Browser b)
        {
            this.driv = b.driv;
        }
        public IWebDriver driv;
        public int GlobalTimeOUT = 10;


        bool WasSuccessful;
        private IWebElement HH;
        private IWebElement ClickOn, UntilThisIsClickable;
        private int countr = 0;
        private static int attempt = 0;
        private int TimeOutSeconds = 0;
        private int TimeOutSecondsForClickUntilClickable = 0;
        private static System.Timers.Timer aTimer, bTimer;
        string OptionToClick { get; set; }

        public void AcceptAlert()
        {
            Wait(1.5);
            IAlert alert = driv.SwitchTo().Alert();
            alert.Accept();
        }

        public void Click(by ByString, int TimeoutSeconds = 10)
        {
            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(ByString));
            scrollintoView(driv.FindElement(ByString));
            IWebElement item = driv.FindElement(ByString);
            Click(item, TimeoutSeconds);
        }
        public void Click(IWebElement item, int TimeoutSeconds = 10)
        {
            countr = 0;
            bool didItWork = false;
            HH = item;
            TimeOutSeconds = TimeoutSeconds * 2;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            aTimer.Elapsed += TimedClickOnWebElement;
            aTimer.Enabled = true;
            while (aTimer.Enabled == true)
            { }
            if (countr > TimeoutSeconds && WasSuccessful == false) { didItWork = false; }
            else if (countr <= TimeoutSeconds || WasSuccessful == true) { didItWork = true; }
            aTimer.Dispose();
            didItWork.Should().BeTrue();
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


        private string GetValueOfElement(IWebElement web)
        {

            string Value = string.Empty;
            IWebElement webElement = web;

            Log(string.Format("{0}", GetAttribute(webElement, "type")));
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
            else if (GetAttribute(webElement, "tagname").ToLower() == "li")
            {
                if (webElement.Text.Length >= 1) Value = webElement.Text;
                else if (webElement.GetAttribute("textContent").Length >= 1) Value = webElement.GetAttribute("textContent");
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
        public string GetValue(by ElementIDentity)
        {
            string Value = string.Empty;

            IWebElement webElement = driv.FindElement(ElementIDentity);

            return GetValueOfElement(webElement);
        }
        public string GetValue(IWebElement ElementIDentity)
        {
            return GetValueOfElement(ElementIDentity);
        }
        public void Log(string Message)
        {

            Console.WriteLine(string.Format("Logging message: '{0}'", Message));
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
        public void SetCheckBox(by CheckBox, bool ShouldBeChecked, int TimeoutSeconds = 10)
        {

            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(CheckBox));

            bool IsCurrentlyChecked = Convert.ToBoolean(GetValue(CheckBox));

            if (IsCurrentlyChecked != ShouldBeChecked)
            {
                Click(CheckBox);
            }

        }
        public void SetCheckBox(by CheckBox, by ClickForCheck, bool ShouldBeChecked, int TimeoutSeconds = 10)
        {

            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(CheckBox));
            scrollintoView(driv.FindElement(CheckBox));


            bool IsCurrentlyChecked = Convert.ToBoolean(GetValue(CheckBox));

            if (IsCurrentlyChecked != ShouldBeChecked)
            {
                Click(ClickForCheck);
            }

        }
        public void scrollintoView(IWebElement test)
        {
            ((IJavaScriptExecutor)driv).ExecuteScript("arguments[0].scrollIntoView(true);", test);
        }

        public enum Direction
        {
            Down,
            Up
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

        private void TimedClickOnWebElement(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (countr <= TimeOutSeconds)
            {
                try
                {
                    if (countr >= 5 && countr <= 10)
                    {
                        scroll(HH, Direction.Up);
                    }
                    else if (countr >= 10)
                    {
                        scroll(HH, Direction.Down);
                    }
                    // Log("Height is "+ Convert.ToString(HH.Size.Height));
                    HH.Click();
                    WasSuccessful = true;
                    aTimer.Enabled = false;
                }
                catch
                {
                    if (countr < 5)
                    {
                        scrollintoView(HH);
                    }

                    HighLight(HH, 1);
                    countr++;
                    Log(string.Format("Can't click {0} on attempt number {1}", Convert.ToString(HH), Convert.ToString(countr)));
                }

            }
            else if (countr > TimeOutSeconds)
            {
                WasSuccessful = false;
                aTimer.Enabled = false;
            }
        }
        private void TimedRightClickOnWebElement(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (countr <= TimeOutSeconds)
            {
                try
                {
                    Actions action = new Actions(driv);
                    action.ContextClick(HH);
                    action.Perform();

                    int TimeoutSeconds = 1;

                    var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));

                    wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
                    wait.Until(driver => driv.FindElement(By.LinkText(OptionToClick)));

                    IWebElement Op = driv.FindElement(By.LinkText(OptionToClick));
                    Op.Click();


                    WasSuccessful = true;
                    aTimer.Enabled = false;
                    OptionToClick = string.Empty;
                }
                catch
                {
                    scrollintoView(HH);
                    HighLight(HH, 1);
                    countr++;
                    Log(string.Format("Can't right click {0} on attempt number {1}", Convert.ToString(HH), Convert.ToString(countr)));
                }

            }
            else if (countr > TimeOutSeconds)
            {
                WasSuccessful = false;
                aTimer.Enabled = false;
            }
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

























        public WebElem(IWebDriver Driv)
        {
            driv = Driv;
        }
       
        public WebElem()
        {
        }


       
        public void ClickAnchorInTable(by Table, string Find, int ColumNumber, string ButtonToClick, int NumberOfPagesToCheck = 10, int TimeToWaitPerPage = 10)
        {

            int startr = 2;
            try
            {
                Click(Table, ".//td[contains(.,'" + Find + "') and  position() = " + ColumNumber + "]/..//a[contains(.,'" + ButtonToClick + "')]", TimeToWaitPerPage);
            }
            catch
            {
                while (startr <= NumberOfPagesToCheck)
                {
                    try
                    {
                        Click(By.XPath(".//a[contains(.,'" + Convert.ToString(startr) + "')]"));
                        Click(Table, ".//td[contains(.,'" + Find + "') and  position() = " + ColumNumber + "]/..//a[contains(.,'" + ButtonToClick + "')]", TimeToWaitPerPage);
                        break;
                    }
                    catch { }
                    startr++;
                }
            }

        }
       
        public void Click(by Parent, string XpathToClick, int TimeoutSeconds = 10)
        {
            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            var wait2 = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(Parent));
            IWebElement Par = driv.FindElement(Parent);

            wait2.Until(driver => Par.FindElement(By.XPath(XpathToClick)));
            scrollintoView(Par.FindElement(By.XPath(XpathToClick)));
            IWebElement item = Par.FindElement(By.XPath(XpathToClick));
            Click(item, TimeoutSeconds);
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
                            Click(By.XPath(".//a[contains(.,'" + Convert.ToString(startr) + "')]"));
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

        public void Click(By ByString, int TimeoutSeconds = 10)
        {
            var wait = new WebDriverWait(driv, TimeSpan.FromSeconds(TimeoutSeconds));
            wait.Until(driver => driv.FindElement(ByString));
            scrollintoView(driv.FindElement(ByString));
            IWebElement item = driv.FindElement(ByString);
            Click(item, TimeoutSeconds);
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
            countr = 0;
            bool didItWork = false;
            HH = item;
            TimeOutSeconds = TimeoutSeconds * 2;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            aTimer.Elapsed += TimedRightClickOnWebElement;
            aTimer.Enabled = true;
            while (aTimer.Enabled == true)
            { }
            if (countr > TimeoutSeconds && WasSuccessful == false) { didItWork = false; }
            else if (countr <= TimeoutSeconds || WasSuccessful == true) { didItWork = true; }
            aTimer.Dispose();
            didItWork.Should().BeTrue();
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
      
        private class URLRoot
        {
            public string Items { get; set; }
        }


    }
}

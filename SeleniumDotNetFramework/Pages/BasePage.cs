using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumDotNetFramework.Tests;
using System;
using System.IO;
using System.Linq;

namespace SeleniumDotNetFramework.Pages
{
    public class BasePage : BaseTest
    {
        protected new IWebDriver _driver;
        protected new ExtentTest _extentTest;
        protected WebDriverWait _webDriverWait;
        protected bool capturePassScreenshot = Convert.ToBoolean(_configProperties["captureScreenShot"]);
        
        public BasePage(IWebDriver driver,ExtentTest test)
        {
            _driver = driver;
            _extentTest = test;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.Parse(_configProperties["webdriverwait"]));
        }
                   
        public void Click(IWebElement _webElement, string elementName)
        {
            try
            {
                _webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(_webElement));

                _webElement.Click();
                
                _extentTest.Log(Status.Pass, "Sucessfully Clicked on the element : " + elementName);

                if(capturePassScreenshot)
                _extentTest.Log(Status.Pass, "" +_extentTest.AddScreenCaptureFromPath(screenshotFolder()));
            }
            catch (NoSuchElementException ex)
            {
                _extentTest.Log(Status.Fail, "Unable to click on the element : " + elementName + _extentTest.AddScreenCaptureFromPath(screenshotFolder()));

                throw new NoSuchElementException("Unable to click on the element " + elementName + "throwing the exception : " + ex.Message);
            }
        }

        public void ClickByJavascript(IWebElement _webElement, string elementName)
        {
            try
            {
                ExecuteScript("arguments[0].click();", _webElement);

                _extentTest.Log(Status.Pass, "Sucessfully Clicked on the element : " + elementName);

                if (capturePassScreenshot)
                    _extentTest.Log(Status.Pass, "" + _extentTest.AddScreenCaptureFromPath(screenshotFolder()));
            }
            catch(Exception ex)
            {
                _extentTest.Log(Status.Pass, "Sucessfully Clicked on the element : " + elementName + _extentTest.AddScreenCaptureFromPath(screenshotFolder()));

                throw new NoSuchElementException("Unable to click on the element " + elementName + "throwing the exception : " + ex.Message);
            }
        }
               
        private string screenshotFolder()
        {
            ITakesScreenshot _takesScreenshot = (ITakesScreenshot)_driver;

            Screenshot _screenshot = _takesScreenshot.GetScreenshot();
            
            string _projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

            _projectPath = _projectPath + "/ExtentReport/Reports/"+ hmtlreport + "/screenshots";

            if(!Directory.Exists(_projectPath))
            {
                Directory.CreateDirectory(_projectPath);
            }

            string[] _dateTime = DateTime.Now.ToString().Split(' ');

            string _screenShotName = _dateTime[0].Replace("/", "-").Replace("/", "-").Replace("/", "-") + "-" + _dateTime[1].Replace(":", "-").Replace(":", "-").Replace(":", "-");

            string _finalpath = _projectPath + "/"+ _screenShotName + ".png";

            string _localPath = new Uri(_finalpath).LocalPath;

            _screenshot.SaveAsFile(_localPath);

            return _localPath;
        }

        public void EnterText(IWebElement _webElement, String text, string elementName)
        {
            try
            {
                _webElement.SendKeys(text);

                _extentTest.Log(Status.Pass, "Entering the text in the element : " + elementName);

                if (capturePassScreenshot)
                    _extentTest.Log(Status.Pass, "" + _extentTest.AddScreenCaptureFromPath(screenshotFolder()));
            }
            catch (Exception ex)
            {
                _extentTest.Log(Status.Fail, "Unable to enter the text in the element : " + elementName + _extentTest.AddScreenCaptureFromPath(screenshotFolder()));

                throw new Exception("Tried to enter the text '" + text + "' into an element but failed", ex);
            }
        }

       
        public string ExecuteScript(String script, params object[] args)
        {
            return (String)((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
        }

                
        public int GetCount(By elementSelector)
        {
            var links = _driver.FindElements(elementSelector);

            return links.Count();

        }
              

        public bool isElementDisplayed(IWebElement _webElement)
        {
            try
            {
                return _webElement.Displayed;
            }
            catch (Exception)
            {
                return _webElement.Displayed;
            }
        }

        public bool isElementDisplayed(IWebElement _webElement, string elementName)
        {
            try
            {
                _extentTest.Log(Status.Pass, "Finding the element in the app : " + elementName);

                if (capturePassScreenshot)
                    _extentTest.Log(Status.Pass, "" + _extentTest.AddScreenCaptureFromPath(screenshotFolder()));

                return _webElement.Displayed;
            }
            catch (Exception)
            {
                _extentTest.Log(Status.Fail, "Required element is not visible : " + elementName + _extentTest.AddScreenCaptureFromPath(screenshotFolder()));

                return _webElement.Displayed;
            }
        }

       

        public string GetUrl()
        {
            return _driver.Url;
        }
               

      
        public bool IsElementVisible(IWebElement _webElement)
        {
            return _webElement.Displayed;
        }

        public void Log(string message)
        {
            var timeStamp = DateTime.Now.ToString("hh:mm:s");
            Console.WriteLine(timeStamp + ": " + message);
        }

        public void NavigateTo(String url)
        {
            try
            {
                _driver.Navigate().GoToUrl(url);
            }
            catch (WebDriverTimeoutException)
            {
                if (!GetUrl().Contains(url))
                {
                    throw;
                }
            }
            catch (WebDriverException wde)
            {
                throw new Exception($"Tried to navigate to '{url}' but got an exception", wde);
            }
        }

               
        public void WaitForUrl(String url, TimeSpan timeout)
        {
            var wait = new WebDriverWait(_driver, timeout);
            try
            {
                wait.Until(d => d.Url.Contains(url));
            }
            catch (Exception e)
            {
                throw new Exception("Tried to wait for url '" + url + "', but failed. Current url: " + _driver.Url, e);
            }
        }
    }
}

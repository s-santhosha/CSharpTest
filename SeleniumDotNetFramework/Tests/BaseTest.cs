using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using SeleniumDotNetFramework.Extensions;
using SeleniumDotNetFramework.Reporting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SeleniumDotNetFramework.Tests
{
    [TestFixture]
    public class BaseTest : ExtentReporting
    { 
        protected IWebDriver _driver;
      
        [SetUp]
        public void BaseTestSetup()
        {
            // read properties
            LoadPropertiesFromFile();

            // Invoking the extent reporting
            createReportInstance();

            // read the properties file and invoke the driver based on the test type
            if(_configProperties["testType"].ToString().ToLower().Equals("ui"))
            {
                switch (_envProperties["browser"].ToLower())
                {
                    case "chrome":
                        {
                            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                            break;
                        }
                    case "firefox":
                        {
                            _driver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                            break;
                        }
                    case "ie":
                        {
                            var options = new InternetExplorerOptions();
                            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                            options.ElementScrollBehavior = InternetExplorerElementScrollBehavior.Bottom;
                            _driver = new InternetExplorerDriver(options);
                            break;
                        }
                    default:
                        {
                            throw new Exception("No browser type specified in the propoerties file");
                        }
                }
                _driver.Manage().Window.Maximize();
                _driver.Manage().Timeouts().ImplicitWait = 5.Seconds();
                _driver.Manage().Timeouts().PageLoad = 60.Seconds();
            }
        }

        [TearDown]
        public void BaseTestCleanup()
        {
            // closing the driver
            if (_configProperties["testType"].ToString().ToLower().Equals("ui"))
            {
                // if the test failed, take a screenshot
                if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
                {
                    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    var screenShotFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" +
                        TestContext.CurrentContext.Test.FullName + DateTime.Now.ToString("MMM-dd-HHmm") + ".jpg";
                    screenshot.SaveAsFile(screenShotFileName);
                    Console.WriteLine("Screenshot saved at: " + screenShotFileName);
                }

                _driver.Quit();
            }

            // Flushing the extent reports to the html
            _extentReports.Flush();


        }
        
        public void GoToUrl(String url)
        {
            _driver.Navigate().GoToUrl(url);
        }
    }
}

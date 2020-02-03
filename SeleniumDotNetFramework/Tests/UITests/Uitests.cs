using AventStack.ExtentReports;
using NUnit.Framework;
using SeleniumDotNetFramework.Pages.ExampleTestPages;
using System;
using System.IO;
using System.Reflection;

namespace SeleniumDotNetFramework.Tests
{
    [TestFixture]
    [Category("UI")]
    class Uitests : BaseTest
    {
        private ExtentTest _childScenarioTest = null;
        private HomePage _homePage = null;
        private string _scenarioName = null;

        [SetUp]
        public void TestSetup()
        {
            //getting the class name
            _scenarioName = this.GetType().Name;

            //creating the Parent Node for extent report
            createParentNode(_scenarioName);

            GoToUrl("https://www.gumtree.com.au/");
        }

        [Test]
        [Category("UI-1")]
        public void TC_SearchFunctionality()
        {
            try
            {
                //Getting the current method name
                var childTestName = MethodBase.GetCurrentMethod().Name;

                //creating the child node to the parent report
                _childScenarioTest = _extentTest.CreateNode(childTestName);

                _homePage = new HomePage(_driver, _childScenarioTest);

                _homePage.VerifyLandingPage();

                _homePage.SearchCriteria();

                _homePage.InputText();

                _homePage.InputArea();

                _homePage.InputRadius();

                _homePage.VerifyResultsPage();

                _homePage.ResultPageTest();

                _homePage.VerifyPageNavigation();

                _homePage.ImageVerify();

                //Printing the log to the extent test
                _childScenarioTest.Pass("Test case executed succesffully");

                //Printing the log to the nunit
                Assert.IsTrue(true, "Test case executed succesffully");
            }
            catch(Exception ex)
            {
                Assert.IsTrue(false, "Test case got failed due to following error : " + ex.Message);

                _childScenarioTest.Fail("test case got failed due to : " + ex.Message);
            }
        }
    }
}

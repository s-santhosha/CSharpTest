using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SeleniumDotNetFramework.Reporting
{
    public class ExtentReporting
    {
        protected static ExtentReports _extentReports;
        protected ExtentV3HtmlReporter _htmlReporter = null;
        protected ExtentTest _extentTest = null;
        protected static string hmtlreport;
        protected static Dictionary<String, String> _apiEnvProperties;
        protected static Dictionary<String, String> _configProperties;
        protected static Dictionary<String, String> _envProperties;

        //Creating the extent report instance
        protected void createReportInstance()
        {
            if (_extentReports == null)
            {
                hmtlreport = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                
                string reportname = _configProperties["reportName"] + hmtlreport + ".html";

                string _reportPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                
                _reportPath = _reportPath +"/ExtentReport/Reports/"+ hmtlreport +"/"+reportname;

                _extentReports = new ExtentReports();

                _htmlReporter = new ExtentV3HtmlReporter(_reportPath);

                string _testerName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().Split('\\')[1].ToString();

                _extentReports.AttachReporter(_htmlReporter);

                _extentReports.AddSystemInfo("Reporter name", _testerName);

                _extentReports.AddSystemInfo("Environment", _configProperties["environment"].ToString());

                _extentReports.AddSystemInfo("username", _testerName);

                _extentReports.AddSystemInfo("Location", _configProperties["Location"].ToString());
                
                _htmlReporter.LoadConfig(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName + "/Configuration/extent-config.xml");
            }
        }

        //Creating the parent node for the extent test report
        protected void createParentNode(string _scenarioName)
        {
            _extentTest = _extentReports.CreateTest(_scenarioName);
        }

        protected void LoadPropertiesFromFile()
        {
            string[] configFiles = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + "/Configuration/");

            foreach (var configProp in configFiles)
            {
                if (configProp.Contains("config"))
                {
                    _configProperties = new Dictionary<string, string>();
                    var envProperties = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + "/Configuration/" + "config" + ".properties");
                    foreach (var prop in envProperties)
                    {
                        var keyValue = prop.Split(new[] { '=' }, 2);
                        _configProperties.Add(keyValue[0].Trim(), keyValue[1].Trim());
                    }
                }
                else if (configProp.Contains("environment"))
                {
                    _envProperties = new Dictionary<string, string>();
                    var envproperties = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + "/Configuration/" + "environment" + ".properties");
                    foreach (var prop in envproperties)
                    {
                        var keyValue = prop.Split(new[] { '=' }, 2);
                        _envProperties.Add(keyValue[0].Trim(), keyValue[1].Trim());
                    }
                }
            }
        }
    }
}

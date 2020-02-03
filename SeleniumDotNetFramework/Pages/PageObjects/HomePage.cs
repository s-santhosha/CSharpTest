using AventStack.ExtentReports;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;


namespace SeleniumDotNetFramework.Pages.ExampleTestPages
{
    public class HomePage : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "[href='\\/m-my-ads\\.html\\?c\\=1']")]
        private IWebElement MyGumtree { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".search-category > [tabindex]")]
        private IWebElement SearchCategory { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[(text()='Cars & Vehicles')]/../span")]
        private IWebElement MainCategory { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[.='Cars, Vans & Utes']")]
        private IWebElement SubCategory { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='search-query']")]
        private IWebElement SearchText { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#search-area")]
        private IWebElement SearchArea { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='j-selectbox__text'][contains(text(),'250km')]")]
        private IWebElement SelectRadiusOption { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#srch-radius-input")]
        private IWebElement SearchRadius { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".header__search-button-icon--search")]
        private IWebElement SearchButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".results-per-page-selector .select--clear")]
        private IWebElement ResultsPerPage { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".select__select > option[value='96']")]
        private IWebElement PerPage96 { get; set; }

        [FindsBy(How = How.LinkText, Using = "2")]
        private IWebElement PageNumber2 { get; set; }

        [FindsBy(How = How.LinkText, Using = "3")]
        private IWebElement PageNumber3 { get; set; }

        [FindsBy(How = How.LinkText, Using = "4")]
        private IWebElement PageNumber4 { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".panel.search-results-page__top-ads-wrapper.user-ad-collection.user-ad-collection--row > .panel-body.panel-body--flat-panel-shadow.user-ad-collection__list-wrapper > a:nth-of-type(2)")]             
        private IWebElement ClickAD { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".vip-ad-image__legend > button")]
        private IWebElement ImageButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".icon-slider-arrow.vip-ad-gallery__arrow-icon")]
        private IWebElement RightSlideArrow { get; set; }


        public HomePage(IWebDriver driver, ExtentTest extentTest) : base(driver, extentTest)
        {
            PageFactory.InitElements(driver, this);
        }

        public void VerifyLandingPage()
        {
            Click(MyGumtree, "click on mygumtree");
        }

        public void SearchCriteria()
        {
            Click(SearchCategory, "Click search category");
            Click(MainCategory, "Click main category");
            Click(SubCategory, "Click sub category");

        }

        public void InputText()
        {
            //SearchText.SendKeys("Toyota");
            EnterText(SearchText, "Toyota", "Enter Vehicle Model");
        }

        public void InputArea()
        {
            //SearchArea.SendKeys("Wollongong Region, NSW");
            EnterText(SearchArea, "Wollongong Region, NSW", "Provide Region for search");
        }

        public void InputRadius()
        {
            Click(SearchRadius, "Click on Radius");
            Click(SelectRadiusOption, "Select Radius 250km");
            Click(SearchButton, "Click search Button");
            
        }

        public bool VerifyResultsPage()
        {
            return isElementDisplayed(ResultsPerPage);

        }

        public bool ResultPageTest()
        {
                                             
            Click(ResultsPerPage, "click result page");

            Click(PerPage96, "click 96");

                        
            var TotalResultsOnPage = GetCount(By.CssSelector(".search-results-page__main-ads-wrapper.user-ad-collection>div>a"));

            if (TotalResultsOnPage == 96)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
                
        public void VerifyPageNavigation()
        { 

            ClickByJavascript(PageNumber2, "Page 2");

            ClickByJavascript(PageNumber3, "Page 3");

            ClickByJavascript(PageNumber4, "Page 4");
                        
        }

        public void ImageVerify()
        {
            ClickByJavascript(ClickAD, "Click AD on the results page");
            ClickByJavascript(ImageButton, "click the images");
                       
            while(IsElementVisible(RightSlideArrow))
            {
                ClickByJavascript(RightSlideArrow, "Click the right slide arrow");
            }
            
        }

    }
}

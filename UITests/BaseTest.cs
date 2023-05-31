using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using Pages.BasePages;
using Pages.HomeLoginPages;
using Tools;
using WebDriverManager.DriverConfigs.Impl;
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]


namespace UITest
{
    [TestFixture]
    [AllureNUnit]
    [AllureParentSuite("SXA UI Tests")]
    public class BaseTest
    {

        protected IWebDriver webDriver;
        protected LoginPage loginPage;
        protected HomePage homePage;
        protected Actions actions;

        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            webDriver.Url = ConfigReader.Url;
            loginPage = new LoginPage(webDriver);
            homePage = new HomePage(webDriver);
            actions = new Actions(webDriver);
        }

        [TearDown]
        public void TearDown()
        {

            webDriver.Quit();
        }

        protected GridPage ChooseItemFromMainMenu(string module, string subMenu)
        {
            actions.MoveToElement(webDriver.FindElement(By.XPath("//button[@class='btn-menu-hamburger']")))
                    .Click()
                    .MoveToElement(webDriver.FindElement(By.XPath("//span[@data-module-name='" + module + "']")))
                    .Click()
                    .MoveToElement(webDriver.FindElement(By.XPath("//ul[@data-role='menu']/li/a/span[@title='" + subMenu + "']")))
                    .Click()
                    .Perform();
            return new GridPage(webDriver);
        }

        protected GridPage LoginAndOpenEntityGrid(string module, string subMenu)
        {
            loginPage.LoginToSXA();
            return ChooseItemFromMainMenu(module, subMenu);
        }




    }
}
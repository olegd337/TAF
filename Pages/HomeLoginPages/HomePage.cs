using NUnit.Allure.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Pages.BasePages;

namespace Pages.HomeLoginPages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver webdriver) : base(webdriver)
        {
        }

        private IWebElement userMenu => webDriver.FindElement(By.Id("userMenu"));

        [AllureStep]
        public bool CheckIsUserMenuDisplayed()
        {
            return IsElementDisplayed(userMenu);
        }



    }
}

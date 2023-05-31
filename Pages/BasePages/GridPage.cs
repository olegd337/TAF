using NUnit.Allure.Attributes;
using OpenQA.Selenium;

namespace Pages.BasePages
{
    public class GridPage : BasePage
    {
        public GridPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        private IWebElement createButton => webDriver.FindElement(By.ClassName("btn-entity-create"));
        private IWebElement entityGrid => webDriver.FindElement(By.ClassName("gridArea"));
        private IWebElement popupDialog => webDriver.FindElement(By.Id("popupDialog"));

        [AllureStep]
        public CreatePage ClickCreateButtonOnGrid()
        {
            ClickOnElement(createButton);
            return new CreatePage(webDriver);
        }
    }
}

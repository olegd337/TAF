using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using OpenQA.Selenium;
using Pages.BasePages;
using Tools;

namespace Pages.HomeLoginPages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver webdriver) : base(webdriver)
        {
        }

        private IWebElement usernameField => webDriver.FindElement(By.Id("UserName"));
        private IWebElement passwordField => webDriver.FindElement(By.Id("Password"));
        private IWebElement logInButton => webDriver.FindElement(By.XPath("//button[@type='submit']"));

        [AllureStep]
        public HomePage ClickOnLoginInButton()
        {
            ClickOnElement(logInButton);
            return new HomePage(webDriver);
        }

        [AllureStep]
        public LoginPage InputLoginToUserNameField(string login)
        {
            EnterTextToElement(usernameField, login);
            return this;
        }

        [AllureStep]
        public LoginPage InputPasswordToPasswordField(string password)
        {
            EnterTextToElement(passwordField, password);
            return this;
        }

        [AllureStep]
        public HomePage LoginToSXA()
        {
            InputLoginToUserNameField(ConfigReader.User);
            InputPasswordToPasswordField(ConfigReader.Password);
            ClickOnLoginInButton();
            return new HomePage(webDriver);
        }
    }
}

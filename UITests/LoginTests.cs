using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace UITest
{
    [TestFixture]
    [AllureNUnit]
    public class LoginTests : BaseTest
    {
        [Test]
        public void LoginWithValidCredentialsTest()
        {
            loginPage.InputLoginToUserNameField("testh12")
                     .InputPasswordToPasswordField("1")
                     .ClickOnLoginInButton();

            Assert.IsTrue(homePage.CheckIsUserMenuDisplayed(), "User menu was not displayed");
        }




    }
}

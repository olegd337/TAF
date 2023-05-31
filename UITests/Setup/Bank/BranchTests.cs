using NUnit.Allure.Core;
using NUnit.Framework;

namespace UITest.Setup.Bank
{
    [TestFixture]
    [AllureNUnit]
    public class BranchTests : BaseTest
    {

        [Test]
        public void CreateBranchTest()
        {
            LoginAndOpenEntityGrid("Setup", "Branch");
                

        }




    }

}

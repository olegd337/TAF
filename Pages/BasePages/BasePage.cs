using NUnit.Allure.Attributes;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Pages.BasePages
{
    public class BasePage
    {
        protected IWebDriver webDriver;
        protected WebDriverWait webDriverWait1;
        protected WebDriverWait webDriverWait15;
        public BasePage(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
            webDriverWait1 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(1));
            webDriverWait15 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(15));
        }

        protected void ClickOnElement(IWebElement webElement)
        {
            WaitForJSandJQueryToLoad();
            webElement.Click();
        }

        protected void EnterTextToElement(IWebElement webElement, string text)
        {
            webElement.Clear();
            webElement.SendKeys(text);
        }

        protected bool IsElementDisplayed(IWebElement webElement)
        {
            return webElement.Displayed;
        }

        protected void WriteErrorAndStopTest(string message, Exception e)
        {
            string msg;
            msg = message + " ";
            if (e == null)
            {
                Assert.Fail(message);
            }
            else
            {
                Assert.Fail(msg + e);
            }
        }

        protected bool WaitForJSandJQueryToLoad()
        {

            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));

            // wait for jQuery to load
            var jQueryLoad = (bool)((IJavaScriptExecutor)webDriver).ExecuteScript("return (window.jQuery != null) && (jQuery.active === 0);");

            // wait for Javascript to load
            var jsLoad = ((string)((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState")).ToLower().Equals("complete");

            return wait.Until(d => jQueryLoad) && wait.Until(i => jsLoad);
        }

        [AllureStep]
        protected bool ClickOnNextPageButton(IWebElement grid)
        {
            try
            {
                IWebElement pager = grid.FindElement(By.XPath(".//div[@data-role='pager']"));
                IWebElement buttonNext = pager.FindElement(By.XPath(".//a[@title='Go to the next page']"));
                if (!buttonNext.GetAttribute("class").Contains("k-state-disabled"))
                    ClickOnElement(buttonNext);
                else
                    return false;
            }
            catch (Exception e)
            {
                //smth to write
            }
            return true;

        }

        [AllureStep]
        protected void ClickOnFirstPageButton(IWebElement grid)
        {
            try
            {
                IWebElement pager = grid.FindElement(By.XPath(".//div[@data-role='pager']"));
                IWebElement buttonFirst = pager.FindElement(By.XPath(".//a[@title='Go to the first page']"));
                if (!buttonFirst.GetAttribute("class").Contains("k-state-disabled"))
                    ClickOnElement(buttonFirst);
            }
            catch (Exception ignored)
            {
                //why ignores fail ignoring?!
            }
        }

        private IWebElement GetGridCell(IWebElement grid, string title)
        {
            return webDriverWait1.Until(we => grid.FindElement(By.XPath(".//span[@title='" + title + "']")));
        }

        protected IWebElement FindGridCell(IWebElement grid, string title)
        {
            IWebElement result;
            do
            {
                result = GetGridCell(grid, title);
            } while (result == null && ClickOnNextPageButton(grid));
            return result;
        }

        protected IWebElement GetParentNode(IWebElement item)
        {
            if (item == null)
                return null;
            try
            {
                return item.FindElement(By.XPath("./.."));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private bool AllTrue(bool[] arrayBool)
        {
            var ret = arrayBool[0] != null && arrayBool[0];
            foreach (bool item in arrayBool)
            {
                ret = ret && item != null && item;
            }
            return ret;
        }

        private IWebElement GetRowByCell(IWebElement cell)
        {
            return GetParentNode(GetParentNode(cell));
        }


        private void FindGridCell(IWebElement grid, string[] values)
        {
            IWebElement result;
            var selected = new bool[values.Length];
            do
            {
                for (int index = 0; index < values.Length; index++)
                {
                    if (selected[index] != null && selected[index]) continue;
                    result = GetGridCell(grid, values[index]);
                    selected[index] = result != null;
                    if (result == null) continue;
                    //set check box true
                    var chkBox = GetRowByCell(result).FindElements(By.XPath(".//input[@type=\"checkbox\"]"))[0];
                    SwitchCheckBox(chkBox, true);
                }
            } while (!AllTrue(selected) && ClickOnNextPageButton(grid));
        }



        protected void SwitchToTab(IWebElement tabList, string tabName)
        {
            ClickOnElement(tabList.FindElement(By.XPath(".//a[text()='" + tabName + "']")));
        }

        protected void SelectInSingleSelect(IWebElement webElement, string itemName)
        {
            IWebElement selectButton = webElement.FindElements(By.XPath(".//button[contains(@title, 'Select')]"))[0];
            ClickOnElement(selectButton);
            List<IWebElement> lookupDialog = null;
            int timer = 0;
            while ((lookupDialog == null || lookupDialog.Count == 0) && timer < 5)
            {
                lookupDialog = webElement.FindElements(By.XPath("//div[@class='modal fade in']")).ToList();
                // setPause(1);
                timer++;
            }
            if (lookupDialog == null || lookupDialog.Count == 0)
            {
                WriteErrorAndStopTest("LookUp was not found!", null);
                return;
            }
            IWebElement lookupTable = GetParentNode(lookupDialog[0].FindElement(By.TagName("table")));
            ClickOnElement(FindGridCell(lookupTable, itemName));
        }

        protected void SwitchCheckBox(IWebElement checkBox, bool value)
        {
            var status = checkBox.GetAttribute("checked");//point to verify
            if (value != "true".Equals(status.ToLower()))
                ClickOnElement(GetParentNode(checkBox));
        }

        protected void SwitchExtenderCheckBox(IWebElement label, bool value)
        {
            var checkBox = label.FindElement(By.XPath(".//preceding-sibling::input[@type='checkbox']"));
            var status = checkBox.GetAttribute("checked");//point to verify
            if (value != "true".Equals(status.ToLower()))
                ClickOnElement(label);
        }

        protected void SelectInListBox(IWebElement listBox, string item)
        {
            string valuesId = listBox.GetAttribute("aria-owns");
            ClickOnElement(listBox);
            IWebElement uList = webDriver.FindElement(By.Id(valuesId));
            IWebElement itemList = uList.FindElement(By.XPath(".//li[contains(.,'" + item + "')]"));
            ClickOnElement(itemList);
        }


        protected void SelectInMultiSelect(IWebElement webElement, string[] values)
        {
            var multiContainer = GetParentNode(webElement);
            var selectButton = multiContainer.FindElements(By.XPath(".//div[contains(@class, 'btn') and contains(text(), 'Select')]"))[0];
            ClickOnElement(selectButton);
            var modalWindow = multiContainer.FindElements(By.XPath(".//div[@class='modal fade in']"))[0];
            var multiLookupGrid = modalWindow.FindElements(By.XPath(".//div[@data-role='grid']"))[0];
            FindGridCell(multiLookupGrid, values);
            var buttonAccept = multiContainer.FindElements(By.XPath(".//a[contains(@class, 'btn') and contains(text(), 'Accept')]"))[0];
            ClickOnElement(buttonAccept);
        }


        protected void FillKendoGrid(IWebElement grid, string sectionName, string[][] values)
        {
            List<IWebElement> buttonsAdd = grid.FindElements(By.XPath(".//a[@role=\"button\" and text()='Add']")).ToList();
            if (buttonsAdd.Count == 0)
                buttonsAdd = grid.FindElements(By.XPath(".//span[@class=\"k-button-text\" and text()='Add']")).ToList();
            IWebElement buttonAdd = buttonsAdd[0];

            foreach (string[] value in values)
            {
                ClickOnElement(buttonAdd);
                IWebElement row = grid.FindElements(By.XPath(".//tr[@class=\"k-master-row\"]"))[0];
                IWebElement chkbxMain = row.FindElement(By.XPath(".//input[@type=\"checkbox\"]"));
                SwitchCheckBox(chkbxMain, "true".Equals(value[0].ToLower()));
                row = grid.FindElements(By.XPath(".//tr[@class=\"k-master-row\"]"))[0];
                IWebElement cellType = row.FindElements(By.XPath(".//td"))[1];//[@data-field="Type_DDL_Id"]
                ClickOnElement(cellType);
                IWebElement spanListBox = cellType.FindElement(By.XPath(".//span[@role=\"listbox\"]"));
                SelectInListBox(spanListBox, value[1]);
                switch (sectionName)
                {
                    case "Phones":
                        {
                            row = grid.FindElements(By.XPath(".//tr[@class=\"k-master-row\"]"))[0];
                            IWebElement cellPhone = row.FindElements(By.XPath(".//td"))[2];//[@data-field="Phone"]
                            ClickOnElement(cellPhone);
                            IWebElement inputPhone = cellPhone.FindElement(By.Id("Phone"));
                            EnterTextToElement(inputPhone, value[2]);
                            break;
                        }
                    case "Identifiers":
                        {
                            row = grid.FindElements(By.XPath(".//tr[@class=\"k-master-row\"]"))[0];
                            IWebElement cellPhone = row.FindElements(By.XPath(".//td"))[2];//
                            ClickOnElement(cellPhone);
                            IWebElement inputPhone = cellPhone.FindElement(By.Id("Identifier__IdentifierCode"));
                            EnterTextToElement(inputPhone, value[2]);
                            break;
                        }
                    case "NetAddresses":
                        {
                            row = grid.FindElements(By.XPath(".//tr[@class=\"k-master-row\"]"))[0];
                            IWebElement cellAddress = row.FindElements(By.XPath(".//td"))[2];//[@data-field="Address"]
                            ClickOnElement(cellAddress);
                            IWebElement inputAddress = cellAddress.FindElement(By.Id("Address"));
                            EnterTextToElement(inputAddress, value[2]);
                            if (value.Length < 4) break;
                            row = grid.FindElements(By.XPath(".//tr[contains(@class, \"k-master-row\")]"))[0];
                            IWebElement cellComment = row.FindElements(By.XPath(".//td"))[3];//[@data-field="Comment"]
                            ClickOnElement(cellComment);//this click finalized previous action
                            row = grid.FindElements(By.XPath(".//tr[@class=\"k-master-row\"]"))[0];
                            cellComment = row.FindElements(By.XPath(".//td"))[3];//[@data-field="Comment"]
                            ClickOnElement(cellComment);
                            IWebElement inputComment = cellComment.FindElement(By.Id("Comment"));
                            EnterTextToElement(inputComment, value[3]);
                            break;
                        }
                        //default: continue;
                }
            }
        }


    }
}
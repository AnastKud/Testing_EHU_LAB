using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

[assembly: CollectionBehavior(DisableTestParallelization = false)]

namespace EhuTests_xUnit
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver driver;

        public BaseTest()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void Dispose()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }

    public class AboutTests : BaseTest
    {
        [Fact]
        [Trait("Category", "Smoke")]
        public void Test_AboutPage()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            driver.FindElement(By.PartialLinkText("About")).Click();

            Assert.Contains("/about", driver.Url);
            Assert.Contains("About", driver.Title);
        }
    }

    public class SearchTests : BaseTest
    {
        [Theory]
        [InlineData("study programs")]
        [InlineData("business")]
        [Trait("Category", "Regression")]
        public void Test_Search(string query)
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var js = (IJavaScriptExecutor)driver;

            IWebElement searchInput = (IWebElement)js.ExecuteScript(
                "return document.querySelector('input[name=\"s\"], input[type=\"search\"], .search-field');");

            Assert.NotNull(searchInput);

            js.ExecuteScript(
                "arguments[0].style.display='block'; arguments[0].style.visibility='visible';",
                searchInput);

            js.ExecuteScript(
                "arguments[0].value=arguments[1];",
                searchInput, query);

            IWebElement form = (IWebElement)js.ExecuteScript(
                "return arguments[0].closest('form');",
                searchInput);

            js.ExecuteScript("arguments[0].submit();", form);

            Assert.Contains(query.Replace(" ", "+"), driver.Url.ToLower());
        }
    }

    public class LanguageTests : BaseTest
    {
        [Fact]
        [Trait("Category", "UI")]
        public void Test_LanguageChange()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var enButton = driver.FindElement(By.LinkText("EN"));
            enButton.Click();

            var ltOption = driver.FindElement(By.LinkText("LT"));
            ltOption.Click();

            Assert.Contains("https://lt.ehuniversity.lt/", driver.Url);
        }
    }

    public class ContactTests : BaseTest
    {
        [Fact]
        [Trait("Category", "Regression")]
        public void Test_Contacts()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");

            var page = driver.PageSource;

            Assert.Contains("franciskscarynacr@gmail.com", page);
            Assert.Contains("+370 68 771365", page);
            Assert.Contains("+375 29 5781488", page);
        }
    }
}
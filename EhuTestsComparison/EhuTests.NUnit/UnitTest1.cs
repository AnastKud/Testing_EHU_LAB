using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace EhuTests
{
    public class BaseTest
    {
        protected IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }

    [TestFixture]
    [Category("Smoke")]
    public class AboutTests : BaseTest
    {
        [Test]
        public void Test_AboutPage()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            driver.FindElement(By.PartialLinkText("About")).Click();

            Assert.That(driver.Url, Does.Contain("/about"));
            Assert.That(driver.Title, Does.Contain("About"));
        }
    }

    [TestFixture]
    [Category("Regression")]
    public class SearchTests : BaseTest
    {
        [TestCase("study programs")]
        [TestCase("business")]
        public void Test_Search(string query)
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var js = (IJavaScriptExecutor)driver;

            IWebElement searchInput = (IWebElement)js.ExecuteScript(
                "return document.querySelector('input[name=\"s\"], input[type=\"search\"], .search-field');");

            Assert.That(searchInput, Is.Not.Null, "Search input not found");

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

            Assert.That(driver.Url.ToLower(), Does.Contain(query.Replace(" ", "+")));
        }
    }

    [TestFixture]
    [Category("UI")]
    public class LanguageTests : BaseTest
    {
        [Test]
        public void Test_LanguageChange()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var enButton = driver.FindElement(By.LinkText("EN"));
            enButton.Click();

            var ltOption = driver.FindElement(By.LinkText("LT"));
            ltOption.Click();

            Assert.That(driver.Url, Does.Contain("https://lt.ehuniversity.lt/"));
        }
    }

    [TestFixture]
    [Category("Regression")]
    public class ContactTests : BaseTest
    {
        [Test]
        public void Test_Contacts()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");

            var page = driver.PageSource;

            Assert.That(page, Does.Contain("franciskscarynacr@gmail.com"));
            Assert.That(page, Does.Contain("+370 68 771365"));
            Assert.That(page, Does.Contain("+375 29 5781488"));
        }
    }
}
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace StudentApp.Tests
{
    public class UITests
    {
        private ChromeDriver driver; 

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void OpenHomePage()
        {
            driver.Navigate().GoToUrl("https://www.google.com");
            Assert.AreEqual("Google", driver.Title);
        }

        [TearDown]
        public void Teardown()
        {
            driver.Dispose(); 
        }
    }
}

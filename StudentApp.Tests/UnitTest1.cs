using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace StudentApp.Tests
{
    public class RegisterTests
    {
        private ChromeDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void RegisterNewStudent()
        {
            driver.Navigate().GoToUrl("https://localhost:7047/User/Register");

            string email = $"juan.perez{DateTime.Now.Ticks}danivarelacor@gmail.com"; 
            driver.FindElement(By.Id("r-name")).SendKeys("Juan Pérez");
            driver.FindElement(By.Id("r-email")).SendKeys(email);
            driver.FindElement(By.Id("r-password")).SendKeys("Password123!");
            driver.FindElement(By.Id("confirm-password")).SendKeys("Password123!");

            
            driver.FindElement(By.Id("form-submit")).Click();

          
            Thread.Sleep(6000); 
            // 5️⃣ Esperar el mensaje de validación en la interfaz
            IWebElement validationMessage = wait.Until(d => d.FindElement(By.Id("validation")));
            string messageText = validationMessage.Text;

            // 6️⃣ Lista de mensajes esperados del backend
            string[] expectedMessages = {
                "Su solicitud ha sido enviada correctamente. Revisa tu correo.",
                "El correo electrónico ya ha sido registrado en el sistema.",
                "No ha sido posible hacer la solicitud de registro. Intente de nuevo más tarde.",
                "Ha ocurrido un error inesperado en el sistema. Intente de nuevo más tarde."
            };

            // 7️⃣ Validar que el mensaje recibido esté en la lista de mensajes esperados
            Assert.IsTrue(Array.Exists(expectedMessages, msg => messageText.Contains(msg)),
                          $"Mensaje recibido inesperado: {messageText}");
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Dispose(); // ✅ Usa Dispose() para cerrar el navegador correctamente
            }
        }
    }
}

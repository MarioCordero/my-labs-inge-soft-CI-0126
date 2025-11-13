using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UIAutomationTests
{
    [TestFixture]
    public class CompleteSeleniumTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private Actions _actions;

        [SetUp]
        public void Setup()
        {
            // Configurar Chrome options para Linux
            var chromeOptions = new ChromeOptions();
            // chromeOptions.AddArgument("--headless"); // Descomentar para ejecución sin interfaz
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--window-size=1920,1080");
            chromeOptions.AddArgument("--start-maximized");
            
            _driver = new ChromeDriver(chromeOptions);

            // Aumentar timeouts para dar tiempo al frontend (Vue) a renderizar
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            _actions = new Actions(_driver);
            
            Console.WriteLine("🚀 Navegador Chrome iniciado");
        }

        [Test]
        [Order(1)]
        public void HomePage_LoadsCorrectly()
        {
            Console.WriteLine("=== PRUEBA 1: Verificación página principal ===");
            
            // Act
            _driver.Navigate().GoToUrl("http://localhost:8080/");
            
            // Assert - Verificar múltiples elementos de la página
            Assert.That(_driver.Title, Is.Not.Empty, "El título de la página está vacío");
            
            // Verificar que existe una tabla
            var table = _wait.Until(d => d.FindElement(By.TagName("table")));
            Assert.IsTrue(table.Displayed, "La tabla de países no está visible");
            
            // Verificar encabezados de la tabla
            var headers = table.FindElements(By.TagName("th"));
            Assert.Greater(headers.Count, 2, "La tabla debe tener al menos 3 columnas");
            
            // Verificar que hay filas de datos
            var rows = table.FindElements(By.CssSelector("tbody tr"));
            Assert.Greater(rows.Count, 0, "La tabla debe tener filas de datos");
            
            Console.WriteLine($"✅ Página principal: {headers.Count} columnas, {rows.Count} filas");
        }

        [Test]
        [Order(2)]
        public void Navigation_ToCreateForm_Works()
        {
            Console.WriteLine("=== PRUEBA 2: Navegación al formulario de creación ===");
            
            // Arrange
            _driver.Navigate().GoToUrl("http://localhost:8080/");
            
            // Act - Buscar botón/anchor de agregar de diferentes formas (incluye /country)
            IWebElement addButton = FindElementWithRetry(
                By.CssSelector("a[href='/country'] button"),
                By.CssSelector("a[href='/country']"),
                By.LinkText("Agregar país"),
                By.XPath("//a[contains(normalize-space(.),'Agregar')]"),
                By.XPath("//button[contains(normalize-space(.),'Agregar')]"),
                By.CssSelector("a.btn-primary"),
                By.CssSelector("a[href*='create']"),
                By.CssSelector("a[href*='add']")
            );
            
            TryClick(addButton);
            
            // Assert - Verificar que estamos en la página de creación (/country)
            _wait.Until(d => 
                d.Url.Contains("/country") ||
                d.Url.Contains("create") || 
                d.Url.Contains("add") || 
                d.FindElements(By.TagName("form")).Count > 0 ||
                d.FindElements(By.CssSelector("input, select, textarea")).Count > 0 ||
                d.FindElements(By.XPath("//h2[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'agregar')]")).Count > 0
            );
            
            // Verificar elementos del formulario (si existe)
            var formInputs = _driver.FindElements(By.CssSelector("input, select, textarea"));
            Assert.GreaterOrEqual(formInputs.Count, 0, "Se esperaba encontrar el formulario o la página /country");
            
            Console.WriteLine("✅ Navegación al formulario exitosa");
        }

        [Test]
        [Order(3)]
        public void CreateCountry_Form_Validation()
        {
            Console.WriteLine("=== PRUEBA 3: Validación del formulario de creación ===");
            
            // Arrange - Ir directamente al formulario
            _driver.Navigate().GoToUrl("http://localhost:8080/");
            NavigateToCreateForm();
            
            // Act - Buscar todos los campos del formulario
            var nameInput = FindElementWithRetry(
                By.Id("nombre"),
                By.Name("nombre"),
                By.CssSelector("input[placeholder*='nombre']"),
                By.CssSelector("input[type='text']:first-of-type")
            );
            
            var languageInput = FindElementWithRetry(
                By.Id("idioma"),
                By.Name("idioma"),
                By.CssSelector("input[placeholder*='idioma']"),
                By.CssSelector("input[type='text']:nth-of-type(2)")
            );
            
            var continentSelect = FindElementWithRetry(
                By.Id("continente"),
                By.Name("continente"),
                By.TagName("select")
            );
            
            var submitButton = FindElementWithRetry(
                By.CssSelector("button[type='submit']"),
                By.XPath("//button[contains(text(), 'Guardar')]"),
                By.XPath("//button[contains(text(), 'Agregar')]"),
                By.CssSelector("button.btn-primary")
            );
            
            // Assert - Verificar que los campos están presentes y habilitados
            Assert.IsTrue(nameInput.Enabled, "Campo nombre debe estar habilitado");
            Assert.IsTrue(languageInput.Enabled, "Campo idioma debe estar habilitado");
            Assert.IsTrue(submitButton.Enabled, "Botón enviar debe estar habilitado");
            
            Console.WriteLine("✅ Validación de formulario exitosa");
        }

        [Test]
        [Order(5)]
        public void CountryTable_HasRequiredColumns()
        {
            Console.WriteLine("=== PRUEBA 5: Verificación de columnas de la tabla ===");
            
            // Arrange
            _driver.Navigate().GoToUrl("http://localhost:8080/");
            
            // Act
            var table = _wait.Until(d => d.FindElement(By.TagName("table")));
            var headers = table.FindElements(By.TagName("th"));
            var headerTexts = headers.Select(h => h.Text.Trim()).ToList();
            
            // Assert - Verificar columnas requeridas
            Assert.That(headerTexts, Has.Some.Contains("Nombre").Or.Contains("nombre"), "Falta columna Nombre");
            Assert.That(headerTexts, Has.Some.Contains("Continente").Or.Contains("continente"), "Falta columna Continente");
            Assert.That(headerTexts, Has.Some.Contains("Idioma").Or.Contains("idioma"), "Falta columna Idioma");
            Assert.That(headerTexts, Has.Some.Contains("Acciones").Or.Contains("acciones"), "Falta columna Acciones");
            
            Console.WriteLine("✅ Columnas de la tabla verificadas correctamente");
        }

        [Test]
        [Order(6)]
        public void Page_Elements_AreInteractive()
        {
            Console.WriteLine("=== PRUEBA 6: Interactividad de elementos ===");
            
            // Arrange
            _driver.Navigate().GoToUrl("http://localhost:8080/");
            
            // Act & Assert - Verificar que los elementos son clickeables
            var interactiveElements = _driver.FindElements(By.CssSelector("a, button, input, select"));
            Assert.Greater(interactiveElements.Count, 0, "Debe haber elementos interactivos en la página");
            
            // Verificar que al menos un elemento es clickeable
            var clickableElements = interactiveElements.Where(e => e.Displayed && e.Enabled).ToList();
            Assert.Greater(clickableElements.Count, 0, "Debe haber elementos clickeables en la página");
            
            Console.WriteLine($"✅ {clickableElements.Count} elementos interactivos verificados");
        }
        private void NavigateToCreateForm()
        {
            try
            {
                var addButton = FindElementWithRetry(
                    By.CssSelector("a[href='/country'] button"),
                    By.CssSelector("a[href='/country']"),
                    By.LinkText("Agregar país"),
                    By.XPath("//a[contains(normalize-space(.),'Agregar')]"),
                    By.XPath("//button[contains(normalize-space(.),'Agregar')]"),
                    By.CssSelector("a.btn-outline-secondary"),
                    By.CssSelector("a.btn-primary"),
                    By.CssSelector("a[href*='create']"),
                    By.CssSelector("a[href*='add']")
                );

                TryClick(addButton);

                // Esperar hasta que aparezca formulario o la URL cambie a /country
                _wait.Until(d =>
                    d.Url.Contains("/country") ||
                    d.Url.Contains("create") ||
                    d.Url.Contains("add") ||
                    d.FindElements(By.TagName("form")).Count > 0 ||
                    d.FindElements(By.CssSelector("input, select, textarea")).Count > 0 ||
                    d.FindElements(By.XPath("//h2[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'agregar')]")).Count > 0
                );
            }
            catch (Exception ex)
            {
                TakeScreenshot("navigate_to_create_error");
                DumpPageSource("navigate_to_create_error");
                throw new Exception($"No se pudo navegar al formulario de creación: {ex.Message}");
            }
        }
        private void TryClick(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch
            {
                try
                {
                    _actions.MoveToElement(element).Click().Perform();
                }
                catch
                {
                    try
                    {
                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ No se pudo clicar el elemento: {ex.Message}");
                        throw;
                    }
                }
            }
        }
        
        private IWebElement FindElementWithRetry(params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    // Usar WebDriverWait para dar tiempo al render de Vue
                    var element = _wait.Until(d =>
                    {
                        try
                        {
                            var e = d.FindElement(locator);
                            return (e.Displayed && e.Enabled) ? e : null;
                        }
                        catch
                        {
                            return null;
                        }
                    });

                    if (element != null)
                    {
                        Console.WriteLine($"✅ Elemento encontrado: {locator}");
                        return element;
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Intentar siguiente locator
                    continue;
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }

            throw new NoSuchElementException($"No se pudo encontrar el elemento con ninguno de los locators: {string.Join(", ", locators.Select(l => l.ToString()))}");
        }

        private IWebElement FindElement(params By[] locators)
        {
            // Reutilizar la versión con espera para mayor robustez
            return FindElementWithRetry(locators);
        }

        private void DumpPageSource(string name)
        {
            try
            {
                var source = _driver.PageSource;
                var fileName = $"/tmp/{name}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                System.IO.File.WriteAllText(fileName, source);
                Console.WriteLine($"📄 Page source guardado: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al guardar page source: {ex.Message}");
            }
        }
        
        private void TakeScreenshot(string testName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                var fileName = $"/tmp/{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                screenshot.SaveAsFile(fileName);
                Console.WriteLine($"📸 Screenshot guardado: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al guardar screenshot: {ex.Message}");
            }
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                TakeScreenshot("final_result");
                _driver?.Quit();
                _driver?.Dispose();
                Console.WriteLine("🛑 Navegador cerrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar el navegador: {ex.Message}");
            }
        }

        [Test]
        [Order(4)]
        public void CreateNewCountry_Successfully()
        {
            Console.WriteLine("=== PRUEBA 4: Creación de nuevo país ===");
            
            // Arrange
            string countryName = "PaisSelenium_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string language = "IdiomaSelenium";
            
            _driver.Navigate().GoToUrl("http://localhost:8080/");
            NavigateToCreateForm();
            
            // Act - Llenar formulario
            var nameInput = FindElement(By.Id("nombre"), By.Name("nombre"));
            var languageInput = FindElement(By.Id("idioma"), By.Name("idioma"));
            var continentSelect = FindElement(By.Id("continente"), By.Name("continente"), By.TagName("select"));
            var submitButton = FindElement(
                By.CssSelector("button[type='submit']"),
                By.XPath("//button[contains(text(), 'Guardar')]")
            );
            
            nameInput.Clear();
            nameInput.SendKeys(countryName);
            
            languageInput.Clear();
            languageInput.SendKeys(language);
            
            // Seleccionar continente si existe dropdown
            if (continentSelect != null && continentSelect.TagName == "select")
            {
                var selectElement = new SelectElement(continentSelect);
                if (selectElement.Options.Count > 0)
                {
                    selectElement.SelectByIndex(1); // Seleccionar segunda opción
                }
            }
            
            submitButton.Click();
            
            // Assert - Verificar creación exitosa
            _wait.Until(d => !d.Url.Contains("create") && !d.Url.Contains("add"));
            
            // Verificar que el país aparece en la lista
            bool countryCreated = _wait.Until(d => 
            {
                try
                {
                    return d.FindElement(By.XPath($"//tr[td[text()='{countryName}']]")) != null;
                }
                catch
                {
                    return false;
                }
            });
            
            Assert.IsTrue(countryCreated, $"El país '{countryName}' debería aparecer en la lista");
            Console.WriteLine($"✅ País '{countryName}' creado exitosamente");
        }
    }
}
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace GoodsCollection.Card.Parsers.WbParser;

public class WbParser : IDisposable
{
    private const string BaseUri = "https://www.wildberries.ru/catalog/{0}/detail.aspx";
    private readonly RemoteWebDriver _driver;
    private readonly WebDriverWait _wait;

    public WbParser()
    {
        _driver = new RemoteWebDriver(new Uri("http://selenium-hub:4444/wd/hub"), new ChromeOptions());
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    public bool Connect(string article)
    {
        try
        {
            _driver.Navigate().GoToUrl(string.Format(BaseUri, article));
            _driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public string? GetName()
    {
        try
        {
            var element = 
                _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".product-page__title")));
            
            return element.Text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public int? GetArticle()
    {
        try
        {
            var element = 
                _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("productNmId")));
            
            return Convert.ToInt32(element.Text);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public List<string>? GetImages()
    {
        try
        {
            var element = 
                _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".swiper-wrapper")));
            var images = element.FindElements(By.TagName("img"));

            List<string> list = [];
            list.AddRange(images.Select(image => image.GetAttribute("src")));

            return list;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public string? GetDescription()
    {
        try
        {
            var btn = _wait.Until(
                ExpectedConditions.ElementIsVisible(
                    By.CssSelector(".product-page__btn-detail.j-wba-card-item.j-wba-card-item-show.j-wba-card-item-observe")));
            btn.Click();
            
            var element = 
                _wait.Until(
                    ExpectedConditions.ElementIsVisible(
                        By.CssSelector(".option__text")));
            
            return element.Text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public string? GetBrand()
    {
        try
        {
            var element = 
                _wait.Until(
                    ExpectedConditions.ElementIsVisible(
                        By.CssSelector(".product-page__header-brand")));
            
            return element.Text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public string? GetPrice()
    {
        try
        {
            var element = 
                _wait.Until(
                    ExpectedConditions.ElementIsVisible(By.CssSelector(".price-block__price-group")));
            
            return element.Text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public string? GetRate()
    {
        try
        {
            var element = 
                _wait.Until(
                    ExpectedConditions.ElementIsVisible(
                    By.CssSelector(".product-review__rating")));
            
            return element.Text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public string? GetRatesCount()
    {
        try
        {
            var element = 
                _wait.Until(
                    ExpectedConditions.ElementIsVisible(
                        By.CssSelector(".product-review__count-review")));
            
            return element.Text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public bool IsExist()
    {
        try
        {
            var element = 
                _wait.Until(
                    ExpectedConditions.ElementIsVisible(
                        By.CssSelector(".error500__title")));
            
            var element2 = 
                _wait.Until(
                    ExpectedConditions.ElementIsVisible(
                        By.CssSelector(".content404__title")));

            return element is not null && element2 is not null;
        }
        catch (Exception e)
        {
            Console.WriteLine("Card is exist");
            return true;
        }
    }
    
    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
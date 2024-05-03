using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace GoodsCollection.Card.Drivers;

public class RemoteDriver : IDriver
{
    public WebDriver GetDriver()
    {
        return new RemoteWebDriver(new Uri("http://selenium-hub:4444/wd/hub"), new ChromeOptions());
    }
}
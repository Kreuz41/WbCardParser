using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace GoodsCollection.Card.Drivers;

public class LocalDriver : IDriver
{
    public WebDriver GetDriver()
    {
        return new ChromeDriver();
    }
}
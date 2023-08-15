using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main()
    {
        // 初始化WebDriver
        IWebDriver driver = new ChromeDriver();

        try
        {
            // 打開網站
            driver.Navigate().GoToUrl("https://tempail.com/");

            // 抓取data-clipboard-text的值
            var clipboardTextElement = driver.FindElement(By.CssSelector("[data-clipboard-text]"));
            string clipboardText = clipboardTextElement.GetAttribute("data-clipboard-text");

            // 放到剪貼簿
            Clipboard.SetText(clipboardText);
            Thread.Sleep(5000); // 等待5秒

            // 刷新頁面，等待連結出現
            while (true)
            {
                driver.Navigate().Refresh();
                Thread.Sleep(1000); // 每秒刷新一次

                try
                {
                    var linkElement = driver.FindElement(By.CssSelector("a[href^='https://tempail.com/en/mail_']"));
                    if (linkElement != null)
                    {
                        linkElement.Click(); // 點擊連結
                        break;
                    }
                }
                catch (NoSuchElementException)
                {
                    // 如果找不到元素，繼續循環
                    continue;
                }
            }

            // 抓取<h2>標籤的內容
            var h2Text = driver.FindElement(By.TagName("h2")).Text;

            // 放到剪貼簿
            Clipboard.SetText(h2Text);
        }
        finally
        {
            driver.Quit(); // 關閉瀏覽器
        }
    }
}

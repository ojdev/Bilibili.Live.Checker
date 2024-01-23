using Microsoft.Extensions.Options;

public static class SeleniumExtend
{
    /// <summary>
    /// 获取直播间信息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="UID">用户的UID</param>
    /// <returns></returns>
    public static (string? uname, string? avatar, string? cover, string? liveTitle, string? liveUrl, string? messageBody) GetBilibiliLiveInfo(this HttpClient client, string UID)
    {
        var url = $"https://space.bilibili.com/{UID}/";
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless");
        chromeOptions.AddArgument("--no-sandbox");
        using var driver = new ChromeDriver(chromeOptions);
        driver.Navigate().GoToUrl(url);
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("i-live")));
        string html = driver.PageSource;
        driver.Quit();
        var document = new HtmlDocument();
        document.LoadHtml(html);
        //po名字
        var uname = document.DocumentNode.SelectSingleNode($"//*[@id=\"h-name\"]")?.InnerText?.Trim();
        //头像
        var avatar = document.DocumentNode.SelectSingleNode($"//*[@id=\"app\"]//div[@class=\"bili-avatar\"]/img")?.GetAttributeValue<string>("src", "");
        var iLiveXpath = $"//*[@id=\"page-index\"]//div[@class=\"section i-live\"]";
        //直播间封面
        var cover = document.DocumentNode.SelectSingleNode($"{iLiveXpath}//img")?.GetAttributeValue<string>("src", "");
        var liveUrl = document.DocumentNode.SelectSingleNode($"{iLiveXpath}//div[@class=\"i-live-on\"]/a")?.GetAttributeValue<string>("href", "");
        //直播间标题
        var liveTitle = document.DocumentNode.SelectSingleNode($"{iLiveXpath}//p[@class=\"i-live-title\"]")?.InnerText?.Trim();

        var messageBody = @$"## {uname}的直播间开播了！
## {liveTitle}
[![直播间封面]({cover})]({liveUrl})"
;
        return (uname, avatar, cover, liveTitle, liveUrl, messageBody);
    }
}
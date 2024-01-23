using ILoggerFactory loggerFactory =
   LoggerFactory.Create(builder =>
       builder.AddSimpleConsole(options =>
       {
           options.IncludeScopes = true;
           options.SingleLine = true;
           options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
       }));
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
using (logger.BeginScope("[scope is enabled]")) ;

ConfigurationBuilder builder = new();
builder.AddJsonFile("appsetting.json", true, true);
var ConfigRoot = builder.Build();//根节点
var interval = ConfigRoot.GetSection("interval").Get<int>();
var configuration = ConfigRoot.GetSection("wxpusher").Get<WXPusher>();

logger.LogInformation("Hello, World!");


ObjectCache cache = MemoryCache.Default;
var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.MaxValue };

logger.LogInformation("关注列表初始化");
foreach (var user in configuration.Users)
{
    logger.LogInformation($"{user.Name}关注了{user.Bilibili.UIDs.Count}个主播;关注了{user.Bilibili.RoomIds.Count}个直播间");
    foreach (var uid in user.Bilibili.UIDs)
    {
        cache.Add(uid, new FollowInfo(uid), cacheItemPolicy);
    }
    foreach (var uid in user.Bilibili.RoomIds)
    {
        cache.Add($"{uid}", new FollowInfo($"{uid}"), cacheItemPolicy);
    }
}

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36 Edg/116.0.1938.76");
client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9");
client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
client.DefaultRequestHeaders.Add("Dnt", "1");
client.DefaultRequestHeaders.Add("Pragma", "no-cache");
client.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Chromium\";v=\"116\", \"Not)A;Brand\";v=\"24\", \"Microsoft Edge\";v=\"116\"");
client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");

var wechatPush = new PushWeChatMessage(configuration?.APPTOKEN ?? "");
logger.LogInformation("后台轮询定时器准备");
using var timer = new PeriodicTimer(TimeSpan.FromSeconds(interval));
int flse = 1;
bool isFlse = false;
logger.LogInformation("开始检测");
while (await timer.WaitForNextTickAsync())
{
    foreach (var user in configuration.Users)
    {
        if (user.IsDNDPeriod(DateTimeOffset.Now))
            continue;
        //按空间
        foreach (var uid in user.Bilibili.UIDs.Where(t => !string.IsNullOrWhiteSpace(t)))
        {
            var item = (FollowInfo?)cache.Get(uid);
            if (item == null) continue;
            cache.Remove(uid);
            client.DefaultRequestHeaders.Remove("Origin");
            client.DefaultRequestHeaders.Add("Origin", "https://space.bilibili.com");
            client.DefaultRequestHeaders.Remove("Referer");
            client.DefaultRequestHeaders.Add("Referer", $"https://space.bilibili.com/{item.UID}/");
            var iLiveInfo = client.GetBilibiliLiveInfo(item.UID);
            if (string.IsNullOrWhiteSpace(iLiveInfo.uname))
            {
                logger.LogWarning($"{uid}\t进入熔断");
                await Task.Delay(TimeSpan.FromMinutes(flse++));
                logger.LogWarning($"{flse}分钟后重试");
                if (!isFlse)
                    isFlse = true;
                continue;
            }
            else
            {
                if (isFlse)
                {
                    logger.LogWarning("解除熔断");
                    flse = 1;
                    isFlse = false;
                }
            }
            if (item.IsNotify)
            {
                if (!string.IsNullOrWhiteSpace(iLiveInfo.messageBody))
                {
                    //通知
                    var uids = new string[] { user.UID };
                    var topicIds = new string[] { user.TopicId };
                    var summary = "";

                    var content = $"{iLiveInfo}直播间开始直播了";
                    string url = $"https://live.bilibili.com/{uid}" ?? "";
                    await wechatPush.SendAsync(uids, topicIds, summary, iLiveInfo.messageBody, iLiveInfo.liveUrl, logger);
                    item.IsNotify = false;
                }
            }
            else
            {
                if (!item.Status)
                    item.IsNotify = true;
            }
            cache.Add(uid, item, cacheItemPolicy);

        }
        //按直播间
        foreach (var uid in user.Bilibili.RoomIds)
        {
            client.DefaultRequestHeaders.Remove("Origin");
            client.DefaultRequestHeaders.Add("Origin", "https://live.bilibili.com");
            client.DefaultRequestHeaders.Remove("Referer");
            client.DefaultRequestHeaders.Add("Referer", $"https://live.bilibili.com/{uid}/");
            var item = (FollowInfo?)cache.Get($"{uid}");
            if (item == null) continue;
            cache.Remove($"{uid}");
            var liveroom = await client.GetLiveRoom(uid, logger);
            item.Status = (liveroom?.Data?.LiveStatus ?? 0) == 1;
            if (item.IsNotify)
            {
                if (item.Status)
                {
                    var uids = new string[] { user.UID };
                    var topicIds = new string[] { user.TopicId };
                    var summary = "";
                    //通知
                    var iLiveInfo = client.GetBilibiliLiveInfo($"{liveroom?.Data?.UID}");
                    if (!string.IsNullOrWhiteSpace(iLiveInfo.messageBody))
                    {
                        await wechatPush.SendAsync(uids, topicIds, summary, iLiveInfo.messageBody, iLiveInfo.liveUrl, logger);
                    }
                    else
                    {
                        await wechatPush.SendAsync(uids, topicIds, summary, $"{uid}直播间开始直播了", $"https://live.bilibili.com/{uid}", logger);
                    }
                    item.IsNotify = false;
                }
            }
            else
            {
                if (!item.Status)
                    item.IsNotify = true;
            }
            cache.Add($"{uid}", item, cacheItemPolicy);
        }
    }
}

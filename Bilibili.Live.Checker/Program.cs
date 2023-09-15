// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Text.Json;
using System.Runtime.Caching;
using Microsoft.Extensions.Configuration;

ConfigurationBuilder builder = new();
builder.AddJsonFile("appsetting.json", true, true);
var ConfigRoot = builder.Build();//根节点
var interval = ConfigRoot.GetSection("interval").Get<int>();
var configuration = ConfigRoot.GetSection("wxpusher").Get<WXPusher>();

Console.WriteLine("Hello, World!");

ObjectCache cache = MemoryCache.Default;
var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.MaxValue };

Console.WriteLine("关注列表初始化");
foreach (var user in configuration.Users)
{
    Console.WriteLine($"{user.Name}关注了{user.Bilibili.UIDs.Count}个主播");
    foreach (var uid in user.Bilibili.UIDs)
    {
        cache.Add(uid, new FollowInfo(uid), cacheItemPolicy);
    }
}

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36 Edg/116.0.1938.76");
var wechatPush = new PushWeChatMessage(configuration?.APPTOKEN ?? "");
Console.WriteLine("后台轮询定时器准备");
using var timer = new PeriodicTimer(TimeSpan.FromSeconds(interval));

Console.WriteLine("开始检测");
while (await timer.WaitForNextTickAsync())
{
    foreach (var user in configuration.Users)
    {
        if (user.IsDNDPeriod(DateTimeOffset.Now))
            continue;
        foreach (var uid in user.Bilibili.UIDs)
        {
            var item = (FollowInfo?)cache.Get(uid);
            if (item == null) continue;
            cache.Remove(uid);
            var json = await client.GetStringAsync($"https://api.bilibili.com/x/space/wbi/acc/info?mid={item.UID}&token=&platform=web");
            var bilibiliScapeInfo = JsonSerializer.Deserialize<BilibiliResponse<BilibiliSpaceInfo>>(json);
            item.Status = (bilibiliScapeInfo?.Data?.LiveRoom?.LiveStatus ?? 0) == 1;
            if (item.IsNotify)
            {
                if (item.Status)
                {
                    //通知
                    var uids = new string[] { user.UID };
                    var topicIds = new string[] { user.TopicId };
                    var summary = "";
                    var content = bilibiliScapeInfo?.Data?.MessageBody();
                    string url = bilibiliScapeInfo?.Data?.LiveRoom?.Url ?? "";
                    await wechatPush.SendAsync(uids, topicIds, summary, content, url);
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
    }
}

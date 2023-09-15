// See https://aka.ms/new-console-template for more information
using System.Text.Json;
/// <summary>
/// 微信推送
/// </summary>
public class PushWeChatMessage
{
    private string AppToken { get; }

    public PushWeChatMessage(string appToken)
    {
        AppToken = appToken ?? throw new ArgumentNullException(nameof(appToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uids"></param>
    /// <param name="topicIds"></param>
    /// <param name="summary"></param>
    /// <param name="content"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task SendAsync(string[] uids, string[] topicIds, string summary, string content, string url = "")
    {
        using HttpClient send = new();
        Console.WriteLine($"推送信息给UID{string.Join('、', uids)}，TopicId{string.Join(',', topicIds)}");
        var sendRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://wxpusher.zjiecode.com/api/send/message"),
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                appToken = AppToken,
                content,
                summary,//消息摘要，显示在微信聊天页面或者模版消息卡片上，限制长度100，可以不传，不传默认截取content前面的内容。
                contentType = 3,//内容类型 1表示文字  2表示html(只发送body标签内部的数据即可，不包括body标签) 3表示markdown 
                uids,
                topicIds,
                url, //原文链接，可选参数
                verifyPay = false //是否验证订阅时间，true表示只推送给付费订阅用户，false表示推送的时候，不验证付费，不验证用户订阅到期时间，用户订阅过期了，也能收到。
            }), System.Text.Encoding.UTF8, "application/json")
        };
        var sendResp = await send.SendAsync(sendRequest);
        var respJson = await sendResp.Content.ReadAsStringAsync();
        Console.WriteLine(respJson);
    }
}

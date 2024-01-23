
public static class BilibiliExtend
{
    /// <summary>
    /// 空间页中的直播信息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="UID"></param>
    /// <returns></returns>
    public static async Task<string> GetSpaceLiveRoom(this HttpClient client, string UID)
    {

        var web = new HtmlWeb();
        var doc = web.Load($"https://space.bilibili.com/{UID}/");
        ////*[@id="h-name"]
        var title = doc.DocumentNode.InnerText;
        return title?.Substring(0, title.IndexOf("的个人空间")) ?? string.Empty;
    }
    /// <summary>
    /// 直播信息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="roomid"></param>
    /// <returns></returns>
    public static async Task<BilibiliResponse<BiliBiliLiveRoomInfo>> GetLiveRoom(this HttpClient client, long roomid)
    {
        var json = await client.GetStringAsync($"https://api.live.bilibili.com/room/v1/Room/room_init?id={roomid}");
        return JsonSerializer.Deserialize<BilibiliResponse<BiliBiliLiveRoomInfo>>(json);
    }
}

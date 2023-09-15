public static class BilibiliExtend
{
    /// <summary>
    /// 空间页中的直播信息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="UID"></param>
    /// <returns></returns>
    public static async Task<BilibiliResponse<BilibiliSpaceInfo>> GetSpaceLiveRoom(this HttpClient client, string UID)
    {
        var json = await client.GetStringAsync($"https://api.bilibili.com/x/space/wbi/acc/info?mid={UID}&token=&platform=web");
        return JsonSerializer.Deserialize<BilibiliResponse<BilibiliSpaceInfo>>(json);
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

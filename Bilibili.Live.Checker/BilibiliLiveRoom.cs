// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;
/// <summary>
/// 直播间状态
/// </summary>
class BilibiliLiveRoom
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("roomStatus")]
    public int RoomStatus { get; set; }
    /// <summary>
    /// 直播间状态，0未开播，1开播
    /// </summary>
    [JsonPropertyName("liveStatus")]
    public int LiveStatus { get; set; }
    /// <summary>
    /// 直播间标题
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    /// <summary>
    /// 直播间地址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    /// <summary>
    /// 封面
    /// </summary>
    [JsonPropertyName("cover")]
    public string? Cover { get; set; }
}
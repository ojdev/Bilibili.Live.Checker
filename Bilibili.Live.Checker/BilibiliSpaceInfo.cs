// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;
/// <summary>
/// 空间信息
/// </summary>
class BilibiliSpaceInfo
{
    /// <summary>
    /// 用户名
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    [JsonPropertyName("sex")]
    public string? Sex { get; set; }
    /// <summary>
    /// 头像
    /// </summary>
    [JsonPropertyName("face")]
    public string? Face { get; set; }
    /// <summary>
    /// 签名
    /// </summary>
    [JsonPropertyName("sign")]
    public string? Sign { get; set; }
    /// <summary>
    /// B站等级
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }
    /// <summary>
    /// 直播间
    /// </summary>
    [JsonPropertyName("live_room")]
    public BilibiliLiveRoom? LiveRoom { get; set; }
    public string MessageBody()
    {
        return 
@$"## Lv{Level} {Name}的直播间开播了！
## {LiveRoom?.Title}
[![直播间封面]({LiveRoom?.Cover})]({LiveRoom?.Url})"
;
    }
}

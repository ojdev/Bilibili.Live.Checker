// See https://aka.ms/new-console-template for more information
/// <summary>
/// 空间页的直播间状态
/// </summary>
/// <param name="RoomStatus">  </param>
/// <param name="LiveStatus"> 直播间状态，0未开播，1开播 </param>
/// <param name="Title"> 直播间标题 </param>
/// <param name="Url"> 直播间地址 </param>
/// <param name="Cover"> 封面 </param>
public record BilibiliSpaceLiveRoom([property: JsonPropertyName("roomStatus")] int RoomStatus, [property: JsonPropertyName("liveStatus")] int LiveStatus, [property: JsonPropertyName("title")] string? Title, [property: JsonPropertyName("url")] string? Url, [property: JsonPropertyName("cover")] string? Cover);

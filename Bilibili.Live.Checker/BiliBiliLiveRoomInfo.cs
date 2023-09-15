// See https://aka.ms/new-console-template for more information
/// <summary>
/// 空间页的直播间状态
/// </summary>
/// <param name="RoomStatus">  </param>
/// <param name="LiveStatus"> 直播间状态，0未开播，1开播 </param>
/// <param name="Title"> 直播间标题 </param>
/// <param name="Url"> 直播间地址 </param>
/// <param name="Cover"> 封面 </param>
/// <summary>
/// 直播间状态
/// </summary>
/// <param name="RoomId"> 房间号 </param>
/// <param name="UID"> UID </param>
/// <param name="LiveStatus"> 直播间状态，0未开播，1开播 </param>
public record BiliBiliLiveRoomInfo([property: JsonPropertyName("room_id")] long? RoomId, [property: JsonPropertyName("uid")] long? UID, [property: JsonPropertyName("live_status")] int LiveStatus);

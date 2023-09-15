// See https://aka.ms/new-console-template for more information
/// <summary>
/// 请求信息
/// </summary>
/// <typeparam name="T"></typeparam>
public class BilibiliResponse<T>
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("ttl")]
    public int TTL { get; set; }
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}

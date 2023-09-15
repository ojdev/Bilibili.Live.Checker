// See https://aka.ms/new-console-template for more information
public class WXPusherUser
{
    /// <summary>
    /// WXPusher的UID
    /// </summary>
    public string UID { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string TopicId { get; set; }
    /// <summary>
    /// 备注名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 关注的主播列表
    /// </summary>
    public WXPusherUserBilibili Bilibili { get; set; } = new();
    /// <summary>
    /// 免打扰时段
    /// </summary>
    public List<string> DNDPeriod { get; set; } = new();
    /// <summary>
    /// 是否免打扰时段
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public bool IsDNDPeriod(DateTimeOffset dateTime)
    {
        foreach (var item in DNDPeriod)
        {
            var time = item.Split('-');
            if (DateTimeOffset.Parse(DateTimeOffset.Now.ToString("yyyy-MM-dd") + " " + time[0]) > dateTime && dateTime < DateTimeOffset.Parse(DateTimeOffset.Now.ToString("yyyy-MM-dd") + " " + time[1]))
            {
                return true;
            }
        }
        return false;
    }
}
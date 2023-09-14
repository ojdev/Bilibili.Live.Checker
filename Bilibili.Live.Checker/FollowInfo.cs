// See https://aka.ms/new-console-template for more information
/// <summary>
/// 主播配置
/// </summary>
class FollowInfo
{
    /// <summary>
    /// UID
    /// </summary>
    public string UID { get; set; }
    /// <summary>
    /// 是否开播
    /// </summary>
    public bool Status { get; set; } = false;
    /// <summary>
    /// 是否要通知
    /// </summary>
    public bool IsNotify { get; set; } = true;

    public FollowInfo(string uID)
    {
        UID = uID ?? throw new ArgumentNullException(nameof(uID));
    }
}

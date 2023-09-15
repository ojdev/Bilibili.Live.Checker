// See https://aka.ms/new-console-template for more information
/// <summary>
/// 
/// </summary>
/// <param name="APPTOKEN">  </param>
public record WXPusher(string? APPTOKEN)
{
    public List<WXPusherUser> Users { set; get; } = new();

}

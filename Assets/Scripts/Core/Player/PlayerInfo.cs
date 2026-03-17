/// <summary>
/// 玩家数据信息类
/// </summary>
public class PlayerInfo
{
    public PlayerInfo(int playerId, string playerName)
    {
        this.playerId = playerId;
        this.playerName = playerName;
    }
    
    /// <summary>
    /// 玩家ID
    /// </summary>
    private int playerId;
    public int PlayerId { get => playerId; set => playerId = value; }
    /// <summary>
    /// 玩家名称
    /// </summary>
    private string playerName;
    public string PlayerName { get => playerName; set => playerName = value; }

}
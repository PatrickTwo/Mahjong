## 一. 总体架构设计

### 1.1 系统架构概览
```
MahjongGame (游戏主控制器)
├── TilePool (牌池管理)
├── PlayerManager (玩家管理)
├── GameFlowController (游戏流程控制)
├── WinChecker (胡牌判定器)
├── ScoreCalculator (计分器)
└── RuleEngine (规则引擎)
```

### 1.2 核心设计原则
- **模块化设计**：各功能模块独立，便于维护和扩展
- **事件驱动**：使用事件机制处理玩家交互和游戏状态变化
- **状态模式**：游戏流程通过状态机管理
- **策略模式**：胡牌判定和计分规则可配置

## 二. 核心类设计

### 2.1 枚举类型定义

#### 2.1.1 牌类型枚举 (TileType)
```csharp
/// <summary>
/// 麻将牌类型枚举
/// </summary>
public enum TileType
{
    // 饼牌
    Dot1, Dot2, Dot3, Dot4, Dot5, Dot6, Dot7, Dot8, Dot9,
    
    // 万牌
    Bamboo1, Bamboo2, Bamboo3, Bamboo4, Bamboo5, Bamboo6, Bamboo7, Bamboo8, Bamboo9,
    
    // 条牌
    Character1, Character2, Character3, Character4, Character5, Character6, Character7, Character8, Character9,
    
    // 风牌
    EastWind, SouthWind, WestWind, NorthWind,
    
    // 将牌
    RedDragon, GreenDragon, WhiteDragon
}
```

#### 2.1.2 牌花色枚举 (SuitType)
```csharp
/// <summary>
/// 牌花色枚举
/// </summary>
public enum SuitType
{
    Dot,        // 饼
    Bamboo,     // 万
    Character,  // 条
    Wind,       // 风牌
    Dragon      // 将牌
}
```

#### 2.1.3 游戏状态枚举 (GameState)
```csharp
/// <summary>
/// 游戏状态枚举
/// </summary>
public enum GameState
{
    Initializing,   // 初始化中
    Dealing,        // 发牌中
    Playing,        // 进行中
    TingDeclared,   // 已听牌
    Win,            // 胡牌
    Draw,           // 流局
    Ended           // 结束
}
```

#### 2.1.4 玩家操作枚举 (PlayerAction)
```csharp
/// <summary>
/// 玩家操作枚举
/// </summary>
public enum PlayerAction
{
    DrawTile,       // 摸牌
    DiscardTile,    // 打牌
    Pung,           // 碰牌
    Kong,           // 杠牌
    DeclareTing,    // 听牌
    Win,            // 胡牌
    Skip            // 跳过
}
```

### 2.2 核心数据结构

#### 2.2.1 麻将牌类 (MahjongTile)
```csharp
/// <summary>
/// 麻将牌类
/// </summary>
public class MahjongTile
{
    public TileType Type { get; private set; }
    public SuitType Suit { get; private set; }
    public int Value { get; private set; }
    public bool IsWindTile => Suit == SuitType.Wind;
    public bool IsDragonTile => Suit == SuitType.Dragon;
    public bool IsHonorTile => IsWindTile || IsDragonTile;
    
    public MahjongTile(TileType type, SuitType suit, int value)
    {
        Type = type;
        Suit = suit;
        Value = value;
    }
    
    public override bool Equals(object obj)
    {
        return obj is MahjongTile tile && tile.Type == Type;
    }
    
    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}
```

#### 2.2.2 玩家手牌类 (PlayerHand)
```csharp
/// <summary>
/// 玩家手牌管理类
/// </summary>
public class PlayerHand
{
    private List<MahjongTile> tiles = new List<MahjongTile>();
    private List<MahjongTile> discardedTiles = new List<MahjongTile>();
    private List<Meld> melds = new List<Meld>();
    
    public bool IsTing { get; private set; } = false;
    public MahjongTile TingTile { get; private set; } // 听的牌
    
    public void AddTile(MahjongTile tile)
    {
        tiles.Add(tile);
        SortTiles();
    }
    
    public void DiscardTile(MahjongTile tile)
    {
        tiles.Remove(tile);
        discardedTiles.Add(tile);
    }
    
    public void DeclareTing(MahjongTile tingTile)
    {
        IsTing = true;
        TingTile = tingTile;
    }
    
    private void SortTiles()
    {
        tiles = tiles.OrderBy(t => t.Suit)
                     .ThenBy(t => t.Value)
                     .ToList();
    }
}
```

#### 2.2.3 牌组类 (Meld)
```csharp
/// <summary>
/// 牌组类（刻子、顺子、对子等）
/// </summary>
public class Meld
{
    public MeldType Type { get; private set; }
    public List<MahjongTile> Tiles { get; private set; }
    public bool IsConcealed { get; private set; } // 是否暗牌
    
    public Meld(MeldType type, List<MahjongTile> tiles, bool concealed = false)
    {
        Type = type;
        Tiles = tiles;
        IsConcealed = concealed;
    }
}

/// <summary>
/// 牌组类型枚举
/// </summary>
public enum MeldType
{
    Pung,       // 刻子
    Chow,       // 顺子
    Kong,       // 杠
    Pair,       // 对子
    Special     // 特殊组合（风头/将头）
}
```

### 2.3 核心管理类

#### 2.3.1 牌池管理类 (TilePool)
```csharp
/// <summary>
/// 牌池管理类
/// </summary>
public class TilePool
{
    private Stack<MahjongTile> wall = new Stack<MahjongTile>();
    private List<MahjongTile> discardPool = new List<MahjongTile>();
    private const int DeadWallSize = 20; // 黄堆大小
    
    public int RemainingTiles => wall.Count;
    public bool IsDeadWallReached => wall.Count <= DeadWallSize;
    
    public void Initialize()
    {
        // 创建136张牌
        var allTiles = CreateAllTiles();
        
        // 洗牌
        var shuffled = allTiles.OrderBy(x => Guid.NewGuid()).ToList();
        
        wall = new Stack<MahjongTile>(shuffled);
    }
    
    public MahjongTile DrawTile()
    {
        return wall.Count > 0 ? wall.Pop() : null;
    }
    
    public MahjongTile DrawFromDeadWall()
    {
        // 从牌尾摸牌（杠牌时使用）
        return wall.Count > DeadWallSize ? wall.Pop() : null;
    }
    
    private List<MahjongTile> CreateAllTiles()
    {
        var tiles = new List<MahjongTile>();
        
        // 创建饼、万、条牌（各36张）
        CreateSuitTiles(SuitType.Dot, tiles);
        CreateSuitTiles(SuitType.Bamboo, tiles);
        CreateSuitTiles(SuitType.Character, tiles);
        
        // 创建风牌（16张）
        CreateWindTiles(tiles);
        
        // 创建将牌（12张）
        CreateDragonTiles(tiles);
        
        return tiles;
    }
}
```

#### 2.3.2 玩家类 (Player)
```csharp
/// <summary>
/// 玩家类
/// </summary>
public class Player
{
    public int PlayerId { get; private set; }
    public string Name { get; private set; }
    public PlayerHand Hand { get; private set; }
    public int Score { get; private set; }
    public bool IsDealer { get; set; }
    
    public Player(int playerId, string name)
    {
        PlayerId = playerId;
        Name = name;
        Hand = new PlayerHand();
        Score = 0;
    }
    
    public void AddScore(int points)
    {
        Score += points;
    }
    
    public void SubtractScore(int points)
    {
        Score -= points;
    }
}
```

#### 2.3.3 游戏主控制器 (MahjongGame)
```csharp
/// <summary>
/// 麻将游戏主控制器
/// </summary>
public class MahjongGame
{
    private TilePool tilePool;
    private PlayerManager playerManager;
    private GameFlowController flowController;
    private WinChecker winChecker;
    private ScoreCalculator scoreCalculator;
    
    public GameState CurrentState { get; private set; }
    public int CurrentPlayerIndex { get; private set; }
    
    public event Action<GameState> OnStateChanged;
    public event Action<Player, PlayerAction> OnPlayerAction;
    
    public MahjongGame()
    {
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        tilePool = new TilePool();
        playerManager = new PlayerManager();
        flowController = new GameFlowController(this);
        winChecker = new WinChecker();
        scoreCalculator = new ScoreCalculator();
    }
    
    public void StartNewGame()
    {
        tilePool.Initialize();
        playerManager.InitializePlayers();
        flowController.StartGame();
    }
}
```

## 三. 游戏流程设计

### 3.1 游戏状态机
```csharp
/// <summary>
/// 游戏流程控制器
/// </summary>
public class GameFlowController
{
    private MahjongGame game;
    private GameState currentState;
    
    public GameFlowController(MahjongGame game)
    {
        this.game = game;
    }
    
    public void StartGame()
    {
        TransitionToState(GameState.Initializing);
        DealTiles();
        TransitionToState(GameState.Playing);
    }
    
    public void ProcessPlayerTurn(Player player, PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.DrawTile:
                HandleDrawTile(player);
                break;
            case PlayerAction.DiscardTile:
                HandleDiscardTile(player);
                break;
            case PlayerAction.DeclareTing:
                HandleDeclareTing(player);
                break;
            // 其他操作处理...
        }
    }
    
    private void TransitionToState(GameState newState)
    {
        currentState = newState;
        game.OnStateChanged?.Invoke(newState);
    }
}
```

### 3.2 回合处理流程
```
回合开始
├── 摸牌阶段
│   └── 玩家从牌墙摸一张牌
├── 操作阶段（按优先级）
│   ├── 听牌判定
│   ├── 胡牌判定（如果已听牌）
│   ├── 暗杠判定
│   └── 其他操作
└── 打牌阶段
    └── 打出一张手牌
```

## 四. 胡牌判定系统设计

### 4.1 胡牌判定器 (WinChecker)
```csharp
/// <summary>
/// 胡牌判定器
/// </summary>
public class WinChecker
{
    public bool CanWin(PlayerHand hand, MahjongTile winningTile)
    {
        // 检查缺门规则
        if (!CheckMissingSuit(hand))
            return false;
            
        // 检查基本胡牌结构
        return CheckWinPattern(hand, winningTile);
    }
    
    private bool CheckMissingSuit(PlayerHand hand)
    {
        // 实现缺门检查逻辑
        // 玩家手中必须只有饼、万、条中的两种花色
        return true;
    }
    
    private bool CheckWinPattern(PlayerHand hand, MahjongTile winningTile)
    {
        // 实现胡牌牌型判定
        // n × AAA + m × ABC + x × (风头+任意两张风牌) + y × (将头+任意两张将牌) + s × DD
        return true;
    }
    
    public bool CanTing(PlayerHand hand)
    {
        // 检查是否可以听牌
        // 玩家只差最后一张牌即可胡牌
        return true;
    }
}
```

### 4.2 胡牌牌型算法
```csharp
/// <summary>
/// 胡牌牌型分析器
/// </summary>
public class WinPatternAnalyzer
{
    public WinPattern Analyze(PlayerHand hand)
    {
        var pattern = new WinPattern();
        
        // 分析刻子、顺子、特殊组合等
        AnalyzeMelds(hand, pattern);
        AnalyzeSpecialCombinations(hand, pattern);
        
        return pattern;
    }
    
    private void AnalyzeMelds(PlayerHand hand, WinPattern pattern)
    {
        // 实现牌组分析逻辑
    }
    
    private void AnalyzeSpecialCombinations(PlayerHand hand, WinPattern pattern)
    {
        // 分析风头/将头等特殊组合
    }
}

/// <summary>
/// 胡牌牌型结果
/// </summary>
public class WinPattern
{
    public int PungCount { get; set; }      // 刻子数量
    public int ChowCount { get; set; }      // 顺子数量
    public int SpecialCount { get; set; }   // 特殊组合数量
    public bool HasWindHead { get; set; }   // 是否有风头
    public bool HasDragonHead { get; set; } // 是否有将头
    public bool IsPureSuit { get; set; }    // 是否清一色
}
```

## 五. 计分系统设计

### 5.1 计分器 (ScoreCalculator)
```csharp
/// <summary>
/// 计分器
/// </summary>
public class ScoreCalculator
{
    private const int BaseScore = 1; // 底分
    
    public ScoreResult CalculateScore(WinPattern pattern, WinMethod method, Player winner, Player payer)
    {
        int multiplier = CalculateMultiplier(pattern, method);
        int totalScore = BaseScore * multiplier;
        
        return new ScoreResult
        {
            Winner = winner,
            Payer = payer,
            BaseScore = BaseScore,
            Multiplier = multiplier,
            TotalScore = totalScore
        };
    }
    
    private int CalculateMultiplier(WinPattern pattern, WinMethod method)
    {
        int multiplier = 1;
        
        // 清一色加倍
        if (pattern.IsPureSuit) multiplier *= 2;
        
        // 杠上开花加倍
        if (method == WinMethod.KongDraw) multiplier *= 2;
        
        // 自摸加倍
        if (method == WinMethod.SelfDraw) multiplier *= 2;
        
        // 风头/将头组合加倍
        if (pattern.HasWindHead) multiplier *= 2;
        if (pattern.HasDragonHead) multiplier *= 2;
        
        // 断风将加倍
        if (pattern.SpecialCount == 0) multiplier *= 2;
        
        return multiplier;
    }
}

/// <summary>
/// 胡牌方式枚举
/// </summary>
public enum WinMethod
{
    SelfDraw,   // 自摸
    Discard,    // 点炮
    KongDraw,   // 杠上开花
    RobKong     // 抢杠胡
}
```

## 六. 事件系统设计

### 6.1 游戏事件定义
```csharp
/// <summary>
/// 游戏事件参数基类
/// </summary>
public abstract class GameEventArgs : EventArgs
{
    public DateTime Timestamp { get; } = DateTime.Now;
}

/// <summary>
/// 玩家操作事件
/// </summary>
public class PlayerActionEventArgs : GameEventArgs
{
    public Player Player { get; set; }
    public PlayerAction Action { get; set; }
    public MahjongTile Tile { get; set; }
}

/// <summary>
/// 胡牌事件
/// </summary>
public class WinEventArgs : GameEventArgs
{
    public Player Winner { get; set; }
    public WinMethod Method { get; set; }
    public MahjongTile WinningTile { get; set; }
    public ScoreResult Score { get; set; }
}
```

## 七. 配置与扩展性设计

### 7.1 游戏规则配置
```csharp
/// <summary>
/// 游戏规则配置类
/// </summary>
public class GameRules
{
    public int DeadWallSize { get; set; } = 20;
    public int BaseScore { get; set; } = 1;
    public bool AllowRobKong { get; set; } = true;
    public bool AllowMultipleWinners { get; set; } = false;
    public int MaxRounds { get; set; } = 4;
}
```

### 7.2 扩展接口设计
```csharp
/// <summary>
/// 规则引擎接口
/// </summary>
public interface IRuleEngine
{
    bool ValidateAction(Player player, PlayerAction action, MahjongTile tile);
    bool CheckWinConditions(PlayerHand hand, MahjongTile winningTile);
    int CalculateScore(WinPattern pattern, WinMethod method);
}

/// <summary>
/// AI玩家接口
/// </summary>
public interface IAIStrategy
{
    PlayerAction DecideAction(Player player, GameState state);
    MahjongTile ChooseDiscard(PlayerHand hand);
}
```

## 八. 数据库设计

### 8.1 游戏记录表结构
```sql
-- 游戏记录表
CREATE TABLE GameRecords (
    GameId INT PRIMARY KEY,
    StartTime DATETIME,
    EndTime DATETIME,
    WinnerPlayerId INT,
    WinMethod VARCHAR(20),
    TotalScore INT
);

-- 玩家得分记录表
CREATE TABLE PlayerScores (
    RecordId INT PRIMARY KEY,
    GameId INT,
    PlayerId INT,
    FinalScore INT,
    IsDealer BIT
);

-- 操作记录表
CREATE TABLE ActionLogs (
    LogId INT PRIMARY KEY,
    GameId INT,
    PlayerId INT,
    ActionType VARCHAR(20),
    TileType VARCHAR(10),
    Timestamp DATETIME
);
```

## 九. 性能优化考虑

### 9.1 内存优化
- 使用对象池管理频繁创建的牌对象
- 懒加载策略减少初始化时间
- 使用值类型存储基本数据

### 9.2 算法优化
- 胡牌判定使用位运算加速
- 缓存常用的牌型分析结果
- 使用高效的排序和搜索算法

### 9.3 并发处理
- 使用异步操作处理网络请求
- 线程安全的数据结构
- 事件驱动的异步编程模式

---

## 总结

本框架设计基于麻将设计文档，采用模块化、事件驱动的架构，确保代码的可维护性和扩展性。核心功能包括：

1. **完整的牌池管理系统**：支持136张牌的管理和洗牌
2. **灵活的胡牌判定系统**：支持复杂的牌型分析和规则验证
3. **可配置的计分系统**：支持多种加倍规则和分数计算
4. **健壮的游戏流程控制**：基于状态机的游戏流程管理
5. **良好的扩展性**：通过接口设计支持规则定制和AI集成

该框架为麻将游戏的实现提供了坚实的基础，后续可根据具体需求进行功能扩展和优化。
# 基于类型的实例事件系统 (TypeEventSystem) 说明文档

该事件系统经过重构，移除了原本的全局静态事件中心，改为支持多实例的独立事件系统，能够为不同模块提供互相隔离的事件流。同时，彻底移除了原本自动注销时的反射调用，大幅提升了性能。

## 核心特性
- **实例级别独立事件中心**：各模块可独立实例化，避免全局总线耦合。
- **强类型驱动**：基于 `IEvent` 接口，事件以类型和对象作为载体传递。
- **高性能自动注销**：采用 `IUnRegister` 接口进行注销，GameObject销毁时0反射开销。

## 使用指南

### 1. 定义事件
所有的事件必须实现 `IEvent` 接口。你可以使用 `struct` 或者 `class` 来定义，支持携带任意参数。

```csharp
using Mahjong.System.TypeEventSystem;

// 无参事件
public struct GameStartEvent : IEvent { }

// 带参事件
public class PlayerScoreChangedEvent : IEvent
{
    public int PlayerId;
    public int NewScore;
}
```

### 2. 创建事件系统实例
在各个独立模块中（如战斗模块、UI模块），你需要自己实例化并持有事件系统的引用：

```csharp
// 在模块的初始化入口处创建
IEventSystem moduleEventSystem = new TypeEventSystem();
```

### 3. 注册事件与自动注销
当你在 MonoBehaviour 中监听事件时，可以利用返回值链式调用 `RemoveListenerWhenGameObjectDestroyed(this.gameObject)` 实现自动注销功能，防止内存泄漏。

```csharp
public class UIManager : MonoBehaviour
{
    private IEventSystem currentEventSystem; // 假设已经赋值

    private void Start()
    {
        // 注册无参事件并自动注销
        currentEventSystem.AddListener<GameStartEvent>(OnGameStart)
                          .RemoveListenerWhenGameObjectDestroyed(this.gameObject);
        
        // 注册带参事件并自动注销
        currentEventSystem.AddListener<PlayerScoreChangedEvent>(OnScoreChanged)
                          .RemoveListenerWhenGameObjectDestroyed(this.gameObject);
    }

    private void OnGameStart()
    {
        Debug.Log("游戏开始");
    }

    private void OnScoreChanged(PlayerScoreChangedEvent e)
    {
        Debug.Log($"玩家 {e.PlayerId} 的新分数是 {e.NewScore}");
    }
}
```

### 4. 发送事件
向当前模块的事件系统发送指定事件，只有注册在同一事件系统实例中的监听者才能收到。

```csharp
// 发送无参事件
moduleEventSystem.Send<GameStartEvent>();

// 发送带参事件
moduleEventSystem.Send<PlayerScoreChangedEvent>(new PlayerScoreChangedEvent 
{ 
    PlayerId = 1, 
    NewScore = 100 
});
```

### 5. 手动注销与清理
如果不使用自动注销组件，你也可以手动注销，或者在模块销毁时一次性清理所有监听。

```csharp
// 手动注销
moduleEventSystem.RemoveListener<GameStartEvent>(OnGameStart);

// 清理整个事件系统实例下的所有事件监听（通常在模块卸载时调用）
moduleEventSystem.Clear();
```

## 注意事项
- 由于去除了全局静态的事件中心，跨模块的通信如果也需要基于本系统，则必须保证它们能够获取到同一个 `IEventSystem` 实例（例如通过依赖注入或将实例存放在顶层的管理器中）。

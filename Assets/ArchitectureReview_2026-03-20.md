# 项目框架设计审查报告

- 审查时间: 2026-03-20 18:22:20 +08:00
- 审查范围: `Assets/Scripts` 业务代码、`JKFrame` 接入方式、`Mirror` 集成边界、场景与状态流转设计
- 审查方式: 静态代码审查

## 一、总体结论

当前项目已经具备比较明确的分层意识，核心结构可以概括为:

- 状态机: `GameFlowController + IGameState`
- 事件系统: `EventSystemManager + TypeEventSystem`
- UI 层: `BaseUI / BasePanelUI / BaseUIRequestHandler`
- 业务层: `PlayerModel / BankerSelectionService / ScoreCalculator / WinChecker`

整体方向是正确的，但目前的问题在于:

- 分层主要停留在目录结构和代码约定层面
- 关键约束没有被框架真正托住
- 生命周期、状态迁移、事件解绑、场景切换之间仍存在较强耦合
- 业务模型尚未与 Unity 场景对象解耦

因此，该项目现阶段更接近“有分层意识的业务原型”，还没有完全形成“稳定可扩展的框架化架构”。

## 二、主要审查发现

### 1. 状态机主干不闭合

`LobbyUIRequestHandler` 中启动游戏会切换到 `GameState.BankerSelection`，但 `GameFlowController` 的状态字典中没有注册 `BankerSelectionState`。

影响:

- 游戏启动流程在架构层面已经出现断裂
- 状态枚举、状态实现、状态注册之间没有统一来源
- 后续新增状态时容易继续遗漏

建议:

- 将所有状态注册集中到一个明确的 composition root
- 保证 `GameState`、状态实现类、状态字典配置三者同步维护
- 对非法状态或缺失状态在开发阶段直接失败，而不是仅打印日志

### 2. 状态迁移约束未真正生效

`GameFlowController.TransitionToState()` 中即使 `CanTransitionTo()` 返回 `false`，仍然继续执行退出和进入流程。

影响:

- 状态机规则失效
- 任意模块都可能绕过状态图推进流程
- 问题会在运行时被掩盖，增加排查成本

建议:

- 非法迁移后立即 `return`
- 或在开发阶段直接抛出异常
- 所有状态切换统一走一个入口做校验和日志审计

### 3. 全局事件系统生命周期管理不一致

项目中已经设计了 `IUnRegister` 和 `RemoveListenerWhenGameObjectDestroyed()`，说明框架层考虑过事件解绑问题。但 `LobbyUIRequestHandler` 等对象在 `Awake()` 中注册全局监听后，没有保存注销句柄，也没有在销毁时回收。

影响:

- 场景反复加载后可能产生重复监听
- 可能出现悬空委托和重复处理逻辑
- 事件总线容易成为隐性状态污染源

建议:

- 对所有全局事件监听统一使用自动回收机制
- 给 `BaseUIRequestHandler` 增加统一注册/解绑模板
- 区分“场景级事件中心”和“全局事件中心”

### 4. 状态副作用没有完整收口到状态对象

`BankerSelectionState` 中，事件注册与注销逻辑方向相反，而且 `isSelectionComplete` 没有在正式流程中被设置完成。状态推进依赖服务和事件，但状态对象本身并没有完整拥有“进入、执行、完成、退出”的闭环。

影响:

- 状态对象只是名义上的状态载体
- 真正流程散落在 UI、Service、Manager 中
- 难以维护和测试

建议:

- 每个状态对象完整拥有自己的副作用管理
- 进入时注册，退出时解绑
- 将“完成条件”和“下一状态决策”封装在状态内部

### 5. Model/Service 直接持有 MonoBehaviour，领域层与场景层耦合

`PlayerModel` 直接保存 `Player` 组件，`BankerSelectionService` 也继续围绕 `Player` 组件运转。

影响:

- 模型层天然依赖 Unity 场景生命周期
- 不利于接入 Mirror 的权威同步逻辑
- 不利于服务器逻辑、回放、AI 仿真和单元测试

建议:

- 将 `PlayerModel` 改为纯数据模型
- 将 `Player` 组件下沉为 View/Adapter 层
- 业务服务层优先操作纯数据对象，再由外层同步到场景表现

## 三、架构层面的补充观察

### 1. 自研框架层与第三方框架边界尚不清晰

项目中已引入 `JKFrame` 和 `Mirror`，但业务层仍自行实现了:

- 单例体系
- 事件系统
- UI 生命周期管理
- 场景管理
- 状态机体系

其中 `JKFrame` 在业务代码中的实际使用非常有限，当前更多只是局部工具依赖，而不是作为统一基础设施。

建议:

- 明确哪些能力由 `JKFrame` 提供，哪些由项目自研层提供
- 避免同一类基础设施重复实现
- 对 `Mirror` 的接入应尽早规划成独立边界，而不是后续再硬接入当前单机逻辑

### 2. 业务程序集尚未切分

当前核心业务代码仍编译在默认 `Assembly-CSharp` 中，没有按模块或层次建立业务 asmdef。

影响:

- 缺少编译期依赖约束
- 目录分层不能阻止跨层引用
- 后续规模扩大后编译和维护成本会上升

建议:

- 至少拆出以下程序集:
- `Mahjong.Domain`
- `Mahjong.Application`
- `Mahjong.Presentation`
- `Mahjong.Infrastructure.Unity`

说明:

- `Domain` 放纯规则、纯模型、纯算法
- `Application` 放流程编排、服务、用例
- `Presentation` 放 UI、View、面板、表现逻辑
- `Infrastructure.Unity` 放 MonoBehaviour、场景加载、Unity 适配器

### 3. 场景管理仍偏工具化，尚未进入流程架构

`GameSceneManager` 目前只是简单封装 `LoadScene/UnloadScene`，还没有被纳入明确的应用流程编排中。

建议:

- 用“流程状态”驱动场景加载，而不是由业务代码零散调用
- 明确 `ManagerScene`、`LobbyScene`、`GameScene` 的职责边界
- 统一管理跨场景单例和持久对象

## 四、建议的演进方向

### 第一阶段: 先把现有框架补稳

- 修复状态注册缺失和非法状态迁移问题
- 统一所有事件监听的生命周期管理
- 补齐状态对象的完整闭环
- 清理明显未完成或占位式的流程代码

### 第二阶段: 明确层级边界

- 把规则计算、胜负判断、计分逻辑迁移为纯 C# 领域层
- 把 `PlayerModel`、牌局模型、房间模型改造成纯数据结构
- 把 MonoBehaviour 控制器降为适配器而非核心模型

### 第三阶段: 为网络化与扩展做准备

- 将 Mirror 集成边界放在应用层或基础设施层
- 区分本地表现状态与网络同步状态
- 为回放、AI 托管、断线重连预留统一数据入口

## 五、最终评价

这个项目的优点是:

- 已经开始主动抽象状态机、事件系统和 UI 基类
- 目录结构清晰，业务意图可读性较强
- 对后续扩展方向有明显预留意识

当前主要短板是:

- 架构约束还没有真正落到编译期和运行时机制上
- 生命周期和状态流转的收敛度不够
- 领域模型仍与 Unity 表现层耦合过深

如果后续目标只是单机场景原型，当前结构可以继续推进一段时间；但如果目标包含联网、多人同步、AI 托管、回放或长期演进，那么建议尽快开始“纯领域层 + 应用编排层 + Unity 适配层”的拆分。

## 六、备注

本报告基于当前工作区静态代码生成，未结合完整运行时场景验证结果。

# TypeEventSystem 代码审查报告

## 概述

该事件系统是一个基于类型的全局事件系统，支持自动事件注销功能。本次审查发现了 7 个问题，其中 2 个严重问题、3 个中等问题和 2 个轻微问题。

---

## 🔴 严重问题

### 问题1: 反射调用移除事件存在类型不匹配bug

**位置**: `RemoveListenersForType` 方法 (第 74-92 行)

**问题描述**:
当前实现通过反射获取 `ListenerAutoRemover<T>` 的 `OnEvent` 委托，然后传递给 `RemoveListener<T>` 方法。由于委托在运行时作为 `object` 类型传递，如果事件类型 `T` 在编译时和运行时不一致，会导致 `InvalidCastException`。

**影响**: 可能导致游戏崩溃

**建议修复**:
- 使用接口或抽象基类来避免反射
- 或者添加类型安全检查

---

### 问题2: 事件委托未使用 event 关键字

**位置**: `ListenersSet<T>` 类 (第 101-103 行)

**问题描述**:
```csharp
public Action<T> OnEvent = obj => { };
```

未使用 `event` 关键字，导致外部代码可以直接调用 `listenersSet.OnEvent.Invoke(e)`，绕过了注册机制。

**影响**:
- 无法控制事件的触发顺序
- 存在意外清空所有监听器的风险（直接赋值 null）
- 破坏了封装性

**建议修复**:
```csharp
public event Action<T> OnEvent = obj => { };
```

---

## 🟡 中等问题

### 问题3: 字典使用 object 类型丢失类型安全

**位置**: `eventListenerSet` 字典 (第 106 行)

**问题描述**:
```csharp
private static readonly Dictionary<Type, object> eventListenerSet = new();
```

使用 `Dictionary<Type, object>` 导致每次访问都需要类型转换，编译时无法检查类型正确性。

**影响**:
- 更容易出现运行时错误
- 代码可维护性降低

**建议改进**:
- 使用 `Dictionary<Type, ListenersSet<object>>`
- 或使用泛型封装来保持类型安全

---

### 问题4: 反射性能问题

**位置**: `OnDestroy` (第 63-70 行) 和 `RemoveListenersForType` (第 74-92 行)

**问题描述**:
每次 GameObject 销毁时都执行反射操作：
```csharp
MethodInfo removeMethod = typeof(TypeEventSystem).GetMethod("RemoveListener");
MethodInfo genericRemoveMethod = removeMethod.MakeGenericMethod(eventType);
```

**影响**:
- 反射调用比直接调用慢 10-100 倍
- 在 `OnDestroy` 中使用反射会影响游戏性能

**建议改进**:
- 使用接口或委托缓存
- 避免在频繁调用的代码路径中使用反射

---

### 问题5: 无线程安全保护

**位置**: `TypeEventSystem` 整个类 (第 98-169 行)

**问题描述**:
多线程同时注册/注销事件可能导致数据竞争。Unity 的 Update/事件回调可能在不同线程执行。

**影响**:
- 可能导致数据竞争
- 潜在的内存访问冲突

**建议改进**:
- 添加锁机制保护字典访问
- 或确保所有调用都在主线程执行

---

## 🟢 轻微问题/优化建议

### 问题6: Send 方法的默认参数行为

**位置**: `Send<T>` 方法 (第 113-123 行)

**问题描述**:
```csharp
public static void Send<T>() where T : IEvent => Send<T>(default);
```

对于 struct 事件类型，`default` 会创建一个新实例，可能导致意外行为。对于引用类型，`default` 是 null，但日志会打印 `e.GetType().Name`，如果 e 为 null 可能有问题。

**建议改进**:
- 添加 null 检查
- 考虑为 struct 类型提供不同的重载

---

### 问题7: 缺少功能

**问题描述**:
该事件系统缺少一些常见功能：
- ❌ 无法查看某事件类型的监听者数量
- ❌ 无法一次性移除某类型的所有监听者
- ❌ 无法判断某监听者是否已注册
- ❌ 不支持事件优先级
- ❌ 没有事件触发前的过滤机制

**建议**:
根据实际需求考虑添加这些功能

---

## 总结

| 类别 | 数量 | 严重程度 |
|------|------|----------|
| 严重问题 | 2 | 🔴 |
| 中等问题 | 3 | 🟡 |
| 轻微问题 | 2 | 🟢 |

**核心风险**: 当前自动注销功能依赖反射实现，存在运行时类型转换失败的风险，可能导致游戏崩溃。

**建议优先修复**:
1. 问题1 - 反射类型匹配bug
2. 问题2 - 添加 event 关键字
3. 问题4 - 优化反射性能

using System.Collections.Generic;
using JKFrame;
using Mahjong.System.TypeEventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Mahjong.Core.UI
{
    /// <summary>
    /// UI基类，提供自动事件生命周期管理
    /// <para>继承结构：BaseInitializer → BaseUI → [具体UI类]</para>
    /// <para>基本功能：</para>
    /// <para>- 自动管理UI事件生命周期，避免内存泄漏</para>
    /// <para>- 提供UI显示/隐藏控制</para>
    /// <para>- 模版方法模式定义UI初始化流程</para>
    /// <para>用法：</para>
    /// <para>1. 继承BaseUI创建具体UI类</para>
    /// <para>2. 重写AddUIListener()方法添加UI事件监听</para>
    /// <para>3. 使用RegisterUIListener()方法注册事件，父类自动管理生命周期</para>
    /// <para>4. 重写RegisterUIUpdate()方法实现UI更新监听</para>
    /// <para>5. 调用Show()/Hide()方法控制UI显示状态</para>
    /// </summary>
    public class BaseUI : BaseInitializer
    {
        protected IEventSystem UIControlEventSystem => EventSystemManager.Instance.UIControlEventSystem;
        protected IEventSystem UIRequestEventSystem => EventSystemManager.Instance.UIRequestEventSystem;
        protected IEventSystem ModelEventSystem => EventSystemManager.Instance.ModelEventSystem;

        private readonly List<IUIEventAutoHandler> uiEventListeners = new();
        #region 生命周期
        protected override void Awake()
        {
            base.Awake();
            InitializeUI();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterUIEvents();
        }
        #endregion
        #region 初始化
        /// <summary>
        /// 模版方法，固定算法顺序
        /// </summary>
        private void InitializeUI()
        {
            AddUIListener();
            RegisterUIEvents();
            RegisterUIUpdate();
        }
        /// <summary>
        /// 子类在该方法中添加UI事件<br/>
        /// </summary>
        protected virtual void AddUIListener() { }

        /// <summary>
        /// 注册UI更新监听<br/>
        /// 子类重写此方法实现更新注册
        /// </summary>
        protected virtual void RegisterUIUpdate() { }

        /// <summary>
        /// 注册UI事件监听（无参事件）<br/>
        /// 子类调用此方法添加UI事件，父类自动管理生命周期<br/>
        /// </summary>
        /// <param name="unityEvent">UI事件源（如button.onClick）</param>
        /// <param name="callback">回调方法</param>
        protected void RegisterUIListener(UnityEvent unityEvent, UnityAction callback)
        {
            uiEventListeners.Add(new UIEventAutoHandler(unityEvent, callback));
        }

        /// <summary>
        /// 注册UI事件监听（带参数事件）<br/>
        /// 子类调用此方法添加UI事件，父类自动管理生命周期<br/>
        /// </summary>
        /// <typeparam name="T">事件参数类型（如bool）</typeparam>
        /// <param name="unityEvent">UI事件源（如toggle.onValueChanged）</param>
        /// <param name="callback">回调方法</param>
        protected void RegisterUIListener<T>(UnityEvent<T> unityEvent, UnityAction<T> callback)
        {
            uiEventListeners.Add(new UIEventAutoHandler<T>(unityEvent, callback));
        }
        private void RegisterUIEvents()
        {
            uiEventListeners.ForEach(listener => listener.Register());
        }

        private void UnregisterUIEvents()
        {
            uiEventListeners.ForEach(listener => listener.Unregister());
            uiEventListeners.Clear();
        }
        #endregion
        #region 显示控制
        /// <summary>
        /// 显示UI
        /// </summary>
        [ContextMenu("显示")]
        public virtual void Show()
        {
            if (!TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        /// <summary>
        /// 隐藏UI
        /// </summary>
        [ContextMenu("隐藏")]
        public virtual void Hide()
        {
            if (!TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        #endregion
    }
    #region IUIEventAutoHandler
    public interface IUIEventAutoHandler
    {
        void Register();
        void Unregister();
    }
    public class UIEventAutoHandler : IUIEventAutoHandler
    {
        private readonly UnityEvent unityEvent;
        private readonly UnityAction callback;
        public virtual void Register()
        {
            if (unityEvent == null || callback == null) return;
            unityEvent.AddListener(callback);
        }

        public virtual void Unregister()
        {
            if (unityEvent == null || callback == null) return;
            unityEvent.RemoveListener(callback);
        }

        // 无参构造函数
        public UIEventAutoHandler(UnityEvent evt, UnityAction action)
        {
            unityEvent = evt;
            callback = action;
        }
    }
    public class UIEventAutoHandler<T> : IUIEventAutoHandler
    {
        private readonly UnityEvent<T> unityEvent;
        private readonly UnityAction<T> callback;
        public virtual void Register()
        {
            if (unityEvent == null || callback == null) return;

            unityEvent.AddListener(callback);
        }

        public virtual void Unregister()
        {
            if (unityEvent == null || callback == null) return;

            unityEvent.RemoveListener(callback);
        }

        // 泛型构造函数
        public UIEventAutoHandler(UnityEvent<T> evt, UnityAction<T> action)
        {
            unityEvent = evt;
            callback = action;
        }
    }
    #endregion
}
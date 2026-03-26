using System.Collections.Generic;
using Mahjong.System.TypeEventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Mahjong.Core.UI
{
    /// <summary>
    /// UI基类，提供自动事件生命周期管理与显示/隐藏控制。
    /// <para>继承结构：MonoBehaviour → BaseUI → BasePanelUI → [具体UI类]</para>
    /// <para>基本功能：</para>
    /// <para>- 自动管理UnityEvent生命周期（绑定/解绑），避免内存泄漏</para>
    /// <para>- 通过CanvasGroup提供UI显示/隐藏控制</para>
    /// <para>- 模版方法模式定义UI初始化流程</para>
    /// <para>用法：</para>
    /// <para>1. 继承BaseUI创建具体UI类</para>
    /// <para>2. 重写SetupUIEvents()方法，在其中调用BindUIEvent()注册需要自动管理的UnityEvent</para>
    /// <para>3. Awake时自动执行绑定流程：SetupUIEvents() → ApplyEventBindings()</para>
    /// <para>4. OnDestroy时自动执行UnbindAllEvents()，无需手动解绑</para>
    /// <para>5. 调用Show()/Hide()方法控制UI显示状态</para>
    /// </summary>
    public class BaseUI : MonoBehaviour
    {
        protected IEventSystem UIControlEventSystem => EventSystemManager.Instance.UIControlEventSystem;
        protected IEventSystem UIRequestEventSystem => EventSystemManager.Instance.UIRequestEventSystem;
        protected IEventSystem ModelEventSystem => EventSystemManager.Instance.ModelEventSystem;


        private readonly List<IUIEventAutoHandler> uiEventListeners = new();
        private CanvasGroup canvasGroup;

        #region 生命周期

        protected virtual void Awake()
        {
            TryGetComponent(out canvasGroup);
            OnInit();
        }

        protected virtual void OnDestroy()
        {
            UnbindAllEvents();
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化入口，执行顺序：SetupUIEvents() → ApplyEventBindings()
        /// </summary>
        private void OnInit()
        {
            SetupUIEvents();
            ApplyEventBindings();
        }

        /// <summary>
        /// 子类重写：在此声明要绑定的 UI 事件。
        /// 调用 BindUIEvent() / BindUIEvent&lt;T&gt;() 注册需要自动管理生命周期的事件。
        /// </summary>
        protected virtual void SetupUIEvents() { }


        #endregion

        #region 事件绑定

        /// <summary>
        /// 注册无参 UI 事件（如 button.onClick），框架自动管理生命周期。
        /// </summary>
        protected void BindUIEvent(UnityEvent unityEvent, UnityAction callback)
        {
            uiEventListeners.Add(new UIEventAutoHandler(unityEvent, callback));
        }

        /// <summary>
        /// 注册带参 UI 事件（如 slider.onValueChanged），框架自动管理生命周期。
        /// </summary>
        protected void BindUIEvent<T>(UnityEvent<T> unityEvent, UnityAction<T> callback)
        {
            uiEventListeners.Add(new UIEventAutoHandler<T>(unityEvent, callback));
        }

        /// <summary>
        /// 批量注册所有已收集的UI事件监听器
        /// </summary>
        private void ApplyEventBindings()
        {
            uiEventListeners.ForEach(h => h.Register());
        }

        /// <summary>
        /// 批量解绑所有UI事件监听器并清空列表
        /// </summary>
        private void UnbindAllEvents()
        {
            uiEventListeners.ForEach(h => h.Unregister());
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
            EnsureCanvasGroup();
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
            EnsureCanvasGroup();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        private void EnsureCanvasGroup()
        {
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
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

        public UIEventAutoHandler(UnityEvent evt, UnityAction action)
        {
            unityEvent = evt;
            callback = action;
        }

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
    }

    public class UIEventAutoHandler<T> : IUIEventAutoHandler
    {
        private readonly UnityEvent<T> unityEvent;
        private readonly UnityAction<T> callback;

        public UIEventAutoHandler(UnityEvent<T> evt, UnityAction<T> action)
        {
            unityEvent = evt;
            callback = action;
        }

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
    }

    #endregion
}

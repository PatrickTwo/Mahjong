using System.Collections.Generic;
using Mahjong.System.TypeEventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Mahjong.Core.UI
{
    /// <summary>
    /// UI 基类，提供事件自动解绑与基础显隐能力。
    /// </summary>
    public class BaseUI : MonoBehaviour
    {
        #region 字段与属性

        protected IEventSystem UIControlEventSystem => EventBusService.Instance.UIControlEventSystem;
        protected IEventSystem ModelEventSystem => EventBusService.Instance.ModelEventSystem;

        private readonly List<IUIEventAutoHandler> uiEventListeners = new List<IUIEventAutoHandler>();
        private CanvasGroup canvasGroup;

        #endregion

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
        /// 初始化入口。
        /// </summary>
        private void OnInit()
        {
            SetupUIEvents();
            ApplyEventBindings();
        }

        /// <summary>
        /// 子类重写，声明需要绑定的 UI 事件。
        /// </summary>
        protected virtual void SetupUIEvents()
        {
        }

        #endregion

        #region 事件绑定

        /// <summary>
        /// 绑定无参 UI 事件。
        /// </summary>
        /// <param name="unityEvent">Unity 事件。</param>
        /// <param name="callback">事件回调。</param>
        protected void BindUIEvent(UnityEvent unityEvent, UnityAction callback)
        {
            uiEventListeners.Add(new UIEventAutoHandler(unityEvent, callback));
        }

        /// <summary>
        /// 绑定带参 UI 事件。
        /// </summary>
        /// <typeparam name="T">事件参数类型。</typeparam>
        /// <param name="unityEvent">Unity 事件。</param>
        /// <param name="callback">事件回调。</param>
        protected void BindUIEvent<T>(UnityEvent<T> unityEvent, UnityAction<T> callback)
        {
            uiEventListeners.Add(new UIEventAutoHandler<T>(unityEvent, callback));
        }

        /// <summary>
        /// 应用事件绑定。
        /// </summary>
        private void ApplyEventBindings()
        {
            uiEventListeners.ForEach(handler => handler.Register());
        }

        /// <summary>
        /// 解绑全部事件。
        /// </summary>
        private void UnbindAllEvents()
        {
            uiEventListeners.ForEach(handler => handler.Unregister());
            uiEventListeners.Clear();
        }

        #endregion

        #region 显示控制

        /// <summary>
        /// 显示 UI。
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
        /// 隐藏 UI。
        /// </summary>
        [ContextMenu("隐藏")]
        public virtual void Hide()
        {
            EnsureCanvasGroup();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 确保挂载 CanvasGroup。
        /// </summary>
        private void EnsureCanvasGroup()
        {
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
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
            if (unityEvent == null || callback == null)
            {
                return;
            }

            unityEvent.AddListener(callback);
        }

        public virtual void Unregister()
        {
            if (unityEvent == null || callback == null)
            {
                return;
            }

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
            if (unityEvent == null || callback == null)
            {
                return;
            }

            unityEvent.AddListener(callback);
        }

        public virtual void Unregister()
        {
            if (unityEvent == null || callback == null)
            {
                return;
            }

            unityEvent.RemoveListener(callback);
        }
    }

    #endregion
}

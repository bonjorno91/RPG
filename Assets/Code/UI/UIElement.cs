using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
    public abstract class UIElement : MonoBehaviour,IUIElement
    {
        private UIElement _rootElement;
        [SerializeField] protected List<UIElement> _childElements = new();
    
        private void Awake()
        {
            if (!transform.parent) return;
        
            if (transform.parent.TryGetComponent(out _rootElement))
            {
                _rootElement.RegisterMe(this);
            }
        }

        private void Reset()
        {
            Awake();
        }

        private void RegisterMe(UIElement uiElement)
        {
            if(!_childElements.Contains(uiElement)) _childElements.Add(uiElement);
        }

        private void UnregisterMe(UIElement uiElement)
        {
            _childElements.Remove(uiElement);
        }
    
        private void OnDestroy()
        {
            if (_rootElement != null) _rootElement.UnregisterMe(this);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            if(_rootElement) _rootElement.Show();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            if(_rootElement) _rootElement.Hide();
        }
    }
}
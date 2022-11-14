using System.Collections.Generic;
using Code.ActionsModule.Abilities;
using Code.InDev.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Code.UI
{
    public interface ITooltip
    {
        ITooltipValue AddResource(Resource resource);
        void SetName(string nameString);
        void SetCooldown(string text);
        void SetDescription(string description);
    }

    public interface ITooltipHandle : ITooltip
    {
        void Hide();
        void Show();
    }

    public class Tooltip : MonoBehaviour, ITooltipHandle 
    {
        [SerializeField] private Image _panelImage;
        [SerializeField] private RectTransform _values;
        [SerializeField] private TooltipValue _valuePrefab;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;

        [Header("Default Resources")] [SerializeField]
        private Sprite _cooldownIcon;

        private readonly List<TooltipValue> _activeValues = new();
        private TooltipValue _cooldown;
        private ObjectPool<TooltipValue> _valuesPool;

        private void Awake()
        {
            _valuesPool = new ObjectPool<TooltipValue>(OnCreateValue, OnActivateValue, OnDeactivateValue, OnDestroyValue);
        }

        private void OnEnable()
        {
            if (_activeValues?.Count > 0)
                foreach (var value in _activeValues)
                    value.enabled = true;
            
            _panelImage.enabled = true;
        }

        private void OnDisable()
        {
            if (_activeValues?.Count > 0)
                foreach (var value in _activeValues)
                    value.enabled = false;

            _panelImage.enabled = false;
        }

        public void SetName(string nameString)
        {
            _name.text = nameString;
        }

        public ITooltipValue AddResource(Resource resource)
        {
            var value = _valuesPool.Get();
            _activeValues.Add(value);
            value.SetResource(resource);

            return value;
        }

        public void SetCooldown(string text)
        {
            if (!_cooldown)
            {
                _valuesPool.Get(out _cooldown);
                _cooldown.SetIcon(_cooldownIcon);
            }

            _cooldown.SetValue(text);
        }

        public void SetDescription(string description)
        {
            _description.text = description;
        }

        private TooltipValue OnCreateValue()
        {
            return Instantiate(_valuePrefab, _values);
        }

        private void OnActivateValue(TooltipValue value)
        {
            value.enabled = false;
            value.gameObject.SetActive(true);
        }

        private void OnDeactivateValue(TooltipValue value)
        {
            value.Clear();
            _activeValues.Remove(value);
            value.gameObject.SetActive(false);
        }

        private void OnDestroyValue(TooltipValue value)
        {
            Destroy(value);
            _activeValues.Remove(value);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ClearValues();
        }

        private void ClearValues()
        {
            if (_cooldown)
            {
                _valuesPool.Release(_cooldown);
                _cooldown = null;
            }
            
            if (_activeValues?.Count > 0)
            {
                foreach (var value in _activeValues.ToArray())
                {
                    _valuesPool.Release(value);
                }
                
                _activeValues.Clear();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
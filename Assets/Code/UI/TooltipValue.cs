using Code.ActionsModule.Abilities;
using Code.InDev.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public interface ITooltipValue
    {
        void SetValue(string value);
        void SetIcon(Sprite icon);
    }

    public sealed class TooltipValue : MonoBehaviour, ITooltipValue
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _valueText;
        private string _defaultDescription;
        private Sprite _defaultIcon;

        private void Awake()
        {
            _defaultIcon = _iconImage.sprite;
            _defaultDescription = _valueText.text;
        }

        private void OnEnable()
        {
            _iconImage.enabled = true;
            _valueText.enabled = true;
        }

        private void OnDisable()
        {
            _iconImage.enabled = false;
            _valueText.enabled = false;
        }

        public void SetValue(string value)
        {
            _valueText.text = value;
        }

        public void SetIcon(Sprite icon)
        {
            _iconImage.sprite = icon;
        }

        public void Clear()
        {
            _iconImage.sprite = _defaultIcon;
            _valueText.text = _defaultDescription;
        }

        public void SetResource(Resource resource)
        {
            _iconImage.sprite = resource.ResourceType.Icon;
            _valueText.text = resource.Value.ToString();
        }
    }
}
using System;
using Code.ActionsModule.Abilities;
using Code.InDev.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI
{
    public class Button : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler, IButton
    {
        [SerializeField] private Cooldown _cooldownImage;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private TMP_Text _hotkey;
        private IButtonsPanel _actionPanel;
        private Action<IButtonsPanel> _leftClickCommand;
        private Action<IButtonsPanel> _rightClickCommand;
        private ITooltipHandle _tooltip;
        private ITooltipDrawer _tooltipDrawer;

        public void Clear()
        {
            _icon.sprite = null;
            _cost.text = string.Empty;
            _hotkey.text = string.Empty;
            _cost.gameObject.SetActive(false);
            _hotkey.gameObject.SetActive(false);
        }

        public void SetPanel(IButtonsPanel actionPanel)
        {
            _actionPanel = actionPanel;
        }

        public void SetTooltip(ITooltipHandle tooltip)
        {
            _tooltip = tooltip;
        }

        public void SetCost(int value)
        {
            if (value > 0)
            {
                _cost.text = value.ToString();
                if (!_cost.gameObject.activeSelf) _cost.gameObject.SetActive(true);
            }
            else
            {
                if (_cost.gameObject.activeSelf) _cost.gameObject.SetActive(false);
            }
        }

        public void SetHotkey(KeyCode key)
        {
            _hotkey.text = key.ToString();
            if (!_hotkey.gameObject.activeSelf) _hotkey.gameObject.SetActive(true);
        }

        public void SetCooldown(float progress) => _cooldownImage.SetProgress(progress);
        public void SetLeftClickCommand(Action<IButtonsPanel> buttonCommand) => _leftClickCommand = buttonCommand;

        public void SetRightClickCommand(Action<IButtonsPanel> buttonCommand) => _rightClickCommand = buttonCommand;

        public void SetTooltipDrawer(ITooltipDrawer tooltipDrawer)
        {
            _tooltipDrawer = tooltipDrawer;
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
            if(!_icon.gameObject.activeSelf) _icon.gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _leftClickCommand?.Invoke(_actionPanel);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _rightClickCommand?.Invoke(_actionPanel);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltipDrawer?.DrawTooltip(_tooltip);
            _tooltip?.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltip?.Hide();
        }
    }
}
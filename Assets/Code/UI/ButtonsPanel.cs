using System;
using System.Collections.Generic;
using Code.ActionsModule.Abilities;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Code.UI
{
    public interface IButtonsPanel
    {
        IButton GetButton(Sprite icon);
        void HideAllButtons();
        void ShowAllButtons();
        void BeginTargeting(TargetingType targetingType);
    }

    public enum TargetingType
    {
        Single,
        AoE
    }

    public interface IInitializable
    {
        void Initialize();
    }
    
    public class ButtonsPanel : MonoBehaviour, IButtonsPanel, IInitializable
    {
        [SerializeField] private GameObject _areaTargetingEffect;
        [SerializeField] private Texture2D _singleTargetingCursor;
        [SerializeField] private Button _button;
        [SerializeField] private Tooltip _tooltip;
        private ObjectPool<Button> _buttonsPool;
        private readonly List<Button> _activeButtons = new();
        private bool _isHideButtons;
        private bool _isTargeting;
        private Camera _camera;
        private bool _single;
        private Transform _buttonsPoolRect;

        public void Initialize()
        {
            _areaTargetingEffect = Instantiate(_areaTargetingEffect);
            _areaTargetingEffect.SetActive(false);
            _tooltip = Instantiate(_tooltip,transform);
            _tooltip.gameObject.SetActive(false);
            _buttonsPool = new ObjectPool<Button>(OnCreateButton, OnActivateButton, OnDeactivateButton, OnDestroyButton);
            _buttonsPoolRect = new GameObject("[ButtonsPool]").transform;
            _buttonsPoolRect.SetParent(transform);
            _buttonsPoolRect.gameObject.SetActive(false);
        }

        public IButton GetButton(Sprite icon)
        {
            var button = _buttonsPool.Get();
            button.SetIcon(icon);
            button.SetPanel(this);
            button.SetTooltip(_tooltip);
            _activeButtons.Add(button);
            return button;
        }

        public void BeginTargeting(TargetingType targetingType)
        {
            if (targetingType == TargetingType.AoE)
            {
                _isTargeting = true;
                Cursor.visible = false;
                _camera = Camera.main;
                _areaTargetingEffect.SetActive(true);
            }
            else
            {
                Cursor.SetCursor(_singleTargetingCursor, Vector2.zero, CursorMode.Auto);
                _single = true;
            }
        }

        private void Update()
        {
            if (_isTargeting)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                var layerMask = LayerMask.GetMask("Terrain");
                if (Physics.Raycast(ray, out var hit, _camera.farClipPlane + 500, layerMask))
                {
                    _areaTargetingEffect.transform.position = hit.point;
                }

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    _areaTargetingEffect.SetActive(false);
                    _isTargeting = false;
                    Cursor.visible = true;
                }
            }
            else if (_single)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    _single = false;
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        public void HideAllButtons()
        {
            if (_isHideButtons) return;

            _isHideButtons = !_isHideButtons;
            
            if (_activeButtons.Count > 0)
            {
                foreach (var button in _activeButtons)
                {
                    if (button.gameObject.activeSelf) button.gameObject.SetActive(false);
                }
            }
        }

        public void ClearPanel()
        {
            if (_activeButtons.Count > 0)
            {
                foreach (var button in _activeButtons)
                {
                    if (button.gameObject.activeSelf) _buttonsPool.Release(button);
                }
            }
        }

        public void ShowAllButtons()
        {
            if (!_isHideButtons) return;

            _isHideButtons = !_isHideButtons;

            if (_activeButtons.Count > 0)
            {
                foreach (var button in _activeButtons)
                {
                    if (!button.isActiveAndEnabled) button.gameObject.SetActive(true);
                }
            }
        }

        private Button OnCreateButton() => _button ? Instantiate(_button, transform) : default;

        private void OnActivateButton(Button button)
        {
            button.transform.SetParent(transform);
            button.gameObject.SetActive(true);
        }

        private void OnDeactivateButton(Button button)
        {
            button.Clear();
            button.transform.SetParent(_buttonsPoolRect);
            button.gameObject.SetActive(false);
        }

        private void OnDestroyButton(Button button) => Destroy(button);
    }
}
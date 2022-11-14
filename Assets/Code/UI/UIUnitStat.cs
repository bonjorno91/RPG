using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class UIUnitStat : MonoBehaviour, IUIStatDrawerHandle
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _value;

        public void SetVisibility(bool flag) => gameObject.SetActive(flag);
        public void SetIcon(Sprite icon) => _icon.sprite = icon;
        public void SetName(string statName) => _name.text = statName;
        public void SetValue(int value) => _value.text = value.ToString();
    }
}
using UnityEngine;

namespace Code.UI
{
    public interface IUIStatDrawerHandle
    {
        void SetIcon(Sprite icon);
        void SetName(string statName);
        void SetValue(int value);
        void SetVisibility(bool flag);
    }
}
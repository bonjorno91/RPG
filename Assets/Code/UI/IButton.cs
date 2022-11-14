using System;
using Code.InDev.Abilities;
using Code.UI;
using UnityEngine;

namespace Code.ActionsModule.Abilities
{
    public interface IButton
    {
        void SetIcon(Sprite icon);
        void SetHotkey(KeyCode key);
        void SetCost(int value);
        void SetCooldown(float progress);
        void SetLeftClickCommand(Action<IButtonsPanel> buttonCommand);
        void SetRightClickCommand(Action<IButtonsPanel> buttonCommand);
        void SetTooltipDrawer(ITooltipDrawer tooltipDrawer);
    }
}
using Code.ActionsModule.Abilities;
using Code.InDev.Abilities;
using UnityEngine;

namespace Code.UI
{
    public interface IStatsPanel
    {
        IUIStatDrawerHandle GetStatDrawer(Sprite icon, string statName, int value);
        IUIStatDrawerHandle GetStatDrawer(Stat stat);
    }
}
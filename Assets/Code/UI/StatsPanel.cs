using System.Collections.Generic;
using Code.ActionsModule.Abilities;
using Code.InDev.Abilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.UI
{
    public class StatsPanel : MonoBehaviour, IStatsPanel, IInitializable
    {
        [SerializeField] private UIUnitStat _stat;
        private readonly List<UIUnitStat> _visibleStats = new();
        private ObjectPool<UIUnitStat> _statsPool;
        private Transform _statsPoolRect;

        public void Initialize()
        {
            _statsPool = new ObjectPool<UIUnitStat>(OnCreateStat, OnActivateStat, OnDeactivateStat, OnDestroyStat);
            _statsPoolRect = new GameObject("[StatsPool]").transform;
            _statsPoolRect.SetParent(transform);
            _statsPoolRect.gameObject.SetActive(false);
        }
        
        public IUIStatDrawerHandle GetStatDrawer(Sprite icon, string statName, int value)
        {
            var statDrawer = _statsPool.Get();
            if(icon) statDrawer.SetIcon(icon);
            if(!string.IsNullOrEmpty(statName)) statDrawer.SetName(statName);
            statDrawer.SetValue(value);
            statDrawer.SetVisibility(true);
            _visibleStats.Add(statDrawer);
            if(!gameObject.activeSelf) gameObject.SetActive(true);
            
            return statDrawer;
        }

        public IUIStatDrawerHandle GetStatDrawer(Stat stat)
        {
            return GetStatDrawer(stat.StatType.Icon, stat.StatType.Name, stat.Value);
        }

        public void ClearPanel()
        {
            if (_visibleStats.Count > 0)
            {
                foreach (var stat in _visibleStats)
                {
                    _statsPool.Release(stat);
                }

                _visibleStats.Clear();
            }

            gameObject.SetActive(false);
        }

        private UIUnitStat OnCreateStat()
        {
            var instance = Instantiate(_stat,transform);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void OnActivateStat(UIUnitStat stat)
        {
            stat.transform.SetParent(transform);
            stat.gameObject.SetActive(false);
        }

        private void OnDeactivateStat(UIUnitStat stat)
        {
            stat.gameObject.SetActive(false);
            stat.transform.SetParent(_statsPoolRect);
        }

        private void OnDestroyStat(UIUnitStat obj) => Destroy(obj);
    }
}
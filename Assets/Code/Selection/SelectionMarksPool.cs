using System.Collections.Generic;
using Code.Behaviours;
using Code.Installers;
using Code.PlayerModule;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Selection
{
    public class SelectionMarksPool
    {
        private readonly Dictionary<Relations, IObjectPool<SelectionCircle>> _markPools = new();
        private readonly Dictionary<int, Relations> _activeSelections = new();
        private readonly SelectionMarkConfig _config;
        private readonly Transform _root;

        public SelectionMarksPool(SelectionMarkConfig config)
        {
            _config = config;
            _root = new GameObject("[Selection HUD Root]").transform;
            _markPools[Relations.Ally] =
                new ObjectPool<SelectionCircle>(OnCreateAlly, OnActivate, OnDeactivate, OnDestroy);
            _markPools[Relations.Enemy] =
                new ObjectPool<SelectionCircle>(OnCreateEnemy, OnActivate, OnDeactivate, OnDestroy);
            _markPools[Relations.Neutral] =
                new ObjectPool<SelectionCircle>(OnCreateNeutral, OnActivate, OnDeactivate, OnDestroy);
            _markPools[Relations.Unknown] =
                new ObjectPool<SelectionCircle>(OnCreateUnknown, OnActivate, OnDeactivate, OnDestroy);
        }

        private void OnDeactivate(SelectionCircle selectionCircle)
        {
            selectionCircle.transform.parent = _root;
            selectionCircle.gameObject.SetActive(false);
        }

        private void OnActivate(SelectionCircle selectionCircle)
        {
            selectionCircle.gameObject.SetActive(true);
        }

        private SelectionCircle OnCreateUnknown()
        {
            var instance = Object.Instantiate(_config.Marks[Relations.Unknown]);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private SelectionCircle OnCreateNeutral()
        {
            var instance = Object.Instantiate(_config.Marks[Relations.Neutral]);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private SelectionCircle OnCreateEnemy()
        {
            var instance = Object.Instantiate(_config.Marks[Relations.Enemy]);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private SelectionCircle OnCreateAlly()
        {
            var instance = Object.Instantiate(_config.Marks[Relations.Ally]);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void OnDestroy(SelectionCircle selectionCircle)
        {
            Object.Destroy(selectionCircle);
        }

        public SelectionCircle Get(Relations relation)
        {
            var instance = _markPools[relation].Get();

            if (instance)
            {
                _activeSelections[instance.GetInstanceID()] = relation;
                return instance;
            }

            return default;
        }

        public void Release(SelectionCircle selectionCircle)
        {
            var id = selectionCircle.GetInstanceID();
            if (_activeSelections.Remove(selectionCircle.GetInstanceID(), out var relations))
            {
                _markPools[relations].Release(selectionCircle);
                return;
            }

            Object.Destroy(selectionCircle);
        }
    }
}
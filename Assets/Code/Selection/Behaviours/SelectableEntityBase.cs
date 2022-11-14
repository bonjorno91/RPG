using System;
using System.Collections.Generic;
using Code.Abstract;
using Code.Attributes;
using Code.EntityModule;
using Code.PlayerModule;
using Code.Selection.Abstract;
using UnityEngine;

namespace Code.Selection.Behaviours
{
    [ExecuteAlways][RequireComponent(typeof(Entity))]
    public abstract class SelectableEntityBase : MonoBehaviour, ISelectableEntity
    {
        public static IReadOnlyCollection<SelectableEntityBase> SelectableObjects => SelectableList;
        private static readonly List<SelectableEntityBase> SelectableList = new();

        [Header("Drag Selection Box")] [SerializeField] [InspectorLabel("Center")]
        private Vector3 _dragCenter;

        [SerializeField] [InspectorLabel("Size")]
        private Vector3 _dragSize;

        [Header("Click Selection Box")] [SerializeField] [InspectorLabel("Center")]
        private Vector3 _clickCenter;

        [SerializeField] [InspectorLabel("Size")]
        private Vector3 _clickSize;

        private static readonly Color DragBoxColor = Color.cyan;
        private static readonly Color ClickBoxColor = Color.yellow;

        [field: SerializeField] public Transform HighlightOrigin { get; private set; }
        [field: SerializeField] public IEntity Entity { get; private set; }
        public Bounds DragSelectionBox => new(gameObject.transform.position + _dragCenter, _dragSize);

        public Bounds ClickSelectionBox => new(gameObject.transform.position + _clickCenter, _clickSize);

        protected virtual void Awake()
        {
            if (Entity == null)
            {
                Entity = gameObject.GetComponent<Entity>();
            }
        }

        private void Reset()
        {
            HighlightOrigin = gameObject.transform;
            Entity = gameObject.GetComponent<Entity>();
            if (gameObject.TryGetComponent<Collider>(out var colliderComponent))
                Setup(colliderComponent.bounds);
            else if (gameObject.TryGetComponent<Renderer>(out var rendererComponent)) 
                Setup(rendererComponent.bounds);
        }

        private void OnBecameInvisible() => enabled = false;

        private void OnBecameVisible() => enabled = true;

        private void OnEnable() => SelectableList.Add(this);

        private void OnDisable() => SelectableList.Remove(this);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = DragBoxColor;
            Gizmos.DrawWireCube(DragSelectionBox.center, DragSelectionBox.size);
            Gizmos.color = ClickBoxColor;
            Gizmos.DrawWireCube(ClickSelectionBox.center, ClickSelectionBox.size);
        }
        
        private void Setup(Bounds bounds)
        {
            _clickCenter = bounds.center - gameObject.transform.position;
            _clickSize = bounds.size;
            _dragCenter = _clickCenter;
            _dragSize = _clickSize * 0.75f;
        }

        public abstract void VisitSelection(ISelectionVisitor selectionVisitor);

        public abstract void HandleCommand(ICommand command);
    }
}
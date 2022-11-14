using System.Collections.Generic;
using System.Linq;
using Code.Selection.Abstract;
using Code.Selection.Behaviours;
using Code.Selection.Commands;
using Code.Selection.Events;
using Code.Services.CommandBus;
using Code.Services.EventService;
using UnityEngine;

namespace Code.Selection
{
    /// <summary>
    /// Search game entities on scene in selection box. Invoke IOnDragSelectionCleared
    /// </summary>
    public class SelectionDragHandler : 
        IEventHandler<IOnDragSelectionBegin>,
        IEventHandler<IOnDragSelectionHandled>,
        ICommandHandler<IClearBoxSelection>,
        ISelectionDragHandler
    {
        private class BoxSelectionEvent : IOnDragSelectionEnter, IOnDragSelectionExit
        {
            public ISelectableEntity Selectable { get; set; }
        }
        
        private class OnDragSelectionCleared : IOnDragSelectionCleared
        {
            
        }
        
        private const Camera.MonoOrStereoscopicEye CameraEyeType = Camera.MonoOrStereoscopicEye.Mono;
        private const float DragThreshold = 2;
        private const int CornersAmount = 4;
        private const int PlanesAmount = 6;

        private readonly Camera _camera;
        private readonly BoxSelectionEvent _event;
        private readonly IEventInvoker _eventInvoker;
        private readonly HashSet<ISelectableEntity> _boxSelection;
        private readonly IOnDragSelectionCleared _onDragSelectionCleared;
        private Vector3 _startScreenPointCached;
        private float NearClipPlane => _camera.nearClipPlane;
        private float FarClipPlane => _camera.farClipPlane;

        public SelectionDragHandler(Camera camera, IEventInvoker eventInvoker)
        {
            _eventInvoker = eventInvoker;
            _camera = camera;
            _event = new BoxSelectionEvent();
            _boxSelection = new HashSet<ISelectableEntity>();
            _onDragSelectionCleared = new OnDragSelectionCleared();
        }

        public void Handle(IOnDragSelectionBegin triggeredEvent)
        {
            _startScreenPointCached = triggeredEvent.PointerPosition;
        }

        public void Handle(IOnDragSelectionHandled triggeredEvent)
        {
            var difference = (triggeredEvent.PointerPosition - _startScreenPointCached);
            
            if (InThreshold(difference))
            {
                ClearAll();
                return;
            }

            var frustumPlanes = GetFrustumPlanes(triggeredEvent.PointerPosition);
            HandleSelections(frustumPlanes, SelectableEntityBase.SelectableObjects);
        }
        

        private bool InThreshold(Vector3 difference)
        {
            return Mathf.Abs(difference.x) <= DragThreshold || Mathf.Abs(difference.y) <= DragThreshold;
        }

        public bool Handle(IClearBoxSelection command)
        {
            command?.Execute(this);
            return true;
        }

        public void ClearAll()
        {
            foreach (var selectable in _boxSelection.ToArray())
            {
                Remove(selectable);
            }
            
            _boxSelection.Clear();
            _eventInvoker.Invoke(_onDragSelectionCleared);
        }

        public void OnUnbind()
        {
            Debug.Log($"Disable {nameof(SelectionDragHandler)}.");
        }

        private void HandleSelections(Plane[] frustumPlanes, IEnumerable<ISelectableEntity> selectables)
        {
            foreach (var selectable in selectables)
            {
                if (GeometryUtility.TestPlanesAABB(frustumPlanes, selectable.DragSelectionBox))
                {
                    Add(selectable);
                }
                else if (_boxSelection.Contains(selectable))
                {
                    Remove(selectable);
                }
            }
        }

        private void Add(ISelectableEntity selectableEntity)
        {
            if (_boxSelection.Add(selectableEntity))
            {
                _event.Selectable = selectableEntity;
                _eventInvoker.Invoke<IOnDragSelectionEnter>(_event);
            }
        }

        private void Remove(ISelectableEntity selectableEntity)
        {
            if (_boxSelection.Remove(selectableEntity))
            {
                _event.Selectable = selectableEntity;
                _eventInvoker.Invoke<IOnDragSelectionExit>(_event);
            }
        }

        private Plane[] GetFrustumPlanes(Vector3 currentScreenPoint)
        {
            var startScreenPoint = _startScreenPointCached;

            AdjustMinMaxScreenRectPoints(ref startScreenPoint, ref currentScreenPoint);

            var startViewportPoint = _camera.ScreenToViewportPoint(startScreenPoint);
            var currentViewportPoint = _camera.ScreenToViewportPoint(currentScreenPoint);

            var size = currentViewportPoint - startViewportPoint;
            var viewportRect = new Rect(startViewportPoint, size);

            var nearCorners = GetFrustumCorners(viewportRect, NearClipPlane);
            var farCorners = GetFrustumCorners(viewportRect, FarClipPlane);

            return GetFrustumPlanesFromCorners(nearCorners, farCorners);
        }

        private Vector3[] GetFrustumCorners(Rect viewportRect, float clipPlaneDistance)
        {
            var corners = new Vector3[CornersAmount];
            _camera.CalculateFrustumCorners(viewportRect, clipPlaneDistance, CameraEyeType, corners);
            ConvertFrustumCornersToWorldSpace(ref corners);

            return corners;
        }

        private void AdjustMinMaxScreenRectPoints(ref Vector3 startScreenPoint, ref Vector3 currentScreenPoint)
        {
            if (startScreenPoint.x > currentScreenPoint.x)
                (startScreenPoint.x, currentScreenPoint.x) = (currentScreenPoint.x, startScreenPoint.x);

            if (startScreenPoint.y > currentScreenPoint.y)
                (startScreenPoint.y, currentScreenPoint.y) = (currentScreenPoint.y, startScreenPoint.y);
        }

        private void ConvertFrustumCornersToWorldSpace(ref Vector3[] corners)
        {
            for (var i = 0; i < corners.Length; i++)
            {
                corners[i].z = -corners[i].z;
                corners[i] = _camera.cameraToWorldMatrix * corners[i];
                corners[i] += _camera.transform.position;
            }
        }

        private Plane[] GetFrustumPlanesFromCorners(Vector3[] nearCorners, Vector3[] farCorners)
        {
            var frustumPlanes = new Plane[PlanesAmount];

            // [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far
            frustumPlanes[0] = new Plane(nearCorners[0], farCorners[1], farCorners[0]);
            frustumPlanes[1] = new Plane(farCorners[3], nearCorners[2], nearCorners[3]);
            frustumPlanes[2] = new Plane(nearCorners[0], farCorners[3], nearCorners[3]);
            frustumPlanes[3] = new Plane(farCorners[1], nearCorners[2], farCorners[2]);
            frustumPlanes[4] = new Plane(nearCorners[3], nearCorners[1], nearCorners[0]);
            frustumPlanes[5] = new Plane(farCorners[0], farCorners[2], farCorners[3]);

            return frustumPlanes;
        }
    }
}
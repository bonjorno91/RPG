using Code.Services.QueryBus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Queries
{
    public class ComponentFromSceneQueryHandler<TResult> : IQueryHandler<IComponentFromSceneQuery<TResult>,TResult>
    {
        private readonly Camera _camera;

        public ComponentFromSceneQueryHandler(Camera camera) => _camera = camera;

        public TResult Handle(IComponentFromSceneQuery<TResult> query)
        {
            // Brake when pointer above UI.
            if (EventSystem.current.IsPointerOverGameObject()) return default;
            
            var ray = _camera.ScreenPointToRay(query.ScreenPoint);

            if (Physics.Raycast(ray, out var raycastHit))
            {
                if (raycastHit.transform.TryGetComponent<TResult>(out var result))
                {
                    return result;
                }
            }

            return default;
        }
    }
}
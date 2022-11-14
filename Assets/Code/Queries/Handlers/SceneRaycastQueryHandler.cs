using Code.Services.QueryBus;
using UnityEngine;

namespace Code.Queries
{
    public class SceneRaycastQueryHandler : IQueryHandler<ISceneRaycastQuery,RaycastHit>
    {
        private readonly Camera _camera;

        public SceneRaycastQueryHandler(Camera camera) => _camera = camera;

        public RaycastHit Handle(ISceneRaycastQuery query)
        {
            var ray = _camera.ScreenPointToRay(query.ScreenPoint);
            Physics.Raycast(ray, out var result);
            return result;
        }
    }
}
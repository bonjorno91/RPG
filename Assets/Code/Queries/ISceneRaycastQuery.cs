using Code.Services.QueryBus;
using UnityEngine;

namespace Code.Queries
{
    public interface ISceneRaycastQuery : IQuery<RaycastHit>
    {
        Vector3 ScreenPoint { get; }
    }
}
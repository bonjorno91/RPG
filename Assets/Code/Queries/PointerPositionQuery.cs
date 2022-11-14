using UnityEngine;

namespace Code.Queries
{
    public class PointerPositionQuery : ISceneRaycastQuery
    {
        public Vector3 ScreenPoint => Input.mousePosition;
    }




}
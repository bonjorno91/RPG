using Code.Services.QueryBus;
using UnityEngine;

namespace Code.Queries
{
    public interface IComponentFromSceneQuery<TResultComponent> : IQuery<TResultComponent>
    {
        Vector3 ScreenPoint { get; }
    }

    public class ComponentFromSceneQuery<TResultComponent> : IComponentFromSceneQuery<TResultComponent>
    {
        public Vector3 ScreenPoint { get; set; }
        
    }
}
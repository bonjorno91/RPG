using UnityEngine;
using Zenject;

namespace Code.Services
{
    public interface IPointerService
    {
        Vector3 Position { get; }
    }

    public sealed class PointerService : ITickable, IPointerService
    {
        public Vector3 Position { get; private set; }

        public void Tick() => Position = UnityEngine.Input.mousePosition;
    }
}
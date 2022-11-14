using UnityEngine;

namespace Code.Extras
{
    public static class PhysicsExt
    {
        public static bool RaycastGet<T>(Ray ray, out T result)
        {
            result = default;
            
            return Physics.Raycast(ray, out var hit) && hit.transform.TryGetComponent(out result);
        }

        public static bool RaycastGetObscured<T>(Ray ray, out T result)
        {
            result = default;
            var hits = new RaycastHit[10];
            var count = Physics.RaycastNonAlloc(ray, hits);

            return count > 0 && hits.TryGetComponent(out result);
        }
    }
}
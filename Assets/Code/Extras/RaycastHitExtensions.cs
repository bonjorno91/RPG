using System.Collections.Generic;
using UnityEngine;

namespace Code.Extras
{
    public static class RaycastHitExtensions
    {
        public static bool TryGetComponent<T>(this IEnumerable<RaycastHit> hits, out T result)
        {
            result = default;
            
            foreach (var raycastHit in hits)
            {
                if (raycastHit.collider)
                {
                    if (raycastHit.transform.TryGetComponent(out result))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
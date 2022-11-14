using UnityEngine;

namespace Code.Highlight
{
    public interface IHighlightable
    {
        Transform HighlightOrigin { get; }
    }
}
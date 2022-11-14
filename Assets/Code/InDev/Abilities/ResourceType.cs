using UnityEngine;

namespace Code.InDev.Abilities
{
    [CreateAssetMenu(menuName = "Game/New Resource Type", fileName = "ResourceType", order = 0)]
    public sealed class ResourceType : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}
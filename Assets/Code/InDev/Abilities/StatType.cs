using UnityEngine;

namespace Code.InDev.Abilities
{
    [CreateAssetMenu(menuName = "Game/Create Stat Type", fileName = "StatType", order = 0)]
    public sealed class StatType : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using Code.Behaviours;
using Code.PlayerModule;
using UnityEngine;

namespace Code.Installers
{
    [CreateAssetMenu(menuName = "Create SelectionMarkConfig", fileName = "SelectionMarkConfig", order = 0)]
    public class SelectionMarkConfig : ScriptableObject
    {
        public readonly Dictionary<Relations, SelectionCircle> Marks = new();
        [field: SerializeField] public SelectionCircle SelectionCircleMarkRed { get; private set; }
        [field: SerializeField] public SelectionCircle SelectionCircleMarkYellow { get; private set; }
        [field: SerializeField] public SelectionCircle SelectionCircleMarkGreen { get; private set; }
        [field: SerializeField] public SelectionCircle SelectionCircleMarkWhite { get; private set; }

        private void Awake()
        {
            Marks[Relations.Ally] = SelectionCircleMarkGreen;
            Marks[Relations.Enemy] = SelectionCircleMarkRed;
            Marks[Relations.Neutral] = SelectionCircleMarkYellow;
            Marks[Relations.Unknown] = SelectionCircleMarkWhite;
        }
    }
}
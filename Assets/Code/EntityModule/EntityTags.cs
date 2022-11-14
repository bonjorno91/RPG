using System;
using UnityEngine;

namespace Code.EntityModule
{
    [Flags]
    public enum EntityTags : int
    {
        [InspectorName("Is Fly")]
        Air = 1 << 1,
        [InspectorName("Is Grounded")]
        Ground = 1 << 2,
        [InspectorName("Is Alive")]
        Alive = 1 << 3,
        [InspectorName("Is Dead")]
        Dead = 1 << 4,
        [InspectorName("Is Unit")]
        Unit = 1 << 5,
        [InspectorName("Is Item")]
        Item = 1 << 6,
        [InspectorName("Is Building")]
        Building = 1 << 7,
        [InspectorName("Is Hero")]
        Hero = 1 << 8,
        [InspectorName("Is Enemy")]
        Enemy = 1 << 9,
        [InspectorName("Is Ally")]
        Ally = 1 << 10,
        [InspectorName("Is Neutral")]
        Neutral = 1 << 11,
        [InspectorName("Is Destructible")]
        Destructible = 1 << 12
    }
}
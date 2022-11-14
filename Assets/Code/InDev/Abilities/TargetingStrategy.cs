using System;
using Code.PlayerModule;
using Code.UI;
using UnityEngine;

namespace Code.InDev.Abilities
{

    public class TargetingStrategy : ButtonCommand,IPlayerInputStrategy
    {
        [field: SerializeField] public GameObject AreaVisualizer { get; private set; }
        [field: SerializeField] public Func<bool> Validator { get; set; }
        
        public override void Execute(IButtonsPanel buttonsPanel)
        {
            Debug.Log("TARGETING");
        }

        public void Handle(IPlayer player)
        {
            
        }

        public void Cancel()
        {
            
        }
    }
}
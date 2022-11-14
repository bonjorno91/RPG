using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public interface IInfoPanel
    {
        string EntityName { set; }
    }

    public class InfoPanel : MonoBehaviour, IInfoPanel, IInitializable
    {
        [SerializeField] private TMP_Text _name;

        public void Initialize()
        {
        }
        
        public string EntityName
        {
            set => _name.text = value;
        }
    }
}
using System;
using Code.Selection.Abstract;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Code.Behaviours
{
    [RequireComponent(typeof(DecalProjector))]
    public class SelectionCircle : MonoBehaviour
    {
        private DecalProjector _projector;

        private void Awake()
        {
            if (!_projector)
            {
                _projector = GetComponent<DecalProjector>();
            }
        }

        public void Select(Transform selectionTransform)
        {
            if (selectionTransform)
            {
                _projector.fadeFactor = 1;
                gameObject.transform.parent = selectionTransform;
                gameObject.transform.position = selectionTransform.position;
                gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(true);
            }
        }
        
        public void Highlight(Transform selectionTransform)
        {
            if (selectionTransform)
            {
                _projector.fadeFactor = 0.33f;
                gameObject.transform.parent = selectionTransform;
                gameObject.transform.position = selectionTransform.position;
                gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(true);
            }
        }
    }
}
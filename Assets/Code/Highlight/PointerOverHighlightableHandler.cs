using UnityEngine;

namespace Code.Highlight
{
    public class PointerOverHighlightableHandler : PointerOverEventHandler<IHighlightable>
    {
        private readonly GameObject _highlightEffect;
        private IHighlightable _highlightable;
        
        public PointerOverHighlightableHandler(GameObject highlightEffect)
        {
            _highlightEffect = highlightEffect;
            _highlightEffect.SetActive(false);
            _highlightEffect.transform.parent = null;
            _highlightEffect.transform.position = Vector3.zero;
        }

        public override void Handle(PointerOverEvent<IHighlightable> triggeredEvent)
        {
            if (triggeredEvent.Hovered == null)
            {
                if (_highlightable != null)
                {
                    DisableEffect();
                }
            }
            else
            {
                if (triggeredEvent.Hovered != _highlightable)
                {
                    EnableEffect(triggeredEvent.Hovered);
                }
            }
        }

        private void EnableEffect(IHighlightable highlightable)
        {
            _highlightable = highlightable;
            _highlightEffect.transform.parent = highlightable.HighlightOrigin;
            _highlightEffect.transform.localPosition = Vector3.zero;
            _highlightEffect.SetActive(true);
        }

        private void DisableEffect()
        {
            _highlightable = null;
            _highlightEffect.SetActive(false);
            _highlightEffect.transform.parent = null;
        }
    }
}
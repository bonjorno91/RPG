using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.ActionsModule.Abilities
{
    public class Cooldown : Image, ICooldown, IPointerDownHandler
    {
        public void Show() => gameObject.SetActive(true);
        
        public void SetProgress(float progressClamped)
        {
            progressClamped = Mathf.Clamp01(progressClamped);
            if(progressClamped > 0 && !gameObject.activeSelf) Show(); 
            fillAmount = progressClamped;
            if (progressClamped <= 0) Hide();
        }

        public void Hide() => gameObject.SetActive(false);
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Is not ready yet.");
        }

        protected override void Reset()
        {
            base.Reset();
            color = new Color(0, 0, 0, 0.9f);
            type = Type.Filled;
            fillMethod = FillMethod.Radial360;
            fillClockwise = false;
            fillOrigin = 2;
            fillAmount = 0;
            gameObject.SetActive(false);
        }
    }
}
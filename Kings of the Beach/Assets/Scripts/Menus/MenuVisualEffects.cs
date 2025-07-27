using UnityEngine;

namespace KotB.Menus
{
    public class MenuVisualEffects : MonoBehaviour
    {
        [Header("Scale Effects")]
        [SerializeField] private bool useScaleEffect = true;
        [SerializeField] private float selectedScale = 1.1f;
        [SerializeField] private float animationDuration = 0.2f;

        [Header("Movement Effects")]
        [SerializeField] private bool useMovementEffect = false;
        [SerializeField] private Vector3 selectedOffset = Vector3.zero;

        private Vector3 originalScale;
        private Vector3 originalPosition;
        private MenuItemBase menuItem;

        private void Awake()
        {
            menuItem = GetComponent<MenuItemBase>();
            originalScale = transform.localScale;
            originalPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            if (menuItem != null)
            {
                StartCoroutine(CheckSelectionState());
            }
        }

        private System.Collections.IEnumerator CheckSelectionState()
        {
            bool wasSelected = false;
            
            while (enabled)
            {
                if (menuItem.IsSelected != wasSelected)
                {
                    wasSelected = menuItem.IsSelected;
                    
                    if (wasSelected)
                        OnSelected();
                    else
                        OnDeselected();
                }
                
                yield return null;
            }
        }

        private void OnSelected()
        {
            if (useScaleEffect)
            {
                StopAllCoroutines();
                StartCoroutine(AnimateScale(originalScale * selectedScale));
            }

            if (useMovementEffect)
            {
                StartCoroutine(AnimatePosition(originalPosition + selectedOffset));
            }
        }

        private void OnDeselected()
        {
            if (useScaleEffect)
            {
                StopAllCoroutines();
                StartCoroutine(AnimateScale(originalScale));
            }

            if (useMovementEffect)
            {
                StartCoroutine(AnimatePosition(originalPosition));
            }
        }

        private System.Collections.IEnumerator AnimateScale(Vector3 targetScale)
        {
            Vector3 startScale = transform.localScale;
            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / animationDuration;
                transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                yield return null;
            }

            transform.localScale = targetScale;
        }

        private System.Collections.IEnumerator AnimatePosition(Vector3 targetPosition)
        {
            Vector3 startPosition = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / animationDuration;
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            transform.localPosition = targetPosition;
        }
    }
}

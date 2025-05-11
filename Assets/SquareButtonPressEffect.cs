using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SquareButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    private Vector3 pressedScale;
    private Vector3 originalPosition;
    private Vector3 pressedPosition;

    public float pressAmount = 0.05f; // How much the button moves down
    public float shrinkFactor = 0.9f; // Shrink the button evenly

    void Start()
    {
        originalScale = transform.localScale;
        pressedScale = originalScale * shrinkFactor; // Shrink from all sides
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -pressAmount, 0); // Move down slightly
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(transform.localScale, pressedScale, transform.position, pressedPosition, 0.05f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(transform.localScale, originalScale, transform.position, originalPosition, 0.1f));
    }

    private IEnumerator SmoothTransition(Vector3 startScale, Vector3 targetScale, Vector3 startPos, Vector3 targetPos, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        transform.position = targetPos;
    }
}

using UnityEngine;

public class UIWobbleEffect : MonoBehaviour
{
    [SerializeField] private float wobbleSpeed = 1f;
    [SerializeField] private float wobbleIntensity = 0.1f;

    private RectTransform rectTransform;
    private Vector3 initialScale;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    private void Update()
    {
        // Apply wobble effect to the scale
        float scaleAmount = Mathf.Sin(Time.time * wobbleSpeed) * wobbleIntensity;
        Vector3 newScale = initialScale + new Vector3(scaleAmount, scaleAmount, 0f);
        rectTransform.localScale = newScale;
    }
}

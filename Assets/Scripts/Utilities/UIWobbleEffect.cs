using UnityEngine;

public class UIWobbleEffect : MonoBehaviour
{
    [SerializeField] private float wobbleSpeed = 1f;
    [SerializeField] private float wobbleIntensity = 0.1f;

    private RectTransform rectTransform;
    private Vector3 initialScale;
    private float timeOffset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        ApplyWobbleEffect();
    }

    private void ApplyWobbleEffect()
    {
        float time = Time.time + timeOffset;
        float wobble = Mathf.Sin(time * wobbleSpeed) * wobbleIntensity;
        Vector3 newScale = initialScale + new Vector3(wobble, wobble, 0f);
        rectTransform.localScale = newScale;
    }
}

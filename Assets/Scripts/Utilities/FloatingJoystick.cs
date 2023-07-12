using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    private Vector2 joystickPosition = Vector2.zero;
    private Vector2 direction = Vector2.zero;
    private bool isJoystickBeingUsed = false;

    public Vector2 Direction => direction;

    private void Start()
    {
        joystickPosition = background.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = ClampJoystickPosition(eventData.position);
        handle.position = position;

        direction = (position - joystickPosition).normalized;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isJoystickBeingUsed = true;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isJoystickBeingUsed = false;
        handle.position = joystickPosition;
        direction = Vector2.zero;
    }

    public float GetHorizontalValue()
    {
        return direction.x;
    }

    public float GetVerticalValue()
    {
        return direction.y;
    }

    private Vector2 ClampJoystickPosition(Vector2 position)
    {
        Vector2 backgroundMinPosition = (Vector2)background.position - background.sizeDelta * 0.5f;
        Vector2 backgroundMaxPosition = (Vector2)background.position + background.sizeDelta * 0.5f;

        return new Vector2(
            Mathf.Clamp(position.x, backgroundMinPosition.x, backgroundMaxPosition.x),
            Mathf.Clamp(position.y, backgroundMinPosition.y, backgroundMaxPosition.y)
        );
    }
}

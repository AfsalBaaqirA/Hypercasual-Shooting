using UnityEngine;

public class FloatingRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float floatingHeight = 0.5f;
    [SerializeField] private float floatingSpeed = 1f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Rotate the object around its local Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Float the object up and down along its Y-axis
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

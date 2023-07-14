using UnityEngine;

public class FloatingRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float floatingHeight = 0.5f;
    [SerializeField] private float floatingSpeed = 1f;

    private Vector3 initialPosition;
    private float initialTime;

    private void Start()
    {
        initialPosition = transform.position;
        initialTime = Time.time;
    }

    private void Update()
    {
        RotateObject();
        FloatObject();
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void FloatObject()
    {
        float newY = initialPosition.y + Mathf.Sin((Time.time - initialTime) * floatingSpeed) * floatingHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

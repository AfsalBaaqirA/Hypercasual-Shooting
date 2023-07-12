using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 5f;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float gravityMultiplier = 1f;


    protected Rigidbody rb;
    protected Vector3 movementDirection;
    protected Animator animator;
    private float gravity = -9.81f;
    private float gravityVelocity = 0f;
    private bool isShooting = false;

    public bool IsShooting
    {
        get { return isShooting; }
        set { isShooting = value; }
    }

    protected virtual void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Move()
    {
        // Calculate the movement velocity
        Vector3 movementVelocity = movementDirection * movementSpeed;

        // Apply the movement velocity to the transform position
        transform.position += movementVelocity * Time.deltaTime;

        // Handle the animation
        HandleAnimation();

        if (!isShooting)
        {
            // Handle the rotation
            HandleRotationInput();
        }
    }

    private void HandleAnimation()
    {
        // Get the magnitude of the movement direction
        float movementMagnitude = this.movementDirection.magnitude;

        // Trigger the run animation if the character is moving
        if (movementMagnitude > 0f)
        {
            // Trigger the run animation
            animator.SetFloat("Blend", 1);
            animator.SetFloat("BlendSide", 1);
        }
        else
        {
            // Trigger the idle animation
            animator.SetFloat("Blend", 0);
            animator.SetFloat("BlendSide", 0);
        }
    }

    private void HandleRotationInput()
    {
        transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotationSpeed);
    }

    private void ApplyGravity()
    {
        // Calculate the gravity velocity
        gravityVelocity += gravity * gravityMultiplier * Time.deltaTime;

        // Apply the gravity velocity to the transform position
        transform.position += Vector3.up * gravityVelocity * Time.deltaTime;
    }
}

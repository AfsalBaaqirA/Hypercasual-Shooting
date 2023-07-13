using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputAction playerInput;

    private CharacterController controller;
    private Rigidbody rb;
    private Animator animator;

    private Vector3 playerVelocity;

    [SerializeField] private Transform target;

    public Transform Target
    {
        get { return target; }
        set
        {
            target = value;
        }
    }

    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 5f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private void Awake()
    {
        playerInput = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameInProgress())
            return;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Set Player Rotation to Face Target
        if (target)
        {
            Vector3 targetDirection = target.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, playerSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            // Set Player Rotation to Face Movement Direction
            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(move);
            }
        }


        // Check if the player y position is below the kill plane
        if (transform.position.y < GameManager.Instance.KillPlaneY)
        {
            GameManager.Instance.GameOver();
        }

        HandleAnimation(move.magnitude);
    }

    private void HandleAnimation(float movementMagnitude)
    {
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

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.GameOver();
        }
    }

    public void CollectCoin()
    {
        // Perform coin collection actions (increase player's coins, play sound, etc.)
        GameManager.Instance.PlayerCoins++;
    }
}

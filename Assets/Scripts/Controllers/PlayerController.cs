using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
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
        Instance = this;
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
        if (target && target.gameObject.activeSelf)
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

        HandleAnimation(move.magnitude);
    }

    private void HandleAnimation(float movementMagnitude)
    {
        if (movementMagnitude > 0f)
        {
            animator.SetFloat("BlendSide", 1);
        }
        else
        {
            animator.SetFloat("BlendSide", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is an enemy
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            // Increase the count of colliding enemies
            int collidingEnemyCount = CountCollidingEnemies();

            // Check the number of colliding enemies and trigger AdsManager capture
            if (collidingEnemyCount > AdsManager.Instance.CaptureThreshold)
            {
                AdsManager.Instance.TriggerEnemyOnPlayerDeath();
            }
        }

        if (!GameManager.Instance.IsGameInProgress())
            return;

        // Check if the player has collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private int CountCollidingEnemies()
    {
        int count = 0;
        Collider playerCollider = GetComponent<Collider>();
        HashSet<EnemyController> uniqueEnemies = new HashSet<EnemyController>();

        // Get all the colliders overlapping with the player collider
        Collider[] colliders = Physics.OverlapBox(playerCollider.bounds.center, Vector3.one * 10f, playerCollider.transform.rotation);

        // Check if each overlapping collider belongs to an enemy
        foreach (Collider collider in colliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null && !uniqueEnemies.Contains(enemy))
            {
                uniqueEnemies.Add(enemy);
                count++;
            }
        }

        return count;
    }



    public void CollectCoin()
    {
        // Perform coin collection actions (increase player's coins, play sound, etc.)
        GameManager.Instance.PlayerCoins++;
    }
}

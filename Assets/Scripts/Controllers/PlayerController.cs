using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private PlayerInputAction playerInput;
    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;
    private Transform target;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;

    public Transform Target
    {
        get => target;
        set => target = value;
    }

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
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

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameInProgress())
            return;

        UpdatePlayerMovement();
        UpdatePlayerRotation();
        HandleAnimation();
    }

    private void UpdatePlayerMovement()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void UpdatePlayerRotation()
    {
        if (target != null && target.gameObject.activeSelf)
        {
            Vector3 targetDirection = target.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, playerSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            Vector3 movementInput = playerInput.Player.Move.ReadValue<Vector2>();
            Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(move);
            }
        }
    }

    private void HandleAnimation()
    {
        float movementMagnitude = playerInput.Player.Move.ReadValue<Vector2>().magnitude;
        animator.SetFloat("BlendSide", movementMagnitude > 0f ? 1f : 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyController enemy))
        {
            int collidingEnemyCount = CountCollidingEnemies();
            if (collidingEnemyCount > AdsManager.Instance.CaptureThreshold)
            {
                AdsManager.Instance.TriggerEnemyOnPlayerDeath();
            }
        }

        if (!GameManager.Instance.IsGameInProgress())
            return;

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

        Collider[] colliders = Physics.OverlapBox(playerCollider.bounds.center, Vector3.one * 10f, playerCollider.transform.rotation);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyController enemy) && !uniqueEnemies.Contains(enemy))
            {
                uniqueEnemies.Add(enemy);
                count++;
            }
        }

        return count;
    }

    public void CollectCoin()
    {
        GameManager.Instance.PlayerCoins++;
    }

    public void PlayerWonAnimation()
    {
        animator.Play("Anim-Celebration-Dance01");
    }
}

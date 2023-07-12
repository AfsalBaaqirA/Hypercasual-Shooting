using UnityEngine;

public class PlayerController : BaseController
{
    [SerializeField] private WeaponController weaponController;

    private Transform target;
    private InputManager gameInput;
    private GameObject[] enemies;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        gameInput = InputManager.Instance;
    }

    private void Update()
    {
        // Handle movement input
        HandleMovementInput(gameInput.GetMovementInputNormalized());
    }

    private void FixedUpdate()
    {
        // Check if the game is started or over
        if (GameManager.Instance.GameState == GameState.Started)
        {
            // Check if the player has fallen off the map
            if (transform.position.y < -10)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                Move();
            }
        }
    }

    private void HandleMovementInput(Vector2 movementInput)
    {
        Vector2 movementVector = movementInput;

        this.movementDirection = new Vector3(movementVector.x, 0f, movementVector.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.GameOver();
        }
    }
}

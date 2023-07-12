using UnityEngine;

public class EnemyController : BaseController
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject targetIndicator;

    private Transform player;
    private int currentHealth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        // Hide the target indicator
        HideTargetIndicator();
    }

    private void Update()
    {
        // Check if player is within detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            // Move towards the player
            this.movementDirection = (player.position - transform.position).normalized;
        }
    }


    private void FixedUpdate()
    {
        // Check if the game is started or over
        if (GameManager.Instance.GameState == GameState.Started)
        {
            Move();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if the enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Perform death-related actions (e.g., play death animation, spawn particles, etc.)
        Debug.Log("Enemy died");
        // Set back to pool
        gameObject.SetActive(false);
    }

    public void ShowTargetIndicator()
    {
        Debug.Log("Show target indicator");
        // Show the target indicator
        targetIndicator.SetActive(true);
        // healthBar.SetActive(true);
    }

    public void HideTargetIndicator()
    {
        Debug.Log("Hide target indicator");
        // Hide the target indicator
        targetIndicator.SetActive(false);
        // healthBar.SetActive(false);
    }

    public void Reset()
    {
        currentHealth = maxHealth;

    }
}

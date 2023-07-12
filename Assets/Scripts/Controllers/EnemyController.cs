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
            Move();
        }
    }

    protected override void Move()
    {
        // Move towards the player
        this.movementDirection = (player.position - transform.position).normalized;
        Debug.Log(movementDirection);
    }

    private void MoveTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the enemy towards the player
        transform.Translate(direction * movementSpeed * Time.deltaTime);
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

        // Destroy the enemy object
        Destroy(gameObject);
    }

    public void ShowTargetIndicator()
    {
        // Show the target indicator
        targetIndicator.SetActive(true);
        healthBar.SetActive(true);
    }

    public void HideTargetIndicator()
    {
        // Hide the target indicator
        targetIndicator.SetActive(false);
        healthBar.SetActive(false);
    }
}

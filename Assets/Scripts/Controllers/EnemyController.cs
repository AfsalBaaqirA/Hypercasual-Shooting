using UnityEngine;

public class EnemyController : BaseController
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject targetIndicator;

    private Transform player;
    private int currentHealth;
    private bool isDead = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        HideTargetIndicator();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameInProgress())
            return;

        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            // Move towards the player
            this.movementDirection = (player.position - transform.position).normalized;
        }
        else
        {
            // Stop moving
            this.movementDirection = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameInProgress())
        {
            Move();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        SpawnCoin();
        Destroy(gameObject);
    }

    public void ShowTargetIndicator()
    {
        Debug.Log("Show target indicator");
        targetIndicator.SetActive(true);
        // healthBar.SetActive(true);
    }

    public void HideTargetIndicator()
    {
        Debug.Log("Hide target indicator");
        targetIndicator.SetActive(false);
        // healthBar.SetActive(false);
    }

    public void Reset()
    {
        currentHealth = maxHealth;
    }

    private void SpawnCoin()
    {
        GameObject coin = ObjectPooler.Instance.GetCoinObjectFromPool();
        coin.transform.position = this.transform.position + new Vector3(0, 1, 0);
        coin.SetActive(true);
    }
}

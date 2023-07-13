using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject targetIndicator;

    private Transform player;
    private int currentHealth;
    private bool isDead = false;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

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
            navMeshAgent.destination = player.position;
        }
        else
        {
            navMeshAgent.destination = transform.position;
        }

        HandleAnimation(navMeshAgent.velocity.magnitude);
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
        targetIndicator.SetActive(true);
        // healthBar.SetActive(true);
    }

    public void HideTargetIndicator()
    {
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

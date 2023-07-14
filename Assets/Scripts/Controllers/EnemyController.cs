using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject targetIndicator;

    private ObjectPool<EnemyController> _enemyPool;
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
        player = PlayerController.Instance.transform;
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        isDead = false;
        healthBar.SetActive(false);
        targetIndicator.SetActive(false);
    }

    public void SetPool(ObjectPool<EnemyController> enemyPool)
    {
        this._enemyPool = enemyPool;
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
            animator.SetFloat("BlendSide", 1);
        }
        else
        {
            animator.SetFloat("BlendSide", 0);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;
        CancelInvoke("HideHealthBar");
        currentHealth -= damage;
        this.healthBar.SetActive(true);
        this.healthBar.GetComponent<HealthBar>().SetHealth(currentHealth);
        Invoke("HideHealthBar", 2f);

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private void HideHealthBar()
    {
        healthBar.SetActive(false);
    }

    private void Die()
    {
        SpawnCoin();
        AdsManager.Instance.EnemyKillCount++;
        _enemyPool.Release(this);
    }

    public void ShowTargetIndicator()
    {
        targetIndicator.SetActive(true);
    }

    public void HideTargetIndicator()
    {
        targetIndicator.SetActive(false);
    }

    private void SpawnCoin()
    {
        CoinMagnet coin = ObjectPooler.Instance.GetCoin(transform.position + new Vector3(0, 1, 0));
    }
}

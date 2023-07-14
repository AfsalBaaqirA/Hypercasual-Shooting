using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private GameObject targetIndicatorObject;

    private ObjectPool<EnemyController> enemyPool;
    private Transform playerTransform;
    private int currentHealth;
    private bool isDead;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private HealthBar healthBar;
    private static readonly int BlendSideHash = Animator.StringToHash("BlendSide");
    private static readonly int AnimatorFloatValue = 1;
    private static readonly int AnimatorZeroValue = 0;
    private static readonly float HideHealthBarDelay = 2f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        healthBar = healthBarObject.GetComponent<HealthBar>();
    }

    private void Start()
    {
        playerTransform = PlayerController.Instance.transform;
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        isDead = false;
        healthBarObject.SetActive(false);
        targetIndicatorObject.SetActive(false);
    }

    public void SetPool(ObjectPool<EnemyController> pool)
    {
        enemyPool = pool;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameInProgress())
            return;

        var playerPosition = playerTransform.position;
        var distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        navMeshAgent.destination = distanceToPlayer < detectionRange ? playerPosition : transform.position;

        HandleAnimation(navMeshAgent.velocity.magnitude);
    }

    private void HandleAnimation(float movementMagnitude)
    {
        animator.SetFloat(BlendSideHash, movementMagnitude > 0f ? AnimatorFloatValue : AnimatorZeroValue);
        animator.Play(movementMagnitude > 0f ? "Anim-Walk-Enemy" : "Anim-Celebration-Dance01");
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        CancelInvoke(nameof(HideHealthBar));
        currentHealth -= damage;
        healthBarObject.SetActive(true);
        healthBar.SetHealth(currentHealth);
        Invoke(nameof(HideHealthBar), HideHealthBarDelay);

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private void HideHealthBar()
    {
        healthBarObject.SetActive(false);
    }

    private void Die()
    {
        SpawnCoin();
        AdsManager.Instance.IncreaseEnemyKillCount();
        enemyPool.Release(this);
    }

    public void ShowTargetIndicator()
    {
        targetIndicatorObject.SetActive(true);
    }

    public void HideTargetIndicator()
    {
        targetIndicatorObject.SetActive(false);
    }

    private void SpawnCoin()
    {
        CoinMagnet coin = ObjectPooler.Instance.GetCoin(transform.position + Vector3.up);
    }
}

using UnityEngine;
using UnityEngine.Pool;

public class CoinMagnet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;

    private Transform player;
    private ObjectPool<CoinMagnet> coinPool;

    private void Start()
    {
        player = PlayerController.Instance.transform;
    }

    public void SetPool(ObjectPool<CoinMagnet> pool)
    {
        coinPool = pool;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameInProgress())
        {
            return;
        }

        if (player != null)
        {
            MoveTowardsPlayer();
            RotateCoin();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void RotateCoin()
    {
        transform.Rotate(Vector3.up * 100f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.CollectCoin();

        coinPool.Release(this);
    }
}

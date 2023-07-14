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
            return;

        if (player != null)
        {
            // Move towards the player in collectible effect mode
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            // Rotate the coin in collectible effect mode
            transform.Rotate(Vector3.up * 100f * Time.deltaTime);

        }
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
        player.GetComponent<PlayerController>().CollectCoin();

        // Disable the coin
        coinPool.Release(this);
    }
}

using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        Debug.Log("Coin collected!");
        player.GetComponent<PlayerController>().CollectCoin();

        // Deactivate the coin and return it to the object pool
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);
    }
}

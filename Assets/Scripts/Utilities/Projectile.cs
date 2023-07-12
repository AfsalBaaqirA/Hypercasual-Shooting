using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject hitObstacleEffectPrefab;
    private int damageAmount;
    public int DamageAmount { set { damageAmount = value; } }

    private void OnEnable()
    {
        Invoke("DisableProjectile", 2f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void DisableProjectile()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to the player
            EnemyController playerHealth = other.gameObject.GetComponent<EnemyController>();
            playerHealth?.TakeDamage(damageAmount);

            // Instantiate the hit effect
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

            // Disable the bullet
            gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            // Instantiate the hit effect
            Instantiate(hitObstacleEffectPrefab, transform.position, Quaternion.identity);

            // Disable the bullet
            gameObject.SetActive(false);
        }
    }
}

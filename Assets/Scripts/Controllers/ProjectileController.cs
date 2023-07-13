using UnityEngine;
using static Weapon;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject hitObstacleEffectPrefab;

    private Weapon weapon;
    public Weapon Weapon { get => weapon; set => weapon = value; }

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
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            switch (weapon.WeaponFireType)
            {
                case WeaponType.Single:
                    other.gameObject.GetComponent<EnemyController>().TakeDamage(weapon.Damage);
                    break;
                case WeaponType.Spread:
                    // Get near enemies
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
                    foreach (Collider collider in colliders)
                    {
                        EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
                        enemy?.TakeDamage(weapon.Damage);
                    }
                    break;
            }

            // Instantiate the hit effect
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            DisableProjectile();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle");
            // Instantiate the hit effect
            Instantiate(hitObstacleEffectPrefab, transform.position, Quaternion.identity);

            DisableProjectile();
        }
    }
}

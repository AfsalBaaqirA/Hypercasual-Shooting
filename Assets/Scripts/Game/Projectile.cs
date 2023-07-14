using UnityEngine;
using static Weapon;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private ObjectPool<Projectile> bulletPool;
    private Weapon weapon;
    private float disableDelay = 2f;

    public Weapon Weapon { get => weapon; set => weapon = value; }

    private void OnEnable()
    {
        Invoke(nameof(DisableProjectile), disableDelay);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(DisableProjectile));
    }

    private void DisableProjectile()
    {
        weapon = null;
        bulletPool.Release(this);
    }

    public void SetPool(ObjectPool<Projectile> pool)
    {
        bulletPool = pool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            switch (weapon.WeaponFireType)
            {
                case WeaponType.Single:
                    other.gameObject.GetComponent<EnemyController>()?.TakeDamage(weapon.Damage);
                    break;
                case WeaponType.Spread:
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
                    foreach (Collider collider in colliders)
                    {
                        collider.gameObject.GetComponent<EnemyController>()?.TakeDamage(weapon.Damage);
                    }
                    break;
            }

            ParticlePool particleEffect = ObjectPooler.Instance.GetParticle(weapon.WeaponFireType, transform.position, transform.rotation);
            particleEffect.transform.localScale = Vector3.one + Vector3.one * weapon.Damage * 0.1f;

            DisableProjectile();
        }
    }
}

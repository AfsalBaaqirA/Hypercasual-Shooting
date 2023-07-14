using UnityEngine;
using static Weapon;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private ObjectPool<Projectile> _bulletPool;
    private Weapon _weapon;
    public Weapon Weapon { get => _weapon; set => _weapon = value; }


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
        _weapon = null;
        _bulletPool.Release(this);
    }

    public void SetPool(ObjectPool<Projectile> bulletPool)
    {
        this._bulletPool = bulletPool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            switch (_weapon.WeaponFireType)
            {
                case WeaponType.Single:
                    other.gameObject.GetComponent<EnemyController>().TakeDamage(_weapon.Damage);
                    break;
                case WeaponType.Spread:
                    // Get near enemies
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
                    foreach (Collider collider in colliders)
                    {
                        EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
                        enemy?.TakeDamage(_weapon.Damage);
                    }
                    break;
            }

            ParticlePool particleEffect = ObjectPooler.Instance.GetParticle(_weapon.WeaponFireType, transform.position, transform.rotation);
            particleEffect.transform.localScale = Vector3.one + Vector3.one * _weapon.Damage * 0.1f;

            DisableProjectile();
        }
    }
}

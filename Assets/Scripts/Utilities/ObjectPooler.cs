using System;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    public int InitialPoolSize => initialPoolSize;
    public int MaxPoolSize => maxPoolSize;

    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private int maxPoolSize = 100;
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private ParticlePool bulletParticle;
    [SerializeField] private ParticlePool missileParticle;

    private ObjectPool<EnemyController> enemyPool;
    private ObjectPool<Projectile> bulletPool;
    private ObjectPool<Projectile> missilePool;
    private ObjectPool<CoinMagnet> coinPool;
    private ObjectPool<ParticlePool> bulletParticlePool;
    private ObjectPool<ParticlePool> missileParticlePool;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        enemyPool = new ObjectPool<EnemyController>(CreateEnemy, OnEnemyPooled, OnEnemyDePooled, OnEnemyDestroyed, collectionCheck, initialPoolSize, maxPoolSize);
        bulletPool = new ObjectPool<Projectile>(CreateBullet, OnBulletPooled, OnBulletDePooled, OnBulletDestroyed, collectionCheck, initialPoolSize, maxPoolSize);
        missilePool = new ObjectPool<Projectile>(CreateMissile, OnMissilePooled, OnMissileDePooled, OnMissileDestroyed, collectionCheck, initialPoolSize, maxPoolSize);
        coinPool = new ObjectPool<CoinMagnet>(CreateCoin, OnCoinPooled, OnCoinDePooled, OnCoinDestroyed, collectionCheck, initialPoolSize, maxPoolSize);
        bulletParticlePool = new ObjectPool<ParticlePool>(CreateBulletParticle, OnBulletParticlePooled, OnBulletParticleDePooled, OnBulletParticleDestroyed, collectionCheck, initialPoolSize, maxPoolSize);
        missileParticlePool = new ObjectPool<ParticlePool>(CreateMissileParticle, OnMissileParticlePooled, OnMissileParticleDePooled, OnMissileParticleDestroyed, collectionCheck, initialPoolSize, maxPoolSize);

        // Preload the pools with the initial pool size amount and disable the objects
        for (int i = 0; i < initialPoolSize; i++)
        {
            EnemyController enemy = CreateEnemy();
            enemyPool.Release(enemy);
            Projectile bullet = CreateBullet();
            bulletPool.Release(bullet);
            Projectile missile = CreateMissile();
            missilePool.Release(missile);
            CoinMagnet coin = CreateCoin();
            coinPool.Release(coin);
            ParticlePool bulletParticle = CreateBulletParticle();
            bulletParticlePool.Release(bulletParticle);
            ParticlePool missileParticle = CreateMissileParticle();
            missileParticlePool.Release(missileParticle);
        }
    }

    private EnemyController CreateEnemy()
    {
        EnemyController enemy = Instantiate(enemyPrefab);
        enemy.gameObject.SetActive(false);
        enemy.SetPool(enemyPool);
        return enemy;
    }

    private void OnEnemyPooled(EnemyController enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnEnemyDePooled(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnEnemyDestroyed(EnemyController enemy)
    {
        Destroy(enemy.gameObject);
    }

    private Projectile CreateBullet()
    {
        Projectile bullet = Instantiate(bulletPrefab).GetComponent<Projectile>();
        bullet.gameObject.SetActive(false);
        bullet.SetPool(bulletPool);
        return bullet;
    }

    private void OnBulletPooled(Projectile bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnBulletDePooled(Projectile bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnBulletDestroyed(Projectile bullet)
    {
        Destroy(bullet.gameObject);
    }

    private Projectile CreateMissile()
    {
        Projectile missile = Instantiate(missilePrefab).GetComponent<Projectile>();
        missile.gameObject.SetActive(false);
        missile.SetPool(missilePool);
        return missile;
    }

    private void OnMissilePooled(Projectile missile)
    {
        missile.gameObject.SetActive(true);
    }

    private void OnMissileDePooled(Projectile missile)
    {
        missile.gameObject.SetActive(false);
    }

    private void OnMissileDestroyed(Projectile missile)
    {
        Destroy(missile.gameObject);
    }

    private CoinMagnet CreateCoin()
    {
        CoinMagnet coin = Instantiate(coinPrefab).GetComponent<CoinMagnet>();
        coin.gameObject.SetActive(false);
        coin.SetPool(coinPool);
        return coin;
    }

    private void OnCoinPooled(CoinMagnet coin)
    {
        coin.gameObject.SetActive(true);
    }

    private void OnCoinDePooled(CoinMagnet coin)
    {
        coin.gameObject.SetActive(false);
    }

    private void OnCoinDestroyed(CoinMagnet coin)
    {
        Destroy(coin.gameObject);
    }

    private ParticlePool CreateBulletParticle()
    {
        ParticlePool bulletParticle = Instantiate(this.bulletParticle).GetComponent<ParticlePool>();
        bulletParticle.gameObject.SetActive(false);
        bulletParticle.SetPool(bulletParticlePool);
        return bulletParticle;
    }

    private void OnBulletParticlePooled(ParticlePool bulletParticle)
    {
        bulletParticle.gameObject.SetActive(true);
    }

    private void OnBulletParticleDePooled(ParticlePool bulletParticle)
    {
        bulletParticle.gameObject.SetActive(false);
    }

    private void OnBulletParticleDestroyed(ParticlePool bulletParticle)
    {
        Destroy(bulletParticle.gameObject);
    }

    private ParticlePool CreateMissileParticle()
    {
        ParticlePool missileParticle = Instantiate(this.missileParticle).GetComponent<ParticlePool>();
        missileParticle.gameObject.SetActive(false);
        missileParticle.SetPool(missileParticlePool);
        return missileParticle;
    }

    private void OnMissileParticlePooled(ParticlePool missileParticle)
    {
        missileParticle.gameObject.SetActive(true);
    }

    private void OnMissileParticleDePooled(ParticlePool missileParticle)
    {
        missileParticle.gameObject.SetActive(false);
    }

    private void OnMissileParticleDestroyed(ParticlePool missileParticle)
    {
        Destroy(missileParticle.gameObject);
    }

    public EnemyController GetEnemy(Vector3 position)
    {
        EnemyController enemy = enemyPool.Get();
        enemy.transform.position = position;
        return enemy;
    }

    public Projectile GetBullet(Vector3 position, Quaternion rotation)
    {
        Projectile bullet = bulletPool.Get();
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        return bullet;
    }

    public Projectile GetMissile(Vector3 position, Quaternion rotation)
    {
        Projectile missile = missilePool.Get();
        missile.transform.position = position;
        missile.transform.rotation = rotation;
        return missile;
    }

    public CoinMagnet GetCoin(Vector3 position)
    {
        CoinMagnet coin = coinPool.Get();
        coin.transform.position = position;
        return coin;
    }

    public ParticlePool GetBulletParticle(Vector3 position, Quaternion rotation)
    {
        ParticlePool bulletParticle = bulletParticlePool.Get();
        bulletParticle.transform.position = position;
        bulletParticle.transform.rotation = rotation;
        return bulletParticle;
    }

    public ParticlePool GetMissileParticle(Vector3 position, Quaternion rotation)
    {
        ParticlePool missileParticle = missileParticlePool.Get();
        missileParticle.transform.position = position;
        missileParticle.transform.rotation = rotation;
        return missileParticle;
    }

    public ParticlePool GetParticle(Weapon.WeaponType weaponType, Vector3 position, Quaternion rotation)
    {
        switch (weaponType)
        {
            case Weapon.WeaponType.Single:
                return GetBulletParticle(position, rotation);
            case Weapon.WeaponType.Spread:
                return GetMissileParticle(position, rotation);
            default:
                return GetBulletParticle(position, rotation);
        }
    }
}

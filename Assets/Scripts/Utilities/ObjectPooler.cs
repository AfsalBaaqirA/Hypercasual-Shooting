using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> bulletPool;
    private Queue<GameObject> missilePool;
    private Queue<GameObject> coinPool;

    private Dictionary<Queue<GameObject>, GameObject> poolPrefabMap;

    private void Awake()
    {
        Instance = this;
        bulletPool = CreateObjectPool(bulletPrefab);
        missilePool = CreateObjectPool(missilePrefab);
        coinPool = CreateObjectPool(coinPrefab);

        poolPrefabMap = new Dictionary<Queue<GameObject>, GameObject>
        {
            { bulletPool, bulletPrefab },
            { missilePool, missilePrefab },
            { coinPool, coinPrefab }
        };
    }

    private Queue<GameObject> CreateObjectPool(GameObject prefab)
    {
        Queue<GameObject> pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        return pool;
    }

    private GameObject GetObjectFromPool(Queue<GameObject> pool)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // return ExpandPool(pool);
        return null;
    }

    private GameObject ExpandPool(Queue<GameObject> pool)
    {
        if (poolPrefabMap.TryGetValue(pool, out GameObject prefab))
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(true);
            pool.Enqueue(newObj);
            return newObj;
        }

        return null;
    }

    public GameObject GetBulletObjectFromPool()
    {
        return GetObjectFromPool(bulletPool);
    }

    public GameObject GetMissileObjectFromPool()
    {
        return GetObjectFromPool(missilePool);
    }

    public GameObject GetCoinObjectFromPool()
    {
        Debug.Log("Getting coin from pool");
        return GetObjectFromPool(coinPool);
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        bulletPool.Enqueue(obj);
        missilePool.Enqueue(obj);
        coinPool.Enqueue(obj);
    }

    public void ReturnAllObjectsToPool()
    {
        ReturnObjectsToPool(bulletPool);
        ReturnObjectsToPool(missilePool);
        ReturnObjectsToPool(coinPool);
    }

    private void ReturnObjectsToPool(Queue<GameObject> pool)
    {
        while (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(false);
        }
    }

    public void Update()
    {
        Debug.Log("Bullet pool size: " + bulletPool.Count);
        Debug.Log("Missile pool size: " + missilePool.Count);
        Debug.Log("Coin pool size: " + coinPool.Count);
    }
}

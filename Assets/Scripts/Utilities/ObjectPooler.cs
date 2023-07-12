using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> enemyPool;
    private List<GameObject> bulletPool;
    private List<GameObject> missilePool;

    private void Awake()
    {
        Instance = this;
        // enemyPool = CreateObjectPool(enemyPrefab);
        bulletPool = CreateObjectPool(bulletPrefab);
        missilePool = CreateObjectPool(missilePrefab);
    }

    private List<GameObject> CreateObjectPool(GameObject prefab)
    {
        List<GameObject> pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }

        return pool;
    }

    public GameObject GetObjectFromPool(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // return ExpandPool(pool);
        return null;
    }

    private GameObject ExpandPool(List<GameObject> pool)
    {
        GameObject prefab;
        if (pool == enemyPool)
            prefab = enemyPrefab;
        else if (pool == bulletPool)
            prefab = bulletPrefab;
        else if (pool == missilePool)
            prefab = missilePrefab;
        else
            return null;

        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }

    public GameObject GetEnemyObjectFromPool()
    {
        return GetObjectFromPool(enemyPool);
    }

    public GameObject GetBulletObjectFromPool()
    {
        return GetObjectFromPool(bulletPool);
    }

    public GameObject GetMissileObjectFromPool()
    {
        return GetObjectFromPool(missilePool);
    }
}

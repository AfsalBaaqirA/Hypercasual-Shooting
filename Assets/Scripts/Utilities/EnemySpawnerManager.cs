using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Collider initialSpawnAreaCollider;
    [SerializeField] private int spawnEnemiesCount = 3;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnEnemies(spawnEnemiesCount, initialSpawnAreaCollider);
    }

    public void SpawnEnemies(int count, Collider spawnAreaCollider)
    {
        if (spawnAreaCollider == null)
            return;

        Debug.Log("Spawning " + count + " enemies");

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInCollider(spawnAreaCollider);

            EnemyController enemy = ObjectPooler.Instance.GetEnemy(spawnPosition);
        }
    }

    private Vector3 GetRandomPositionInCollider(Collider collider)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            0,
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );
        return randomPosition;
    }
}

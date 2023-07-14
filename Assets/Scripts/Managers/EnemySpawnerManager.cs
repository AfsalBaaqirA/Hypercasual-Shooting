using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance;

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
        if (spawnAreaCollider == null || count <= 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInCollider(spawnAreaCollider);
            SpawnEnemy(spawnPosition);
        }
    }

    private Vector3 GetRandomPositionInCollider(Collider collider)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            1,
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );
        return randomPosition;
    }

    private void SpawnEnemy(Vector3 spawnPosition)
    {
        EnemyController enemy = ObjectPooler.Instance.GetEnemy(spawnPosition);
    }
}

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int objectCount = 10;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int spawnIndex = 0;
        int enemyCount = 1;

        for (int i = 0; i < objectCount; i++)
        {
            Transform spawnPoint = spawnPoints[spawnIndex];
            Vector3 spawnOffset = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position + spawnOffset, Quaternion.identity);
            enemyObject.GetComponent<EnemyController>().Reset();

            enemyCount++;
            if (enemyCount > i / spawnPoints.Length + 1)
            {
                enemyCount = 1;
                spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
            }
        }
    }
}

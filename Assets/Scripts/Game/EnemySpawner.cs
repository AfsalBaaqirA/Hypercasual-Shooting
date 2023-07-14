using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int spawnEnemiesCount = 3;
    [SerializeField] private Collider spawnAreaCollider;

    private bool hasSpawned = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasSpawned)
        {
            EnemySpawnerManager.Instance.SpawnEnemies(spawnEnemiesCount, spawnAreaCollider);
            hasSpawned = true;
        }
    }
}

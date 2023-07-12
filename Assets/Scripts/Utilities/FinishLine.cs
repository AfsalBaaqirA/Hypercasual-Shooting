using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with an enemy
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameWon();
        }
    }
}

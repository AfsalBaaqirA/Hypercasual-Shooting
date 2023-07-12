using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add the weapon to the player's inventory
            other.GetComponent<WeaponController>().EquipWeapon(weapon);

            // Destroy the weapon in the scene
            Destroy(gameObject);
        }
    }
}

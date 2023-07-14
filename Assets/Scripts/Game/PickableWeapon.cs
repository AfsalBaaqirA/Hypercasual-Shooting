using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponController weaponController = other.GetComponent<WeaponController>();
            if (weaponController != null)
            {
                weaponController.EquipWeapon(weapon);
            }

            Destroy(gameObject);
        }
    }
}

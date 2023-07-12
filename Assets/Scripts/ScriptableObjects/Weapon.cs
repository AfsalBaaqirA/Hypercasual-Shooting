using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private float fireRate;
    [SerializeField] private int damage;
    [SerializeField] private float shootingRange;
    [SerializeField] private GameObject projectilePrefab;

    public string WeaponName => weaponName;
    public float FireRate => fireRate;
    public int Damage => damage;
    public float ShootingRange => shootingRange;
}

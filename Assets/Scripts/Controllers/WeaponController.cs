using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 100f;

    private Weapon currentWeapon;

    private Transform target;
    private float shootingRange;
    private float nextFireTime = 0f;
    private float fireRate;
    private int damage;
    private string weaponName;
    private GameObject[] enemies;

    private void Start()
    {
        HideAllWeapons();
    }

    private void Update()
    {
        if (currentWeapon == null || GameManager.Instance.GameState != GameState.Started)
            return;

        // Find the nearest enemy within the targeting range
        FindNearestEnemy();

        // Check if a target is found and if enough time has passed to fire

        if (target != null && Vector3.Distance(transform.position, target.position) <= shootingRange)
        {
            Shoot();
        }
        else
        {
            player.IsShooting = false;
        }
    }

    private void FindNearestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy")?.Where(x => x.activeSelf).ToArray();
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        if (target != null && target != closestEnemy)
        {
            // Hide the target indicator of the previous target
            target.GetComponent<EnemyController>().HideTargetIndicator();
        }

        target = closestEnemy;

        if (target == null)
            return;

        // Check if the target is within the targeting range
        if (Vector3.Distance(transform.position, target.position) <= shootingRange)
        {
            // Display the target indicator
            target.GetComponent<EnemyController>().ShowTargetIndicator();
        }
        else
        {
            // Hide the target indicator
            target.GetComponent<EnemyController>().HideTargetIndicator();
        }
    }

    private void Shoot()
    {
        // Check if enough time has passed to fire
        if (Time.time < nextFireTime)
            return;
        player.IsShooting = true;
        // Perform shooting-related actions (e.g., play shooting animation, spawn particles, etc.)
        GameObject bulletObject = null;
        switch (weaponName)
        {
            case "Pistol":
                bulletObject = ObjectPooler.Instance.GetBulletObjectFromPool();
                break;
            case "Shotgun":
                bulletObject = ObjectPooler.Instance.GetBulletObjectFromPool();
                break;
            case "Machinegun":
                bulletObject = ObjectPooler.Instance.GetBulletObjectFromPool();
                break;
            case "Launcher":
                bulletObject = ObjectPooler.Instance.GetMissileObjectFromPool();
                break;
            default:
                break;
        }

        bulletObject.transform.position = firePoint.position;
        bulletObject.transform.rotation = firePoint.rotation;
        bulletObject.GetComponent<Projectile>().DamageAmount = damage;
        bulletObject.SetActive(true);
        bulletObject.GetComponent<Rigidbody>().velocity = bulletObject.transform.forward * bulletSpeed;
        AudioManager.Instance.PlaySoundEffect(currentWeapon.FireSound);
        // Update the next fire time
        nextFireTime = Time.time + fireRate;

        // Smoothly rotate towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    public void EquipWeapon(Weapon weapon)
    {
        // Set the current weapon
        currentWeapon = weapon;

        // Set the fire rate
        fireRate = currentWeapon.FireRate;

        // Set the shooting range
        shootingRange = currentWeapon.ShootingRange;

        // Set the damage
        damage = currentWeapon.Damage;

        // Set the weapon name
        weaponName = currentWeapon.WeaponName;

        foreach (GameObject wp in weapons)
        {
            if (wp.name == weaponName)
            {
                wp.SetActive(true);
            }
            else
            {
                wp.SetActive(false);
            }
        }

        GameManager.Instance.WeaponName = weaponName;
    }

    public void HideAllWeapons()
    {
        foreach (GameObject wp in weapons)
        {
            wp.SetActive(false);
        }
    }
}

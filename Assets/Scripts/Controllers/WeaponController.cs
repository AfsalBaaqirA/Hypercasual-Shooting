using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletForce = 20f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem catridgeEjection;

    private Weapon currentWeapon;

    private Transform target;
    private float shootingRange;
    private float nextFireTime = 0f;
    private float fireRate;
    private string weaponName;
    private GameObject[] enemies;
    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        HideAllWeapons();
    }

    private void Update()
    {
        if (currentWeapon == null || !GameManager.Instance.IsGameInProgress())
            return;

        // Find the nearest enemy within the targeting range
        FindNearestEnemy();

        // Check if a target is found and if enough time has passed to fire
        if (target != null && Vector3.Distance(transform.position, target.position) <= shootingRange)
        {
            Shoot();
        }
    }

    private void FindNearestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeSelf)
                continue;

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
            player.Target = target;
            target.GetComponent<EnemyController>().ShowTargetIndicator();
        }
        else
        {
            // Hide the target indicator
            player.Target = null;
            target.GetComponent<EnemyController>().HideTargetIndicator();
        }
    }

    private void Shoot()
    {
        // Check if enough time has passed to fire
        if (Time.time < nextFireTime)
            return;

        // Perform shooting-related actions (e.g., play shooting animation, spawn particles, etc.)
        GameObject bulletObject = null;

        switch (weaponName)
        {
            case "Pistol":
            case "Shotgun":
            case "Machinegun":
                bulletObject = objectPooler.GetBulletObjectFromPool();
                break;
            case "Launcher":
                bulletObject = objectPooler.GetMissileObjectFromPool();
                break;
        }

        bulletObject.transform.position = firePoint.position;
        Vector3 direction = (target.position - firePoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        bulletObject.transform.rotation = rotation;
        bulletObject.GetComponent<ProjectileController>().Weapon = currentWeapon;
        bulletObject.SetActive(true);

        Rigidbody bulletRigidbody = bulletObject.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.AddForce(firePoint.transform.forward * bulletForce, ForceMode.Impulse);
        }

        AudioManager.Instance.PlaySoundEffect(currentWeapon.FireSound);
        muzzleFlash.Play();
        catridgeEjection.Play();

        // Update the next fire time
        nextFireTime = Time.time + fireRate;
    }

    public void EquipWeapon(Weapon weapon)
    {
        // Set the current weapon
        currentWeapon = weapon;

        // Set the fire rate
        fireRate = currentWeapon.FireRate;

        // Set the shooting range
        shootingRange = currentWeapon.ShootingRange;

        // Set the weapon name
        weaponName = currentWeapon.WeaponName;

        foreach (GameObject wp in weapons)
        {
            wp.SetActive(wp.name == weaponName);
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

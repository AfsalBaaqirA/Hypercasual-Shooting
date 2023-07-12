using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons;

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
        // Find all enemies in the scene
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        HideAllWeapons();
    }

    private void Update()
    {
        if (target == null || currentWeapon == null)
            return;

        // Find the nearest enemy within the targeting range
        FindNearestEnemy();

        // Check if a target is found and if enough time has passed to fire
        if (target != null && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    private void FindNearestEnemy()
    {
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

        target = closestEnemy;

        // Check if the closest enemy is within the targeting range
        if (closestDistance <= shootingRange)
        {
            // Show the target indicator
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
        // Perform the shooting logic
        target.GetComponent<EnemyController>().TakeDamage(damage);
        // Set any additional properties or behaviors for the bullet
        Debug.Log("Shooting at " + target.name);
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

        Debug.Log("Equipped " + currentWeapon.WeaponName);
    }

    public void HideAllWeapons()
    {
        foreach (GameObject wp in weapons)
        {
            wp.SetActive(false);
        }
    }
}

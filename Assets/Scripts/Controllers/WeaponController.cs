using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private PlayerController player;

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
        if (currentWeapon == null)
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
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        // Perform the shooting logic
        target.GetComponent<EnemyController>().TakeDamage(damage);
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

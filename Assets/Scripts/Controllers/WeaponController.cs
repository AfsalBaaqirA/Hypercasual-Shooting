using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem catridgeEjection;

    private Weapon currentWeapon;
    private Animator animator;
    private Transform target;
    private float shootingRange;
    private float nextFireTime = 0f;
    private float fireRate;
    private string weaponName;
    private ObjectPooler objectPooler;
    private GameObject[] enemies;

    public Weapon CurrentWeapon => currentWeapon;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        HideAllWeapons();
    }

    private void Update()
    {
        if (currentWeapon == null || !GameManager.Instance.IsGameInProgress())
            return;

        FindNearestEnemy();

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
            target.GetComponent<EnemyController>().HideTargetIndicator();
        }

        target = closestEnemy;

        if (target == null)
            return;

        if (Vector3.Distance(transform.position, target.position) <= shootingRange)
        {
            player.Target = target;
            target.GetComponent<EnemyController>().ShowTargetIndicator();
        }
        else
        {
            player.Target = null;
            target.GetComponent<EnemyController>().HideTargetIndicator();
        }
    }

    private void Shoot()
    {
        if (Time.time < nextFireTime)
            return;

        Projectile bulletObject = null;

        Vector3 direction = (target.position - firePoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        switch (weaponName)
        {
            case "Pistol":
            case "Shotgun":
            case "Machinegun":
                bulletObject = objectPooler.GetBullet(firePoint.position, rotation);
                break;
            case "Launcher":
                bulletObject = objectPooler.GetMissile(firePoint.position, rotation);
                break;
        }

        bulletObject.Weapon = currentWeapon;

        Rigidbody bulletRigidbody = bulletObject.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction * currentWeapon.ProjectileSpeed;
        }

        AudioManager.Instance.PlaySoundEffect(currentWeapon.FireSound);
        muzzleFlash.Play();
        catridgeEjection.Play();

        nextFireTime = Time.time + fireRate;
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        fireRate = currentWeapon.FireRate;
        shootingRange = currentWeapon.ShootingRange;
        weaponName = currentWeapon.WeaponName;

        foreach (GameObject wp in weapons)
        {
            wp.SetActive(wp.name == weaponName);
        }

        animator.Play("Anim-" + weaponName);

        GameManager.Instance.WeaponName = weaponName;

        if (currentWeapon.WeaponName == AdsManager.Instance.MorePowerfulWeapon)
        {
            AdsManager.Instance.CaptureScreenshot();
        }
    }

    public void HideAllWeapons()
    {
        foreach (GameObject wp in weapons)
        {
            wp.SetActive(false);
        }
    }
}

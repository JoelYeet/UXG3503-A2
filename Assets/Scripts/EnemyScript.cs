using UnityEngine;
using UnityEngine.Splines;

public class EnemyScript : MonoBehaviour
{
    public bool isActive = false;

    [Header("Enemy Health Settings")]
    public int maxHealth = 10;
    private int currentHealth;
    public bool canShoot;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 35f;
    public float fireRate = 2f;
    public float fireRateRandomVariance = 0.5f;
    private float nextFireTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        firePoint = gameObject.transform.GetChild(0);
    }

    void Update()
    {
        if (!isActive) return;

        if (Time.time >= nextFireTime)
        {
            ShootProjectile();
            float randomAddition = Random.Range(-fireRateRandomVariance, fireRateRandomVariance);
            nextFireTime = Time.time + fireRate + randomAddition;
        }
    }

    public void SetActive()
    {
        // Use this to move enemies into position when combat starts
        isActive = true;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    private void ShootProjectile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && !player.transform.parent.GetComponent<PlayerController>().isCrouching)
        {
            Vector3 direction = (player.transform.position - firePoint.position).normalized;
            GameObject projectileInstance = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));

            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
}

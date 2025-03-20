using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float fireRate = 0.1f;
    // Raycast distance
    public float range = 100f;
    public int damage = 10;

    // for cooldown
    private float nextFireTime = 0f;
    public Camera camera;

    private void Start()
    {
        camera = gameObject.transform.GetChild(0).transform.GetComponent<Camera>();    
    }
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);
            EnemyScript enemy = hit.collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                 enemy.TakeDamage(damage);
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);
    }
}

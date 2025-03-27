using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    [Header("Player Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Shooting Settings")]
    public float fireRate = 0.1f;
    public float range = 100f;
    public int damage = 10;
    private float nextFireTime = 0f;

    public Camera camera;

    [Header("Crouch Settings")]
    public KeyCode crouchKey = KeyCode.Space;
    public float crouchScaleY = 0.5f;
    public float crouchYOffset = 0.5f;
    public float crouchTransitionSpeed = 5f;   
    private Vector3 originalScale;
    private float baseY;
    public bool isCrouching = false;

    private Collider playerCollider;
    /*public SplineAnimate splineAnimate;*/

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;

        camera = gameObject.transform.GetChild(0).transform.GetComponent<Camera>();
        originalScale = transform.localScale;
        baseY = transform.position.y;
        playerCollider = transform.GetChild(1).transform.GetComponent<Collider>();
        /*splineAnimate = GetComponent<SplineAnimate>();

        splineAnimate.Play();*/
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // check if the crouch key (Space) is held down.
        isCrouching = Input.GetKey(crouchKey);

        // determine target scale based on crouching.
        float targetScaleY = isCrouching ? originalScale.y * crouchScaleY : originalScale.y;
        Vector3 newScale = transform.localScale;
        newScale.y = Mathf.Lerp(transform.localScale.y, targetScaleY, Time.deltaTime * crouchTransitionSpeed);
        transform.localScale = newScale;

        // determine target Y position based on crouching.
        float targetY = baseY - (isCrouching ? crouchYOffset : 0f);
        Vector3 newPos = transform.position;
        newPos.y = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * crouchTransitionSpeed);
        transform.position = newPos;

        // disable collider when in cover
        if (playerCollider != null)
        {
            playerCollider.enabled = !isCrouching;
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {damage}. {currentHealth}");

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Debug.Log("Player died!");
    }

    /*IEnumerator SmoothPause(float duration)
    {
        // Capture the current speed (make sure your splineAnimate has a speed property)
        float initialSpeed = PlayerController.instance.splineAnimate.MaxSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // Ease out quadratic: EaseOutQuad(t) = t * (2 - t)
            float easedT = t * (2 - t);
            // Lerp from initial speed to 0 using the eased value
            PlayerController.instance.splineAnimate.MaxSpeed = Mathf.Lerp(initialSpeed, 0f, easedT);
            yield return null;
        }

        // Ensure the speed is set to 0 and then pause
        PlayerController.instance.splineAnimate.MaxSpeed = 0f;
        PlayerController.instance.splineAnimate.Pause();
    }*/
}

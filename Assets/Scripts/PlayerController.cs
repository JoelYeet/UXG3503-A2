using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour
{
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

    private Collider playerCollider;
    private SplineAnimate splineAnimate; // player movment


    private void Start()
    {
        camera = gameObject.transform.GetChild(0).transform.GetComponent<Camera>();
        originalScale = transform.localScale;
        baseY = transform.position.y;
        playerCollider = transform.GetChild(1).transform.GetComponent<Collider>();
        splineAnimate = GetComponent<SplineAnimate>();

        // Find all GameObjects with tag "Enemy" and add them to the list.
    }
    void Update()
    {
        if (GameController.Instance.AreAllEnemiesDead())
        {
            // Trigger the animation.
            splineAnimate.Play();
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // check if the crouch key (Space) is held down.
        bool crouching = Input.GetKey(crouchKey);

        // determine target scale based on crouching.
        float targetScaleY = crouching ? originalScale.y * crouchScaleY : originalScale.y;
        Vector3 newScale = transform.localScale;
        newScale.y = Mathf.Lerp(transform.localScale.y, targetScaleY, Time.deltaTime * crouchTransitionSpeed);
        transform.localScale = newScale;

        // determine target Y position based on crouching.
        float targetY = baseY - (crouching ? crouchYOffset : 0f);
        Vector3 newPos = transform.position;
        newPos.y = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * crouchTransitionSpeed);
        transform.position = newPos;

        // disable collider when in cover
        if (playerCollider != null)
        {
            playerCollider.enabled = !crouching;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.PlayerTakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Cover"))
        {
            Destroy(gameObject);
        }
    }
}

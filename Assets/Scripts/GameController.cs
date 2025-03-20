using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Player Health Settings")]
    public int playerMaxHealth = 100;
    public int playerCurrentHealth;

    [Header("Section 1")]
    public List<GameObject> enemyListSec1 = new List<GameObject>();
    /*public int player1MaxHealth = 100;
    public int player1CurrentHealth;

    public int player2MaxHealth = 100;
    public int player2CurrentHealth;*/

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemyListSec1.Add(enemy);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        playerCurrentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + playerCurrentHealth);
        if (playerCurrentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player died!");
    }

    public bool AreAllEnemiesDead()
    {
        // Loop through the enemy list. If any enemy still exists (not null), return false.
        foreach (GameObject enemy in enemyListSec1)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        // If the loop completes without finding any active enemies, then all are dead.
        return true;
    }
}

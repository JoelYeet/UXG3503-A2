using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Player Health Settings")]
    public int playerMaxHealth = 100;
    public int playerCurrentHealth;

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
}

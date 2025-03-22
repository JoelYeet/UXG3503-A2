using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    [Header("Zone Enemies")]
    public List<GameObject> enemyList = new List<GameObject>();

    private bool isTriggered = false;
    private bool hasEndedZone = false;

    private void Update()
    {
        if (isTriggered && !hasEndedZone)
        {
            if (AreAllEnemiesDead())
            {
                hasEndedZone = true;
                PlayerController.instance.splineAnimate.Play();
            }
        }
    }

    public bool AreAllEnemiesDead()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggered = true;

            foreach (GameObject enemy in enemyList)
            {
                enemy.GetComponent<EnemyScript>().SetActive();
            }

            PlayerController.instance.splineAnimate.Pause();
        }
    }
}

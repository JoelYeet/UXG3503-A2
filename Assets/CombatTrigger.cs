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
                /*PlayerController.instance.splineAnimate.Play();*/
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

            /*StartCoroutine(SmoothPause(3f));*/
        }
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

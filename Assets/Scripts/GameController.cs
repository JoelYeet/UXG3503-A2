using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject); 
        else instance = this;
    }
}

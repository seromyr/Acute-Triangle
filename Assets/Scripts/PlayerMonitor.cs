using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    public static PlayerMonitor main;

    private Vector3 startPos;

    private void Awake()
    {
        SingletonMaker();
        startPos = transform.position;
    }

    public void Reset()
    {
        transform.rotation = Quaternion.identity;
        transform.position = startPos;
    }

    void Update()
    {
        
    }

    private void SingletonMaker()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
            Debug.Log("Player is created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }
}

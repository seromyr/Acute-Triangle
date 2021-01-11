using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    public static UI_Control main;

    private void Awake()
    {
        // Make the Control Panel a Singleton
        SingletonMaker();
    }

    private void SingletonMaker()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
            Debug.Log("Control Panel created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }
}

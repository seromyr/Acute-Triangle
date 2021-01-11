using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WinPanel : MonoBehaviour
{
    public static UI_WinPanel main;

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
            Debug.Log("Win Panel created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }
}

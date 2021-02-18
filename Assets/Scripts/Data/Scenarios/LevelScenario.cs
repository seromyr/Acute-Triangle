using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.SceneManagement;

public class LevelScenario : MonoBehaviour
{
    private void Start()
    {
        // Get current scene name to load level scenario
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case Map.LV000:
                gameObject.AddComponent<LevelScenario_01>();
                break;
            case Map.LV001:
                gameObject.AddComponent<LevelScenario_02>();
                break;
            case Map.LV002:
                gameObject.AddComponent<LevelScenario_03>();
                break;
            case Map.LV003:
                gameObject.AddComponent<LevelScenario_04>();
                break;
            case Map.LV004:
                gameObject.AddComponent<LevelScenario_05>();
                break;
            case Map.LV005:
                gameObject.AddComponent<LevelScenario_06>();
                break;
            case Map.LV006:
                gameObject.AddComponent<LevelScenario_07>();
                break;
            case Map.LV007:
                gameObject.AddComponent<LevelScenario_08>();
                break;
            case Map.LV008:
                gameObject.AddComponent<LevelScenario_09>();
                break;
            case Map.LV009:
                gameObject.AddComponent<LevelScenario_10>();
                break;
        }
    }
}

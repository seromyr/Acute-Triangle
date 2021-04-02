using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.SceneManagement;

public class LevelScenario : MonoBehaviour
{
    private GameObject boss;

    private void Awake()
    {
        if (Player.main == null)
        {
            SceneManager.LoadScene("Preload");
        }
    }

    private void Start()
    {
        // Get current scene name to load level scenario
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case Map.LV001:
                gameObject.AddComponent<LevelScenario_01>();
                break;
            case Map.LV002:
                gameObject.AddComponent<LevelScenario_02>();
                break;
            case Map.LV003:
                gameObject.AddComponent<LevelScenario_03>();
                break;
            case Map.LV004:
                gameObject.AddComponent<LevelScenario_04>();
                break;
            case Map.LV005:
                gameObject.AddComponent<LevelScenario_05>();
                break;
            case Map.LV006:
                gameObject.AddComponent<LevelScenario_06>();
                break;
            case Map.LV007:
                gameObject.AddComponent<LevelScenario_07>();
                break;
            case Map.LV008:
                gameObject.AddComponent<LevelScenario_08>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(1);
                break;
            case Map.LV009:
                gameObject.AddComponent<LevelScenario_09>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(2);
                break;
            case Map.LV010:
                gameObject.AddComponent<LevelScenario_10>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(3);
                break;
            case Map.LV011:
                gameObject.AddComponent<LevelScenario_11>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(4);
                break;
            case Map.LV012:
                gameObject.AddComponent<LevelScenario_12>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(5);
                break;
            case Map.LV013:
                gameObject.AddComponent<LevelScenario_13>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(6);
                break;
            case Map.LV014:
                gameObject.AddComponent<LevelScenario_14>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(7);
                break;
            case Map.LV015:
                gameObject.AddComponent<LevelScenario_15>();
                UI_InGameMenu_Mechanic.main.ChangeBossHPBars(8);
                break;
            case Map.LV016:
                gameObject.AddComponent<LevelScenario_16>();
                break;
            case Map.LV017:
                gameObject.AddComponent<LevelScenario_17>();
                break;
            case Map.LV018:
                gameObject.AddComponent<LevelScenario_18>();
                break;
            case Map.LV019:
                gameObject.AddComponent<LevelScenario_19>();
                break;
            case Map.LV020:
                gameObject.AddComponent<LevelScenario_20>();
                break;
        }
    }

    public void GetBoss()
    {

    }
}


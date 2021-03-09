using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_05 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int bossBlasterCount;

    private void Awake()
    {
        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer").transform;
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();
    }

    // Scenario 01 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-1]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, -20));

        // Add enemy into the list
        boss = new Enemy_Default
        (
            // Boss name
            "Boss",
            // Boss appearance
            Enemy.Sphere_Large_Black,
            // Boss placemenent
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            5,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

    }

    #region Scenario Stuff
    private void BossMonitor(object sender, EventArgs e)
    {
        // Victory Condition
        if (!boss.IsAlive)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss remaining");
        }
    }
}
    #endregion

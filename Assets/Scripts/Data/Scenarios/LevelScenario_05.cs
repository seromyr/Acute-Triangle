using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_05 : MonoBehaviour
{
    private List<EnemyEntity> _enemyList;

    private GameObject enemyContainer;

    private int bossCount;

    private void Awake()
    {
        // Create a list of enemy to use in any level
        _enemyList = new List<EnemyEntity>();

        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer");
    }

    private void Start()
    {
        // Clear the enemy list to clean garbage
        _enemyList.Clear();

        // Instantiate level scenario
        BuildScenario();
    }

    // Scenario 05 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-5]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, 0));

        // Add enemy into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Boss",
                // Boss appearance
                Enemy.Sphere_Large_Black,
                // Boss placemenent
                enemyContainer.transform,
                // Boss material
                "default",
                // Boss health
                30,
                // Register dead event action
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;

        // *IMPORTANT* Get enemy container reference for features accessing
        _enemyList[0].Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        // Enable self rotation mode
        //_enemyList[0].Mechanics.Add(Mechanic.SelfRotation);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 10));
    }

    #region Scenario Stuff
    private void BossCountMonitor(object sender, EventArgs e)
    {
        bossCount--;

        // Victory Condition
        if (bossCount == 0)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss left");

            _enemyList.Clear();
        }
    }
    #endregion
}

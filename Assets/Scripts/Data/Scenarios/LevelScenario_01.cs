using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_01 : MonoBehaviour
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

    // Scenario 01 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-1]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, -20));

        // Add enemy into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Boss",
                // Boss appearance
                Enemy.Sphere_Large,
                // Boss placemenent
                enemyContainer.transform,
                // Boss material
                "Shader Graphs_Boss_01_Shader",
                // Boss health
                30,
                // Register dead event action
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;

        // Enable self rotation mode
        _enemyList[0].Mechanics.Add(Mechanic.SelfRotation);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 10));

        // Set patrol parameter
        _enemyList[0].Mechanics.Add(Mechanic.Patrol);
        _enemyList[0].Mechanics.SetPatrolParams(true, Direction.Right, 8, 0.4f);

        _enemyList[0].Mechanics.Add(Mechanic.AggressiveRadius);
        _enemyList[0].Mechanics.ProximityMonitor.OnEnterProximity += EnableAll;
        _enemyList[0].Mechanics.ProximityMonitor.OnExitProximity += EnableOne;

        // Add shooters
        _enemyList[0].Mechanics.Add(Mechanic.Shoot);

        int cannonCount = 0;

        for (int i = 0; i < 6; i++)
        {
            _enemyList[0].Mechanics.SetShootingParams(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.Euler(new Vector3(0, cannonCount, 0)), 50, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Destructible);
            cannonCount += 60;
        }

        // Default boss state
        EnableOne(null, null);
    }

    private void EnableOne(object sender, EventArgs e)
    {
        for (int i = 1; i < 6; i++)
        {
            _enemyList[0].Mechanics.Cannons[i].SetActive(false);
        }
        _enemyList[0].Mechanics.SetRotationParameters(100f);
    }

    private void EnableAll(object sender, EventArgs e)
    {
        for (int i = 0; i < 6; i++)
        {
            _enemyList[0].Mechanics.Cannons[i].SetActive(true);
        }
        _enemyList[0].Mechanics.SetRotationParameters(36f);
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

            _enemyList[0].Mechanics.ProximityMonitor.OnEnterProximity -= EnableAll;
            _enemyList[0].Mechanics.ProximityMonitor.OnExitProximity -= EnableOne;

            _enemyList.Clear();
        }
    }
    #endregion
}

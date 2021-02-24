using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_05 : MonoBehaviour
{
    private List<EnemyEntity> _enemyList;

    private GameObject enemyContainer;

    private int bossCount, cannonCount;

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
        _enemyList[0].SetPosition(new Vector3(56.25f, 0.5f, 26.5f));


        // Add cannons
        cannonCount = 4;
        float cannonAngle = 90;
        _enemyList[0].Mechanics.Add(Mechanic.Shoot);
        //_enemyList[0].Mechanics.CreateCannon(Quaternion.identity, 1,1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Destructible);
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 45, cannonAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);

        cannonCount = 4;
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Indestructible);

        //// Default boss cannon state
        //EnableFistShooter(null, null);
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

            //_enemyList[0].Mechanics.ProximityMonitor.OnEnterProximity -= EnableAllShooters;
            //_enemyList[0].Mechanics.ProximityMonitor.OnExitProximity -= EnableFistShooter;

            _enemyList.Clear();
        }
    }
    #endregion
}

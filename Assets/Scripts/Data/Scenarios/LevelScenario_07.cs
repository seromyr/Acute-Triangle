using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_07 : MonoBehaviour
{
    private List<EnemyEntity> _enemyList;

    private GameObject enemyContainer;

    private int bossCount, cannonCount, bossStates, currentBossState;

    private Timer bossStateTimer;

    private void Awake()
    {
        // Create a list of enemy to use in any level
        _enemyList = new List<EnemyEntity>();

        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer");

        // Create timer to change boss state
        bossStateTimer = gameObject.AddComponent<Timer>();
    }

    private void Start()
    {
        // Clear the enemy list to clean garbage
        _enemyList.Clear();

        // Instantiate level scenario
        BuildScenario();
    }

    // Scenario 07 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-7]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, 0));

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;

        // Add enemy into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Beholder",
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

        // *IMPORTANT* Get enemy container reference for features accessing
        _enemyList[0].Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        // This boss has 3 states
        bossStates = 3;
        currentBossState = bossStates--;

        // Setup timer to change boss state
        bossStateTimer.SetTimer(2, 1, () =>
        {
            currentBossState = (currentBossState + 1) % bossStates;
            //Debug.LogError(currentBossState);

            switch (currentBossState)
            {
                default:
                case 0:
                    ActivateStateOne();
                    break;
                case 1:
                    ActivateStateTwo();
                    break;
                case 2:
                    ActivateStateThree();
                    break;
            }
        });


        bossStateTimer.SetLoop(true);


        // Enable self rotation mode
        //_enemyList[0].Mechanics.Add(Mechanic.SelfRotation);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 10));

        //// Set patrol parameter
        //_enemyList[0].Mechanics.Add(Mechanic.Patrol);
        //_enemyList[0].Mechanics.SetPatrolParams(true, Direction.Right, 8, 0.4f);

        //_enemyList[0].Mechanics.Add(Mechanic.AggressiveRadius);
        //_enemyList[0].Mechanics.ProximityMonitor.OnEnterProximity += EnableAllShooters;
        //_enemyList[0].Mechanics.ProximityMonitor.OnExitProximity += EnableFistShooter;

        //// Add cannons
        //cannonCount = 6;
        //float cannonAngle = 60;
        //_enemyList[0].Mechanics.Add(Mechanic.Shoot);
        //_enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Destructible);

        //// Default boss cannon state
        //EnableFistShooter(null, null);
    }

    private void ActivateStateOne()
    {
        Debug.LogError("1 State " + currentBossState);
    }

    private void ActivateStateTwo()
    {
        Debug.LogError("2 State " + currentBossState);
    }

    private void ActivateStateThree()
    {
        Debug.LogError("3 State " + currentBossState);
    }
    //private void EnableFistShooter(object sender, EventArgs e)
    //{
    //    for (int i = 1; i < cannonCount; i++)
    //    {
    //        _enemyList[0].Mechanics.Cannons[i].SetActive(false);
    //    }
    //    _enemyList[0].Mechanics.SetRotationParameters(100f);
    //}

    //private void EnableAllShooters(object sender, EventArgs e)
    //{
    //    for (int i = 0; i < cannonCount; i++)
    //    {
    //        _enemyList[0].Mechanics.Cannons[i].SetActive(true);
    //    }
    //    _enemyList[0].Mechanics.SetRotationParameters(36f);
    //}

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

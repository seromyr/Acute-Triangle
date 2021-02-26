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

    private float cannonAngle;

    private Vector3[] localWaypoints;

    private void Awake()
    {
        // Create a list of enemy to use in any level
        _enemyList = new List<EnemyEntity>();

        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer");

        // Create timer to change boss state
        bossStateTimer = gameObject.AddComponent<Timer>();

        localWaypoints = new Vector3[]
        {
            new Vector3(0.23f, 0.5f, 0.15f),
            new Vector3(21.98f, 0.5f, 21.9f),
            new Vector3(21.98f, 0.5f, -21.9f),
            new Vector3(-21.98f, 0.5f, 21.9f),
            new Vector3(-21.98f, 0.5f, -21.9f),
        };
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
        Player.main.SetPosition(new Vector3(0.23f, 0, -20));

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
                100,
                // Register dead event action
                BossCountMonitor
            )
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        _enemyList[0].Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        // This boss has 3 states
        bossStates = 3;
        currentBossState = 0;

        // Set default position
        _enemyList[0].SetPosition(new Vector3(0.23f, 0.5f, 0.15f));

        // Add shield mechanic
        _enemyList[0].Mechanics.Add(Mechanic.Shield);
        _enemyList[0].Mechanics.SwitchToVioletShield();

        // Add look at player mechanic
        _enemyList[0].Mechanics.Add(Mechanic.LookAtPlayer);
        _enemyList[0].Mechanics.SetLookingSpeed(360);

        // Add shoot mechanic
        _enemyList[0].Mechanics.Add(Mechanic.Shoot);
       
        // Add complex movement mechanic
        _enemyList[0].Mechanics.Add(Mechanic.ComplexeMovement);

        // Add self rotation mechanic
        _enemyList[0].Mechanics.Add(Mechanic.SelfRotation);
        _enemyList[0].Mechanics.SetRotationParameters(false);

        // Setup timer to change boss state after 10 seconds
        bossStateTimer.SetTimer(10, 1, () =>
        {
            // Loop through boss state
            currentBossState = (currentBossState + 1) % bossStates;
            _enemyList[0].Transform.rotation = Quaternion.identity;
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

        // Keep boss state switching recurring
        bossStateTimer.SetLoop(true);

        // Default state
        ActivateStateOne();
    }

    private void ActivateStateOne()
    {
        Debug.Log("I - State " + currentBossState);
       
        ActivateAttackPattern(1);

        _enemyList[0].HitMonitor.SetDamageAcceptance(false);
        _enemyList[0].Mechanics.ActivateShield();
        _enemyList[0].Mechanics.SetRotationStatus(false);
        _enemyList[0].Mechanics.SetLookingStatus(true);
        _enemyList[0].Mechanics.SetRunningAroundParams(true, 6);

    }

    private void ActivateStateTwo()
    {
        Debug.Log("II - State " + currentBossState);
        ActivateAttackPattern(2);

        _enemyList[0].HitMonitor.SetDamageAcceptance(true);
        _enemyList[0].Mechanics.DeactivateShield();
        _enemyList[0].Mechanics.SetLookingStatus(false);
        _enemyList[0].Mechanics.SetGoToWayPointParams(true, localWaypoints[UnityEngine.Random.Range(0,4)], 8);
    }

    private void ActivateStateThree()
    {
        Debug.Log("III - State " + currentBossState);
        ActivateAttackPattern(3);

        _enemyList[0].Mechanics.SetRotationStatus(true);
    }

    private void ActivateAttackPattern(int id)
    {
        // Clear all cannon objects
        _enemyList[0].Mechanics.DestroyAllCannons();

        switch (id)
        {
            default:
            case 1:
                cannonCount = 1;
                cannonAngle = 0;
                _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 0.45f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE + 1, BulletType.Destructible);
                break;

            case 2:
                cannonCount = 8;
                cannonAngle = 45;
                _enemyList[0].Mechanics.Add(Mechanic.Shoot);
                _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 3.25f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 3, BulletType.Destructible);

                cannonCount = 180;
                cannonAngle = 2;
                _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 2f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 1, BulletType.Indestructible);
                break;

            case 3:
                cannonCount = 4;
                cannonAngle = 90;
                _enemyList[0].Mechanics.Add(Mechanic.Shoot);
                _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 45, cannonAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE - 4, BulletType.Destructible);

                cannonCount = 4;
                _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, (2 / 3f), 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 1, BulletType.Indestructible);

                break;
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_07 : MonoBehaviour
{
    private EnemyEntity boss;

    private Transform enemyContainer;

    private int bossBlasterCount, bossStates, currentBossState;

    private Timer bossStateTimer;

    private float shootingAngle;

    private Vector3[] localWaypoints;

    private void Awake()
    {
        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer").transform;

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
        // Instantiate level scenario
        BuildScenario();
    }

    // Scenario 07 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-7]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0.23f, 0, -20));

        // Add enemy into the list
        boss = new Enemy_Default
        (
            // Boss name
            "Beholder",
            // Boss appearance
            Enemy.Sphere_Large_Black,
            // Boss placemenent
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            100,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // This boss has 3 states
        bossStates = 3;
        currentBossState = 0;

        // Set default position
        boss.SetPosition(new Vector3(0.23f, 0.5f, 0.15f));

        // Activate Shield mechanic
        boss.Mechanics.Add(Mechanic.Shield);
        boss.Mechanics.SwitchToVioletShield();

        // Activate Look At Player mechanic
        boss.Mechanics.Add(Mechanic.LookAtPlayer);
        boss.Mechanics.SetLookingSpeed(360);

        // Add blasters to boss
        boss.Mechanics.Add(Mechanic.Shoot);
       
        // Activate Complex Movement mechanic
        boss.Mechanics.Add(Mechanic.ComplexeMovement);

        // Activate Self Rotation mechanic
        boss.Mechanics.Add(Mechanic.SelfRotation);
        boss.Mechanics.SetRotationParameters(false);

        // Setup timer to change boss state after 10 seconds
        bossStateTimer.SetTimer(10, 1, () =>
        {
            // Loop through boss state
            currentBossState = (currentBossState + 1) % bossStates;
            boss.Transform.rotation = Quaternion.identity;
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

        boss.HitMonitor.SetDamageAcceptance(false);
        boss.Mechanics.ActivateShield();
        boss.Mechanics.SetRotationStatus(false);
        boss.Mechanics.SetLookingStatus(true);
        boss.Mechanics.SetRunningAroundParams(true, 6);

    }

    private void ActivateStateTwo()
    {
        Debug.Log("II - State " + currentBossState);
        ActivateAttackPattern(2);

        boss.HitMonitor.SetDamageAcceptance(true);
        boss.Mechanics.DeactivateShield();
        boss.Mechanics.SetLookingStatus(false);
        boss.Mechanics.SetGoToWayPointParams(true, localWaypoints[UnityEngine.Random.Range(0,4)], 8);
    }

    private void ActivateStateThree()
    {
        Debug.Log("III - State " + currentBossState);
        ActivateAttackPattern(3);

        boss.Mechanics.SetRotationStatus(true);
    }

    private void ActivateAttackPattern(int id)
    {
        // Clear all cannon objects
        boss.Mechanics.DestroyAllCannons();

        switch (id)
        {
            default:
            case 1:
                bossBlasterCount = 1;
                shootingAngle = 0;
                boss.Mechanics.CreateMultipleCannons(bossBlasterCount, 0, shootingAngle, 0.45f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE + 1, BulletType.Destructible);
                break;

            case 2:
                bossBlasterCount = 8;
                shootingAngle = 45;
                boss.Mechanics.Add(Mechanic.Shoot);
                boss.Mechanics.CreateMultipleCannons(bossBlasterCount, 0, shootingAngle, 3.25f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 3, BulletType.Destructible);

                bossBlasterCount = 180;
                shootingAngle = 2;
                boss.Mechanics.CreateMultipleCannons(bossBlasterCount, 0, shootingAngle, 2f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 1, BulletType.Indestructible);
                break;

            case 3:
                bossBlasterCount = 4;
                shootingAngle = 90;
                boss.Mechanics.Add(Mechanic.Shoot);
                boss.Mechanics.CreateMultipleCannons(bossBlasterCount, 45, shootingAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE - 4, BulletType.Destructible);

                bossBlasterCount = 4;
                boss.Mechanics.CreateMultipleCannons(bossBlasterCount, 0, shootingAngle, (2 / 3f), 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 1, BulletType.Indestructible);

                break;
        }
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
    #endregion
}

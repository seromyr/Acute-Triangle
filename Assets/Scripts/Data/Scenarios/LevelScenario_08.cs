using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_08 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int blasterCount;

    private void Awake()
    {
        // Create enemy container for organized objects managing
        enemyContainer = new GameObject("EnemyContainer").transform;
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();
    }

    // Scenario 01 [https://sites.google.com/view/acutetriangle/game-design/enemy-design/level-1-boss-juliette]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, -10));

        // Create level boss
        boss = new Enemy_Default
        (
            // Boss name
            "Julliette",
            // Boss appearance
            Enemy.Boss_01,
            // Boss placemenent
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            30,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Activate Self Rotation mechanic
        boss.Mechanics.Add(Mechanic.SelfRotation);

        // Set boss default position
        boss.SetPosition(new Vector3(0, 0.5f, 0));

        // Activate Aggressive Proximity mechanic
        boss.Mechanics.Add(Mechanic.AggressiveRadius);
        boss.Mechanics.SetAuraProximityIndicator(1);
        boss.Mechanics.SetAgressiveDiameteMutiplierr(7f);
        boss.Mechanics.ProximityMonitor.OnEnterProximity += AggressiveState;
        boss.Mechanics.ProximityMonitor.OnExitProximity += NonAggresiveState;

        // Activate Patrol mechanic
        boss.Mechanics.Add(Mechanic.Patrol);
        boss.Mechanics.SetPatrolParams(true, Direction.Right, 8, 0.8f);

        // Add blasters to boss
        blasterCount = 6;
        float cannonAngle = 60;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateBlasters(blasterCount, 0, cannonAngle, 0.1f, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Mixed);

        // Set boss default state
        NonAggresiveState(null, null);

        #region Create Sentries
        List<Enemy_Default> sentries = new List<Enemy_Default>();
        Vector3[] sentryPositions = new Vector3[]
        {
            new Vector3(-4,0.5f, 4),
            //new Vector3(-2,0.5f, 4),
            //new Vector3( 0,0.5f, 4),
            //new Vector3( 2,0.5f, 4),
            new Vector3( 4,0.5f, 4),
            //new Vector3(-4,0.5f, 2),
            //new Vector3( 4,0.5f, 2),
            //new Vector3(-4,0.5f,-2),
            //new Vector3( 4,0.5f,-2),
            new Vector3(-4,0.5f,-4),
            //new Vector3(-2,0.5f,-4),
            //new Vector3( 0,0.5f,-4),
            //new Vector3( 2,0.5f,-4),
            new Vector3( 4,0.5f,-4),
        };

        for (int i = 0; i < sentryPositions.Length; i++)
        {
            sentries.Add
                (
                    new Enemy_Default
                        (
                            // Boss name
                            "Sentry 0" + i,
                            // Boss appearance
                            Enemy.Cylinder_Medium_Black,
                            // Boss placemenent
                            enemyContainer,
                            // Boss material
                            "default",
                            // Boss health
                            10,
                            // Register dead event action
                            null
                        )
                );

            // Set Sentry default position
            sentries[i].SetPosition(sentryPositions[i]);
            sentries[i].Mechanics.Add(Mechanic.Shoot);

            blasterCount = 4;
            float blasterAngle = 90;
            sentries[i].Mechanics.CreateBlasters(blasterCount, 45, blasterAngle, 0.5f, 5, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Mixed);
        }

        #endregion
    }

    private void NonAggresiveState(object sender, EventArgs e)
    {
        for (int i = 1; i < blasterCount; i++)
        {
            boss.Mechanics.Blasters[i].SetActive(false);
        }

        boss.Mechanics.SetRotationParameters(true, 100f);
        boss.Mechanics.SetPatrollingStatus(true);
    }

    private void AggressiveState(object sender, EventArgs e)
    {
        for (int i = 0; i < blasterCount; i++)
        {
            boss.Mechanics.Blasters[i].SetActive(true);
        }
        boss.Mechanics.SetRotationParameters(true, 36f);
        boss.Mechanics.SetPatrollingStatus(false);
    }

    #region Scenario Stuff
    private void BossMonitor(object sender, EventArgs e)
    {
        // Victory Condition
        if (!boss.IsAlive)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss remaining");

            boss.Mechanics.ProximityMonitor.OnEnterProximity -= AggressiveState;
            boss.Mechanics.ProximityMonitor.OnExitProximity -= NonAggresiveState;
        }
    }
    #endregion
}

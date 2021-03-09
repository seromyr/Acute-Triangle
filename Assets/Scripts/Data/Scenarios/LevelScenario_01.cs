using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_01 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int bossBlasterCount;

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
        Player.main.SetPosition(new Vector3(0, 0, -20));

        // Add enemy into the list
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
        boss.SetPosition(new Vector3(0, 0.5f, 10));

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
        bossBlasterCount = 6;
        float cannonAngle = 60;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateMultipleBlasters(bossBlasterCount, 0, cannonAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Destructible);

        // Set boss default state
        NonAggresiveState(null, null);
    }

    private void NonAggresiveState(object sender, EventArgs e)
    {
        for (int i = 1; i < bossBlasterCount; i++)
        {
            boss.Mechanics.Blasters[i].SetActive(false);
        }

        boss.Mechanics.SetRotationParameters(true, 100f);
        boss.Mechanics.SetPatrollingStatus(true);
    }

    private void AggressiveState(object sender, EventArgs e)
    {
        for (int i = 0; i < bossBlasterCount; i++)
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

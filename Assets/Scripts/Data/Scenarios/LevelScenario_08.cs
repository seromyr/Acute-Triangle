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
    private float blasterAngle;

    private void Awake()
    {
        // Create enemy container for organized objects managing
        enemyContainer = new GameObject("EnemyContainer").transform;
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();

        // Send mission instruction
        UI_InGameMenu_Mechanic.main.SendInstruction("Defeat Julliette The Dancer");
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
            "Boss_Julliette",
            // Boss appearance
            Enemy.Boss_01,
            // Boss placemenent
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            20,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Activate Self Rotation mechanic
        boss.Mechanics.Add(Mechanic.SelfRotation);

        // Set boss default position
        boss.SetPosition(new Vector3(0, 0.5f, 0));

        // Activate Shooting mechanic
        boss.Mechanics.Add(Mechanic.Shoot);

        // Activate Chasing mechanic
        boss.Mechanics.Add(Mechanic.ComplexeMovement);

        // Activate Aggressive Proximity mechanic
        boss.Mechanics.Add(Mechanic.AggressiveRadius);
        boss.Mechanics.SetAuraProximityIndicator(1);
        boss.Mechanics.SetAgressiveDiameteMutiplierr(7f);
        boss.Mechanics.ProximityMonitor.OnEnterProximity += AggressiveState;
        boss.Mechanics.ProximityMonitor.OnExitProximity += NonAggresiveState;

        // Set boss default state
        NonAggresiveState(null, null);
    }

    private void NonAggresiveState(object sender, EventArgs e)
    {
        ActivateAttackPattern(1);

        boss.Mechanics.SetRotationParameters(true, 180);
        boss.Mechanics.SetRunningAroundParams(true, 15);
    }

    private void AggressiveState(object sender, EventArgs e)
    {
        ActivateAttackPattern(2);
        boss.Mechanics.SetRotationParameters(true, 36f);
        boss.Mechanics.SetRunningAroundParams(true, 10f);
    }

    private void ActivateAttackPattern(int id)
    {
        // Clear all cannon objects
        boss.Mechanics.DestroyAllCannons();

        switch (id)
        {
            default:
            case 1:
                blasterCount = 1;
                blasterAngle = 0;
                boss.Mechanics.CreateBlasters(blasterCount, 0, blasterAngle, 0.01f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 2, BulletType.Indestructible);
                boss.Mechanics.SetShootingDelay(0, 0.2f);
                break;

            case 2:
                blasterCount = 6;
                blasterAngle = 60;
                boss.Mechanics.CreateBlasters(blasterCount, 0, blasterAngle, 0.02f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 2, BulletType.Destructible);

                for (int i = 0; i < 6; i++)
                {
                    boss.Mechanics.SetShootingDelay(i, 0.1f);
                }

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

            boss.Mechanics.ProximityMonitor.OnEnterProximity -= AggressiveState;
            boss.Mechanics.ProximityMonitor.OnExitProximity -= NonAggresiveState;
        }
    }
    #endregion
}

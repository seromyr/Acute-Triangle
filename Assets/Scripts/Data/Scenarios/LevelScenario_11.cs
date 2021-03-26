using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_11 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int bossBlasterCount;
    private Vector3[] reactorPositions;

    private void Awake()
    {
        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer").transform;
        reactorPositions = new Vector3[]
        {
            new Vector3( 8 , -0.5f ,  8 ),
            new Vector3(-8 , -0.5f ,  8 ),
            new Vector3( 8 , -0.5f , -8 ),
            new Vector3(-8 , -0.5f , -8 )
        };
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();

        // Send mission instruction
        UI_InGameMenu_Mechanic.main.SendInstruction("Defeat Navel The Hoarder");
    }

    // Scenario 04 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-4]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, -10));

        // Add enemy into the list
        boss = new Enemy_Default
        (
            // Boss name
            "Boss_Navel",
            // Boss appearance
            Enemy.Sphere_Medium_Red,
            // Boss placemenent
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            80,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Add blasters to boss
        bossBlasterCount = 9;
        float cannonAngle = 12;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateBlasters(bossBlasterCount, -48, cannonAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);

        bossBlasterCount = 2;
        cannonAngle = 120;
        boss.Mechanics.CreateBlasters(bossBlasterCount, -60, cannonAngle, 0.6f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Indestructible);

        // Activate Hard Shells mechanic
        boss.Mechanics.Add(Mechanic.HardShells);
        boss.Mechanics.CreateShells("Shell_02");
        boss.Mechanics.OnAllPillarsDestroyed += ActivateWeakenState;
        for (int i = 0; i < reactorPositions.Length; i++)
        {
            boss.Mechanics.CreatePillar(reactorPositions[i], enemyContainer);
        }

        boss.Mechanics.OnReactorsRegenerationCallback += () => ActivateInvincibleState(null, null);

        // Set boss default position
        boss.SetPosition(new Vector3(0, 0.5f, 0));

        // Activate Look At Player mechanic
        boss.Mechanics.Add(Mechanic.LookAtPlayer);
        boss.Mechanics.SetLookingSpeed(50);

        // Activate Minion Summoning mechanic
        boss.Mechanics.Add(Mechanic.SummonMinions);
        boss.Mechanics.DeactivateShield();
        boss.Mechanics.OnAllMinionDieCallback += () => boss.HitMonitor.SetDamageAcceptance(false); ;

        // Set boss default state
        ActivateInvincibleState(null, null);
    }

    private void ActivateWeakenState(object sender, EventArgs e)
    {
        isWeaken = true;
        boss.Mechanics.SetShootingStatus(true);
        boss.Mechanics.SetLookingStatus(true);
        boss.HitMonitor.SetDamageAcceptance(true);
    }

    private void ActivateInvincibleState(object sender, EventArgs e)
    {
        isWeaken = false;
        boss.Mechanics.SetShootingStatus(false);
        boss.Mechanics.SetLookingStatus(false);
        boss.HitMonitor.SetDamageAcceptance(false);

        // Local countdown tick for the timer to work
        boss.Mechanics.SetMaximumMinion(6);
        int tick = 6;
        boss.Mechanics.SummonTimer.SetTimer(1f, tick, () =>
        {
            tick--;

            Vector3 minionSpawnSpot = reactorPositions[UnityEngine.Random.Range(0, 4)] + UnityEngine.Random.insideUnitSphere * 2;
            minionSpawnSpot.y = 0;

            boss.Mechanics.SpawnMinion(minionSpawnSpot, 4, 2, 10);
        });

    }

    #region Scenario Stuff
    private void BossMonitor(object sender, EventArgs e)
    {
        // Victory Condition
        if (!boss.IsAlive)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss remaining");

            boss.Mechanics.OnAllPillarsDestroyed -= ActivateWeakenState;
            boss.Mechanics.OnReactorsRegenerationCallback = delegate { };
        }
    }

    private bool isWeaken = false;

    private void FixedUpdate()
    {
        if (isWeaken && boss.IsAlive)
        {
            boss.Mechanics.SplitShells(3f);
        }
        else if (!isWeaken && boss.IsAlive)
        {
            boss.Mechanics.CloseShells();
        }
    }
    #endregion
}
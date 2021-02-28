using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_08 : MonoBehaviour
{
    private EnemyEntity boss;

    private Transform enemyContainer;

    private Vector3[] minionSpawnSpots, pillarSpawnSpots;

    private void Awake()
    {
        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer").transform;

        // Create a collection of spawning spots for minions
        minionSpawnSpots = new Vector3[]
        {
            new Vector3(13.25f      ,   0    ,   0           ),
            new Vector3(9.369165f   ,   0    ,   -9.369164f  ),
            new Vector3(0           ,   0    ,   -13.25f     ),
            new Vector3(-9.369167f  ,   0    ,   -9.369164f  ),
            new Vector3(-13.25f     ,   0    ,   0           ),
            new Vector3(-9.369169f  ,   0    ,   9.369162f   ),
            new Vector3(0           ,   0    ,   13.25f      ),
            new Vector3(9.369162f   ,   0    ,   9.369168f   )
        };

        // Create a collection of spawning spots for pillars
        pillarSpawnSpots = new Vector3[]
        {
            new Vector3(17.41573f   + 0.25f   , -0.5f   ,   0   ),
            new Vector3(0           + 0.25f   , -0.5f   ,   0   ),
            new Vector3(-17.41573f  + 0.25f   , -0.5f   ,   0   ),
        };
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();
    }

    // Scenario 08 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-8]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, -6));

        // Add enemy into the list
        boss = new Enemy_Default
        (
            // Boss name
            "Gearbox",
            // Boss appearance
            Enemy.Sphere_Medium_Red,
            // Boss placemenent, needs to be placed at root for now
            null,
            // Boss material
            "default",
            // Boss health
            50,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Activate Hard Shells mechanic
        boss.Mechanics.Add(Mechanic.HardShells);
        boss.Mechanics.OnAllPillarsDestroyed += ActivateWeakenState;

        for (int i = 0; i < pillarSpawnSpots.Length; i++)
        {
            boss.Mechanics.CreatePillar(pillarSpawnSpots[i], GameObject.Find("PillarContainer").transform);
        }

        boss.Mechanics.OnPillarsRegenerationCallback += () => ActivateInvincibleState(null, null);

        // Set default position
        boss.SetPosition(new Vector3(0, 0.5f, 21));

        boss.Transform.parent = GameObject.Find("RotaryRing").transform;

        // Add blasters to boss
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateCannon(Quaternion.identity, 0.2f, 1, 4, BulletType.Destructible);

        // Activate Look At Player mechanic
        boss.Mechanics.Add(Mechanic.LookAtPlayer);
        boss.Mechanics.SetLookingSpeed(35f);
        boss.Mechanics.SetLookingStatus(true);

        // Activate Summon Minion mechanic
        boss.Mechanics.Add(Mechanic.SummonMinions);
        boss.Mechanics.DeactivateShield();
    }


    private void ActivateWeakenState(object sender, EventArgs e)
    {
        isWeaken = true;
        boss.HitMonitor.SetDamageAcceptance(true);
        boss.Mechanics.SetShootingStatus(false);

        // Local countdown tick for the timer to work
        int tick = 8;
        boss.Mechanics.SetMaximumMinion(tick);

        boss.Mechanics.SummonTimer.SetTimer(0.1f, tick, () =>
        {
            tick--;
            boss.Mechanics.SpawnMinion(minionSpawnSpots[minionSpawnSpots.Length - tick], 1, 3, 6);
        });
    }

    private void ActivateInvincibleState(object sender, EventArgs e)
    {
        isWeaken = false;
        boss.HitMonitor.SetDamageAcceptance(false);
        boss.Mechanics.SetShootingStatus(true);
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

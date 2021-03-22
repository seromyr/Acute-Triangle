using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_10 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int maxMinion;
    private Timer timer;
    private float shootingAngle;

    private void Awake()
    {
        // Create enemy container for organized objects managing
        enemyContainer = new GameObject("EnemyContainer").transform;

        // Create timer to change boss state
        timer = gameObject.AddComponent<Timer>();
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();

        // Send mission instruction
        UI_InGameMenu_Mechanic.main.SendInstruction("Defeat Warwick The Impostor King");
    }

    // Scenario 03 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-3]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, 0));

        boss = new Enemy_Default
        (
            // Boss name
            "Boss_Warwick",
            // Boss appearance
            Enemy.Sphere_Large_Black,
            // Boss container
            enemyContainer.transform,
            // Boss material
            "default",
            // Boss health
            20,
            // Boss dead event handler
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Set Boss default position
        boss.SetPosition(new Vector3(0, 0.5f, 15));

        // Activate Chase mechanic
        boss.Mechanics.Add(Mechanic.Chase);

        // Activate Shoot mechanic
        boss.Mechanics.Add(Mechanic.Shoot);

        // Activate Minion Summoning mechanic
        boss.Mechanics.Add(Mechanic.SummonMinions);
        maxMinion = 6;


        // Setup Retreat state
        boss.Mechanics.OnAllMinionDieCallback += () => ActivateRetreatState();

        // Activate default state
        ActivateSwarmingState();
    }

    private void ActivateSwarmingState()
    {
        // Set chasing params
        boss.Mechanics.SetChaseParams(true, 2);

        // Boss takes no damage until the shield is down
        boss.HitMonitor.SetDamageAcceptance(false);
        boss.Mechanics.ActivateShield();

        // Summon minions
        SummonMinion();
    }

    private void SummonMinion()
    {
        // Stop shooting
        boss.Mechanics.DestroyAllCannons();

        boss.Mechanics.SetMaximumMinion(maxMinion);

        // Set local countdown tick for the timer to work
        int tick = maxMinion;
        boss.Mechanics.SummonTimer.SetTimer(1f, tick, () =>
        {
            Vector3 randomPositionAroundBoss = boss.GetPosition + (Player.main.GetPosition - boss.GetPosition) / 2 + UnityEngine.Random.insideUnitSphere * 2;
            randomPositionAroundBoss.y = 0;

            boss.Mechanics.SpawnMinion(randomPositionAroundBoss, 2.5f, 2, 10);

            tick--;
        });
    }

    private void ActivateRetreatState()
    {
        boss.Mechanics.SetChaseParams(true, -7);

        Quaternion firstAngle = Quaternion.LookRotation(transform.position - Player.main.GetPosition);

        boss.Mechanics.CreateBlasters(1, boss.Transform.rotation.eulerAngles.y, 0, 0.1f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE + 1 , BulletType.Destructible);
        boss.Mechanics.SetShootingDelay(0, 0.5f);


        shootingAngle = 30;
        boss.Mechanics.CreateBlasters(4, boss.Transform.rotation.eulerAngles.y -45, shootingAngle, 0.4f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW + 2, BulletType.Indestructible);
        boss.Mechanics.SetShootingDelay(1, 0.5f);
        boss.Mechanics.SetShootingDelay(2, 0.5f);
        boss.Mechanics.SetShootingDelay(3, 0.5f);
        boss.Mechanics.SetShootingDelay(4, 0.5f);

        // Return to Swarming state after 15 seconds
        timer.SetTimer(15, 1, () => ActivateSwarmingState());
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

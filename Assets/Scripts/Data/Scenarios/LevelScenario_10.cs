using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_10 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;

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
        UI_InGameMenu_Mechanic.main.SendInstruction("Defeat Warwick The Impostor King");
    }

    // Scenario 03 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-3]
    private void BuildScenario()
    {
        boss = new Enemy_Default
        (
            // Boss name
            "Warwick",
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
        boss.SetPosition(new Vector3(0, 0.5f, 20));

        // Activate Chase mechanic
        boss.Mechanics.Add(Mechanic.Chase);
        boss.Mechanics.SetChaseParams(true, 2);

        // Activate Minion Summoning mechanic
        boss.Mechanics.Add(Mechanic.SummonMinions);
        boss.Mechanics.SetMaximumMinion(10);

        // Boss takes no damage until the shield is down
        boss.HitMonitor.SetDamageAcceptance(false);

        // Set local countdown tick for the timer to work
        int tick = 10;
        boss.Mechanics.SummonTimer.SetTimer(0.5f, tick, () =>
        {
            tick--;

            Vector3 randomPositionAroundBoss = UnityEngine.Random.insideUnitSphere * 15;
            randomPositionAroundBoss.y = 0;

            boss.Mechanics.SpawnMinion(boss.GetPosition + randomPositionAroundBoss, 2.5f, 2, 10);
        });

        boss.Mechanics.OnAllMinionDieCallback += () =>
        {
            boss.Mechanics.SetChaseParams(true, -5);

            boss.Mechanics.Add(Mechanic.Shoot);
            Quaternion firstAngle = Quaternion.LookRotation(transform.position - Player.main.GetPosition);

            boss.Mechanics.CreateBlasters(1, boss.Transform.rotation.eulerAngles.y, 0, 0.05f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE, BulletType.Destructible);
            boss.Mechanics.SetShootingDelay(0, 1f);

            boss.Mechanics.CreateBlasters(1, boss.Transform.rotation.eulerAngles.y, 0, 1.05f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE + 2, BulletType.Explosive);
            boss.Mechanics.SetShootingDelay(0, 1f);
        };
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

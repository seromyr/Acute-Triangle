using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_03 : MonoBehaviour
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
            30,
            // Boss dead event handler
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        boss.SetPosition(new Vector3(0, 0.5f, 20));

        // Activate Chase mechanic
        boss.Mechanics.Add(Mechanic.Chase);
        boss.Mechanics.SetChaseParams(true, 2);

        // Activate Minion Summoning mechanic
        boss.Mechanics.Add(Mechanic.SummonMinions);
        boss.Mechanics.SetMaximumMinion(30);

        // Boss takes no damage until the shield is down
        boss.HitMonitor.SetDamageAcceptance(false);

        // Set local countdown tick for the timer to work
        int tick = 30;
        boss.Mechanics.SummonTimer.SetTimer(0.5f, tick, () =>
        {
            tick--;

            Vector3 randomPositionAroundBoss = UnityEngine.Random.insideUnitSphere * 15;
            randomPositionAroundBoss.y = 0;

            boss.Mechanics.SpawnMinion(boss.GetPosition + randomPositionAroundBoss, 4, 2, 10);
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
        }
    }
    #endregion
}

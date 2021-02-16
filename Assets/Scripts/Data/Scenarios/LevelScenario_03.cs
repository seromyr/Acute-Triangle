using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_03 : MonoBehaviour
{
    private List<EnemyEntity> _enemyList;

    private GameObject enemyContainer;

    private int bossCount;

    private void Awake()
    {
        // Create a list of enemy to use in any level
        _enemyList = new List<EnemyEntity>();

        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer");
    }

    private void Start()
    {
        // Clear the enemy list to clean garbage
        _enemyList.Clear();

        // Instantiate level scenario
        BuildScenario();
    }

    // Scenario 03 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-3]
    private void BuildScenario()
    {
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Boss",
                // Boss appearance
                Enemy.Sphere_Large,
                // Boss container
                enemyContainer.transform,
                // Boss material
                "default",
                // Boss health
                30,
                // Boss dead event handler
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 7));

        // Boss takes no damage until the shield is down
        _enemyList[0].Mechanic.SetDamageAcceptance(false);

        // Add Minion Summoning feature
        _enemyList[0].Mechanics.Add(Mechanic.SummonMinions);

        // Set maximum number of minions this boss has
        _enemyList[0].Mechanics.minionCount = 10;

        // Local count down tick for the timer to work
        int tick = 10;
        _enemyList[0].Mechanics.Timer.SetTimer(1f, tick, () => { tick--; _enemyList[0].Mechanics.SpawnMinion(enemyContainer.transform); });
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_06 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;

    private void Awake()
    {
        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer").transform;
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();

        // Send mission instruction
        UI_InGameMenu_Mechanic.main.SendInstruction("Destroy the sphere");
    }

    // Tutorial Level 6
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, 0));

        // Add enemy into the list
        boss = new Enemy_Default
        (
            // Boss name
            "Boss",
            // Boss appearance
            Enemy.Sphere_Medium_Red_HalfShell,
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

        // Set boss default position
        boss.SetPosition(new Vector3(0, 0.5f, 15));

        // Activate Look At Player mechanic
        boss.Mechanics.Add(Mechanic.LookAtPlayer);
        boss.Mechanics.SetLookingSpeed(18);
        boss.Mechanics.SetLookingStatus(true);
    }

    #region Scenario Stuff
    private void BossMonitor(object sender, EventArgs e)
    {
        // Victory Condition
        if (!boss.IsAlive)
        {
            GameManager.main.WinGame();
        }
    }
}
#endregion

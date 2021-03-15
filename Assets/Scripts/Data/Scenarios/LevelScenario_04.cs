using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_04 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int bossBlasterCount;

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

    // Tutorial Level 4
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
            Enemy.Sphere_Large_Black,
            // Boss placemenent
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            10,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Set boss default position
        boss.SetPosition(new Vector3(0, 0.5f, 20));

        // Add blasters to boss
        bossBlasterCount = 1;
        float blasterAngle = 0;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateBlasters(bossBlasterCount, 0, blasterAngle, 0.1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);

        // Activate Look At Player mechanic
        boss.Mechanics.Add(Mechanic.LookAtPlayer);
        boss.Mechanics.SetLookingSpeed(40);
        boss.Mechanics.SetLookingStatus(true);

        #region Create Obstacles
        List<EnemyEntity> obstacles = new List<EnemyEntity>();
        int row = 10;
        int collumn = 5;

        for (int i = 0; i < collumn; i++)
        {
            for (int j = 0; j < row; j++)
            {
                obstacles.Add
                (
                    new Enemy_Default
                    (
                        // Name
                        EnemyName.Cube_Small + " " + (i + j),
                        // Appearance
                        Enemy.Cube_Medium_Black,
                        // Container
                        enemyContainer,
                        // Material
                        "default",
                        // Health
                        2,
                        null
                    )
                );

                // Set position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(-2 + i, 0, 5 + j));
            }
        }
        #endregion
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

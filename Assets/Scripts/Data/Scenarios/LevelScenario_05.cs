using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_05 : MonoBehaviour
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

    // Tutorial Level 5
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
        boss.Mechanics.CreateBlasters(bossBlasterCount, 0, blasterAngle, 0.5f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Mixed);

        // Activate Look At Player mechanic
        boss.Mechanics.Add(Mechanic.LookAtPlayer);
        boss.Mechanics.SetLookingSpeed(180);
        boss.Mechanics.SetLookingStatus(true);

        // Activate Minion Summoning mechanic
        boss.Mechanics.Add(Mechanic.SummonMinions);
        boss.Mechanics.SetMaximumMinion(2);

        // Boss takes no damage until the shield is down
        boss.HitMonitor.SetDamageAcceptance(false);

        // Minion position
        Vector3[] minionPositions = new Vector3[]
        {
            new Vector3(-10, 0, 20),
            new Vector3(10, 0, 20)
        };

        // Set local countdown tick for the timer to work
        int tick = minionPositions.Length;

        boss.Mechanics.SummonTimer.SetTimer(0.5f, tick, () =>
        {
            boss.Mechanics.SpawnMinion(minionPositions[minionPositions.Length - tick], 0, 2, 10);
            tick--;

        });

        #region Create Obstacles
        List<EnemyEntity> obstacles = new List<EnemyEntity>();

        //Cluster 01
        int row = 5;
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
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(-2 + i, 0, 10 + j));
            }
        }

        //Cluster 02
        row = 5;
        collumn = 5;

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
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(-7 + i, 0, 18 + j));
            }
        }

        //Cluster 03
        row = 5;
        collumn = 5;

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
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(3 + i, 0, 18 + j));
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

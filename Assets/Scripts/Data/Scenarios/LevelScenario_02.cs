using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_02 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int bossBlasterCount;

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

    // Scenario 02 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-2]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(Vector3.zero);

        boss = new Enemy_Default
        (
            // Boss name
            "Ragazzino",
            // Boss appearance
            Enemy.Boss_02,
            // Boss container
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            30,
            // Boss dead event handler
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Set boss default position
        boss.SetPosition(new Vector3(24, 0.5f, 21.25f));

        // Add blasters to boss
        bossBlasterCount = 6;
        float cannonAngle = 30;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateMultipleCannons(bossBlasterCount, 195, cannonAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Indestructible);

        // Activate Patrol mechanic
        boss.Mechanics.Add(Mechanic.Patrol);
        boss.Mechanics.SetPatrolParams(true, Direction.Forward, 7, 1f);

        #region Create Destructible Obstacles
        List<EnemyEntity> obstacles = new List<EnemyEntity>();
        // Cluster 01 - 2 rows 4 collumns
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
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
                        5,
                        null
                    )
                );

                // Set default position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(-7.5f + i, 0, 4 + j));
            }
        }

        // Cluster 02 - 6 rows 7 collumns
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
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
                        5,
                        null
                    )
                );

                // Set default position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(-16 + i, 0, 13 + j));

            }
        }

        // Cluster 03 - 5 rows 3 collumns
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
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
                        5,
                        null
                    )
                );

                // Set default position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(8 + i, 0, 19 + j));

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
            Debug.Log("No boss remaining");
        }
    }
    #endregion
}

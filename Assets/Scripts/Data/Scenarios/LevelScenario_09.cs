using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_09 : MonoBehaviour
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

        // Send mission instruction
        UI_InGameMenu_Mechanic.main.SendInstruction("Defeat Ragazzino The Introvert");
    }

    // Scenario 02 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-2]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(Vector3.zero);

        boss = new Enemy_Default
        (
            // Boss name
            "Boss_Ragazzino",
            // Boss appearance
            Enemy.Boss_02,
            // Boss container
            enemyContainer,
            // Boss material
            "default",
            // Boss health
            20,
            // Boss dead event handler
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        boss.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Set boss default position
        boss.SetPosition(new Vector3(15, 0.5f, 0));

        // Activate Chase mechanic
        boss.Mechanics.Add(Mechanic.Chase);
        boss.Mechanics.SetChaseParams(true, -5);

        // Add blasters to boss
        bossBlasterCount = 1;
        float blasterAngle = 180;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateBlasters(bossBlasterCount, 0, blasterAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Explosive);

        #region Create Destructible Obstacles
        List<EnemyEntity> obstacles = new List<EnemyEntity>();

        // Cluster surrounds boss at starting point - 30 rows 30 collumns
        int row = 10;
        int column = 11;
        for (int i = 0; i < column; i++)
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
                        1,
                        null
                    )
                );

                // Set default position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(14 + i, 0, 4 + j));
            }
        }

        row = 10;
        column = 11;
        for (int i = 0; i < column; i++)
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
                        1,
                        null
                    )
                );

                // Set default position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(14 + i, 0, -14f + j));
            }
        }

        row = 28;
        column = 10;
        for (int i = 0; i < column; i++)
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
                        1,
                        null
                    )
                );

                // Set default position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(4 + i, 0, -14 + j));
            }
        }

        row = 28;
        column = 5;
        for (int i = 0; i < column; i++)
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
                        1,
                        null
                    )
                );

                // Set default position
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(25 + i, 0, -14 + j));
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

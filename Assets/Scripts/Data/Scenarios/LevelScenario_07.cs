using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_07 : MonoBehaviour
{
    private EnemyEntity boss;
    private Transform enemyContainer;
    private int bossBlasterCount;
    private Vector3[] reactorPositions;
    private List<EnemyEntity> obstacles;

    private void Awake()
    {
        // Create enemy container for organized object managing
        enemyContainer = new GameObject("EnemyContainer").transform;

        // Create power reactor position
        reactorPositions = new Vector3[]
        {
            new Vector3( -9f , -0.5f , 10 ),
            new Vector3(  9f , -0.5f , 10 ),
        };

        // Create obstacle list
        obstacles = new List<EnemyEntity>();
    }

    private void Start()
    {
        // Instantiate level scenario
        BuildScenario();

        // Send mission instruction
        UI_InGameMenu_Mechanic.main.SendInstruction("Destroy the sphere");
    }

    // Tutorial Level 7
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
            Enemy.Sphere_Medium_Red,
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

        // Activate Hard Shells mechanic
        boss.Mechanics.Add(Mechanic.HardShells);
        boss.Mechanics.CreateShells();
        boss.Mechanics.OnAllPillarsDestroyed += ActivateWeakenState;
        for (int i = 0; i < reactorPositions.Length; i++)
        {
            boss.Mechanics.CreatePillar(reactorPositions[i], enemyContainer);
        }

        boss.Mechanics.OnReactorsRegenerationCallback += () => ActivateInvincibleState(null, null);

        // Set boss default position
        boss.SetPosition(new Vector3(0, 0.5f, 10f));

        // Add blasters to boss
        bossBlasterCount = 1;
        float blasterAngle = 0;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateBlasters(bossBlasterCount, 0, blasterAngle, 0.5f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Mixed);

        // Activate Look At Player mechanic
        boss.Mechanics.Add(Mechanic.LookAtPlayer);
        boss.Mechanics.SetLookingSpeed(18);
        boss.Mechanics.SetLookingStatus(true);

        // Set boss default state
        ActivateInvincibleState(null, null);
        CreateObstacles();
    }

    private void ActivateWeakenState(object sender, EventArgs e)
    {
        isWeaken = true;
        boss.HitMonitor.SetDamageAcceptance(true);

        CreateObstacles();
    }

    private void CreateObstacles()
    {
        foreach (EnemyEntity obstacle in obstacles)
        {
            Destroy(obstacle.Body);
        }
        obstacles.Clear();

        //Cluster 01
        int row = 2;
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

        //Cluster 02
        row = 2;
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
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(-11 + i, 0, 5 + j));
            }
        }

        //Cluster 03
        row = 2;
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
                obstacles[obstacles.Count - 1].SetPosition(new Vector3(7 + i, 0, 5 + j));
            }
        }
    }

    private void ActivateInvincibleState(object sender, EventArgs e)
    {
        isWeaken = false;
         boss.HitMonitor.SetDamageAcceptance(false);
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
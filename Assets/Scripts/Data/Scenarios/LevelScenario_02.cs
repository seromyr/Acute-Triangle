using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_02 : MonoBehaviour
{
    private List<EnemyEntity> _enemyList;

    private GameObject enemyContainer;

    private int bossCount, cannonCount;

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


    // Scenario 02 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-2]
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

        // Add cannons
        cannonCount = 6;
        float cannonAngle = 30;
        _enemyList[0].Mechanics.Add(Mechanic.Shoot);
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 195, cannonAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Indestructible);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(24, 0.5f, 21.25f));

        // Set patrol parameter
        _enemyList[0].Mechanics.Add(Mechanic.Patrol);
        _enemyList[0].Mechanics.SetPatrolParams(true, Direction.Forward, 7, 1f);

        // Add destroyable cubes
        // Cluster 01 - 2 rows 4 collumns
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                _enemyList.Add
                (
                    new Enemy_SmallDestructableObstacle
                    (
                        // Name
                        EnemyName.Cube_Small + " " + (i + j),
                        // Appearance
                        Enemy.Cube_Small,
                        // Container
                        enemyContainer.transform,
                        // Material
                        "default",
                        // Health
                        10
                    )
                );

                // Set default position
                _enemyList[_enemyList.Count - 1].SetPosition(new Vector3(-7.5f + i, 0, 4 + j));

            }
        }

        // Cluster 02 - 6 rows 7 collumns
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                _enemyList.Add
                (
                    new Enemy_SmallDestructableObstacle
                    (
                        // Name
                        EnemyName.Cube_Small + " " + (i + j),
                        // Appearance
                        Enemy.Cube_Small,
                        // Container
                        enemyContainer.transform,
                        // Material
                        "default",
                        // Health
                        10
                    )
                );

                // Set default position
                _enemyList[_enemyList.Count - 1].SetPosition(new Vector3(-16 + i, 0, 13 + j));

            }
        }

        // Cluster 03 - 5 rows 3 collumns
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                _enemyList.Add
                (
                    new Enemy_SmallDestructableObstacle
                    (
                        // Name
                        EnemyName.Cube_Small + " " + (i + j),
                        // Appearance
                        Enemy.Cube_Small,
                        // Container
                        enemyContainer.transform,
                        // Material
                        "default",
                        // Health
                        10
                    )
                );

                // Set default position
                _enemyList[_enemyList.Count - 1].SetPosition(new Vector3(8 + i, 0, 19 + j));

            }
        }
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

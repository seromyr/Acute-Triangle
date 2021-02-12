using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.SceneManagement;

public class LevelScenario : MonoBehaviour
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

        // Get current scene name to load level scenario
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case Map.LV000:
                BuildScenario_00();
                break;
            case Map.LV001:
                BuildScenario_01();
                break;
            case Map.LV002:
                BuildScenario_02();
                break;
            case Map.LV003:
                //BuildScenario_03();
                break;
        }
    }

    // Scenario 01 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-1]
    private void BuildScenario_00()
    {
        // Add enemy into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Boss",
                // Boss appearance
                Enemy.Sphere_Large,
                // Boss placemenent
                enemyContainer.transform,
                // Boss health
                30,
                // Register dead event action
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;

        // Add shooter in front
        _enemyList[0].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.Euler(new Vector3(0, 0, 0)), 50, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Destructible);

        // Enable self rotation mode
        var selfRotation = _enemyList[0].Body.AddComponent<SelfRotation>();
        selfRotation.SetRotationParameters(250);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 10));

        // Set patrol parameter
        //_enemyList[0].Patrol(true, Direction.Right, 8, 0.4f);
        _enemyList[0].Features.Add(Feature.Patrol);
        _enemyList[0].Features.SetPatrolParams(true, Direction.Right, 8, 0.4f);

    }

    // Scenario 02 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-2]
    private void BuildScenario_01()
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
                // Boss health
                30,
                // Boss dead event handler
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;
        // Add 6 shooters
        // Starting shooter angle
        float angle = 195;
        for (int i = 0; i < 6; i++)
        {
            _enemyList[0].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.Euler(new Vector3(0, angle, 0)), 2, 1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Indestructible);
            angle += 30;
        }

        _enemyList[0].SetPosition(new Vector3(24, 0.5f, 21.25f));

        // No self rotation

        // Set patrol parameter
        _enemyList[0].Features.Add(Feature.Patrol);
        _enemyList[0].Features.SetPatrolParams(true, Direction.Forward, 7, 1f);

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
                        // Health
                        10
                    )
                );

                // Set default position
                _enemyList[_enemyList.Count - 1].SetPosition(new Vector3(8 + i, 0, 19 + j));

            }
        }
    }
    
    // Scenario 03 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-3]
    private void BuildScenario_02()
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
                // Boss health
                30,
                // Boss dead event handler
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 7));

        // Add Minion Summoning feature
        _enemyList[0].Features.Add(Feature.SummonMinions);

        // Set maximum number of minions this boss has
        _enemyList[0].Features.minionCount = 10;

        // Local count down tick for the timer to work
        int tick = 10;
        _enemyList[0].Features.Timer.SetTimer(1f, tick, () => { tick--; _enemyList[0].Features.SpawnMinion(enemyContainer.transform); });
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

    #region Deprecated
    private void CreateMinions()
    {
        //// Create minions
        //_enemyList[0].Features.CreateMinions("Minion", "Prefabs/Enemy/Cone", enemyContainer.transform);
        //for (int i = 0; i < 10; i++)
        //{
        //    _enemyList[0].Features.MinionList.Add
        //        (
        //            new Enemy_Minion
        //            (
        //                "Minion " + i,
        //                Enemy.Cone,
        //                enemyContainer.transform,
        //                5,
        //                // Register dead event action
        //                (object Sender, EventArgs e) => { }
        //            )
        //        );
        //    _enemyList[0].Features.MinionList[_enemyList[0].Features.MinionCount - 1].SetPosition(new Vector3(-5 + i, 0.25f, 5));
        //    _enemyList[0].Features.MinionList[_enemyList[0].Features.MinionCount - 1].Features.Add(Feature.Chase);
        //    _enemyList[0].Features.MinionList[_enemyList[0].Features.MinionCount - 1].Features.SetChaseParams(true, 1);
        //    _enemyList[0].Features.MinionList[_enemyList[0].Features.MinionCount - 1].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.identity, 2, 0.5f, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);
        //}
    }

    private void SpawnMinion(int minionCount, EnemyEntity boss)
    {
        //boss.Features.MinionList.Add
        //        (
        //            new Enemy_Minion
        //            (
        //                "Minion " + minionCount,
        //                Enemy.Cone,
        //                enemyContainer.transform,
        //                5,
        //                // Register dead event action
        //                (object Sender, EventArgs e) => { }
        //            )
        //        );
        //int minionID = boss.Features.MinionCount - 1;

        //boss.Features.MinionList[minionID].SetPosition(new Vector3(-5 + minionCount, 0.25f, 5));
        //boss.Features.MinionList[minionID].Features.Add(Feature.Chase);
        //boss.Features.MinionList[minionID].Features.SetChaseParams(true, 1);
        //boss.Features.MinionList[minionID].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.identity, 2, 0.5f, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);
    }
    #endregion
    /*
    private void BuildScenario_03()
    {
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                "Boss",
                Enemy.Sphere_Large,
                enemyContainer.transform,
                30
            )
        );
        _enemyList[0].Avatar.transform.position = new Vector3(0, 0.5f, 7);
        _enemyList[0].Patrol(true, Direction.Right, 8, 0.4f);
    }*/
}

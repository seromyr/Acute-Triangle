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
    }

    // Scenario 05 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-5]
    private void BuildScenario()
    {
        //number of enemies
        int enemyNum = 12;

        // Set player start position
        Player.main.SetPosition(Vector3.zero);

        // Add enemy into the list
        boss = new Enemy_Default
        (
            // Boss name
            "Minotaur",
            // Boss appearance
            Enemy.Sphere_Large_Black,
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

        // Set default position
        boss.SetPosition(new Vector3(56.25f, 0.5f, 26.5f));

        // Add blasters to boss
        bossBlasterCount = 4;
        float cannonAngle = 90;
        boss.Mechanics.Add(Mechanic.Shoot);
        boss.Mechanics.CreateMultipleBlasters(bossBlasterCount, 45, cannonAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);
        
        bossBlasterCount = 4;
        boss.Mechanics.CreateMultipleBlasters(bossBlasterCount, 0, cannonAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Indestructible);
        boss.Mechanics.Add(Mechanic.SummonMinions);
        
        boss.Mechanics.Add(Mechanic.SelfRotation);
        boss.Mechanics.SetRotationParameters(true, 45f);

        // Minion placements / summoning
        boss.Mechanics.SetMaximumMinion(enemyNum);

        boss.HitMonitor.SetDamageAcceptance(false);

        GameObject[] spawnLocations = GameObject.FindGameObjectsWithTag("Enemy Spawn");

        int index = UnityEngine.Random.Range(0, spawnLocations.Length);

        List<int> selected = new List<int>();
        for(int i = 0; i < enemyNum; i++)
        {
            index = DuplicateCatcher(UnityEngine.Random.Range(0, spawnLocations.Length), spawnLocations.Length, selected);
            selected.Add(i);
        }

        for(int spot = 0; spot < enemyNum; spot++)
        {
            boss.Mechanics.SpawnMinion(spawnLocations[selected[spot]].transform.position, 0f, 4, 7.5f);
        }

        

        #region Create Destructible Obstacles / Blockades
        List<EnemyEntity> obstacles = new List<EnemyEntity>();

        for (int x = 0; x < 3; x++)
        {
            for(int y = 0; y < 7; y++)
            {
                obstacles.Add
                (
                    new Enemy_Default
                        (
                            EnemyName.Cube_Small + " " + (x + y),
                            Enemy.Cube_Medium_Black,
                            enemyContainer,
                            "default",
                            10,
                            null
                        )
                );

                obstacles[obstacles.Count - 1].SetPosition(new Vector3(-1f + x, 0, 18.5f + y));
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                obstacles.Add
                (
                    new Enemy_Default
                        (
                            EnemyName.Cube_Small + " " + (x + y),
                            Enemy.Cube_Medium_Black,
                            enemyContainer,
                            "default",
                            10,
                            null
                        )
                );

                obstacles[obstacles.Count - 1].SetPosition(new Vector3(37.75f + x, 0, -18.5f + y));
            }
        }

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                obstacles.Add
                (
                    new Enemy_Default
                        (
                            EnemyName.Cube_Small + " " + (x + y),
                            Enemy.Cube_Medium_Black,
                            enemyContainer,
                            "default",
                            10,
                            null
                        )
                );

                obstacles[obstacles.Count - 1].SetPosition(new Vector3(74.5f + x, 0, 25.5f + y));
            }
        }
        #endregion
    }

    private int DuplicateCatcher(int number, int maxNum, List<int> list)
    {
        if (list.Contains(number))
        {
            return DuplicateCatcher(UnityEngine.Random.Range(0, maxNum), maxNum, list);
        }

        else
        {
            return number;
        }

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_05 : MonoBehaviour
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

    // Scenario 01 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-1]
    private void BuildScenario()
    {
        //number of enemies
        int enemyNum = 12;

        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, 0));

        // Add enemy into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Boss",
                // Boss appearance
                Enemy.Sphere_Large_Black,
                // Boss placemenent
                enemyContainer.transform,
                // Boss material
                "default",
                // Boss health
                30,
                // Register dead event action
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;

        // *IMPORTANT* Get enemy container reference for features accessing
        _enemyList[0].Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        // Enable self rotation mode
        //_enemyList[0].Mechanics.Add(Mechanic.SelfRotation);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(56.25f, 0.5f, 26.5f));


        // Add cannons
        cannonCount = 4;
        float cannonAngle = 90;
        _enemyList[0].Mechanics.Add(Mechanic.Shoot);
        //_enemyList[0].Mechanics.CreateCannon(Quaternion.identity, 1,1, GeneralConst.ENEMY_BULLET_SPEED_FAST, BulletType.Destructible);
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 45, cannonAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);
        _enemyList[0].Mechanics.Add(Mechanic.SelfRotation);
        _enemyList[0].Mechanics.SetRotationParameters(true, 45f);
        cannonCount = 4;
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Indestructible);
        _enemyList[0].Mechanics.Add(Mechanic.SummonMinions);
        
        //enemy placement / summoning
        _enemyList[0].Mechanics.SetMaximumMinion(enemyNum);

        //Vector3[] spawnPoints = new Vector3[10];
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
            _enemyList[0].Mechanics.SpawnMinion(spawnLocations[selected[spot]].transform.position, 0f, 4, 7.5f);
        }
        

        //_enemyList[0].Mechanic

        //// Default boss cannon state
        //EnableFistShooter(null, null);
        
        //blockades
        for(int x = 0; x < 3; x++)
        {
            for(int y = 0; y < 7; y++)
            {
                _enemyList.Add
                (
                    new Enemy_SmallDestructableObstacle
                        (
                            EnemyName.Cube_Small + " " + (x + y),

                            Enemy.Cube_Medium_Black,
                            enemyContainer.transform,
                            "default",
                            10
                        )
                );

                _enemyList[_enemyList.Count - 1].SetPosition(new Vector3(-1f + x, 0, 18.5f + y));
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                _enemyList.Add
                (
                    new Enemy_SmallDestructableObstacle
                        (
                            // Name
                            EnemyName.Cube_Small + " " + (x + y),

                            Enemy.Cube_Medium_Black,
                            enemyContainer.transform,
                            "default",
                            10
                        )
                );

                _enemyList[_enemyList.Count - 1].SetPosition(new Vector3(37.75f + x, 0, -18.5f + y));
            }
        }

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                _enemyList.Add
                (
                    new Enemy_SmallDestructableObstacle
                        (
                            // Name
                            EnemyName.Cube_Small + " " + (x + y),

                            Enemy.Cube_Medium_Black,
                            enemyContainer.transform,
                            "default",
                            10
                        )
                );

                _enemyList[_enemyList.Count - 1].SetPosition(new Vector3(74.5f + x, 0, 25.5f + y));
            }
        }
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

    //private void EnableFistShooter(object sender, EventArgs e)
    //{
    //    for (int i = 1; i < cannonCount; i++)
    //    {
    //        _enemyList[0].Mechanics.Cannons[i].SetActive(false);
    //    }
    //    _enemyList[0].Mechanics.SetRotationParameters(100f);
    //}

    //private void EnableAllShooters(object sender, EventArgs e)
    //{
    //    for (int i = 0; i < cannonCount; i++)
    //    {
    //        _enemyList[0].Mechanics.Cannons[i].SetActive(true);
    //    }
    //    _enemyList[0].Mechanics.SetRotationParameters(36f);
    //}

    #region Scenario Stuff
    private void BossCountMonitor(object sender, EventArgs e)
    {
        bossCount--;

        // Victory Condition
        if (bossCount == 0)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss left");

            //_enemyList[0].Mechanics.ProximityMonitor.OnEnterProximity -= EnableAllShooters;
            //_enemyList[0].Mechanics.ProximityMonitor.OnExitProximity -= EnableFistShooter;

            _enemyList.Clear();
        }
    }
    #endregion
}

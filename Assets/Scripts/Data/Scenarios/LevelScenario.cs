using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.SceneManagement;

public class LevelScenario : MonoBehaviour
{
    private List<EnemyEntity> _enemyList;
    public List<EnemyEntity> EnemyList { get { return _enemyList; } }
    public bool BossIsAlive { get { return BossCheck(); } }

    private GameObject enemyContainer;
    public int Remaining { get { return enemyContainer.transform.childCount; } }

    private string currentScene;

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
        currentScene = SceneManager.GetActiveScene().name;
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
                BuildScenario_03();
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
                30
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        // Add shooter in front
        _enemyList[0].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.Euler(new Vector3(0, 0, 0)), 500, BulletType.Destructible);

        // Enable self rotation mode
        var selfRotation = _enemyList[0].Body.AddComponent<SelfRotation>();
        selfRotation.SetRotationParameters(250);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 10));

        // Set patrol parameter
        _enemyList[0].Patrol(true, Direction.Right, 8, 0.4f);
    }

    // Scenario 01 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-2]
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
                30
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0

        // Add 6 shooters
        // Starting shooter angle
        float angle = 195;
        for (int i = 0; i < 6; i++)
        {
            _enemyList[0].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.Euler(new Vector3(0, angle, 0)), 2, BulletType.Indestructible);
            angle += 30;
        }

        _enemyList[0].Body.transform.position = new Vector3(24, 0.5f, 21.25f);

        // No self rotation

        // Set patrol parameter
        _enemyList[0].Patrol(true, Direction.Forward, 7, 1f);

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

    private void BuildScenario_02()
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
        _enemyList[0].Avatar.transform.position = new Vector3(0, 1, 7);
        _enemyList[0].Patrol(true, Direction.Right, 8, 0.4f);
        //_enemyList[0].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/WeakCannon"));
        Player.main.Body.transform.position = new Vector3(0, 0.5f, -7);
    }

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
        _enemyList[0].Avatar.transform.position = new Vector3(0, 1, 7);
        _enemyList[0].Patrol(true, Direction.Right, 8, 0.4f);
        //_enemyList[0].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/WeakCannon"));
        Player.main.Body.transform.position = new Vector3(0, 0.5f, -7);
    }

    private bool BossCheck()
    {
        // This is quite heavy on performance
        return enemyContainer.transform.Find("Boss");
    }
}

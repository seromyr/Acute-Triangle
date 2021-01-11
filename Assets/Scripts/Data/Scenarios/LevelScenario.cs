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
        _enemyList = new List<EnemyEntity>();
        enemyContainer = new GameObject("EnemyContainer");
    }

    private void Start()
    {
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
        }
    }

    private void BuildScenario_00()
    {
        _enemyList.Add
        (
            new Enemy_Sphere_L
            (
                EnemySkin.Sphere_Large,
                "Boss",
                enemyContainer.transform,
                30,
                30
            )
        );

        _enemyList[0].Avatar.transform.position = new Vector3(0, 1, 7);
        _enemyList[0].Patrol(Direction.Right, 8, 0.4f);
        GameManager.player.Avatar.transform.position = new Vector3(0, 0.5f, 0);
    }

    private void BuildScenario_01()
    {
        _enemyList.Add
        (
            new Enemy_Sphere_L
            (
                EnemySkin.Sphere_Large,
                "Boss",
                enemyContainer.transform,
                30,
                30
            )
        );

        _enemyList[0].Avatar.transform.position = new Vector3(0, 1, 4);
        _enemyList[0].Patrol(Direction.Right, 0, 1);

        for (int i = 1; i < 17; i++)
        {
            _enemyList.Add
            (
                new Enemy_Cube_S
                (
                    EnemySkin.Cube_Small,
                    EnemyName.Cube_Small + " " + i,
                    enemyContainer.transform,
                    10,
                    10
                )
            );

            _enemyList[i].Avatar.transform.position = new Vector3(-10.2f + i * 1.2f, 1, -4);
        }

        for (int i = 17 ; i < 33; i++)
        {
            _enemyList.Add
            (
                new Enemy_Cube_S
                (
                    EnemySkin.Cube_Small,
                    EnemyName.Cube_Small + " " + i,
                    enemyContainer.transform,
                    10,
                    10
                )
            );

            _enemyList[i].Avatar.transform.position = new Vector3(-11.4f + i * 1.2f - 18, 1, -2.8f);
        }

        for (int i = 33; i < 49; i++)
        {
            _enemyList.Add
            (
                new Enemy_Cube_S
                (
                    EnemySkin.Cube_Small,
                    EnemyName.Cube_Small + " " + i,
                    enemyContainer.transform,
                    10,
                    10
                )
            );

            _enemyList[i].Avatar.transform.position = new Vector3(-12.6f + i * 1.2f - 36, 1, -1.6f);
        }

        for (int i = 49; i < 65; i++)
        {
            _enemyList.Add
            (
                new Enemy_Cube_S
                (
                    EnemySkin.Cube_Small,
                    EnemyName.Cube_Small + " " + i,
                    enemyContainer.transform,
                    10,
                    10
                )
            );

            _enemyList[i].Avatar.transform.position = new Vector3(-13.8f + i * 1.2f - 54, 1, -0.4f);
        }

        GameManager.player.Avatar.transform.position = new Vector3(0, 0.5f, -7);
    }

    private void BuildScenario_02()
    {
        _enemyList.Add
        (
            new Enemy_Sphere_L
            (
                EnemySkin.Sphere_Large,
                "Boss",
                enemyContainer.transform,
                30,
                30
            )
        );
        _enemyList[0].Avatar.transform.position = new Vector3(0, 1, 7);
        _enemyList[0].Patrol(Direction.Right, 8, 0.4f);
        _enemyList[0].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/WeakCannon"));
        GameManager.player.Avatar.transform.position = new Vector3(0, 0.5f, -7);
    }

    private bool BossCheck()
    {
        // This is quite heavy on performance
        return enemyContainer.transform.Find("Boss");
    }
}

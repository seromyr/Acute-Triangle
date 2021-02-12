using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCondition : MonoBehaviour
{
    //private LevelScenario scenario;

    //// Cheat
    //private Transform enemyContainer;

    //private void Awake()
    //{
    //    scenario = GetComponent<LevelScenario>();
    //}

    //private void Start()
    //{
    //    enemyContainer = GameObject.Find("EnemyContainer").transform;
    //}

    //private void Update()
    //{
    //    //VictoryCondition_01();
    //    VictoryCondition_02();

    //    // Test

    //    //Debug.LogError(enemyContainer.childCount);



    //    //if (enemyContainer.childCount == 1)
    //    //{
    //    //    scenario.EnemyList[0].DestroyShield();
    //    //}

    //    //scenario.EnemyList[0].Body.transform.LookAt(Player.main.Body.transform);
    //}

    ////private void VictoryCondition_01()
    ////{
    ////    // Kill all enemies
    ////    if (scenario.Remaining == 0)
    ////    {
    ////        GameManager.main.WinGame();
    ////        Debug.Log("Enemy defeated");
    ////        Destroy(GetComponent<VictoryCondition>());
    ////        scenario.EnemyList.Clear();
    ////    }
    ////}

    //private void VictoryCondition_02()
    //{
    //    // Defeat Boss
    //    if (!scenario.BossIsAlive)
    //    {
    //        GameManager.main.WinGame();
    //        Debug.Log("Boss killed");
    //        Destroy(GetComponent<VictoryCondition>());
    //        scenario.EnemyList.Clear();
    //    }
    //}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_04 : MonoBehaviour
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

    // Scenario 04 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-4]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, -10));

        // Add enemy into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Boss",
                // Boss appearance
                Enemy.Sphere_Medium_Red,
                // Boss placemenent
                enemyContainer.transform,
                // Boss material
                "default",
                // Boss health
                100,
                // Register dead event action
                BossCountMonitor
            )
        );

        // Because this level only has 1 boss, so the boss id automatically known as 0
        bossCount = 1;

        // *IMPORTANT* Get enemy container reference for features accessing
        _enemyList[0].Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        //Enable shooting mechanic by adding some cannons
        cannonCount = 9;
        float cannonAngle = 12;
        _enemyList[0].Mechanics.Add(Mechanic.Shoot);
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, -48, cannonAngle, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);

        cannonCount = 2;
        cannonAngle = 120;
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, -60, cannonAngle, 0.6f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Indestructible);


        // Enable hardshells mechanic
        _enemyList[0].Mechanics.Add(Mechanic.HardShells);
        _enemyList[0].Mechanics.OnAllPillarsDestroyed += ActivateWeakenState;
        _enemyList[0].Mechanics.CreatePillar(new Vector3(10, -0.5f, 10), enemyContainer.transform);
        _enemyList[0].Mechanics.CreatePillar(new Vector3(-10, -0.5f, 10), enemyContainer.transform);
        _enemyList[0].Mechanics.CreatePillar(new Vector3(10, -0.5f, -10), enemyContainer.transform);
        _enemyList[0].Mechanics.CreatePillar(new Vector3(-10, -0.5f, -10), enemyContainer.transform);
        _enemyList[0].Mechanics.OnPillarsRegenerationCallback += () => ActivateInvincibleState(null, null);

       // Set boss default position
        _enemyList[0].SetPosition(new Vector3(0, 0.5f, 0));

        // Enable look at player mechanic
        _enemyList[0].Mechanics.Add(Mechanic.LookAtPlayer);
        _enemyList[0].Mechanics.SetLookingSpeed(50);

        // Addition feature when boss is invincible
        _enemyList[0].Mechanics.Add(Mechanic.SummonMinions);
        _enemyList[0].Mechanics.DestroyShield();

        // Set boss default state
        ActivateInvincibleState(null, null);
    }

    private void ActivateWeakenState(object sender, EventArgs e)
    {
        isWeaken = true;
        _enemyList[0].Mechanics.SetShootingStatus(true);
        _enemyList[0].Mechanics.SetLookingStatus(true);
        _enemyList[0].Mechanic.SetDamageAcceptance(true);

    }

    private void ActivateInvincibleState(object sender, EventArgs e)
    {
        isWeaken = false;
        _enemyList[0].Mechanics.SetShootingStatus(false);
        _enemyList[0].Mechanics.SetLookingStatus(false);
        _enemyList[0].Mechanic.SetDamageAcceptance(false);

        // Local count down tick for the timer to work
        _enemyList[0].Mechanics.SetMaximumMinion(10);
        int tick = 10;
        _enemyList[0].Mechanics.SummonTimer.SetTimer(1f, tick, () =>
        {
            tick--;

            Vector3 randomPositionAroundBoss = UnityEngine.Random.insideUnitSphere * 15;
            randomPositionAroundBoss.y = 0;

            _enemyList[0].Mechanics.SpawnMinion(_enemyList[0].GetPosition + randomPositionAroundBoss, 4, 2, 10);
        });

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

            _enemyList[0].Mechanics.OnAllPillarsDestroyed -= ActivateWeakenState;
            _enemyList[0].Mechanics.OnPillarsRegenerationCallback = delegate { };
            _enemyList.Clear();
        }
    }

    private bool isWeaken = false;

    private void FixedUpdate()
    {
        if (isWeaken && bossCount > 0)
        {
            _enemyList[0].Mechanics.SplitShells(3f);
        }
        else if (!isWeaken && bossCount > 0)
        {
            _enemyList[0].Mechanics.CloseShells();
        }
    }
    #endregion
}
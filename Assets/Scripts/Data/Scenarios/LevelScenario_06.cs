using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_06 : MonoBehaviour
{
    private List<EnemyEntity> _enemyList;

    private GameObject enemyContainer;

    private int bossCount, pupuCount, moxieCount, cannonCount;

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

    // Scenario 06 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-6]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(new Vector3(0, 0, 0));

        // According to level design, this level has 2 bosses
        pupuCount = 1;
        moxieCount = 1;
        bossCount = pupuCount + moxieCount;

        #region Pupu Scenario
        // Add 1st boss into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Pupu",
                // Boss appearance
                Enemy.Sphere_Medium_Red,
                // Boss placemenent
                enemyContainer.transform,
                // Boss material
                "default",
                // Boss health
                40,
                // Register dead event action
                PupuMonitor
            )
        );

         // *IMPORTANT* Get enemy container reference for features accessing
        _enemyList[0].Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        // Add cannons
        cannonCount = 8;
        float cannonAngle = 45;
        _enemyList[0].Mechanics.Add(Mechanic.Shoot);
        //_enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 1f, 1, GeneralConst.ENEMY_BULLET_SPEED_MODERATE -3, BulletType.Indestructible);

        cannonCount = 180;
        cannonAngle = 2;
        _enemyList[0].Mechanics.CreateMultipleCannons(cannonCount, 0, cannonAngle, 2f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 1, BulletType.Destructible);

        // Enable hardshells mechanic
        _enemyList[0].Mechanics.Add(Mechanic.HardShells);
        _enemyList[0].Mechanics.OnAllPillarsDestroyed += ActivateWeakenState;
        _enemyList[0].Mechanics.CreatePillar(new Vector3(12, -0.5f, 15.5f), enemyContainer.transform);
        _enemyList[0].Mechanics.CreatePillar(new Vector3(-12, -0.5f, 6.5f), enemyContainer.transform);
        _enemyList[0].Mechanics.OnPillarsRegenerationCallback += () => ActivateInvincibleState(null, null);

        // Set default position
        _enemyList[0].SetPosition(new Vector3(-20f, 0.5f, 20f));

        // Enable self rotation mode
        _enemyList[0].Mechanics.Add(Mechanic.SelfRotation);
        _enemyList[0].Mechanics.SetRotationParameters(10f);

        // Set boss default state
        ActivateInvincibleState(null, null);

        #endregion

        #region Moxie Scenario
        // Add 2nd boss into the list
        _enemyList.Add
        (
            new Enemy_Boss_Default
            (
                // Boss name
                "Moxie",
                // Boss appearance
                Enemy.Sphere_Large_Black,
                // Boss placemenent
                enemyContainer.transform,
                // Boss material
                "default",
                // Boss health
                40,
                // Register dead event action
                MoxieMonitor
            )
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        _enemyList[1].Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        // Enable chase mode
        _enemyList[1].Mechanics.Add(Mechanic.Chase);
        _enemyList[1].Mechanics.SetChaseParams(true, 2);

        // Set default position
        _enemyList[1].SetPosition(new Vector3(30f, 0.5f, -30f));

        // Boss takes no damage until the shield is down
        _enemyList[1].Mechanic.SetDamageAcceptance(false);

        // Add summon minion
        _enemyList[1].Mechanics.Add(Mechanic.SummonMinions);

        // Set maximum number of minions this boss has
        _enemyList[1].Mechanics.SetMaximumMinion(20);

        // Local count down tick for the timer to work
        int tick = 20;
        _enemyList[1].Mechanics.SummonTimer.SetTimer(0.75f, tick, () =>
        {
            tick--;

            Vector3 randomPositionAroundBoss = UnityEngine.Random.insideUnitSphere * 10;
            randomPositionAroundBoss.y = 0;

            _enemyList[1].Mechanics.SpawnMinion(_enemyList[1].GetPosition + randomPositionAroundBoss, 4, 2, 6);
        });

        // Add cannons
        _enemyList[1].Mechanics.Add(Mechanic.Shoot);
        _enemyList[1].Mechanics.CreateCannon(Quaternion.identity, 0.2f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);

        #endregion
    }

    private void ActivateWeakenState(object sender, EventArgs e)
    {
        isWeaken = true;
        _enemyList[0].Mechanic.SetDamageAcceptance(true);

    }

    private void ActivateInvincibleState(object sender, EventArgs e)
    {
        isWeaken = false;
        _enemyList[0].Mechanic.SetDamageAcceptance(false);
    }

    #region Scenario Stuff
    private void PupuMonitor(object sender, EventArgs e)
    {
        pupuCount--;
        bossCount--;


        if (pupuCount == 0)
        {
            // Clean Pupu garbages
            _enemyList[0].Mechanics.OnAllPillarsDestroyed -= ActivateWeakenState;
            _enemyList[0].Mechanics.OnPillarsRegenerationCallback = delegate { };
        }


        // Victory Condition
        if (bossCount == 0)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss left");
            _enemyList.Clear();
        }
    }

    private void MoxieMonitor(object sender, EventArgs e)
    {
        moxieCount--;
        bossCount--;

        if (moxieCount == 0)
        {
            // Clean Moxie garbages
            _enemyList[1].Mechanics.OnAllPillarsDestroyed -= ActivateWeakenState;
            _enemyList[1].Mechanics.OnPillarsRegenerationCallback = delegate { };
        }


        // Victory Condition
        if (bossCount == 0)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss left");
            _enemyList.Clear();
        }
    }

    private bool isWeaken = false;

    private void FixedUpdate()
    {
        if (isWeaken && pupuCount > 0)
        {
            _enemyList[0].Mechanics.SplitShells(3f);
        }
        else if (!isWeaken && pupuCount > 0)
        {
            _enemyList[0].Mechanics.CloseShells();
        }
    }
    #endregion
}

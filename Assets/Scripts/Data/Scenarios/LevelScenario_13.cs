﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelScenario_13 : MonoBehaviour
{
    private EnemyEntity pupu, moxie;

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

        // Send mission instruction
        UI_InGameMenu_Mechanic.main.SendInstruction("Defeat Pupu & Moxie The Ladies");
    }

    // Scenario 06 [https://sites.google.com/view/acutetriangle/game-design/level-design/level-6]
    private void BuildScenario()
    {
        // Set player start position
        Player.main.SetPosition(Vector3.zero);

        #region Pupu Scenario
        // Add 1st boss into the list
        pupu = new Enemy_Default
        (
            // Boss name
            "Boss_Pupu",
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
        pupu.Mechanics.GetEnemyContainerReference(enemyContainer);

        // Add blasters to boss
        bossBlasterCount = 8;
        float shootingAngle = 45;
        pupu.Mechanics.Add(Mechanic.Shoot);
        pupu.Mechanics.CreateBlasters(bossBlasterCount, 0, shootingAngle, 3.25f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 3, BulletType.Indestructible);

        bossBlasterCount = 180;
        shootingAngle = 2;
        pupu.Mechanics.CreateBlasters(bossBlasterCount, 0, shootingAngle, 2.5f, 1, GeneralConst.ENEMY_BULLET_SPEED_SLOW - 1, BulletType.Destructible);

        // Activate Hard Shells mechanic
        pupu.Mechanics.Add(Mechanic.HardShells);
        pupu.Mechanics.CreateShells("Shell_03");
        pupu.Mechanics.OnAllPillarsDestroyed += ActivatePupuWeakenState;
        pupu.Mechanics.CreatePillar(new Vector3(-10.5f, -0.5f, 22.75f), enemyContainer.transform);
        pupu.Mechanics.CreatePillar(new Vector3(-31, -0.5f, 5f), enemyContainer.transform);
        pupu.Mechanics.OnReactorsRegenerationCallback += () => ActivatePupuInvincibleState(null, null);

        // Set Pupu default position
        pupu.SetPosition(new Vector3(-20f, 0.5f, 20f));

        // Enable self rotation mode
        pupu.Mechanics.Add(Mechanic.SelfRotation);
        pupu.Mechanics.SetRotationParameters(true, 10f);

        // Set boss default state
        ActivatePupuInvincibleState(null, null);

        #endregion

        #region Moxie Scenario
        // Add 2nd boss into the list
        moxie = new Enemy_Default
        (
            // Boss name
            "Boss_Moxie",
            // Boss appearance
            Enemy.Boss_06b,
            // Boss placemenent
            enemyContainer.transform,
            // Boss material
            "default",
            // Boss health
            10,
            // Register dead event action
            BossMonitor
        );

        // *IMPORTANT* Get enemy container reference for features accessing
        moxie.Mechanics.GetEnemyContainerReference(enemyContainer.transform);

        // Activate Chase mechanic
        moxie.Mechanics.Add(Mechanic.Chase);
        moxie.Mechanics.SetChaseParams(true, 2);

        // Set Moxie default position
        moxie.SetPosition(new Vector3(30f, 0.5f, -30f));

        // Boss takes no damage until the shield is down
        moxie.HitMonitor.SetDamageAcceptance(false);

        // Activate Summon Minion mechanic
        moxie.Mechanics.Add(Mechanic.SummonMinions);
        moxie.Mechanics.SetMaximumMinion(6);

        // Local countdown tick for the timer to work
        int tick = 6;
        moxie.Mechanics.SummonTimer.SetTimer(0.75f, tick, () =>
        {
            tick--;

            Vector3 randomPositionAroundBoss = moxie.GetPosition + (Player.main.GetPosition - moxie.GetPosition) / 2 + UnityEngine.Random.insideUnitSphere * 2;
            randomPositionAroundBoss.y = 0;

            moxie.Mechanics.SpawnMinion(randomPositionAroundBoss, 2.5f, 2, 10);
        });

        // Add blasters to boss
        moxie.Mechanics.Add(Mechanic.Shoot);
        moxie.Mechanics.CreateBlaster(Quaternion.identity, 0.2f, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);

        #endregion

        UI_InGameMenu_Mechanic.main.SetBossHPMax(20);
        UI_InGameMenu_Mechanic.main.SetBossHPCurrent(20);
        UI_InGameMenu_Mechanic.main.UpdateBossHPCounter(0);

    }

    private void ActivatePupuWeakenState(object sender, EventArgs e)
    {
        isWeaken = true;
        pupu.HitMonitor.SetDamageAcceptance(true);

    }

    private void ActivatePupuInvincibleState(object sender, EventArgs e)
    {
        isWeaken = false;
        pupu.HitMonitor.SetDamageAcceptance(false);
    }

    #region Scenario Stuff
    private void BossMonitor(object sender, EventArgs e)
    {
        if (!pupu.IsAlive)
        {
            // Clean Pupu garbages
            pupu.Mechanics.OnAllPillarsDestroyed -= ActivatePupuWeakenState;
            pupu.Mechanics.OnReactorsRegenerationCallback = delegate { };
        }

        // Victory Condition
        if (!pupu.IsAlive && !moxie.IsAlive)
        {
            GameManager.main.WinGame();
            Debug.Log("No boss remaining");

            // Unlock next level
            UI_MainMenu_Mechanic.main.UnlockLevel(14);
        }

    }

    private bool isWeaken = false;

    private void FixedUpdate()
    {
        if (isWeaken && pupu.IsAlive)
        {
            pupu.Mechanics.SplitShells(3f);
        }
        else if (!isWeaken && pupu.IsAlive)
        {
            pupu.Mechanics.CloseShells();
        }
    }
    #endregion
}

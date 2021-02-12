using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This is the boss features with delegate functions
public class Features
{
    public Action<Feature> Add;
    private GameObject body;

    // Lisf of feature script

    // Boss patrol mechanic, can be customized in Level Scenario if used
    private SimplePatrol simplePatrol;

    // Boss chase mechanic, can be customized in Level Scenario if used
    private SimpleChase simpleChase;

    // Boss shield mechanic, can be custimzed in Level Scenario if used
    private GameObject _shield;

    // Summon minion mechanic
    private List<EnemyEntity> minionList;
    public int minionCount;

    // Timer mechanic, can be used for anything
    private Timer timer;
    public Timer Timer { get { return timer; } }


    public Features(GameObject gameObject)
    {
        Add = AddNewFeature;
        body = gameObject;
    }

    private void AddNewFeature(Feature feature)
    {
        switch (feature)
        {
            default:
            case Feature.AggressiveRadius:
                return;
            case Feature.Fear:
                return;
            case Feature.Retreat:
                return;
            case Feature.HardShells:
                return;
            case Feature.PowerChargers:
                return;
            case Feature.MoveToWaypoints:
                return;
            case Feature.SelfRotation:
                return;
            case Feature.SummonClones:
                return;            
            case Feature.SummonMinions:
                timer = body.AddComponent<Timer>();
                CreateMinions();
                return;
            case Feature.Switches:
                return;
            case Feature.Patrol:
                simplePatrol = body.AddComponent<SimplePatrol>();
                return;
            case Feature.Chase:
                simpleChase = body.AddComponent<SimpleChase>();
                return;
            case Feature.SelfPhase:
                return;
            case Feature.Shield:
                CreateShield();
                return;
        }
    }

    #region Patrol
    // Patrol parameter setup if used in Level Scenario
    public void SetPatrolParams(bool isPatrolling, Direction direction, float distance, float speed)
    {
        simplePatrol.SetPatrollingStatus(isPatrolling);
        simplePatrol.SetPatrolDirection(direction);
        simplePatrol.SetPatrolDistance(distance);
        simplePatrol.SetPatrolSpeed(speed);
    }
    #endregion

    #region Chase
    // Chase parameter setup if used in Level Scenario
    public void SetChaseParams(bool isChasing, float speed)
    {
        if (isChasing)
        {
            simpleChase.StarChase();
            simpleChase.SetChaseSpeed(speed);
        }
    }
    #endregion

    #region Shield
    // Shield setup
    private void CreateShield()
    {
        _shield = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _shield.transform.parent = body.transform;
        _shield.transform.localPosition = Vector3.zero;
        _shield.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Shield");
        _shield.transform.localScale = Vector3.one * 1.2f;
        _shield.name = "Shield";
    }
    public void ActivateShield()
    {
        _shield.SetActive(true);
    }

    public void DestroyShield()
    {
        _shield.SetActive(false);
    }
    #endregion

    #region Summon Minions
    private void CreateMinions()
    {
        // Automatically create shield if a boss has minions
        if (_shield == null)
        {
            CreateShield();
        }
        else
        {
            ActivateShield();
        }

        // Create minion list
        minionList = new List<EnemyEntity>();
    }

    public void SpawnMinion(Transform enemyContainer)
    {
        int minionID = minionList.Count;

        minionList.Add
                (
                    new Enemy_Minion
                    (
                        "Minion " + minionID,
                        Enemy.Cone,
                        enemyContainer,
                        5,
                        // Register dead event action
                        (object Sender, EventArgs e) =>
                        {
                            minionCount--;
                            if (minionCount == 0)
                            {
                                minionList.Clear();
                                Debug.Log("All minions killed");
                                DestroyShield();
                            }
                        }
                    )
                );

        minionList[minionID].SetPosition(new Vector3(-5 + minionID, 0.25f, 5));
        minionList[minionID].Features.Add(Feature.Chase);
        minionList[minionID].Features.SetChaseParams(true, 1);
        minionList[minionID].Shoot(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), Quaternion.identity, 2, 0.5f, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);
    }
    #endregion
}

// This class works as a timer
public class Timer : MonoBehaviour
{
    private Action timerCallback;
    private float timer, referenceTimer;
    private bool loop;
    private int count;

    public void SetLoop(bool value)
    {
        loop = value;
    }

    public void SetTimer(float timer, int count, Action timerCallback)
    {
        // Set time
        this.timer = referenceTimer = timer;

        // Set loop
        loop = false;

        // Set count
        this.count = count;

        // Set call back action
        this.timerCallback = timerCallback;
    }

    private void Update()
    {
        if (timer > 0 && count > 0)
        {
            timer -= Time.deltaTime;
            
            if (IsTimerElapsed)
            {
                // Call back action
                timerCallback();

                // Adjust timer settings
                if (!loop)
                {
                    count--;
                }

                timer = referenceTimer;
            }
        }
    }

    public bool IsTimerElapsed
    {
        get { return timer <= 0f; }
    }
}

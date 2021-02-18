using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This is the boss features with delegate functions
public class Features
{
    public Action<Mechanic> Add;
    private GameObject body;

    public Features(GameObject gameObject)
    {
        Add = AddNewFeature;
        body = gameObject;
    }

    private void AddNewFeature(Mechanic feature)
    {
        switch (feature)
        {
            default:
            case Mechanic.Shoot:
                CreateShootingMechanic();
                return;

            case Mechanic.AggressiveRadius:
                CreateAggressiveMechanic();
                return;

            case Mechanic.Fear:
                return;

            case Mechanic.Retreat:
                return;

            case Mechanic.HardShells:
                return;

            case Mechanic.PowerChargers:
                return;

            case Mechanic.MoveToWaypoints:
                return;

            case Mechanic.SelfRotation:
                CreateSelfRotationMechanic();
                return;

            case Mechanic.SummonClones:
                return;   
                
            case Mechanic.SummonMinions:
                CreateTimerMechanic(out summonTimer);
                CreateMinionSummoningMechanic();
                return;

            case Mechanic.Switches:
                return;

            case Mechanic.Patrol:
                CreatePatrolMechanic();
                return;

            case Mechanic.Chase:
                CreateChasingMechanic();
                return;

            case Mechanic.SelfPhase:
                CreateTimerMechanic(out phaseTimer);
                CreateSelfPhaseMechanic();
                return;

            case Mechanic.Shield:
                CreateShield();
                return;
        }
    }

    #region Timer
    private void CreateTimerMechanic(out Timer featureTimer)
    {
        featureTimer = body.AddComponent<Timer>();
    }
    #endregion

    #region Patrol
    // Boss patrol mechanic, can be customized in Level Scenario if used
    private SimplePatrol simplePatrol;

    private void CreatePatrolMechanic()
    {
        simplePatrol = body.AddComponent<SimplePatrol>();
    }

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
    // Boss chase mechanic, can be customized in Level Scenario if used
    private SimpleChase simpleChase;

    private void CreateChasingMechanic()
    {
        simpleChase = body.AddComponent<SimpleChase>();
    }

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

    #region Self-rotation
    // Boss self-rotation mechanic, can be customized in Level Scenario if used
    private SelfRotation selfRotation;

    private void CreateSelfRotationMechanic()
    {
        selfRotation = body.AddComponent<SelfRotation>();
    }

    public void SetRotationParameters(float rotationSpeed = 500f, float accelerationSpeed = 500f)
    {
        selfRotation.SetRotationParameters(rotationSpeed, accelerationSpeed);
    }

    #endregion

    #region Shield
    // Boss shield mechanic, can be custimzed in Level Scenario if used
    private GameObject _shield;

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
    // Timer instance, used for summoning minions with delay in between
    private Timer summonTimer;
    public Timer SummonTimer { get { return summonTimer; } }

    // Summon minion mechanic, can be customized in Level Scenario if used
    private List<EnemyEntity> minionList;
    private int minionCount;

    private void CreateMinionSummoningMechanic()
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
                        "default",
                        5,
                        // Register dead event action
                        OnMinionDeath
                    )
                );

        minionList[minionID].SetPosition(new Vector3(-5 + minionID, 0.25f, 5));
        minionList[minionID].Mechanics.Add(Mechanic.Chase);
        minionList[minionID].Mechanics.SetChaseParams(true, 1);
        minionList[minionID].Mechanics.Add(Mechanic.Shoot);
        minionList[minionID].Mechanics.CreateCannon(Quaternion.identity, 2, 0.5f, GeneralConst.ENEMY_BULLET_SPEED_SLOW, BulletType.Destructible);
    }

    private void OnMinionDeath(object sender, EventArgs e)
    {
        minionCount--;

        // In case all minions are killed
        if (minionCount == 0)
        {
            minionList.Clear();
            Debug.Log("All minions killed");
            DestroyShield();
            body.GetComponent<Enemy_HitMonitor>().SetDamageAcceptance(true);
        }
    }

    public void SetMaximumMinion(int minionCount)
    {
        this.minionCount = minionCount;
    }
    #endregion

    #region Agressive Proximity
    // Mesh Renderer preference to control material
    private MeshRenderer meshRenderer;
    private ProximityMonitor proximityMonitor;
    public ProximityMonitor ProximityMonitor { get { return proximityMonitor; } }

    //private Material shaderMaterial;

    //private Color color_1, color_2;

    private void CreateAggressiveMechanic()
    {
        proximityMonitor = body.AddComponent<ProximityMonitor>();
        proximityMonitor.OnEnterProximity += StartAggressive;
        proximityMonitor.OnExitProximity += StopAggressive;

        //Create Aggressive Proximity Indicator
        GameObject aggroProxInd = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/Disc"), body.transform);
        //shaderMaterial = aggroProxInd.GetComponent<MeshRenderer>().material;

        ////color_1 = shaderMaterial.GetColorArray;
        //MaterialPropertyBlock mbox = new MaterialPropertyBlock();
        //mbox.SetColor("Color 1", Color.red);

        //aggroProxInd.GetComponent<MeshRenderer>().SetPropertyBlock(mbox);

        body.TryGetComponent(out meshRenderer);

        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component of " + body.name + " not found");
        }
    }

    private void StartAggressive(object sender, EventArgs e)
    {
        proximityMonitor.SetAggressiveStatus(true);
        simplePatrol.SetPatrollingStatus(false);
        //Debug.LogError("Enter");
    }

    private void StopAggressive(object sender, EventArgs e)
    {
        proximityMonitor.SetAggressiveStatus(false);
        simplePatrol.SetPatrollingStatus(true);
        //Debug.LogWarning("Exit");
    }

    #endregion

    #region Shoot
    // Shooter manager
    private List<GameObject> cannons;
    private int cannonCount;
    private float cannonAngle;

    public List<GameObject> Cannons { get { return cannons; } }

    private void CreateShootingMechanic()
    {
        if (cannons == null)
        {
            cannons = new List<GameObject>();
        }
    }

    // Shooting parameter setting used in level scenario
    // This method create boss shooters
    public void CreateCannon(Quaternion pointingAngle, float fireRate, float bulletSize, float bulletSpeed, BulletType bulletType)
    {
        cannons.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/Cannon"), body.transform.position, pointingAngle, body.transform));
        cannons[cannons.Count - 1].GetComponent<Shooter>().SetShootingParameters(fireRate, bulletSize, bulletSpeed, bulletType);
    }

    public void CreateMultipleCannons(int cannonCount, float _initialAngle, float cannonAngle, float fireRate, float bulletSize, float bulletSpeed, BulletType bulletType)
    {
        this.cannonCount = cannonCount;
        this.cannonAngle = cannonAngle;
        float initialAngle = _initialAngle;
        for (int i = 0; i < this.cannonCount; i++)
        {
            CreateCannon(Quaternion.Euler(new Vector3(0, initialAngle, 0)), fireRate, bulletSize, bulletSpeed, bulletType);
            initialAngle += this.cannonAngle;
        }
    }

    public void SetShootingStatus(bool isShooting)
    {
        
        for (int i = 0; i < cannons.Count; i++)
        {
            switch (isShooting)
            {
                case true:
                    cannons[i].GetComponent<Shooter>().ResumeShooting();
                    break;
                case false:
                    cannons[i].GetComponent<Shooter>().PauseShooting();
                    break;
            }
        }
    }

    #endregion

    #region Phase In / Phase Out
    // Boss Phase In / Phase Out mechanic, can be customized in Level Scenario if used

    // Timer instance, used for delay between phases
    private Timer phaseTimer;
    public Timer PhaseTimer { get { return phaseTimer; } }

    private MeshRenderer meshRendererPointer;
    private Collider colliderPointer;

    private void CreateSelfPhaseMechanic()
    {
        meshRendererPointer = body.GetComponent<MeshRenderer>();
        colliderPointer = body.GetComponent<Collider>();
    }
    public void Phase(bool value)
    {

        //Component[] comps = body.GetComponents<Component>();

        //foreach (Component c in comps)
        //{
        //    c. = value;
        //}
        //body.GetComponent<Timer>().enabled = true;
        meshRendererPointer.enabled = value;
        colliderPointer.enabled = value;
        SetShootingStatus(value);
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

    private int pauseCount;

    public void SetLoop(bool value)
    {
        loop = value;
    }

    public void PauseTimer()
    {
        pauseCount = count;
        Debug.LogError("pauseCount: " + pauseCount);
        count = 0;
    }

    public void ResumeTimer()
    {
        count = pauseCount;
        pauseCount = count;
        Debug.LogWarning("count: " + count);
    }

    public void SetTimer(float delay, int count, Action timerCallback)
    {
        // Set delay between each tick
        this.timer = referenceTimer = delay;

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

public class ProximityMonitor : MonoBehaviour
{
    public event EventHandler OnEnterProximity, OnExitProximity;

    private bool isAggressive;

    private float distance, aggressiveDistance;

    private void Start()
    {
        SetAggressiveDistance();
        SetAggressiveStatus(false);
    }

    private void FixedUpdate()
    {
        distance = (Player.main.GetPosition - transform.position).magnitude;

        if (distance <= aggressiveDistance && !isAggressive)
        {
            OnEnterProximity?.Invoke(this, EventArgs.Empty);
        }

        if (distance > aggressiveDistance && isAggressive)
        {
            OnExitProximity?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetAggressiveDistance(float aggressiveDistance = 7f)
    {
        this.aggressiveDistance = aggressiveDistance;
    }

    public void SetAggressiveStatus(bool isAggressive)
    {
        this.isAggressive = isAggressive;
    }
}
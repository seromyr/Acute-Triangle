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
    private Transform enemyContainer;

    public Features(GameObject gameObject)
    {
        Add = AddNewFeature;
        body = gameObject;
    }

    public void GetEnemyContainerReference(Transform enemyContainer)
    {
        this.enemyContainer = enemyContainer;
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

            case Mechanic.ComplexeMovement:
                CreateComplexMovementMechanic();
                return;

            case Mechanic.HardShells:
                CreateHardShellsMechanic();
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
                CreateShieldMechanic();
                return;

            case Mechanic.LookAtPlayer:
                CreateLookAtPlayerMechanic();
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

    public void SetPatrollingStatus(bool status)
    {
        simplePatrol.SetPatrollingStatus(status);
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
    public void SetChaseParams(bool isChasing, float speed, float stopDistance = 0)
    {
        if (isChasing)
        {
            simpleChase.StartChase();
            simpleChase.SetChaseSpeed(speed);
            simpleChase.SetStopDistance(stopDistance);
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

    public void SetRotationParameters(bool isRotating = true, float rotationSpeed = 500f, float accelerationSpeed = 500f)
    {
        selfRotation.SetRotationParameters(isRotating, rotationSpeed, accelerationSpeed);
    }

    public void SetRotationStatus(bool isRotating)
    {
        selfRotation.SetRotatingStatus(isRotating);
    }

    #endregion

    #region Shield
    // Boss shield mechanic, can be custimzed in Level Scenario if used
    private GameObject _shield;

    // Shield setup
    private void CreateShieldMechanic()
    {
        _shield = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _shield.transform.parent = body.transform;
        _shield.transform.localPosition = Vector3.zero;
        _shield.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Shield_Red");
        _shield.transform.localScale = Vector3.one * 2.5f;
        _shield.name = "Shield";
        _shield.layer = 9;
    }
    public void ActivateShield()
    {
        _shield.SetActive(true);
    }

    public void DeactivateShield()
    {
        _shield.SetActive(false);
    }

    public void SwitchToVioletShield()
    {
        _shield.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Shield_Violet");
    }
    #endregion

    #region Summon Minions
    // Timer instance, used for summoning minions with delay in between
    private Timer summonTimer;
    public Timer SummonTimer { get { return summonTimer; } }

    // Summon minion mechanic, can be customized in Level Scenario if used
    private List<EnemyEntity> minionList;
    private int minionCount;

    //Extra callback action to use incase of need
    public Action OnAllMinionDieCallback;

    private void CreateMinionSummoningMechanic()
    {
        // Automatically create shield if a boss has minions
        if (_shield == null)
        {
            CreateShieldMechanic();
        }
        else
        {
            ActivateShield();
        }

        // Create minion list
        minionList = new List<EnemyEntity>();
    }

    public void SpawnMinion(Vector3 minionPosition, float minionSpeed = 1, float minionFireRate = 2, float bulletSpeed = 5, string minionModel = Enemy.Minion)
    {
        int minionID = minionList.Count;

        minionList.Add
                (
                    new Enemy_Minion
                    (
                        "Minion " + minionID,
                        minionModel,
                        enemyContainer,
                        "default",
                        5,
                        // Register dead event action
                        OnMinionDeath
                    )
                );

        minionList[minionID].SetPosition(minionPosition);
        minionList[minionID].Mechanics.Add(Mechanic.Chase);
        minionList[minionID].Mechanics.SetChaseParams(true, minionSpeed, 5f);
        minionList[minionID].Mechanics.Add(Mechanic.Shoot);
        minionList[minionID].Mechanics.CreateBlaster(Quaternion.identity, minionFireRate, bulletSpeed, BulletType.Destructible);
    }
    
    private void OnMinionDeath(object sender, EventArgs e)
    {
        minionCount--;

        // In case all minions are killed
        if (minionCount == 0)
        {
            minionList.Clear();
            Debug.Log("All minions killed");
            DeactivateShield();
            body.GetComponent<Enemy_HitMonitor>().SetDamageAcceptance(true);
            OnAllMinionDieCallback();
        }
    }

    public void SetMaximumMinion(int minionCount)
    {
        this.minionCount = minionCount;
    }
    #endregion

    #region Agressive Proximity
    private ProximityMonitor proximityMonitor;
    public ProximityMonitor ProximityMonitor { get { return proximityMonitor; } }

    private GameObject aggressiveDisc;

    //private Material shaderMaterial;

    //private Color color_1, color_2;

    private void CreateAggressiveMechanic()
    {
        proximityMonitor = body.AddComponent<ProximityMonitor>();
        proximityMonitor.OnEnterProximity += StartAggressive;
        proximityMonitor.OnExitProximity += StopAggressive;
    }

    public void SetAuraProximityIndicator(int discType = 1)
    {
        UnityEngine.Object.Destroy(aggressiveDisc);

        string disc = "Prefabs/Enemy/Disc_";
        disc += discType;

        //Create a new Aggressive Proximity Indicator
        aggressiveDisc = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(disc), body.transform);

        //Set aggressive proximity visual always at ground height
        Vector3 newPos = Vector3.zero;
        newPos.x = body.transform.position.x * body.transform.localScale.z + body.transform.position.x;
        newPos.y = -body.transform.position.y * body.transform.localScale.y + body.transform.position.y + 0.05f;
        newPos.z = body.transform.position.z * body.transform.localScale.z + body.transform.position.z;

        aggressiveDisc.transform.position = aggressiveDisc.transform.InverseTransformPoint(newPos);
    }

    private void StartAggressive(object sender, EventArgs e)
    {
        proximityMonitor.SetAggressiveStatus(true);
        //Debug.LogError("Enter");
    }

    private void StopAggressive(object sender, EventArgs e)
    {
        proximityMonitor.SetAggressiveStatus(false);
        //Debug.LogWarning("Exit");
    }

    public void SetAgressiveDiameteMutiplierr(float diameter)
    {
        aggressiveDisc.transform.localScale *= diameter;
        proximityMonitor.SetAggressiveDistance(diameter);
    }

    #endregion

    #region Shoot
    // Shooter manager
    private List<GameObject> blasters;
    private int blasterCount;
    private float blasterAngle;

    public List<GameObject> Blasters { get { return blasters; } }

    private void CreateShootingMechanic()
    {
        if (blasters == null)
        {
            blasters = new List<GameObject>();
        }
    }

    // Shooting parameter setting used in level scenario
    // This method create enemy blasters
    public void CreateBlaster(Quaternion pointingAngle, float fireRate, float bulletSpeed, BulletType bulletType)
    {
        blasters.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/Blaster"), body.transform.position, pointingAngle, body.transform));
        blasters[blasters.Count - 1].GetComponent<Blaster>().SetShootingParameters(fireRate, bulletSpeed, bulletType);
    }

    public void CreateBlasters(int cannonCount, float _initialAngle, float cannonAngle, float fireRate, float bulletSize, float bulletSpeed, BulletType bulletType)
    {
        this.blasterCount = cannonCount;
        this.blasterAngle = cannonAngle;
        float initialAngle = _initialAngle;
        for (int i = 0; i < this.blasterCount; i++)
        {
            CreateBlaster(Quaternion.Euler(new Vector3(0, initialAngle, 0)), fireRate, bulletSpeed, bulletType);
            initialAngle += this.blasterAngle;
        }
    }

    public void SetShootingStatus(bool isShooting)
    {
        
        for (int i = 0; i < blasters.Count; i++)
        {
            switch (isShooting)
            {
                case true:
                    blasters[i].GetComponent<Blaster>().ResumeShooting();
                    break;
                case false:
                    blasters[i].GetComponent<Blaster>().PauseShooting();
                    break;
            }
        }
    }

    public void DestroyAllCannons()
    {
        for (int i = 0; i < blasters.Count; i++)
        {
            GameObject.Destroy(blasters[i]);
        }

        blasters.Clear();
    }

    public void SetShootingDelay(int blasterID, float delay)
    {
        blasters[blasterID].GetComponent<Blaster>().DelayShooting(delay);
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

    #region Hard Shells
    // Boss hard shell mechanic, can be customized in Level Scenario if used

    // Shell pieces
    private List<GameObject> shells;

    // Power Pillars container
    private List<EnemyEntity> pillars;
    private int pillarCount;
    private List<Vector3> pillarsPosition;

    // Boss is weaken event
    public event EventHandler OnAllPillarsDestroyed;

    // Hardshell Timer
    private Timer hardShellTimer;

    // Callback action when it's time to regenerate pillars
    public Action OnReactorsRegenerationCallback;

    private void CreateHardShellsMechanic()
    {

        pillars = new List<EnemyEntity>();
        pillarsPosition = new List<Vector3>();
        hardShellTimer = body.AddComponent<Timer>();
        OnReactorsRegenerationCallback += ReGeneratePillars;
    }

    public void CreateShells(string shellName = "Shell_01")
    {
        GameObject shells = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/" + shellName));
        shells.transform.parent = body.transform;

        this.shells = new List<GameObject>();
        foreach (Transform child in shells.transform)
        {
            this.shells.Add(child.gameObject);
        }

    }

    public void SplitShells(float distance)
    {
        for (int i = 0; i < shells.Count; i++)
        {
            if ((shells[i].transform.localPosition - Vector3.zero).magnitude <= distance)
            shells[i].transform.Translate(Vector3.back * Time.deltaTime * 2f);
            //shells[i].transform.localPosition = Vector3.MoveTowards(shells[i].transform.localPosition, -shells[i].transform.forward * distance, Time.deltaTime * 50);
        }
    }

    public void CloseShells()
    {
        for (int i = 0; i < shells.Count; i++)
        {
            //shells[i].transform.localPosition = Vector3.zero;
            shells[i].transform.localPosition = Vector3.MoveTowards(shells[i].transform.localPosition, Vector3.zero, Time.deltaTime * 20);
        }
    }

    private Transform pillarContainer;
    public void CreatePillar(Vector3 position, Transform enemyContainer)
    {
        int pillarID = pillars.Count;

        pillarContainer = enemyContainer;

        pillars.Add
            (
                new Enemy_Minion
                (
                    "Power Pillar " + pillarID,
                    Enemy.PowerReactor,
                    enemyContainer,
                    "default",
                    15,
                    // Register dead event action
                    OnPillarDestroy
                )
            );

        pillars[pillarID].SetPosition(position);

        pillarCount++;

        if (!pillarsPosition.Contains(position))
        {
            pillarsPosition.Add(position);
        }
     }

    private void OnPillarDestroy(object sender, EventArgs e)
    {
        pillarCount--;

        if (pillarCount == 0)
        {
            OnAllPillarsDestroyed?.Invoke(this, EventArgs.Empty);
            pillars.Clear();
            //Debug.LogError("All pillars destroyed");
            hardShellTimer.SetTimer(12f, 1, OnReactorsRegenerationCallback);
        }
    }

    private void ReGeneratePillars()
    {
        for (int i = 0; i < pillarsPosition.Count; i++)
        {
            CreatePillar(pillarsPosition[i], pillarContainer);
        }
    }
    #endregion

    #region LookAt
    // Look At mechanic,  can be customized in Level Scenario if used
    private LookAtPlayer lookAtPlayer; 

    private void CreateLookAtPlayerMechanic()
    {
        lookAtPlayer = body.AddComponent<LookAtPlayer>();
    }

    public void SetLookingSpeed(float lookSpeed)
    {
        lookAtPlayer.SetLookSpeed(lookSpeed);
    }

    public void SetLookingStatus(bool isLooking)
    {
        lookAtPlayer.SetLookingStatus(isLooking);
    }

    #endregion

    #region Complex Movement: Run Around, Move To WayPoint
    // Boss run away mechanic,  can be customized in Level Scenario if used
    private ComplexMovement complexMovement;
    private void CreateComplexMovementMechanic()
    {
        complexMovement = body.AddComponent<ComplexMovement>();
    }

    public void SetRunningAroundParams(bool isRunningAround, float speed)
    {
        if (isRunningAround)
        {
            complexMovement.StartRunningAround();
            complexMovement.SetRunSpeed(speed);
        }
        //else
        //{
        //    complexMovement.SetRunSpeed(speed);
        //}
    }

    public void SetGoToWayPointParams(bool isGoingToWayPoint, Vector3 destination, float speed)
    {
        if (isGoingToWayPoint)
        {
            complexMovement.StartGoingToWayPoint();
            complexMovement.SetDestination(destination);
            complexMovement.SetRunSpeed(speed);
        }
    }

    #endregion
}

// This class works as a timer
public class Timer : MonoBehaviour
{
    private Action timerCallback;
    private float timer, referenceTimer;
    private bool loop;
    private int count, pauseCount;

    private float delay, referenceDelay;
    private int turn;

    public void SetLoop(bool value)
    {
        loop = value;
    }

    public void PauseTimer()
    {
        pauseCount = count;
        count = 0;
    }

    public void ResumeTimer()
    {
        count = pauseCount;
        pauseCount = count;
    }

    public void SetDelay(float delay)
    {
        this.delay = delay;
        this.referenceDelay = delay;
        turn = 0;
    }

    public void SetTimer(float delay, int count, Action timerCallback, float delayAtLoopEnd = 0)
    {
        // Set delay between each tick
        this.timer = referenceTimer = delay;

        // Set loop
        loop = false;

        // Set count
        this.count = count;

        // Set call back action
        this.timerCallback = timerCallback;

        // Set delay at the end of the loop
        this.delay = delayAtLoopEnd;
    }

    private void FixedUpdate()
    {
        if (timer > 0 && count > 0)
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0)
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

        if (delay > 0)
        {
            delay -= Time.deltaTime;

            if (delay <= 0)
            {
                delay = referenceDelay;

                turn = (turn + 1) % 2;

                if (turn == 1)
                {
                    timer = referenceTimer + delay;
                }
            }
        }
    }
}

public class ProximityMonitor : MonoBehaviour
{
    public event EventHandler OnEnterProximity, OnExitProximity;

    private bool isAggressive;

    private float distance, aggressiveDistance;

    private void Start()
    {
        //SetAggressiveDistance();
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

public class LookAtPlayer : MonoBehaviour
{
    private bool isLooking;

    private Transform target;

    private Vector3 lookDirection;

    private Quaternion lookRotation;

    private float rotationSpeed;

    private void Start()
    {
        target = Player.main.Transform;
    }

    private void FixedUpdate()
    {
        Look();
    }

    private void Look()
    {
        if (isLooking )
        {
            lookDirection = target.position - transform.position;
            lookDirection.y = 0;
            lookRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetLookingStatus(bool isLooking)
    {
        this.isLooking = isLooking;
        transform.rotation = isLooking ? transform.rotation : Quaternion.identity;
    }
    
    public void SetLookSpeed(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }
}

public class ComplexMovement : MonoBehaviour
{
    private float moveSpeed;
    private bool isRunningAround, isGoingTodWayPoint;
    private Vector3 wayPointDestination;

    private void FixedUpdate()
    {
        RunAround(Player.main.Transform);
        GoToWayPoint();
    }

    private void RunAround(Transform target)
    {
        if (isRunningAround)
        {
            Vector3 direction = (transform.position - target.position).normalized;
            direction.y = 0;
            //transform.Translate(direction * Time.deltaTime * moveSpeed);
            transform.Translate(direction * Time.deltaTime * UnityEngine.Random.Range(1,moveSpeed));
        }
    }

    private void GoToWayPoint()
    {
        if (isGoingTodWayPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPointDestination, moveSpeed * Time.deltaTime);
        }
    }

    public void SetRunSpeed(float runSpeed)
    {
        this.moveSpeed = runSpeed;
    }

    public void StartRunningAround()
    {
        isRunningAround = true;
        isGoingTodWayPoint = false;
    }

    public void StartGoingToWayPoint()
    {
        isGoingTodWayPoint = true;
        isRunningAround = false;
    }

    public void StopRunningAround()
    {
        isRunningAround = false;
    }

    public void StopGoingToWayPoint()
    {
        isGoingTodWayPoint = false;
    }

    public void SetDestination(Vector3 wayPointDestination)
    {
        this.wayPointDestination = wayPointDestination;
    }
}
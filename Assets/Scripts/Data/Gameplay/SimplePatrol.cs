using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class SimplePatrol : MonoBehaviour
{
    [SerializeField]
    private Direction direction;

    private Vector3 sineWave;
    private float amplitude;

    [SerializeField]
    private float patrolDistance;

    [SerializeField]
    private float patrolSpeed;

    [SerializeField]
    private float offset;

    private float orginalPosition;

    private bool isPatrolling;

    // Local time
    private float localTime;

    private void Start()
    {
        //frequency = 0.5f;
        //distanceModifier = 2f;

        switch (direction)
        {
            case Direction.Right:
                orginalPosition = transform.position.x;
                break;

            case Direction.Forward:
                orginalPosition = transform.position.z;
                break;
        }
    }

    private void Update()
    {
        // Delta time updates per frame so it should be calculated in Update
        if (isPatrolling) localTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            amplitude = Mathf.Sin(localTime / patrolSpeed + offset) * patrolDistance;

            switch (direction)
            {
                case Direction.Right:
                    sineWave = new Vector3(amplitude + orginalPosition, transform.position.y, transform.position.z);
                    break;
                case Direction.Forward:
                    sineWave = new Vector3(transform.position.x, transform.position.y, amplitude + orginalPosition);
                    break;
            }

            // use localPosition because this is a child object
            transform.localPosition = sineWave;
        }
    }

    public void SetPatrollingStatus(bool isPatrolling)
    {
        this.isPatrolling = isPatrolling;
    }

    public void SetPatrolDistance(float patrolDistance)
    {
        this.patrolDistance = patrolDistance;
    }

    public void SetPatrolSpeed(float patrolSpeed)
    {
        this.patrolSpeed = patrolSpeed;
    }

    public void SetPatrolDirection(Direction direction)
    {
        this.direction = direction;
    }
}

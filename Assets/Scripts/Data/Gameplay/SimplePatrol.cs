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
    private float distanceModifier;

    [SerializeField]
    private float frequency;

    [SerializeField]
    private float offset;

    private float orginalPosition;

    void Start()
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
        amplitude = Mathf.Sin(Time.time / frequency + offset) * distanceModifier;
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

    public void SetPatrolDistance(float value)
    {
        distanceModifier = value;
    }

    public void SetPatrolSpeed(float value)
    {
        frequency = value;
    }

    public void SetPatrolDirection(Direction _direction)
    {
        direction = _direction;
    }
}

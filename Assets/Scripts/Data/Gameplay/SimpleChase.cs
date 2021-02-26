using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component is used to chase player
public class SimpleChase : MonoBehaviour
{
    private float chaseSpeed;
    private bool isChasing;

    private void FixedUpdate()
    {
        Chase(Player.main.Transform);
    }

    private void Chase(Transform target)
    {
        if (isChasing)
        {
            Vector3 direction = target.position - transform.position;

            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = lookRotation;

            transform.Translate(Vector3.forward * Time.deltaTime * chaseSpeed);
        }
    }

    public void SetChaseSpeed(float chaseSpeed)
    {
        this.chaseSpeed = chaseSpeed;
    }

    public void StarChase()
    {
        isChasing = true;
    }

    public void StopChase()
    {
        isChasing = false;
    }
}

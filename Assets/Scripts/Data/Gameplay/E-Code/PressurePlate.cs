using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject effectedObject;
    public bool xMove, yMove, zMove, xDirection, yDirection, zDirection;
    public float pressureTimer, releaseTimer, moveAmount;
    public float maxTimeOn;

    float timeMark;
    bool moved;

    // Start is called before the first frame update
    void Start()
    {
        moved = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) timeMark = Time.time + pressureTimer;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!moved)
            {
                if(Time.time > timeMark)
                {
                    moved = true;
                    effectedObject.transform.Translate(0f, 0f, 0f);
                }
            }
        }
    }
}

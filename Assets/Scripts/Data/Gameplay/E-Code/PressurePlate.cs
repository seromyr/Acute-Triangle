using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject effectedObject;
    //public bool xMove, yMove, zMove, xDirection, yDirection, zDirection;
    public float pressureTimer, releaseTimer, moveXAmount, moveYAmount, moveZAmount;
    public float maxTimeOn;

    float timeMark;
    bool moved, playerOn, ResetPos;

    // Start is called before the first frame update
    void Start()
    {
        moved = false;
        playerOn = false;
        ResetPos = false;
    }

    private void Update()
    {
        if (ResetPos)
        {
            if (Time.time > timeMark)
            {
                ResetObject();
                moved = false;
                ResetPos = false;
            }
        }
    }

    void MoveObject()
    { 
        effectedObject.transform.Translate(moveXAmount, moveYAmount, moveZAmount);      
    }

    void ResetObject()
    {
        effectedObject.transform.Translate(-moveXAmount, -moveYAmount, -moveZAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) timeMark = Time.time + pressureTimer;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOn = true;
            if (!moved)
            {
                if(Time.time > timeMark)
                {
                    moved = true;
                    MoveObject();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerOn)
            {
                timeMark = Time.time + releaseTimer;
                playerOn = false;
                ResetPos = true;
            }

        }
    }
}

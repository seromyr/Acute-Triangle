using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use this class to automatically destroy object it attaches to
public class SelfDestruct : MonoBehaviour
{
    private float countDown;

    public void SetCountDown(float countDown)
    {
        this.countDown = countDown;
    }
    private void Start()
    {
        if (countDown == 0)
        {
            countDown = 5f;
        }
    }

    private void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }

        if (countDown <= 0)
        {
            Destroy(gameObject);
        }
    }
}

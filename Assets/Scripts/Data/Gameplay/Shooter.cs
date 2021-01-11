using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private GameObject destroyableBullet;
    private float time;

    private float speed;
    void Start()
    {
        destroyableBullet = Resources.Load<GameObject>("Prefabs/KillAbleBullet");
        time = Time.time;
        speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= time + 1/speed)
        {
            var meNew = Instantiate(destroyableBullet, transform.position + transform.forward, transform.rotation);
            meNew.GetComponent<SelfMovingFoward>().direction = transform.position;
            time = Time.time;
        }
    }
}

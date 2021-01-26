using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Shooter : MonoBehaviour
{
    public BulletType bulletType;
    private GameObject bullet;
    private float time;
    private float speed;

    public void SetShootingParameters(float _speed, BulletType _bulletType)
    {
        speed = _speed;
        bulletType = _bulletType;
    }

    private void Start()
    {
        // Decide which type of bullet this shooter will fire
        switch (bulletType)
        {
            case BulletType.Destructible:
                bullet = Resources.Load<GameObject>("Prefabs/DestructableBullet");
                break;
            case BulletType.Indestructible:
                bullet = Resources.Load<GameObject>("Prefabs/IndestructableBullet");
                break;
        }

        time = Time.time;
        //speed = 10;
    }

    private void Update()
    {
        if (Time.time >= time + 1/speed)
        {
            var meNew = Instantiate(bullet, transform.position + transform.forward, transform.rotation);
            //meNew.GetComponent<EnemyBulletSelfMovingForward>().direction = transform.position;
            time = Time.time;
        }
    }
}

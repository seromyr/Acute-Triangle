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
    private float size;
    private float bulletSpeed;

    public void SetShootingParameters(float _shootSpeed, float _size , float _bulletSpeed, BulletType _bulletType)
    {
        speed = _shootSpeed;
        bulletType = _bulletType;
        size = _size;
        bulletSpeed = _bulletSpeed;
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
    }

    private void Update()
    {
        if (Time.time >= time + 1/speed)
        {
            var thisBullet = Instantiate(bullet, transform.position + transform.forward, transform.rotation);
            thisBullet.transform.localScale *= size;
            thisBullet.GetComponent<EnemyBulletMechanic>().SetMovingSpeed(bulletSpeed);
            time = Time.time;
        }
    }
}

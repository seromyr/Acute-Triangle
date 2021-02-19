using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Shooter : MonoBehaviour
{
    public BulletType bulletType;
    private GameObject bullet;
    private float fireRate;
    private float bulletSize;
    private float bulletSpeed;
    private Timer timer;
    private bool isShooting;

    public void SetShootingParameters(float fireRate, float bulletSize, float bulletSpeed, BulletType bulletType, bool isShooting = true)
    {
        this.fireRate = fireRate;
        this.bulletType = bulletType;
        this.bulletSize = bulletSize;
        this.bulletSpeed = bulletSpeed;
        this.isShooting = isShooting;
    }

    public void PauseShooting()
    {
        timer.PauseTimer();
    }

    public void ResumeShooting()
    {
        timer.ResumeTimer();
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

        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(fireRate, 1, () => { InstantiatBullet(); });
        timer.SetLoop(true);
    }

    private void InstantiatBullet()
    {
        var bulletInstance = Instantiate(bullet, transform.position + transform.forward, transform.rotation);
        bulletInstance.transform.localScale *= bulletSize;
        bulletInstance.GetComponent<EnemyBulletMechanic>().SetMovingSpeed(bulletSpeed);
    }
}

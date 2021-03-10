using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Blaster : MonoBehaviour
{
    public BulletType bulletType;
    private GameObject bullet;
    private float fireRate;
    private float bulletSpeed;
    private Timer timer;
    private Transform bulletContainer;

    public void SetShootingParameters(float fireRate, float bulletSpeed, BulletType bulletType)
    {
        this.fireRate = fireRate;
        this.bulletType = bulletType;
        this.bulletSpeed = bulletSpeed;

        StartShooting();
    }

    public void PauseShooting()
    {
        timer.PauseTimer();
        //Debug.Log("Pause Shooting");
    }

    public void ResumeShooting()
    {
        timer.ResumeTimer();
        //Debug.Log("Resume Shooting");
    }

    private void StartShooting()
    {
        // Decide which type of bullet this shooter will fire
        switch (bulletType)
        {
            case BulletType.Destructible:
                LoadBullet(Bullet._03, "Destructible");
                break;
            case BulletType.Indestructible:
                LoadBullet(Bullet._04, "Indestructible");
                break;
            case BulletType.Mixed:
                if (Random.value == 0.5f) LoadBullet(Bullet._03, "Destructible");
                else                      LoadBullet(Bullet._04, "Indestructible");
                break;
        }

        timer.SetTimer(fireRate, 1, () =>
        {
            if (bulletType == BulletType.Mixed)
            {
                if (bullet.name == "Destructible") LoadBullet(Bullet._04, "Indestructible");
                else LoadBullet(Bullet._03, "Destructible");
            }
            InstantiatBullet();
        });
        timer.SetLoop(true);
    }

    private void LoadBullet(string bullet, string name)
    {
        this.bullet = Resources.Load<GameObject>(bullet);
        this.bullet.name = name;
    }

    private void Awake()
    {
        timer = gameObject.AddComponent<Timer>();

        if (!GameObject.Find("BulletContainer"))
        {
            // Create bullet container for organized objects managing
            bulletContainer = new GameObject("BulletContainer").transform;
        }
        else
        {
            bulletContainer = GameObject.Find("BulletContainer").transform;
        }
    }

    private void InstantiatBullet()
    {
        var bulletInstance = Instantiate(bullet, transform.position + transform.forward * 1.5f, transform.rotation, bulletContainer);
        bulletInstance.GetComponent<EnemyBulletMechanic>().SetMovingSpeed(bulletSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Blaster : MonoBehaviour
{
    public BulletType bulletType;
    private GameObject bullet;
    private float fireRate;
    private float bulletSize;
    private float bulletSpeed;
    private Timer timer;
    private Transform bulletContainer;

    public void SetShootingParameters(float fireRate, float bulletSize, float bulletSpeed, BulletType bulletType)
    {
        this.fireRate = fireRate;
        this.bulletType = bulletType;
        this.bulletSize = bulletSize;
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
                bullet = Resources.Load<GameObject>("Prefabs/DestructableBullet");
                break;
            case BulletType.Indestructible:
                bullet = Resources.Load<GameObject>("Prefabs/IndestructableBullet");
                break;
        }

        timer.SetTimer(fireRate, 1, () => { InstantiatBullet(); });
        timer.SetLoop(true);
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
        bulletInstance.transform.localScale *= bulletSize;
        bulletInstance.GetComponent<EnemyBulletMechanic>().SetMovingSpeed(bulletSpeed);
    }
}

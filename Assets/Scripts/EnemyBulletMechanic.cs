using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnemyBulletMechanic : MonoBehaviour
{
    public BulletType bulletType;
    private float bulletSpeed;
    private Transform bulletContainer;

    // Explosi bullet params
    private float actualSpeed = 0;
    private float t = 0;

    private void Start()
    {
        bulletContainer = GameObject.Find("BulletContainer").transform;
    }

    private void FixedUpdate()
    {
        // Automatically moving forward after firing

        if (bulletType != BulletType.Explosive)
        {
            MoveForward(bulletSpeed);
        }

        // Automactically explode if too close to player on Explosive bullet type
        if (bulletType == BulletType.Explosive)
        {
            MoveForwardLerp(bulletSpeed);
            Explode();
        }
    }

    private void MoveForward(float _speed)
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }


    private void MoveForwardLerp(float _speed)
    {
        actualSpeed = Mathf.Lerp(_speed * 1.5f, _speed / 4, t);
        t += Time.deltaTime * 0.9f;

        transform.Translate(Vector3.forward * Time.deltaTime * actualSpeed);
    }

    // This object will self-destruct if collides with bullet tagged objecct
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Bullet") && bulletType == BulletType.Destructible)
        {
            CreateDeadVFX();
            Destroy(gameObject);
        }
    }

    // This object will self-destruct if collide with any game object
    private void OnTriggerEnter(Collider other)
    {
        CreateDeadVFX();
        Destroy(gameObject);
    }

    // Spawn an explosion effect upon destroy
    private void CreateDeadVFX()
    {
        Quaternion lookDir = transform.rotation;
        GameObject spawnVFX = null;

        if (bulletType == BulletType.Destructible)
        {
            spawnVFX = Resources.Load<GameObject>("Prefabs/VFX/DestructibleBulletExplosionVFX");
        }
        else if (bulletType == BulletType.Indestructible)
        {
            spawnVFX = Resources.Load<GameObject>("Prefabs/VFX/IndestructibleBulletExplosionVFX");
        }
        else if (bulletType == BulletType.Explosive)
        {
            spawnVFX = Resources.Load<GameObject>("Prefabs/VFX/ExplosiveBulletExplosionVFX");
        }

        Instantiate(spawnVFX, transform.position, lookDir, bulletContainer);
    }

    private void Explode()
    {
        float distance = (Player.main.GetPosition - transform.position).magnitude;

        if (distance <= 3)
        {
            CreateDeadVFX();
            Destroy(gameObject);

            for (int i = 0; i < 8; i++)
            {
                Quaternion firstAngle = Quaternion.LookRotation(transform.position - Player.main.GetPosition);
                Quaternion thisAngle = Quaternion.Euler(0, 45 * i + firstAngle.eulerAngles.y, 0);
                GameObject bullet = Resources.Load<GameObject>(Bullet._06);
                var bulletInstance = Instantiate(bullet, transform.position, thisAngle, bulletContainer);
                bulletInstance.GetComponent<EnemyBulletMechanic>().SetMovingSpeed(3);
            }
        }
    }

    public void SetMovingSpeed(float speed)
    {
        bulletSpeed = speed;
    }


}

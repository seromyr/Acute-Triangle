using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnemyBulletMechanic : MonoBehaviour
{
    public BulletType bulletType;
    private float bulletSpeed;
    private Transform bulletContainer;

    private void Start()
    {
        bulletContainer = GameObject.Find("BulletContainer").transform;
    }

    private void FixedUpdate()
    {
        // Automatically moving forward after firing
        MoveForward(bulletSpeed);
    }

    private void MoveForward(float _speed)
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    // This object will self-destruct if collides with bullet tagged objecct
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Bullet") && bulletType == BulletType.Destructible)
        {
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

        GameObject.Instantiate(spawnVFX, transform.position, lookDir, bulletContainer);
    }

    public void SetMovingSpeed(float speed)
    {
        bulletSpeed = speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class EnemyBulletMechanic : MonoBehaviour
{
    public BulletType bulletType;
    private float bulletSpeed;

    private void FixedUpdate()
    {
        // Automatically moving forward after firing
        //transform.Translate(Vector3.forward * Time.deltaTime * GeneralConst.ENEMY_BULLET_SPEED);
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
        Destroy(gameObject);
    }

    // Spawn an explosion effect upon destroy
    private void OnDestroy()
    {
        //Vector3 dir = transform.position - Player.main.GetPosition;
        //Quaternion lookDir = Quaternion.LookRotation(dir);

        Quaternion lookDir = transform.rotation;
        if (transform.name.Contains("Destructible"))
        {
            GameObject spawnVFX = Resources.Load<GameObject>("Prefabs/VFX/DestructibleBulletExplosionVFX");
            GameObject.Instantiate(spawnVFX, transform.position, lookDir);
        }
        else if (transform.name.Contains("Indestructible"))
        {
            GameObject spawnVFX = Resources.Load<GameObject>("Prefabs/VFX/IndestructibleBulletExplosionVFX");
            GameObject.Instantiate(spawnVFX, transform.position, lookDir);
        }
            
    }

    public void SetMovingSpeed(float speed)
    {
        bulletSpeed = speed;
    }
}

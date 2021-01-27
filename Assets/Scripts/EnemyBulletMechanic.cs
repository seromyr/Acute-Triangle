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

    public void SetMovingSpeed(float speed)
    {
        bulletSpeed = speed;
    }
}

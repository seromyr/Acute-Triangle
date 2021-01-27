using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBulletCollision : MonoBehaviour
{
    // This object will self-destruct if collides with bullet tagged objecct
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitMonitor3 : MonoBehaviour
{

    private int count;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (count >= 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        
        if (other.CompareTag("Bullet"))
        {
            count++;
        }
    }
}

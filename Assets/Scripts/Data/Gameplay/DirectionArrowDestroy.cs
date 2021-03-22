using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrowDestroy : MonoBehaviour
{
    private SphereCollider trigger;
    private void Start()
    {
        trigger = gameObject.AddComponent<SphereCollider>();
        trigger.radius = 6f;
        trigger.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player.main.Body)
        {
            Destroy(gameObject);
        }
    }
}

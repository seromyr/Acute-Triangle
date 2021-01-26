using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Player takes damange from bullet
        if (other.CompareTag(Constants.GeneralConst.BULLET))
        {
            Player.main.TakeDamage(1);
        }
    }
}

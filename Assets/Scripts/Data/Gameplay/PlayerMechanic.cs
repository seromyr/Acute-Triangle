using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanic : MonoBehaviour
{
    private Material _damageMaterial;
    private float offsetY, rotY;

    private void Start()
    {
        _damageMaterial = transform.Find(Player.main.Avatar.name).Find("Shield").GetComponent<MeshRenderer>().material;
        offsetY = -0.6f;
        _damageMaterial.SetFloat("Vector1_134BF8AF", offsetY);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player takes damange from bullet
        if (other.CompareTag(Constants.GeneralConst.BULLET))
        {
            Player.main.TakeDamage(1);
            Player.main.PlayHitSound();

            Vector3 dir = transform.position - other.transform.position;
            Quaternion angle = Quaternion.LookRotation(dir);
            rotY = angle.eulerAngles.y;
        }
    }

    private void Update()
    {
        if (offsetY >= 0 && offsetY <= 10)
        {
            offsetY += Time.deltaTime * 5f;
            _damageMaterial.SetFloat("Vector1_134BF8AF", offsetY);
            _damageMaterial.SetFloat("Vector1_DBB166E2", rotY);
        }
    }

    public void PlayHitFX()
    {
        offsetY = 0;
    }
}

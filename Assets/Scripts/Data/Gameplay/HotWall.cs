using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWall : MonoBehaviour
{
    private BoxCollider damageTriggerBox;

    private Material material;

    private AudioSource audioSource;

    private void Awake()
    {
        damageTriggerBox = gameObject.AddComponent<BoxCollider>();
        damageTriggerBox.isTrigger = true;
        damageTriggerBox.size = Vector3.one * 1.25f;

        material = transform.GetComponent<MeshRenderer>().material;

        audioSource = transform.GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Player.main.Name)
        {
            material.SetFloat("Intensity_", 5);
            audioSource.Play();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == Player.main.Name)
        {
            Player.main.PushPlayer(transform.position, 500);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == Player.main.Name)
        {
            Player.main.TakeDamage(1);
            material.SetFloat("Intensity_", 1);
        }
    }
}

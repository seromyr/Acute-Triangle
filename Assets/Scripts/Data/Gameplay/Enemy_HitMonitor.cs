using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitMonitor : MonoBehaviour
{
    private ParticleSystem takeDamageFX;

    public event EventHandler OnBulletHit;

    private bool acceptDamage;
    private AudioSource sound;

    private void Awake()
    {
        // All enemies accept damage by default
        SetDamageAcceptance(true);
        //sound = GetComponent<AudioSource>();
    }

    void Start()
    {
        transform.Find("DamageParticle").TryGetComponent(out takeDamageFX);
        //sound.clip = Resources.Load<AudioClip>("SFX/enemy_taking_damage_02");
    }

    private void OnParticleCollision(GameObject other)
    {
        if (acceptDamage)
        {
            takeDamageFX.Play();

            GameObject audio = new GameObject();
            audio.AddComponent<AudioPlayer>();

            // Fire up the bullet hit event
            OnBulletHit?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public void SetDamageAcceptance(bool acceptDamage)
    {
        this.acceptDamage = acceptDamage;
    }
}

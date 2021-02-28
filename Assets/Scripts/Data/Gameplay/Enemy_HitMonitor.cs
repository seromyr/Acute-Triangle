using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitMonitor : MonoBehaviour
{
    private ParticleSystem takeDamageFX;

    public event EventHandler OnBulletHit;

    private bool acceptDamage;

    private void Awake()
    {
        // All enemies accept damage by default
        SetDamageAcceptance(true);
    }

    void Start()
    {
        transform.Find("DamageParticle").TryGetComponent(out takeDamageFX);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (acceptDamage)
        {
            takeDamageFX.Play();

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

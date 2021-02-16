﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitMonitor : MonoBehaviour
{
    private MeshRenderer mr;
    private SphereCollider sc;
    private BoxCollider bc;

    private ParticleSystem takeDamageFX, deathFX;

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
        transform.Find("ExplodeParticle").TryGetComponent(out deathFX);

        TryGetComponent(out mr);
        TryGetComponent(out sc);
        TryGetComponent(out bc);
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

    public void PlayExplosionFX()
    {
        takeDamageFX.gameObject.SetActive(false);
        deathFX.Play(true);
        mr.enabled = false;

        if (sc != null)
        {
            sc.enabled = false;
        }

        if (bc != null)
        {
            bc.enabled = false;
        }

        if (transform.childCount > 2)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void KillSelf(float delay)
    {
        StartCoroutine(DestroySelfWithDelay(delay));
    }

    IEnumerator DestroySelfWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void SetDamageAcceptance(bool acceptDamage)
    {
        this.acceptDamage = acceptDamage;
        Debug.LogWarning(transform.name + " damage acceptance: " + this.acceptDamage);
    }
}

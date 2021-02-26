using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This is a default boss template
// This template is used to create customized boss for each level in Level Scenario used in every level
public class Enemy_Boss_Default : EnemyEntity
{
    // Event handler to handle boss death event
    public event EventHandler OnDestroy;

    // Constructor, declared in Level Scenario
    public Enemy_Boss_Default(string name, string prefabName, Transform parent, string material, float maxHealth, EventHandler OnDeadCallback)
    {
        // Initialize boss game object
        CreateBody(name, prefabName, parent, material);

        // Set up gameplay parameters
        GameplaySetup(maxHealth);

        // Wire up the events
        HitMonitor.OnBulletHit += TakeDamage;
        OnDestroy += DestroySelf;
        OnDestroy += OnDeadCallback;
    }

    public override void TakeDamage(object sender, EventArgs e)
    {
        if (_health > 0)
        {
            // Bosses take damage from player only (might change later)
            ModifyHealth(-Player.main.GetDamage);
            //Debug.Log("hit");
        }
        else
        {
            // HP limiter
            SetHealth(0);

            // Fire up dead event
            OnDestroy?.Invoke(this, EventArgs.Empty);
            Suicide();
        }
    }

    // Automatically called when HP is zero
    public override void DestroySelf(object sender, EventArgs e)
    {
        Debug.Log(_name + " was killed");

        // Play dead effect
        HitMonitor.PlayExplosionFX();

        // Cannot take damage anymore
        HitMonitor.OnBulletHit -= TakeDamage;

        // Clear event subscriptions
        OnDestroy = delegate { };

        // Self-destruct after 2 seconds
        HitMonitor.KillSelf(2);
    }
}
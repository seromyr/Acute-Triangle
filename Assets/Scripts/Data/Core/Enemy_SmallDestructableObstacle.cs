using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the small destructable obstacle template
// This template can be used to create chunks of path blockers in Level Scenario used in any level

public class Enemy_SmallDestructableObstacle : EnemyEntity
{
    // Event handler to handle death event
    public event EventHandler OnDestroy;

    // 

    // Constructor, declared in Level Scenario
    public Enemy_SmallDestructableObstacle(string name, string prefabName, Transform parent, float maxHealth)
    {
        // Initialize obstacle game object
        CreateBody(name, prefabName, parent);

        // Set up gameplay parameters
        GameplaySetup(maxHealth);

        // Wire up the events
        Mechanic.OnBulletHit += TakeDamage;
        OnDestroy += DestroySelf;

    }

    public override void TakeDamage(object sender, EventArgs e)
    {
        if (_health > 0)
        {
            // This object takes damage from player only (might change later)
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
        Mechanic.PlayExplosionFX();

        // Cannot take damage anymore
        Mechanic.OnBulletHit -= TakeDamage;

        // Clear event subscriptions
        OnDestroy = delegate { };

        // Self-destruct after 2 seconds
        Mechanic.KillSelf(2);
    }

    public override void Shoot(GameObject cannon, Quaternion pointingAngle, float shootingSpeed, float bulletSize, float bulletSpeed, BulletType bulletType)
    {
        // This enemy does not shoot.
    }
}

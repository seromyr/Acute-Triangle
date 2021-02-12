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
    public Enemy_Boss_Default(string name, string prefabName, Transform parent, float maxHealth, EventHandler OnDeadCallback)
    {
        // Initialize boss game object
        CreateBody(name, prefabName, parent);

        // Set up gameplay parameters
        GameplaySetup(maxHealth);

        // Wire up the events
        Mechanic.OnBulletHit += TakeDamage;
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
        Mechanic.PlayExplosionFX();

        // Cannot take damage anymore
        Mechanic.OnBulletHit -= TakeDamage;

        // Clear event subscriptions
        OnDestroy = delegate { };

        // Self-destruct after 2 seconds
        Mechanic.KillSelf(2);
    }

    // Shooting parameter setting used in level scenario
    // This method create boss shooters
    public override void Shoot(GameObject cannon, Quaternion pointingAngle, float shootingSpeed, float bulletSize, float bulletSpeed, BulletType bulletType)
    {
        var thisShooter = UnityEngine.Object.Instantiate(cannon, GetPosition, pointingAngle, _body.transform);
        thisShooter.GetComponent<Shooter>().SetShootingParameters(shootingSpeed, bulletSize, bulletSpeed, bulletType);
    }
}
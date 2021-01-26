using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the small destructable obstacle template
// This template can be used to create chunks of path blockers in Level Scenario used in any level

public class Enemy_SmallDestructableObstacle : EnemyEntity
{
    // Mechanic component, used to define take damage ability
    private Enemy_Mechanic mechanic;

    // Event handler to handle death event
    public event EventHandler OnDestroy;

    // Constructor, declared in Level Scenario
    public Enemy_SmallDestructableObstacle(string name, string prefabName, Transform parent, float maxHealth)
    {
        // Initialize obstacle game object
        CreateBody(name, prefabName, parent);

        // Load up mechanics
        mechanic = _body.AddComponent<Enemy_Mechanic>();

        // Wire up the events
        mechanic.OnBulletHit += TakeDamage;
        OnDestroy += DestroySelf;

        // Set up gameplay parameters
        GameplaySetup(maxHealth);
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
        mechanic.PlayExplosionFX();

        // Cannot take damage anymore
        mechanic.OnBulletHit -= TakeDamage;

        // Cannot destroy self anymore
        OnDestroy -= DestroySelf;

        // Self-destruct after 2 seconds
        mechanic.KillSelf(2);
    }

    public override void Patrol(bool isPatrolling, Direction direction, float distance, float speed)
    {
        // This enemy does not move.
    }

    public override void Shoot(GameObject cannon, Quaternion pointingAngle, float shootingSpeed, BulletType bulletType)
    {
        // This enemy does not shoot.
    }
}

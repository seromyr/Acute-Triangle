using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This is a default boss template
// This template is used to create customized boss for each level in Level Scenario used in every level
public class Enemy_Boss_Default : EnemyEntity
{
    // Mechanic component, used to define take damage ability
    protected Enemy_Mechanic mechanic;

    // Boss patrol mechanic, can be customized in Level Scenario if used
    protected SimplePatrol simplePatrol;

    // Event handler to handle boss death event
    public event EventHandler OnDestroy;

    // Constructor, declared in Level Scenario
    public Enemy_Boss_Default(string name, string prefabName, Transform parent, float maxHealth)
    {
        // Initialize boss game object
        CreateBody(name, prefabName, parent);

        // Load up mechanics
        mechanic = _body.AddComponent<Enemy_Mechanic>();
        simplePatrol = _body.AddComponent<SimplePatrol>();

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
        mechanic.PlayExplosionFX();

        // Cannot take damage anymore
        mechanic.OnBulletHit -= TakeDamage;

        // Cannot destroy self anymore
        OnDestroy -= DestroySelf;

        // Self-destruct after 2 seconds
        mechanic.KillSelf(2);
    }

    // Patrol parameter setting if used in Level Scenario
    public override void Patrol(bool isPatrolling, Direction direction, float distance, float speed)
    {
        simplePatrol.SetPatrollingStatus(isPatrolling);
        simplePatrol.SetPatrolDirection(direction);
        simplePatrol.SetPatrolDistance(distance);
        simplePatrol.SetPatrolSpeed(speed);
    }

    // Shooting parameter setting used in level scenario
    // This method create boss shooters
    public override void Shoot(GameObject cannon, Quaternion pointingAngle, float shootingSpeed, BulletType bulletType)
    {
        var thisShooter = UnityEngine.Object.Instantiate(cannon, GetPosition, pointingAngle, _body.transform);
        thisShooter.GetComponent<Shooter>().SetShootingParameters(shootingSpeed, bulletType);
    }
}
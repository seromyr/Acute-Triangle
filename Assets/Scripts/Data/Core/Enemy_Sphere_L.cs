using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sphere_L : EnemyEntity
{
    protected DamageMonitor damageMonitor;
    protected SimplePatrol simplePatrol;
    public event EventHandler OnDestroy;

    public Enemy_Sphere_L(string prefabName, string name, Transform parent, float health, float maxHealth)
    {
        _body = SetForm(prefabName);
        _name = name;
        _hitpoint = health;
        _hitpointMax = maxHealth;
        _isAlive = true;

        CreateAvatar(parent);

        // Assign behavior class
        //......
        damageMonitor = Avatar.AddComponent<DamageMonitor>();
        simplePatrol = Avatar.AddComponent<SimplePatrol>();
        damageMonitor.OnBulletHit += TakeDamage;
        OnDestroy += DestroySelf;
    }

    public override void TakeDamage(object sender, EventArgs e)
    {
        if (_hitpoint > 0)
        {
            _hitpoint--;
            //Debug.Log("hit");
        }
        else
        {
            // HP limiter
            _hitpoint = 0;

            // Fire up dead event
            OnDestroy?.Invoke(this, EventArgs.Empty);
            _isAlive = false;
        }
    }

    public override void DestroySelf(object sender, EventArgs e)
    {
        Debug.Log(_name + " was killed");

        // Play dead effect
        damageMonitor.PlayExplosionFX();

        // Cannot take damage anymore
        damageMonitor.OnBulletHit -= TakeDamage;

        // Cannot destroy self anymore
        OnDestroy -= DestroySelf;

        damageMonitor.KillSelf(2);
    }

    public override void Patrol(Direction direction, float distance, float speed)
    {
        simplePatrol.SetPatrolDirection(direction);
        simplePatrol.SetPatrolDistance(distance);
        simplePatrol.SetPatrolSpeed(speed);
    }

    public override void Shoot(GameObject cannon)
    {
        UnityEngine.Object.Instantiate(cannon, _avatar.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)), _avatar.transform);
    }
}
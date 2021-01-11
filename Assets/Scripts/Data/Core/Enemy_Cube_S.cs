using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Cube_S : EnemyEntity
{
    private DamageMonitor damageMonitor;
    public event EventHandler OnDestroy;
    public Enemy_Cube_S(string prefabName, string name, Transform parent, float health, float maxHealth)
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
        // This enemy does not move.
    }

    public override void Shoot(GameObject cannon)
    {
        // This enemy does not shoot.
    }
}

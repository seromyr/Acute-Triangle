using Entities;
using System;
using Constants;
using UnityEngine;

public abstract class EnemyEntity : Entity, IDamageable, IMoveable, IShootable
{
    protected void CreateBody(string name, string prefabName, Transform parent)
    {
        _name = name;
        _body = GetBodyPrefab(prefabName);
        _body = UnityEngine.Object.Instantiate(GetBodyPrefab(prefabName), parent);
        _body.name = _name;
    }

    protected void GameplaySetup(float maxHealth)
    {
        _maxHealth = maxHealth;
        _health = _maxHealth;
        _isAlive = true;
    }

    public abstract void TakeDamage(object sender, EventArgs e);
    public abstract void DestroySelf(object sender, EventArgs e);
    public abstract void Patrol(bool isPatrolling, Direction direction, float distance, float speed);
    public abstract void Shoot(UnityEngine.GameObject cannon, UnityEngine.Quaternion pointingAngle, float shootingSpeed, Constants.BulletType bulletType);
}
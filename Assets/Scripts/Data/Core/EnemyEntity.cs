using Entities;
using System;
using Constants;
using UnityEngine;

public abstract class EnemyEntity : Entity, IDamageable, IMoveable, IShootable
{
    protected void CreateAvatar(Transform parent)
    {
        _avatar = UnityEngine.Object.Instantiate(_body, parent);
        _avatar.name = _name;
    }

    public abstract void TakeDamage(object sender, EventArgs e);
    public abstract void DestroySelf(object sender, EventArgs e);
    public abstract void Patrol(Direction direction, float distance, float speed);
    public abstract void Shoot(GameObject cannon);
}
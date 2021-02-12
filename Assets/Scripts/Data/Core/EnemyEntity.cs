using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using Entities;

public abstract class EnemyEntity : Entity, IDamageable, IShootable
{
    // Mechanic component, used to define damage taking ability
    private Enemy_Mechanic mechanic;
    public Enemy_Mechanic Mechanic { get { return mechanic; } }

    // Enemy feature (see Feature enum in Constants)
    private Features features;
    public Features Features { get { return features; } }

    protected void CreateBody(string name, string prefabName, Transform parent)
    {
        _name = name;
        _body = GetBodyPrefab(prefabName);
        _body = UnityEngine.Object.Instantiate(GetBodyPrefab(prefabName), parent);
        _body.name = _name;
    }

    protected void GameplaySetup(float maxHealth)
    {
        // Load up mechanics
        mechanic = _body.AddComponent<Enemy_Mechanic>();

        // Instantiate features
        features = new Features(_body);

        // Initialize core parameters
        _maxHealth = maxHealth;
        _health = _maxHealth;
        _isAlive = true;
    }

    public abstract void TakeDamage(object sender, EventArgs e);
    public abstract void DestroySelf(object sender, EventArgs e);
    public abstract void Shoot(UnityEngine.GameObject cannon, UnityEngine.Quaternion pointingAngle, float shootingSpeed, float bulletSize, float bulletSpeed, Constants.BulletType bulletType);

    //// Has minion
    //public List<EnemyEntity> minionList;

    //public void CreateMinions(string name, string prefabName, Transform parent)
    //{
    //    // Automatically create shield if a boss has minions
    //    //CreateShield();

    //    // Create minion list
    //    minionList = new List<EnemyEntity>();
    //}
}
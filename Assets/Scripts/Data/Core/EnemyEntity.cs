using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using Entities;

public abstract class EnemyEntity : Entity, IDamageable
{
    // Component to control material
    private MeshRenderer meshRenderer;
    public MeshRenderer MeshRenderer { get { return meshRenderer; } }

    // Mechanic component, used to define damage taking ability
    private Enemy_HitMonitor hitMonitor;
    public Enemy_HitMonitor HitMonitor { get { return hitMonitor; } }

    // Enemy feature (see Feature enum in Constants)
    private Features mechanics;
    public Features Mechanics { get { return mechanics; } }

    protected void CreateBody(string name, string prefabName, Transform parent, string material)
    {
        _name = name;
        _body = GetBodyPrefab(prefabName);
        _body = UnityEngine.Object.Instantiate(GetBodyPrefab(prefabName), parent);
        _body.name = _name;

        meshRenderer = _body.GetComponent<MeshRenderer>();

        if (material != "default")
        {
            SetMaterial(material);
        }
    }

    protected void GameplaySetup(float maxHealth)
    {
        // Load up mechanics
        hitMonitor = _body.AddComponent<Enemy_HitMonitor>();

        // Instantiate features
        mechanics = new Features(_body);

        // Initialize core parameters
        _maxHealth = maxHealth;
        _health = _maxHealth;
        _isAlive = true;
    }

    protected void SetMaterial(string materialName)
    {
        meshRenderer.material = Resources.Load<Material>("Materials/" + materialName);
    }

    public abstract void TakeDamage(object sender, EventArgs e);
    public abstract void DestroySelf(object sender, EventArgs e);
}
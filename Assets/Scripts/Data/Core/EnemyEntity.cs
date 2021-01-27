using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using Entities;

public abstract class EnemyEntity : Entity, IDamageable, IMoveable, IShootable
{
    private GameObject _shield;


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
    public abstract void Chase(bool isChasing, float speed);
    public abstract void Shoot(UnityEngine.GameObject cannon, UnityEngine.Quaternion pointingAngle, float shootingSpeed, float bulletSize, float bulletSpeed, Constants.BulletType bulletType);

    // Has minion
    public List<EnemyEntity> minionList;

    public void CreateMinions(string name, string prefabName, Transform parent)
    {
        // Automatically create shield if a boss has minions
        CreateShield();

        // Create minion list
        minionList = new List<EnemyEntity>();
    }

    private void CreateShield()
    {
        _shield = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _shield.transform.parent = _body.transform;
        _shield.transform.localPosition = Vector3.zero;
        _shield.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Shield");
        _shield.transform.localScale = Vector3.one * 1.2f;
        _shield.name = "Shield";
    }

    public void DestroyShield()
    {
        //UnityEngine.Object.Destroy(_shield);
        _shield.SetActive(false);
    }
}
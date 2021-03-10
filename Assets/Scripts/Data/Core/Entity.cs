using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entities
{
    public abstract class Entity
    {
        protected string        _name;
        protected GameObject    _body;
        protected GameObject    _avatar;
        protected GameObject    _skin;
        protected float         _health;
        protected float         _maxHealth;
        protected float         _damage;
        protected bool          _isAlive;
        protected bool          _isSingleton;


        public string       Name      { get { return _name;           } }
        public GameObject   Body      { get { return _body;           } }
        public Transform    Transform { get { return _body.transform; } }
        public GameObject   Avatar    { get { return _avatar;         } }
        public float        Health    { get { return _health;         } }
        public float        MaxHealth { get { return _maxHealth;      } }
        public float        Damage    { get { return _damage;         } }
        public bool         IsAlive   { get { return _isAlive;        } }

        protected GameObject GetBodyPrefab(string prefabName)
        {
            return Resources.Load<GameObject>("Prefabs/" + prefabName);
        }

        protected GameObject GetSkinPrefab(string skinName)
        {
            return Resources.Load<GameObject>("Prefabs/Skins/" + skinName);
        }

        public void SetBodyActive(bool status)
        {
            _body.SetActive(status);
        }

        public Vector3 GetPosition { get { return _body.transform.position; } }

        public void SetPosition(Vector3 newPosition)
        {
            _body.transform.position = newPosition;
        }

        public void SetRotation(Quaternion quaternion)
        {
            _body.transform.rotation = quaternion;
        }

        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetHealth(float health)
        {
            _health = health;
        }

        public void ModifyHealth(float value)
        {
            _health += value;
        }

        public float GetDamage{ get { return _damage; } }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        public void Revive()
        {
            _isAlive = true;
        }

        public void Suicide()
        {
            _isAlive = false;
        }
    }
}


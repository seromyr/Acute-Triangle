using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entities
{
    public abstract class Entity
    {
        protected string _name;

        protected GameObject    _body;
        protected GameObject    _avatar;
        protected GameObject    _skin;
        protected float         _hitpoint;
        protected float         _hitpointMax;
        protected float         _damage;
        protected bool          _isAlive;

        public string       Name    { get { return _name; } }
        public GameObject   Form    { get { return _body; } }
        public GameObject   Skin    { get { return _skin; } }
        public GameObject   Avatar  { get { return _avatar; } }
        public float        Health  { get { return _hitpoint; } }
        public float        Damage  { get { return _damage; } }

        protected GameObject SetForm(string prefabName)
        {
            return Resources.Load<GameObject>("Prefabs/" + prefabName);
        }

        protected void CreateAvatar()
        {
            _avatar = Object.Instantiate(_body);
            _avatar.name = _name;
        }
        protected GameObject ChangeSkin(string name)
        {
            return Resources.Load<GameObject>("Prefabs/Skins/" + name);
        }
    }
}


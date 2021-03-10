﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This is a default template for enemy creation
// This template is used to create customized boss for each level in Level Scenario used in every level
public class Enemy_Default : EnemyEntity
{
    // Event handler to handle on destroy event
    public event EventHandler OnDestroy;

    //Audio source
    private AudioSource _soundplayer;
    private List<AudioClip> clips;

    // Constructor, declared in Level Scenario
    public Enemy_Default(string name, string prefabName, Transform parent, string material, float maxHealth, EventHandler OnDeadCallback)
    {
        // Initialize enemy game object in scene
        CreateBody(name, prefabName, parent, material);

        // Set up gameplay parameters
        GameplaySetup(maxHealth);

        // Wire up the events
        HitMonitor.OnBulletHit += TakeDamage;
        OnDestroy += DestroySelf;
        OnDestroy += OnDeadCallback;

        //adding Audiosource and applying clip
        _soundplayer = _body.AddComponent<AudioSource>();
        _soundplayer.clip = Resources.Load<AudioClip>("SFX/taking_damage");
        
        //list created
        clips = new List<AudioClip>();
        clips.Add(Resources.Load<AudioClip>("SFX/taking_damage"));

    }

    public override void TakeDamage(object sender, EventArgs e)
    {
        if (_health > 0)
        {
            // Enemy takes damage from player only (might change later)
            ModifyHealth(-Player.main.GetDamage);

            //Debug.Log("hit");
            _soundplayer.PlayOneShot(_soundplayer.clip);
            
        }
        else
        {
            // HP limiter
            SetHealth(0);

            // Fire up dead event
            OnDestroy?.Invoke(this, EventArgs.Empty);

            // Set dead state
            Suicide();

            //play death sound
            _soundplayer.PlayOneShot(Resources.Load<AudioClip>("Resources/SFX/taking_damage"));
        }
    }

    // Automatically called when HP is zero
    public override void DestroySelf(object sender, EventArgs e)
    {
        Debug.Log(_name + " was killed");

        string deathVFX = "BossDieVFX";
        if (_name.Contains("Minion"))
        {
            deathVFX = "MinionDieVFX";
        }

        // Play dead effect
        UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/VFX/" + deathVFX), GetPosition, Quaternion.identity);

        // Clear event subscriptions
        HitMonitor.OnBulletHit -= TakeDamage;
        OnDestroy = delegate { };

        // Self-destruct and set dead state
        HitMonitor.SelfDestruct();
        Suicide();
    }
}
using System;
using Entities;
using Constants;
using UnityEngine;

public class Player : Entity
{
    // Create static instance
    public static Player main;

    // Create player event
    public event EventHandler OnZeroHealth;

    private PlayerMechanic _mechanic;
    private bool           _allowPlayerControl;
    private AudioSource    _soundplayer;
    private Vector3        startPosition;

    private Rigidbody      _rigidbody;

    // Player constructor
    public Player()
    {
        CreateBody();
        CreateAvatar();
        CreateMechanic();
        GameplaySetup();

        _isSingleton = true;
        Singletonize("Player created as a Singleton");
    }

    // Singletonize player instance
    private void Singletonize(string debugMessage)
    {
        if (_isSingleton)
        {
            if (main == null)
            {
                UnityEngine.Object.DontDestroyOnLoad(_body);
                main = this;
            }
            else if (main != null)
            {
                UnityEngine.Object.Destroy(_body);
            }
            Debug.Log(debugMessage);
        }
    }

    // Initialize player game object
    private void CreateBody()
    {
        _name = GeneralConst.PLAYER;
        _body = UnityEngine.Object.Instantiate(GetBodyPrefab(_name));
        _body.name = _name;
    }

    // Load player visual
    private void CreateAvatar()
    {
        // Using default skin
        _avatar = UnityEngine.Object.Instantiate(GetSkinPrefab(PlayerSkin._01), _body.transform);
        _avatar.name = PlayerSkin.DEFAULT;
    }

    // Load player mechanics
    private void CreateMechanic()
    {
        _mechanic = _body.AddComponent<PlayerMechanic>();
        _body.AddComponent<PlayerController>();
        _rigidbody = _body.GetComponent<Rigidbody>();
        _soundplayer = _body.GetComponent<AudioSource>();
    }

    // Set up gameplay parameters
    private void GameplaySetup()
    {
        SetMaxHealth(PlayerAttributes.PLAYER_MAXHEALTH);
        SetHealth(_maxHealth);
        SetDamage(PlayerAttributes.PLAYER_DAMAGE);
        Revive();

        // Default player position in scene
        SetStartPostion(Vector3.zero);
    }

    public void SetStartPostion(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    // Reset player parameters
    public void Reset()
    {
        SetRotation(Quaternion.identity);

        // Allow to reset player position by using override parameter
        if (GetPosition != startPosition)
        {
            SetPosition(startPosition);
        }

        SetHealth(_maxHealth);
        Revive();
    }

    // Damage taking method
    public void TakeDamage(int damageTaken)
    {
        ModifyHealth(-damageTaken);

        //Dispatch the event that player has died
        if (_health <= 0)
        {
            SetBodyActive(false);
            Suicide();
            OnZeroHealth?.Invoke(this, EventArgs.Empty);
        }
    }

    // Push player away
    public void PushPlayer(Vector3 pusherPosition, float pushPower)
    {
        Vector3 pushDirection = pusherPosition - GetPosition;

        _rigidbody.AddForce(-pushDirection.normalized * pushPower);
    }

    public void PlayHitSound()
    {
        // Player take damage sound effect
        _soundplayer.PlayOneShot(_soundplayer.clip);
    }
}
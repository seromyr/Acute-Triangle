using Entities;
using Constants;
using UnityEngine;

public class PlayerEntity : Entity
{
    //private Vector3 startPos;

    public PlayerEntity()
    {
        _body        = SetForm(PrimeObj.PLAYER);
        _name        = PrimeObj.PLAYER;
        _hitpointMax = PlayerAttributes.PLAYER_MAXHEALTH;
        _hitpoint    = _hitpointMax;
        _damage      = PlayerAttributes.PLAYER_DAMAGE;

        CreateAvatar();

        // Add default skin
        _skin = Object.Instantiate(ChangeSkin(PlayerSkin.DEFAULT), _avatar.transform);
        _skin.name = PlayerSkin.DEFAULT;

        //startPos = _avatar.transform.position;
    }

    public void Reset()
    {
        _avatar.transform.rotation = Quaternion.identity;
        //_avatar.transform.position = startPos;
        _hitpoint = _hitpointMax;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSelfMovingForward : MonoBehaviour
{
    public Vector3 direction;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Constants.GeneralConst.ENEMY_BULLET_SPEED_FAST);
    }
}

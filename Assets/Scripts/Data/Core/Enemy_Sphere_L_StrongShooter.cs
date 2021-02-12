using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sphere_L_StrongShooter : Enemy_Boss_Default
{
    public Enemy_Sphere_L_StrongShooter(string prefabName, string name, Transform parent, float health, float maxHealth, EventHandler OnDeadCallback) : base ( name, prefabName,  parent, maxHealth, OnDeadCallback)
    {
    }

    private void OnDeadCallback(object sender, EventArgs e)
    {

    }

    new public void Shoot(GameObject cannon)
    {
        UnityEngine.Object.Instantiate(cannon, _avatar.transform);
    }
}

using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sphere_L_StrongShooter : Enemy_Sphere_L
{
    public Enemy_Sphere_L_StrongShooter(string prefabName, string name, Transform parent, float health, float maxHealth) : base ( prefabName,  name,  parent,  health,  maxHealth)
    {
    }

    new public void Shoot(GameObject cannon)
    {
        UnityEngine.Object.Instantiate(cannon, _avatar.transform);
    }
}

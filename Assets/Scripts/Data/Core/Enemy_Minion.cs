﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This is a minion template
// This template is used to create customized boss minion for each level in Level Scenario used in every level
public class Enemy_Minion : Enemy_Default
{
    public Enemy_Minion(string name, string prefabName, Transform parent, string material, float maxHealth, EventHandler OnDeadCallback) : base(name, prefabName, parent, material, maxHealth, OnDeadCallback)
    {
        _damage = 0.05f;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSemiWeapon : IRangedWeapon
{
    protected void Update()
    {
        if (attackQueued && CanAttack && OnTarget)
        {
            attackQueued = false;
            StartCoroutine(Fire());
        }
    }
}

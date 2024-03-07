using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAutoWeapon : RangedSemiWeapon
{
    public override IEnumerator Fire() {
        yield return base.Fire();
        if (CanAttack && tryingToAttack)
        {
            StartCoroutine(Fire());
        }
    }
}

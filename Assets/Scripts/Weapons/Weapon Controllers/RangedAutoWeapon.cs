using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAutoWeapon : RangedSemiWeapon
{
    public override IEnumerator Attack() {
        yield return base.Attack();
        if (CanAttack && tryingToAttack)
        {
            StartCoroutine(Attack());
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedChargeWeapon : IRangedWeapon
{
    [SerializeField] private float chargeTime = 1;
    
    private float chargeRate, chargeLevel;
    
    
    private void OnValidate()
    {
        chargeRate = 1 / chargeTime;
    }
    /*
    protected void Update()
    {
        if (attackQueued && CanAttack) {
            if (tryingToAttack)
            {
                chargeLevel = Mathf.Clamp01(chargeLevel + chargeRate * Time.deltaTime);
            }
            else if (OnTarget)
            {
                chargeLevel = 0;
                attackQueued = false;
                StartCoroutine(Fire());
            }
        }
    }*/
}

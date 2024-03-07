using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[Serializable]
public class DamageInstance
{
    public Transform Source;
    
    public Team Team;
    public float Amount;
    public DamageFlags Flags;
    public DamageElement Element;
    public Vector3 Knockback;

    public DamageInstance Clone() {
        DamageInstance instance = new DamageInstance();
        instance.Source = this.Source;
        instance.Team = this.Team;
        instance.Amount = this.Amount;
        instance.Flags = this.Flags;
        instance.Element = this.Element;
        instance.Knockback = this.Knockback;
        return instance;
    }
} 

//Flags that should be checked to apply conditions
[Flags]
public enum DamageFlags
{
    None = 0,
    Crit = 1,
    Poison = 2,
    Burn = 4,
    Freeze = 8
}

public enum DamageElement
{
    None = -1,
    Physical,
    Toxic,
    Fire,
    Cold
}

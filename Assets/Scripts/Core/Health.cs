using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : Progressive, IDamageable
{
    public bool DisplayOverheadHealth;
    private Rigidbody rb;
    public event Action HealthEmpty;
    public Team Team { get => team; set => team = value; }
    private Team team;
    public List<IDamageModifier> DamageModifiers = new List<IDamageModifier>();
    
    public void OnEnable()
    {
        DamageModifiers.Clear();
        rb = transform.GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage)      
    { 
        Current -= damage;
    }
}    

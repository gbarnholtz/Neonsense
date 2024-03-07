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
    [SerializeField] private float receivedCritScalar = 2f;
    
    public void OnEnable()
    {
        DamageModifiers.Clear();
        rb = transform.GetComponent<Rigidbody>();
    }

    public void TakeDamage(DamageInstance damage)      
    {
        for (int i = 0; i < DamageModifiers.Count; i++) { damage = DamageModifiers[i].Modify(damage); }

        if (damage.Team == team && team != Team.Neutral) return;
        rb?.AddForce(damage.Knockback, ForceMode.VelocityChange);
        if ((damage.Flags & DamageFlags.Crit) > 0) damage.Amount = damage.Amount * receivedCritScalar;
        
        Current -= damage.Amount;
        //Debug.Log("received: " + damage.Amount);
        GameEventManager.Instance.InvokeHit(damage, transform);
        if (Current <= 0) {
            HealthEmpty?.Invoke();
            GameEventManager.Instance.InvokeKill(damage, transform);
            //PooledObject.Create(deathEffect, transform.position, transform.rotation);
            if (TryGetComponent(out PooledObject obj)) obj.Release();
            else Destroy(gameObject);
        }
    }
}    

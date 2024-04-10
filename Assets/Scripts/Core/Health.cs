using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Health : Progressive, IDamageable
{
    public bool DisplayOverheadHealth;
    private Rigidbody rb;
    public event Action HealthEmpty;
    public Team Team { get => team; set => team = value; }
    private Team team;
    public List<IDamageModifier> DamageModifiers = new List<IDamageModifier>();

    public void Start()
    {
        team = gameObject.GetComponent<Teamable>().Team;
    }
    
    public void OnEnable()
    {
        DamageModifiers.Clear();
        rb = transform.GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage)      
    { 
        Current -= damage;
        if (Current <= 0)
        {
            if (team == Team.Enemy) Destroy(gameObject);
            if (team == Team.Ally)
            {
                Scene thisScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(thisScene.name);
            }
        }
    }

    public float GetHealth()
    {
        return Current;
    }

    public void AddToHealth(float value)
    {
        Current += value;
    }
}    

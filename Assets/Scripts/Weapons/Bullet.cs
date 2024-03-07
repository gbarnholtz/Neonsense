using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledObject, ITeamable
{
    public float speed;
    public DamageInstance damage;

    [SerializeField] public GameObject body,trail;
    public Vector3 position => transform.position;
    public Vector3 direction => transform.forward;
    public Team Team { get => team; set => team = value; }
    private Team team;

    [SerializeField] public PooledLifetime hitEffect;
}

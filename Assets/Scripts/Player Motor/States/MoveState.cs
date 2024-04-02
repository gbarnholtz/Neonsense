using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveState
{
    protected PlayerStateMotor psm;
    protected Rigidbody rb;
    protected Vector3 velocity => rb.velocity;
    public float Height = 1.5f;
    public abstract bool ShouldApplyGravity { get; }
    public virtual bool OverrideJump => false;
    public virtual bool AllowJump => psm.IsGrounded;

    public virtual void Register(PlayerStateMotor sm)
    {
        psm = sm;
        rb = sm.GetComponent<Rigidbody>();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }
    
    public abstract void MovePlayer();

    public abstract void Update();

    public virtual bool CheckTryDash()
    {
        return true;
    }

    public virtual void DrawGizmos() { }
    public virtual void Jump() {
        Debug.LogError("Move state overrides jump but does not implement Jump method");
    }
}

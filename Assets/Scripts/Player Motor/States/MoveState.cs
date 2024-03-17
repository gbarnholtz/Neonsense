using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveState
{
    protected PlayerStateMotor psm;
    protected Rigidbody rb;
    public float Height = 1.5f;
    public abstract bool ShouldApplyGravity { get; }

    public void Register(PlayerStateMotor sm)
    {
        psm = sm;
        rb = sm.GetComponent<Rigidbody>();
    }

    public virtual void Enter() {
        return;
    }

    public virtual void Exit()
    {
        return;
    }
    
    public abstract void MovePlayer();

    public abstract void Update();

    public virtual bool CheckTryJump() {
        return true;
    }

    public virtual bool CheckTryDash()
    {
        return true;
    }
}

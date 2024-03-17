using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveState
{
    private List<Transition> transitions = new();
    protected PlayerStateMotor psm;
    protected Rigidbody rb;
    public float AccelerationScalar = 6f, Height = 1.5f;

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

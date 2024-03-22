using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class WalkMoveState : MoveState
{
    public override bool ShouldApplyGravity => false;

    public override void Update()
    {   
        if (!psm.IsGrounded) psm.ChangeState(psm.MidairState);
        else if (psm.TryingToStartSlide) psm.ChangeState(psm.SlideState);
    }

    public override void MovePlayer()
    {
        float maxAcceleration = psm.MaxSpeed * AccelerationScalar * Time.fixedDeltaTime;
        Vector3 acceleration = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(psm.TargetVelocity - rb.velocity, Vector3.up), maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);
    }
}
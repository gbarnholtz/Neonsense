using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMoveState : MoveState
{
    [SerializeField] private float slideForce, exitVelocity;

    public override void MovePlayer()
    {
        float maxAcceleration = psm.MaxSpeed * AccelerationScalar * Time.fixedDeltaTime;
        Vector3 acceleration = Vector3.ClampMagnitude(-rb.velocity, maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);
    }

    public override void Update()
    {
        if (!psm.IsGrounded) {
            psm.SetState(psm.MidairState);
        }else if (rb.velocity.magnitude < exitVelocity || !psm.TryingToSlide) psm.SetState(psm.WalkState);
    }

    public override void Enter()
    {
        Vector3 force = slideForce * Vector3.ProjectOnPlane(rb.velocity, psm.ContactNormal).normalized;
        rb.AddForce(force, ForceMode.VelocityChange);
        psm.SlideReset = false;
    }
}

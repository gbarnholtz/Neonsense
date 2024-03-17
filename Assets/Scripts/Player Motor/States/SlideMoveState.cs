using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMoveState : MoveState
{
    public override bool ShouldApplyGravity => true;
    [SerializeField] private float slideForce=10f, exitVelocity=3f, groundFrictionScalar=2f, airFrictionScalar=1f;
    
    public bool FastEnoughToSlide => Vector3.ProjectOnPlane(rb.velocity, psm.ContactNormal).magnitude > exitVelocity;

    public override void MovePlayer()
    {
        float frictionScalar = psm.IsGrounded ? groundFrictionScalar : airFrictionScalar;
        float maxAcceleration = psm.BaseSpeed * frictionScalar * Time.fixedDeltaTime;
        Vector3 acceleration = Vector3.ClampMagnitude(-rb.velocity, maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);
    }

    public override void Update()
    {
        if (!FastEnoughToSlide || !psm.TryingToSlide) psm.ChangeState(psm.WalkState);
    }

    public override void Enter()
    {
        Vector3 force = slideForce * Vector3.ProjectOnPlane(rb.velocity, psm.ContactNormal).normalized;
        rb.AddForce(force, ForceMode.VelocityChange);
        psm.SlideReset = false;
    }
}

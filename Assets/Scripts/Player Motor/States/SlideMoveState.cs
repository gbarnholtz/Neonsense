using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMoveState : MoveState
{
    public override bool ShouldApplyGravity => true;
    [SerializeField] private float groundFrictionScalar = 2f, airFrictionScalar = 1f, slideForce = 10f, slideFullRechargeDelay = 2f, exitVelocity = 3f;
    private float timeLastSlid;
    public bool FastEnoughToSlide => Vector3.ProjectOnPlane(rb.velocity, psm.ContactNormal).magnitude > exitVelocity;

    public override void MovePlayer()
    {
        float frictionScalar = psm.IsGrounded ? groundFrictionScalar : airFrictionScalar;
        float maxAcceleration = psm.BaseSpeed * frictionScalar * Time.fixedDeltaTime;
        if (!psm.IsGrounded) return;
        Vector3 acceleration = Vector3.ClampMagnitude(-rb.velocity, maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);
    }

    public override void Update()
    {
        if (!FastEnoughToSlide || !psm.TryingToSlide) psm.ChangeState(psm.WalkState);
    }

    public override void Enter()
    {
        float availableForce = Mathf.Lerp(0, slideForce, (Time.time - timeLastSlid)/slideFullRechargeDelay);
        Vector3 force = availableForce * Vector3.ProjectOnPlane(psm.TargetVelocity, psm.ContactNormal).normalized;
        rb.AddForce(force, ForceMode.VelocityChange);
        psm.SlideReset = false;
        timeLastSlid = Time.time;
    }
}

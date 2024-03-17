using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MidairMoveState : MoveState
{
    public override bool ShouldApplyGravity => true;

    [SerializeField] private float terminalVelocity=10f, gravity=15f;
    [SerializeField] private int airJumps;
    private int currentAirJumps;

    public override void MovePlayer()
    {
        float maxAcceleration = psm.MaxSpeed * AccelerationScalar * Time.fixedDeltaTime;
        Vector3 acceleration = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(psm.TargetVelocity - rb.velocity, Vector3.up), maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);
    }

    public override void Update()
    {
        if (psm.IsGrounded) {
            if (psm.TryingToStartSlide) psm.ChangeState(psm.SlideState);
            else psm.ChangeState(psm.WalkState);
        }
    }
    public override bool CheckTryJump() {
        bool canJump = currentAirJumps < airJumps;
        if (canJump) currentAirJumps++;
        return canJump;
    }

    public override void Enter()
    {
        currentAirJumps = 0;
    }
}
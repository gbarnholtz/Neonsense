using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MidairMoveState : MoveState
{
    [SerializeField] private float terminalVelocity=10f, gravity=15f;
    [SerializeField] private int airJumps;
    private int currentAirJumps;

    public override void MovePlayer()
    {
        float maxAcceleration = psm.MaxSpeed * AccelerationScalar * Time.fixedDeltaTime;
        Vector3 acceleration = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(psm.TargetVelocity - rb.velocity, Vector3.up), maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);

        ApplyGravity();
    }

    public override void Update()
    {
        if (psm.IsGrounded) {
            if (psm.TryingToStartSlide) psm.SetState(psm.SlideState);
            else psm.SetState(psm.WalkState);
        }
    }

    private void ApplyGravity()
    {
        if (rb.velocity.y < -terminalVelocity) return;
        rb.AddForce(Vector3.ClampMagnitude((-terminalVelocity - rb.velocity.y) * Vector3.up, gravity * Time.fixedDeltaTime), ForceMode.VelocityChange);
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
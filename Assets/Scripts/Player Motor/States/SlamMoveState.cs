using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamMoveState : MoveState
{
    public override bool ShouldApplyGravity => false;
    [SerializeField] private float terminalVelocity, acceleration, entryVelocity, entryHeight;

    public override void MovePlayer()
    {
        if (velocity.y < -terminalVelocity) return;
        Vector3 gravityForce = Vector3.ClampMagnitude((-terminalVelocity - velocity.y) * Vector3.up, acceleration * Time.fixedDeltaTime);
        rb.AddForce(gravityForce, ForceMode.VelocityChange);
    }

    public override void Update()
    {
        if(psm.IsGrounded) {
            if (psm.TryingToSlide) psm.ChangeState(psm.SlideState);
            else psm.ChangeState(psm.WalkState);
        }
    }

    public override void Enter()
    {
        rb.velocity = Vector3.down * entryVelocity;
    }
}

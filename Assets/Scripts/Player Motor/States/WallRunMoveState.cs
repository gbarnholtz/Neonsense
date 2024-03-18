using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallRunMoveState : MoveState, IInputModifier
{
    public override bool ShouldApplyGravity => false;
    public override bool OverrideJump => true;

    [SerializeField] private float wallRunSpeedScalar = 1.5f, accelerationScalar=4f, wallDetectDistance = 0.25f, ejectForce = 2f, downSlip = 2f;

    [SerializeField, Range(0,1)] private float wallJumpBias = 0.5f;
    private Vector3 wallNormal, runDirection;
    private float headRadius;
    public bool ShouldEnterWall => WallEntryCheck();


    public override void Register(PlayerStateMotor sm)
    {
        base.Register(sm);
        headRadius = sm.GetComponent<SphereCollider>().radius;
    }
    public override void Update()
    {
        if (psm.IsGrounded || !WallTraverseCheck()) psm.ChangeState(psm.WalkState);
        else if (Vector3.Dot(psm.TargetVelocity, wallNormal) > 0) {
            rb.AddForce(ejectForce * wallNormal, ForceMode.VelocityChange);
            psm.ChangeState(psm.WalkState);
        }
    }

    public override void MovePlayer()
    {
        rb.AddForce(-Vector3.up * downSlip, ForceMode.Acceleration);
    }

    private bool WallEntryCheck() {
        Vector3 direction = psm.TargetVelocity;
        if (!Physics.Raycast(rb.position, direction, out RaycastHit rayHit, wallDetectDistance+headRadius)) return false;
        wallNormal = rayHit.normal;
        if (Physics.SphereCast(rb.position, headRadius - 0.01f, direction * wallDetectDistance, out RaycastHit sphereHit)) wallNormal = sphereHit.normal;
        runDirection = Vector3.Project(direction, wallNormal);
        return true;
    }

    private bool WallTraverseCheck()
    {
        Vector3 direction = -wallNormal;
        if (!Physics.Raycast(rb.position, direction, out RaycastHit rayHit, wallDetectDistance + headRadius)) return false;
        wallNormal = rayHit.normal;
        if (Physics.SphereCast(rb.position, headRadius - 0.01f, direction * wallDetectDistance, out RaycastHit sphereHit)) wallNormal = sphereHit.normal;
        runDirection = Vector3.Project(direction, wallNormal);
        return true;
    }

    public override void Enter()
    {
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, wallNormal);
    }

    public override void Jump()
    {
        psm.JumpDirectional(Vector3.Lerp(Vector3.up, wallNormal, wallJumpBias));
        rb.AddForce(ejectForce * wallNormal, ForceMode.VelocityChange);
        psm.ChangeState(psm.WalkState);
    }

    private float GetForwardInput()
    {
        Vector3 horizontalLook = Vector3.ProjectOnPlane(psm.LookDirection, Vector3.up);
        return Vector3.Dot(psm.TargetVelocity.normalized, horizontalLook.normalized);
    }

    public InputState ModifyInput(InputState input)
    {
        return input;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallRunMoveState : MoveState, IInputModifier
{
    public override bool ShouldApplyGravity => false;
    public override bool OverrideJump => true;

    [SerializeField] private float wallRunSpeedScalar = 1.5f, accelerationScalar, wallDetectDistance = 2f, passiveEjectForce = 2f, downSlip = 2f;
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
            rb.AddForce(passiveEjectForce * wallNormal, ForceMode.VelocityChange);
            psm.ChangeState(psm.WalkState);
        }
    }

    public override void MovePlayer()
    {
        Vector3 acceleration = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(psm.TargetVelocity - rb.velocity, wallNormal), psm.BaseSpeed * Time.fixedDeltaTime);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);
        rb.AddForce(-Vector3.up * downSlip, ForceMode.Acceleration);
    }

    private bool WallEntryCheck() {
        Vector3 direction = psm.TargetVelocity;
        if (!Physics.Raycast(rb.position, direction, out RaycastHit rayHit, wallDetectDistance)) return false;
        wallNormal = rayHit.normal;
        if (Physics.SphereCast(rb.position, headRadius - 0.01f, direction * Mathf.Max(wallDetectDistance - headRadius,0f), out RaycastHit sphereHit)) wallNormal = sphereHit.normal;
        runDirection = Vector3.Project(direction, wallNormal);
        return true;
    }

    private bool WallTraverseCheck()
    {
        Vector3 direction = -wallNormal;
        if (!Physics.Raycast(rb.position, direction, out RaycastHit rayHit, wallDetectDistance)) return false;
        wallNormal = rayHit.normal;
        if (Physics.SphereCast(rb.position, headRadius - 0.01f, direction * Mathf.Max(wallDetectDistance - headRadius, 0f), out RaycastHit sphereHit)) wallNormal = sphereHit.normal;
        runDirection = Vector3.Project(direction, wallNormal);
        return true;
    }

    public override void Enter()
    {
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, wallNormal);
    }

    public override void Jump()
    {
        return;
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

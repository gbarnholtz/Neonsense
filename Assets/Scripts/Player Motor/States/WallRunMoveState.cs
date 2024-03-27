using UnityEngine;
using Drawing;
using ProPixelizer;

public class WallRunMoveState : MoveState, IInputModifier
{
    public override bool ShouldApplyGravity => false;
    public override bool OverrideJump => true;

    [SerializeField] private float wallRunSpeedScalar = 1.5f, accelerationScalar=4f, ejectForce = 2f, ejectVelocity=1f, downSlip = 2f, slipEjectThreshold=6f;
    [SerializeField, Range(0, 1)] private float wallJumpBias = 0.5f;
    [Header("Entry Properties")]
    [SerializeField] private float wallDetectDistance = 0.25f;
    [SerializeField, Range(0, 90)] private float entryAngleLimit; 
    [Header("Wall Spring Properties")]
    [SerializeField] private float springForce=400f;
    [SerializeField] private float springDamper=50f, targetSpringDistance=0.1f;
    public bool CheckWallRun => RefreshWallEntry();// && EntryAngleCheck();
    private Vector3 wallNormal => wallHit.normal;
    private Vector3 runDirection;
    private float headRadius;
    private RaycastHit wallHit;
    [SerializeField] private LayerMask wallRunLayer;
    public override void Register(PlayerStateMotor sm) {
        base.Register(sm);
        headRadius = sm.GetComponent<SphereCollider>().radius;
    }

    public override void Update() {
        bool onWall = RefreshWallStatus();
        if (psm.IsGrounded || !onWall) psm.ChangeState(psm.WalkState);
        else if (Vector3.Dot(psm.TargetDirection, wallNormal) > 0.5f || rb.velocity.y < -slipEjectThreshold) { Eject(); }
    }

    public override void MovePlayer() {
        Vector3 springDir = wallHit.normal;
        float offset = targetSpringDistance - wallHit.distance;
        float springVelocity = Vector3.Dot(springDir, velocity);
        float force = (offset * springForce) - (springVelocity * springDamper);
        rb.AddForce(force * springDir, ForceMode.Acceleration);

        float heading = Vector3.Dot(velocity, runDirection);
        Vector3 runForce = runDirection * (wallRunSpeedScalar * psm.BaseSpeed - heading);
        //Debug.Log((wallRunSpeedScalar * psm.BaseSpeed) - heading);
        rb.AddForce(Vector3.ClampMagnitude(runForce, psm.BaseSpeed*accelerationScalar*Time.fixedDeltaTime), ForceMode.VelocityChange);

        rb.AddForce(-Vector3.up * downSlip, ForceMode.Acceleration);
    }

    private bool RefreshWallEntry() {
        Vector3 direction = Vector3.ProjectOnPlane(velocity, Vector3.up).normalized;
        Vector3 right = Quaternion.Euler(0, 45f, 0) * direction;
        Vector3 left = Quaternion.Euler(0, -45f, 0) * direction;
        float distance = wallDetectDistance + headRadius;
        Draw.Ray(rb.position, direction.normalized * distance, Color.magenta);
        Draw.Ray(rb.position, right.normalized * distance, Color.magenta);
        Draw.Ray(rb.position, left.normalized * distance, Color.magenta);
        if (!WallCheck(left, out wallHit) &&
            !WallCheck(right, out wallHit) &&
            !WallCheck(direction, out wallHit)) return false;

        runDirection = Vector3.ProjectOnPlane(psm.TargetDirection, wallNormal).normalized;
        return true;
    }

    private bool RefreshWallStatus()
    {
        //Less clean but easier to read
        Vector3 direction = -wallNormal;
        if (!WallCheck(direction, out wallHit)) return false;
        if (!Physics.SphereCast(rb.position, headRadius - 0.01f, direction, out wallHit, wallDetectDistance + 0.01f, wallRunLayer)) return false;
        runDirection = Vector3.ProjectOnPlane(psm.TargetDirection, wallNormal).normalized;
        return true;
    }

    public override void Enter()
    {
        rb.velocity = Vector3.ProjectOnPlane(velocity, wallNormal);
        rb.AddForce(ejectForce * -wallNormal, ForceMode.VelocityChange);
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
    }

    public override void Jump()
    {
        psm.JumpDirectional(Vector3.Lerp(Vector3.up, wallNormal, wallJumpBias));
        Eject();
    }

    public InputState ModifyInput(InputState input)
    {
        //camera stuff here maybe
        return input;
    }

    public override void DrawGizmos()
    {
        using (Draw.WithColor(Color.red)) {
            Draw.Arrow(wallHit.point, wallHit.point + wallHit.normal);
            Draw.Arrow(rb.position, rb.position + runDirection * 2f);
        }
    }

    private void Eject() {
        rb.AddForce(ejectForce * wallNormal, ForceMode.VelocityChange);
        psm.ChangeState(psm.WalkState);
    }

    private bool WallCheck(Vector3 direction, out RaycastHit hit)
    {
        bool floorHit = Physics.Raycast(rb.position - Vector3.up * Height, direction, wallDetectDistance + headRadius, wallRunLayer);
        return Physics.Raycast(rb.position, direction, out hit, wallDetectDistance + headRadius, wallRunLayer);
    }

    private bool EntryAngleCheck() {
        return Vector3.Dot(rb.velocity.normalized, -wallHit.normal) < Mathf.Cos(entryAngleLimit * Mathf.Deg2Rad);
    }
}

using UnityEngine;

public class WalkMoveState : MoveState
{
    public override bool ShouldApplyGravity => !psm.IsGrounded;
    [SerializeField] private float groundAccelerationScalar = 8f, airAccelerationScalar=4f, friction=6f;

    public override void Update() {
        if (psm.IsGrounded && psm.TryingToStartSlide) { psm.ChangeState(psm.SlideState); return; 
        } else {
            //if (psm.TryingToStartSlide) { psm.ChangeState(psm.SlamState);  return; }
            if (!psm.IsGrounded && psm.WallRunState.RefreshWallEntry()) { psm.ChangeState(psm.WallRunState); return; }
        }
    }

    public override void MovePlayer() {
        float groundSpeed = Vector3.ProjectOnPlane(rb.velocity, psm.ContactNormal).magnitude;
        if(psm.IsGrounded && groundSpeed > 0) rb.velocity *= Mathf.Max(groundSpeed - (groundSpeed* friction * Time.fixedDeltaTime)) / groundSpeed;

        float acceleration = psm.BaseSpeed * (psm.IsGrounded ? groundAccelerationScalar : airAccelerationScalar) * Time.fixedDeltaTime;
        float heading = Vector3.Dot(rb.velocity, psm.ProjectedTargetDirection);
        Debug.Log(psm.ProjectedTargetDirection);
        if (heading + acceleration > psm.BaseSpeed) acceleration = psm.BaseSpeed - heading;
        rb.AddForce(acceleration * psm.ProjectedTargetDirection, ForceMode.VelocityChange);
        //Debug.Log(groundSpeed);
    }
}